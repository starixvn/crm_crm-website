// --------------------------------------------------------------------------
// <copyright file="StatisticReportService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Services.Implements.StatisticReport
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web;
    using Fanex.CRM.Infrastructure.Common;
    using Fanex.CRM.Infrastructure.Core;
    using Fanex.CRM.Infrastructure.Extension;
    using Fanex.CRM.Infrastructure.Logging;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Infrastructure.Models.Npoi;
    using Fanex.CRM.Infrastructure.Service.Implements;
    using Fanex.CRM.Report.Criteria.StatisticReport;
    using Fanex.CRM.Report.Entities.StatisticReport;
    using Fanex.CRM.Report.Models.StatisticReport;
    using Fanex.CRM.Report.Resources;
    using Fanex.CRM.Report.Services.Implements.DynamicReport;
    using Fanex.CRM.Report.Services.Interfaces;
    using Fanex.CRM.Setting.Constants;
    using Fanex.CRM.Setting.Models;
    using Fanex.CRM.Setting.Services.Implements;
    using Fanex.CRM.Setting.Services.Interfaces;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;

    /// <summary>
    /// Statistic Report Service.
    /// </summary>
    public class StatisticReportService : ServiceBase, IStatisticReportService
    {
        public readonly string StatisticReportSessionKey = "Fanex.CRM.Report.StatisticReport.StatisticReportSessionKey_" +
            (AppSession.CurrentUser != null ? AppSession.CurrentUser.Id : 0);

        private ISettingService _serviceSetting;

        public StatisticReportService()
            : this(null)
        {
        }

        public StatisticReportService(ISettingService serviceSetting)
        {
            _serviceSetting = serviceSetting ?? new SettingService();
        }

        /// <summary>
        /// Gets all period type.
        /// </summary>
        /// <returns>All period type.</returns>
        public List<SettingInfo> GetAllPeriodType()
        {
            try
            {
                var allPeriodType = new List<SettingInfo>();
                allPeriodType.Add(new SettingInfo
                {
                    Id = PeriodTypeConstants.Daily,
                    Name = Labels.Daily
                });

                allPeriodType.Add(new SettingInfo
                {
                    Id = PeriodTypeConstants.Monthly,
                    Name = Labels.Monthly
                });

                return allPeriodType;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return new List<SettingInfo>();
        }

        /// <summary>
        /// Gets all main category.
        /// </summary>
        /// <returns>All main category.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public List<SettingInfo> GetAllMainCategory()
        {
            try
            {
                var allMainCategory = new List<SettingInfo>();
                var rawData = _serviceSetting.GetAllMainCategory();
                var allSubSettingOption = new SettingInfo
                {
                    Id = SettingConstants.AllSetting,
                    Name = Labels.All,
                };

                var remindSubSettingOption = new SettingInfo
                {
                    Id = SettingConstants.NotApply,
                    Name = Labels.Please_choose_one,
                };

                allMainCategory.Add(new SettingInfo
                {
                    Id = SettingConstants.NotApply,
                    Name = Labels.Please_choose_one,
                    ChildSettings = new List<SettingInfo>
                    {
                       remindSubSettingOption
                    }
                });

                allMainCategory.Add(new SettingInfo
                {
                    Id = SettingConstants.AllSetting,
                    Name = Labels.All,
                    ChildSettings = new List<SettingInfo>
                    {
                       allSubSettingOption
                    }
                });

                foreach (var rawMainCategory in rawData)
                {
                    rawMainCategory.ChildSettings.Insert(0, allSubSettingOption);
                    rawMainCategory.ChildSettings.Insert(0, remindSubSettingOption);
                    allMainCategory.Add(rawMainCategory);
                }

                return allMainCategory;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return new List<SettingInfo>();
        }

        /// <summary>
        /// Gets the statistic report.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The statistic report.</returns>      
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public StatisticReportInfo GetStatisticReport(GetStatisticReportCriteria criteria)
        {
            try
            {
                StatisticReportInfo statisticReport = null;
                if (criteria.IsGetFromCached)
                {
                    statisticReport = (StatisticReportInfo)SessionExtension.GetSession(HttpContext.Current.Session, StatisticReportSessionKey);
                }

                if (statisticReport == null)
                {
                    criteria.FromDate = DateTimeUtilize.ChangeClientFromTimeToServerTime(criteria.FromDate);
                    criteria.ToDate = DateTimeUtilize.ChangeClientToTimeToServerTime(criteria.ToDate);
                    var rawData = GetDynamicData<StatisticReportEntity>(criteria).ToList();

                    var countryList = rawData
                        .GroupBy(statisticReportEntity => statisticReportEntity.CountryId)
                        .Select(group => new CountryInfo
                        {
                            Id = group.First().CountryId,
                            Name = group.First().CountryId == SettingConstants.NotApply ?
                                Labels.Symbol_NotApply :
                                group.First().CountryName
                        })
                        .ToList();
                    var countryDictionary = GetCountryDictionary(countryList);
                    switch (criteria.ReportType)
                    {
                        case PeriodTypeConstants.Daily:
                            statisticReport = GetStatisticReportByDaily(
                                rawData,
                                countryList,
                                countryDictionary,
                                criteria);
                            break;
                        case PeriodTypeConstants.Monthly:
                            statisticReport = GetStatisticReportByMonthly(
                                rawData,
                                countryList,
                                countryDictionary,
                                criteria);
                            break;
                        default:
                            break;
                    }

                    statisticReport.ReportTypeId = criteria.ReportType;
                    statisticReport.TotalChildReports = statisticReport.ChildReports.Count;
                    statisticReport.ChartInfo = GetChartInfo(statisticReport, countryList, countryDictionary);
                    SessionExtension.AddSession(HttpContext.Current.Session, StatisticReportSessionKey, statisticReport);
                }

                return new StatisticReportInfo
                {
                    Name = statisticReport.Name,
                    ShortName = statisticReport.ShortName,
                    ChildReports = criteria.IsGetFullReport ?
                        statisticReport.ChildReports :
                        GetPagingLogList(criteria, statisticReport.ChildReports),
                    Total = statisticReport.Total,
                    TotalChildReports = statisticReport.TotalChildReports,
                    ChartInfo = statisticReport.ChartInfo
                };
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        /// <summary>
        /// Exports the statistic report.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        public void ExportStatisticReport(GetStatisticReportCriteria criteria)
        {
            try
            {
                StatisticReportInfo statisticReport = CacheService.Get<StatisticReportInfo>(StatisticReportSessionKey);
                criteria.IsGetFullReport = true;
                if (statisticReport == null)
                {
                    statisticReport = GetStatisticReport(criteria);
                }

                criteria.FromDate = DateTimeUtilize.ChangeClientFromTimeToServerTime(criteria.FromDate);
                criteria.ToDate = DateTimeUtilize.ChangeClientToTimeToServerTime(criteria.ToDate);
                ExportExcelInfo exportExcelInfo = new ExportExcelInfo
                {
                    ExcelFile = new HSSFWorkbook(),
                    StartRow = 0,
                    StartColumn = 0,
                    CurrentRow = 0,
                    MaxColumnIndex = statisticReport != null &&
                        statisticReport.ChildReports.Count > 0 &&
                        statisticReport.ChildReports[0].ChildReports.Count > 0 ?
                        2 + statisticReport.ChildReports[0].ChildReports[0].ChildReports.Count :
                        2
                };

                ExportStatisticReportBody(ref exportExcelInfo, criteria, statisticReport);
                string reportName = string.Format(
                     Labels.StatisticReport_ExcelTitle,
                     SettingService.GetPeriodTypeName(criteria.ReportType),
                     criteria.FromDate.ToShortFormatedDateWithSlash().Replace("/", "_"),
                     criteria.ToDate.ToShortFormatedDateWithSlash().Replace("/", "_"));

                NpoiExcelExtensions.ResponseStreamExcel(exportExcelInfo.ExcelFile, reportName);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
        }

        /// <summary>
        /// Gets the statistic report by monthly.
        /// </summary>
        /// <param name="rawData">The raw data.</param>
        /// <param name="countryList">The country list.</param>
        /// <param name="countryDictionary">The country dictionary.</param>
        /// <param name="criteria">The criteria.</param>
        /// <returns>
        /// The statistic report by monthly.
        /// </returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private StatisticReportInfo GetStatisticReportByMonthly(
            List<StatisticReportEntity> rawData,
            List<CountryInfo> countryList,
            Dictionary<int, int> countryDictionary,
            GetStatisticReportCriteria criteria)
        {
            var countryLength = countryDictionary.Count;
            var fromDateClient = DateTimeUtilize.ChangeTimeZoneToClientDateTime(criteria.FromDate).ToShortFormatedDate();
            var toDateClient = DateTimeUtilize.ChangeTimeZoneToClientDateTime(criteria.FromDate).ToShortFormatedDate();

            StatisticReportInfo statisticReport = new StatisticReportInfo
            {
                ChildReports = new List<StatisticReportInfo>(),
                Name = string.Format(Labels.StatisticReport_Title, Labels.Monthly, fromDateClient, toDateClient),
                ShortName = string.Format(Labels.StatisticReport_ShortTitle, fromDateClient, toDateClient),
                Total = new StatisticReportInfo
                {
                    ShortName = Labels.Grand_Total,
                    ChildReports = GetDummyCountryList(countryLength, countryList, true)
                }
            };

            if (rawData == null || !rawData.Any())
            {
                return statisticReport;
            }

            var dateGroups = rawData.GroupBy(data => new DateTime(data.DateTime.Year, data.DateTime.Month, 1));
            foreach (var dateGroup in dateGroups)
            {
                var dateReport = new StatisticReportInfo
                {
                    ShortName = dateGroup.Key.ToMonthYear(),
                    ChildReports = new List<StatisticReportInfo>(),
                    PeriodRange = new PeriodRange
                    {
                        FromDate = dateGroup.Key,
                        ToDate = dateGroup.Key.LastDayOfMonth()
                    },
                    Total = new StatisticReportInfo
                    {
                        ShortName = string.Format(
                            Labels.StatisticReport_TotalTitle,
                            dateGroup.Key.ToMonthYear()),
                        ChildReports = GetDummyCountryList(countryLength, countryList, false)
                    }
                };

                dateReport.PeriodRange.FromDateString = dateReport.PeriodRange.FromDate.ToShortFormatedDate();
                dateReport.PeriodRange.ToDateString = dateReport.PeriodRange.ToDate.ToShortFormatedDate();
                var timeGroups = dateGroup.ToList()
                     .OrderBy(data => data.DateTime.Hour)
                     .GroupBy(data => new DateTime(
                        dateGroup.Key.Year,
                        dateGroup.Key.Month,
                        dateGroup.Key.Day,
                        data.DateTime.Hour,
                        0,
                        0));
                foreach (var timeGroup in timeGroups)
                {
                    var timeReport = new StatisticReportInfo
                    {
                        ShortName = string.Format("{0:00}:00 - {0:00}:59", timeGroup.Key.Hour),
                        ChildReports = GetDummyCountryList(countryLength, countryList, false)
                    };

                    GetTimeReport(timeGroup.ToList(), countryDictionary, ref timeReport, ref statisticReport, ref dateReport);
                }

                statisticReport.Total.NumberOfIssues += dateReport.Total.NumberOfIssues;
                statisticReport.ChildReports.Add(dateReport);
            }

            return statisticReport;
        }

        /// <summary>
        /// Gets the statistic report by daily.
        /// </summary>
        /// <param name="rawData">The raw data.</param>
        /// <param name="countryList">The country list.</param>
        /// <param name="countryDictionary">The country dictionary.</param>
        /// <param name="criteria">The criteria.</param>
        /// <returns>
        /// The statistic report by daily.
        /// </returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private StatisticReportInfo GetStatisticReportByDaily(
            List<StatisticReportEntity> rawData,
            List<CountryInfo> countryList,
            Dictionary<int, int> countryDictionary,
            GetStatisticReportCriteria criteria)
        {
            var countryLength = countryDictionary.Count;
            var fromDateClient = DateTimeUtilize.ChangeTimeZoneToClientDateTime(criteria.FromDate).ToShortFormatedDate();
            var toDateClient = DateTimeUtilize.ChangeTimeZoneToClientDateTime(criteria.FromDate).ToShortFormatedDate();

            StatisticReportInfo statisticReport = new StatisticReportInfo
            {
                ChildReports = new List<StatisticReportInfo>(),
                Name = string.Format(Labels.StatisticReport_Title, Labels.Daily, fromDateClient, toDateClient),
                ShortName = string.Format(Labels.StatisticReport_ShortTitle, fromDateClient, toDateClient),
                Total = new StatisticReportInfo
                {
                    ShortName = Labels.Grand_Total,
                    ChildReports = GetDummyCountryList(countryLength, countryList, true)
                }
            };

            if (rawData == null || !rawData.Any())
            {
                return statisticReport;
            }

            var dateGroups = rawData.GroupBy(data => DateTimeUtilize.ChangeTimeZoneToClientDateTime(data.DateTime).ToShortFormatedDate());
            foreach (var dateGroup in dateGroups)
            {
                var dateReport = new StatisticReportInfo
                {
                    ShortName = dateGroup.Key.ToString(),
                    ChildReports = new List<StatisticReportInfo>(),
                    Total = new StatisticReportInfo
                    {
                        ShortName = string.Format(
                            Labels.StatisticReport_TotalTitle,
                            dateGroup.Key.ToString()),
                        ChildReports = GetDummyCountryList(countryLength, countryList, false)
                    }
                };

                var timeGroups = dateGroup.ToList()
                    .OrderBy(data => data.DateTime.Hour)
                    .GroupBy(data => data.DateTime.Hour);
                foreach (var timeGroup in timeGroups)
                {
                    var timeReport = new StatisticReportInfo
                    {
                        ShortName = string.Format("{0:00}:00 - {0:00}:59", timeGroup.Key),
                        ChildReports = GetDummyCountryList(countryLength, countryList, false)
                    };

                    GetTimeReport(timeGroup.ToList(), countryDictionary, ref timeReport, ref statisticReport, ref dateReport);
                }

                statisticReport.Total.NumberOfIssues += dateReport.Total.NumberOfIssues;
                statisticReport.ChildReports.Add(dateReport);
            }

            return statisticReport;
        }

        /// <summary>
        /// Gets the paging log list.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="statisticReportList">The statistic report list.</param>
        /// <returns>
        /// The paging log list.
        /// </returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private List<StatisticReportInfo> GetPagingLogList(
            GetStatisticReportCriteria criteria,
            List<StatisticReportInfo> statisticReportList)
        {
            return statisticReportList.Skip(criteria.PageSize * criteria.PageIndex)
                .Take(criteria.PageSize)
                .ToList();
        }

        /// <summary>
        /// Gets the country dictionary.
        /// </summary>
        /// <param name="countryList">The country list.</param>
        /// <returns>The country dictionary.</returns>
        private Dictionary<int, int> GetCountryDictionary(List<CountryInfo> countryList)
        {
            Dictionary<int, int> countryDictionary = new Dictionary<int, int>();
            var countryLength = countryList.Count;
            var currentCountryIndex = 0;
            for (var i = 0; i < countryLength; i++)
            {
                var country = countryList[i];
                if (countryDictionary.ContainsKey(country.Id))
                {
                    continue;
                }

                countryDictionary.Add(country.Id, currentCountryIndex);
                currentCountryIndex++;
            }

            return countryDictionary;
        }

        /// <summary>
        /// Gets the dummy country list.
        /// </summary>
        /// <param name="countryLength">Length of the country.</param>
        /// <param name="countryList">The country list.</param>
        /// <param name="isGrandTotal">If set to <c>true</c> [is grand total].</param>
        /// <returns>
        /// The dummy country list.
        /// </returns>
        private List<StatisticReportInfo> GetDummyCountryList(
            int countryLength,
            List<CountryInfo> countryList,
            bool isGrandTotal)
        {
            List<StatisticReportInfo> dummyCountries = new List<StatisticReportInfo>();
            for (var i = 0; i < countryLength; i++)
            {
                dummyCountries.Add(new StatisticReportInfo
                {
                    CountryInfo = isGrandTotal ? countryList[i] : null
                });
            }

            return dummyCountries;
        }

        /// <summary>
        /// Gets the chart information.
        /// </summary>
        /// <param name="statisticReport">The statistic report.</param>
        /// <param name="countryList">The country list.</param>
        /// <param name="countryDictionary">The country dictionary.</param>
        /// <returns>The chart information.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private StatisticReportInfo GetChartInfo(
            StatisticReportInfo statisticReport,
            List<CountryInfo> countryList,
            Dictionary<int, int> countryDictionary)
        {
            var chartInfo = new StatisticReportInfo
            {
                Name = statisticReport.Name,
                ShortName = statisticReport.ShortName,
                NumberOfIssues = statisticReport.Total.NumberOfIssues,
                ChildReports = new List<StatisticReportInfo>()
            };

            var countryLength = countryDictionary.Count;
            var dateReportLength = statisticReport.ChildReports.Count;
            for (var dateIndex = 0; dateIndex < dateReportLength; dateIndex++)
            {
                var dateReport = statisticReport.ChildReports[dateIndex];
                var dateChart = new StatisticReportInfo
                {
                    ShortName = dateReport.ShortName,
                    NumberOfIssues = dateReport.Total.NumberOfIssues,
                    ChildReports = new List<StatisticReportInfo>()
                };

                var timeReportLength = dateReport.ChildReports.Count;
                for (var timeIndex = 0; timeIndex < timeReportLength; timeIndex++)
                {
                    var timeReport = dateReport.ChildReports[timeIndex];
                    var timeChart = new StatisticReportInfo
                    {
                        ShortName = timeReport.ShortName,
                        NumberOfIssues = timeReport.NumberOfIssues,
                        ChildReports = new List<StatisticReportInfo>()
                    };

                    for (var i = 0; i < countryLength; i++)
                    {
                        var countryReport = timeReport.ChildReports[i];
                        timeChart.ChildReports.Add(new StatisticReportInfo
                        {
                            HasData = countryReport.HasData,
                            NumberOfIssues = countryReport.NumberOfIssues,
                            CountryInfo = countryReport.CountryInfo
                        });
                    }

                    dateChart.ChildReports.Add(timeChart);
                }

                chartInfo.ChildReports.Add(dateChart);
            }

            return chartInfo;
        }

        /// <summary>
        /// Exports the dynamic report body.
        /// </summary>
        /// <param name="exportExcelInfo">The export excel information.</param>
        /// <param name="criteria">The criteria.</param>
        /// <param name="statisticReport">The statistic report.</param>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
          "SP2101:MethodMustNotContainMoreLinesThan",
          Justification = "Reviewed. Suppression is OK here.")]
        private void ExportStatisticReportBody(
            ref ExportExcelInfo exportExcelInfo,
            GetStatisticReportCriteria criteria,
            StatisticReportInfo statisticReport)
        {
            if (statisticReport == null || statisticReport.ChildReports.Count == 0)
            {
                exportExcelInfo.Worksheet = exportExcelInfo.ExcelFile.CreateSheet(Labels.StatisticReport_SheetName);
                ExportStatisticReportHeader(ref exportExcelInfo, criteria, statisticReport, null);
                NpoiExcelExtensions.RenderNoInfoMessage(ref exportExcelInfo, exportExcelInfo.CurrentRow);
                return;
            }

            var countryLength = statisticReport.Total.ChildReports.Count;
            List<CountryInfo> countryList = new List<CountryInfo>();
            for (var k = 0; k < countryLength; k++)
            {
                var reportByCountryInfo = statisticReport.Total.ChildReports[k];
                var country = reportByCountryInfo.CountryInfo;
                countryList.Add(country);
            }

            var childReportLength = statisticReport.ChildReports.Count;

            var normalRowWriter = new NormalRowExcelWriter(exportExcelInfo);
            var totalRowWriter = new TotalRowExcelWriter(exportExcelInfo);
            var firstGrandTotalRowWriter = new FirstGrandTotalRowExcelWriter(exportExcelInfo);
            NpoiExcelExtensions.SetTotalColorIndex(ref exportExcelInfo);
            NpoiExcelExtensions.SetGrandTotalColorIndex(ref exportExcelInfo);

            var maxRowPerSheet = NpoiExcelExtensions.MaxRowPerSheet;
            var numberOfSheet = (childReportLength / maxRowPerSheet) +
                (childReportLength % maxRowPerSheet != 0 ? 1 : 0);
            for (var sheetIndex = 0; sheetIndex < numberOfSheet; sheetIndex++)
            {
                var sheetName = numberOfSheet > 1 ?
                    string.Format("{0} - {1}", Labels.StatisticReport_SheetName, sheetIndex + 1) :
                    Labels.StatisticReport_SheetName;
                exportExcelInfo.Worksheet = exportExcelInfo.ExcelFile.CreateSheet(sheetName);
                exportExcelInfo.CurrentRow = 0;
                ExportStatisticReportHeader(ref exportExcelInfo, criteria, statisticReport, countryList);

                var startIndex = sheetIndex * maxRowPerSheet;
                var endIndex = startIndex + maxRowPerSheet;
                for (var i = startIndex; i < endIndex && i < childReportLength; i++)
                {
                    var dateReport = statisticReport.ChildReports[i];
                    var timeReportLength = dateReport.ChildReports.Count;
                    for (var j = 0; j < timeReportLength; j++)
                    {
                        exportExcelInfo.CurrentColumn = exportExcelInfo.StartColumn;
                        var timeReport = dateReport.ChildReports[j];
                        normalRowWriter.WriteRow(ref exportExcelInfo, timeReport, dateReport, j);
                    }

                    totalRowWriter.WriteRow(ref exportExcelInfo, dateReport.Total);
                }

                firstGrandTotalRowWriter.WriteRow(ref exportExcelInfo, statisticReport.Total);
            }
        }

        /// <summary>
        /// Exports the statistic report header.
        /// </summary>
        /// <param name="exportExcelInfo">The export excel information.</param>
        /// <param name="criteria">The criteria.</param>
        /// <param name="statisticReport">The statistic report.</param>
        /// <param name="countryList">The country list.</param>
        private void ExportStatisticReportHeader(
            ref ExportExcelInfo exportExcelInfo,
            GetStatisticReportCriteria criteria,
            StatisticReportInfo statisticReport,
            List<CountryInfo> countryList)
        {
            NpoiExcelExtensions.SetHeaderColorIndex(ref exportExcelInfo);
            ICellStyle headerStyle = NpoiExcelExtensions.HeaderStyle(
                exportExcelInfo.ExcelFile,
                new ExcelStyleParam());
            ICellStyle leftHeaderStyle = NpoiExcelExtensions.HeaderStyle(
                exportExcelInfo.ExcelFile,
                new ExcelStyleParam { Alignment = CRMHorizontalAlignment.Left });

            var currentColumn = exportExcelInfo.StartColumn;
            exportExcelInfo.Worksheet.SetColumnWidth(currentColumn++, (statisticReport != null ? 160 : 160) * NpoiExcelExtensions.FontWidth);
            exportExcelInfo.Worksheet.SetColumnWidth(currentColumn++, (statisticReport != null ? 100 : 160) * NpoiExcelExtensions.FontWidth);
            for (var i = 0; i < exportExcelInfo.MaxColumnIndex - 1; i++)
            {
                exportExcelInfo.Worksheet.SetColumnWidth(currentColumn++, 120 * NpoiExcelExtensions.FontWidth);
            }

            exportExcelInfo.Worksheet.SetColumnWidth(currentColumn++, (statisticReport != null ? 140 : 160) * NpoiExcelExtensions.FontWidth);
            string reportTitle = statisticReport != null ?
                statisticReport.Name :
                string.Format(
                Labels.StatisticReport_Title,
                SettingService.GetPeriodTypeName(criteria.ReportType),
                DateTimeUtilize.ChangeTimeZoneToClientDateTime(criteria.FromDate).ToShortFormatedDate(),
                DateTimeUtilize.ChangeTimeZoneToClientDateTime(criteria.ToDate).ToShortFormatedDate());
            CellRangeAddress cellRangeAddress;
            cellRangeAddress = new CellRangeAddress(
                exportExcelInfo.CurrentRow,
                exportExcelInfo.CurrentRow + 1,
                exportExcelInfo.StartColumn,
                exportExcelInfo.MaxColumnIndex);
            NpoiExcelExtensions.RenderMergedCells(ref exportExcelInfo, cellRangeAddress, reportTitle, leftHeaderStyle);
            exportExcelInfo.CurrentRow += 2;

            NpoiExcelExtensions.RenderEmptyRow(ref exportExcelInfo, exportExcelInfo.CurrentRow);
            exportExcelInfo.CurrentRow++;

            IRow row = exportExcelInfo.Worksheet.CreateRow(exportExcelInfo.CurrentRow);
            currentColumn = exportExcelInfo.StartColumn;
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, Labels.Date, headerStyle);
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, Labels.Time, headerStyle);
            for (var i = 0; i < countryList.Count; i++)
            {
                var country = countryList[i];
                NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, country.Name, headerStyle);
            }

            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, Labels.Total, headerStyle);
            exportExcelInfo.CurrentRow++;
        }

        /// <summary>
        /// Gets the time report.
        /// </summary>
        /// <param name="timeGroup">The time group.</param>
        /// <param name="countryDictionary">The country dictionary.</param>
        /// <param name="timeReport">The time report.</param>
        /// <param name="statisticReport">The statistic report.</param>
        /// <param name="dateReport">The date report.</param>
        private void GetTimeReport(
            List<StatisticReportEntity> timeGroup,
            Dictionary<int, int> countryDictionary,
            ref StatisticReportInfo timeReport,
            ref StatisticReportInfo statisticReport,
            ref StatisticReportInfo dateReport)
        {
            foreach (var timeData in timeGroup)
            {
                var countryIndex = countryDictionary[timeData.CountryId];
                var reportInfo = new StatisticReportInfo
                {
                    HasData = true,
                    NumberOfIssues = timeData.NumberOfIssues
                };

                if (!timeReport.ChildReports[countryIndex].HasData)
                {
                    timeReport.ChildReports[countryIndex] = reportInfo;
                }
                else
                {
                    timeReport.ChildReports[countryIndex].NumberOfIssues += reportInfo.NumberOfIssues;
                }

                timeReport.NumberOfIssues += reportInfo.NumberOfIssues;
                dateReport.Total.ChildReports[countryIndex].NumberOfIssues += reportInfo.NumberOfIssues;
                statisticReport.Total.ChildReports[countryIndex].NumberOfIssues += reportInfo.NumberOfIssues;
            }

            dateReport.Total.NumberOfIssues += timeReport.NumberOfIssues;
            dateReport.ChildReports.Add(timeReport);
        }
    }
}
