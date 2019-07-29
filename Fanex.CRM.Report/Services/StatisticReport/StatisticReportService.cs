namespace Fanex.CRM.Report.Services.StatisticReport
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CRM.Common.Enumerations;
    using Fanex.CRM.Common.Extensions;
    using Fanex.CRM.Common.Helpers;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Common.Models.Npoi;
    using Fanex.CRM.Report.Criteria.StatisticReport;
    using Fanex.CRM.Report.Entities.StatisticReport;
    using Fanex.CRM.Report.Models.StatisticReport;
    using Fanex.CRM.Report.Repositories.StatisticReport;
    using Fanex.CRM.Report.Resources;
    using Fanex.CRM.Setting.Constants;
    using Fanex.CRM.Setting.Services;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;

    public class StatisticReportService : IStatisticReportService
    {
        private readonly ISettingService _serviceSetting;
        private readonly IStatisticReportRepository _statisticReportRepository;

        public StatisticReportService()
            : this(null, null)
        {
        }

        public StatisticReportService(IStatisticReportRepository statisticReportRepository, ISettingService serviceSetting)
        {
            _serviceSetting = serviceSetting ?? new SettingService();
            _statisticReportRepository = statisticReportRepository ?? new StatisticReportRepository();
        }

        public List<Setting> AllPeriodType()
        {
            try
            {
                var allPeriodType = new List<Setting>
                {
                    new Setting
                    {
                        Id = PeriodTypeConstants.Daily,
                        Name = Labels.Daily
                    },
                    new Setting
                    {
                        Id = PeriodTypeConstants.Monthly,
                        Name = Labels.Monthly
                    }
                };

                return allPeriodType;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        public List<Setting> AllMainCategory()
        {
            try
            {
                var allMainCategory = new List<Setting>();
                var rawData = _serviceSetting.GetAllMainCategories();
                var allSubSettingOption = new Setting
                {
                    Id = SettingConstants.AllSetting,
                    Name = Labels.All,
                };

                var remindSubSettingOption = new Setting
                {
                    Id = SettingConstants.NotApply,
                    Name = Labels.Please_choose_one,
                };

                allMainCategory.Add(new Setting
                {
                    Id = SettingConstants.NotApply,
                    Name = Labels.Please_choose_one,
                    ChildSettings = new List<Setting>
                    {
                       remindSubSettingOption
                    }
                });

                allMainCategory.Add(new Setting
                {
                    Id = SettingConstants.AllSetting,
                    Name = Labels.All,
                    ChildSettings = new List<Setting>
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
                throw;
            }
        }

        public StatisticReportInfo StatisticReport(GetStatisticReportCriteria criteria)
        {
            StatisticReportInfo statisticReport;
            criteria.FromDate = DateTimeUtilize.ChangeClientFromTimeToServerTime(criteria.FromDate);
            criteria.ToDate = DateTimeUtilize.ChangeClientToTimeToServerTime(criteria.ToDate);
            var rawData = _statisticReportRepository.GetStatisticReport(criteria);

            var countryList = rawData
                .GroupBy(statisticReportEntity => statisticReportEntity.CountryId)
                .Select(group => new Country
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
                    throw new ArgumentException("Invalid ReportType");
            }

            statisticReport.ReportTypeId = criteria.ReportType;
            statisticReport.TotalChildReports = statisticReport.ChildReports.Count;
            statisticReport.ChartInfo = GetChartInfo(statisticReport, countryDictionary);

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

        public void ExportStatisticReport(GetStatisticReportCriteria criteria)
        {
            string reportName = string.Format(
                 Labels.StatisticReport_ExcelTitle,
                 SettingService.GetPeriodTypeName(criteria.ReportType),
                 criteria.FromDate.ToShortFormatedDateWithSlash().Replace("/", "_"),
                 criteria.ToDate.ToShortFormatedDateWithSlash().Replace("/", "_"));

            criteria.IsGetFullReport = true;
            var statisticReport = StatisticReport(criteria);

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

            NpoiExtensions.ResponseStreamExcel(exportExcelInfo.ExcelFile, reportName);
        }

        private StatisticReportInfo GetStatisticReportByMonthly(
            IList<StatisticReportEntity> rawData,
            IList<Country> countryList,
            IDictionary<int, int> countryDictionary,
            GetStatisticReportCriteria criteria)
        {
            var countryLength = countryDictionary.Count;
            var fromDateClient = DateTimeUtilize.ChangeTimeZoneToClientDateTime(criteria.FromDate).ToShortFormatedDate();
            var toDateClient = DateTimeUtilize.ChangeTimeZoneToClientDateTime(criteria.ToDate).ToShortFormatedDate();

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

        private StatisticReportInfo GetStatisticReportByDaily(
            IList<StatisticReportEntity> rawData,
            IList<Country> countryList,
            IDictionary<int, int> countryDictionary,
            GetStatisticReportCriteria criteria)
        {
            var countryLength = countryDictionary.Count;
            var fromDateClient = DateTimeUtilize.ChangeTimeZoneToClientDateTime(criteria.FromDate).ToShortFormatedDate();
            var toDateClient = DateTimeUtilize.ChangeTimeZoneToClientDateTime(criteria.ToDate).ToShortFormatedDate();

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

            if (!rawData.Any())
            {
                return statisticReport;
            }

            var dateGroups = rawData.GroupBy(data => data.DateTime.ToShortFormatedDate());
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

        private IList<StatisticReportInfo> GetPagingLogList(
            GetStatisticReportCriteria criteria,
            IList<StatisticReportInfo> statisticReportList)
        {
            return statisticReportList.Skip(criteria.PageSize * criteria.PageIndex)
                .Take(criteria.PageSize)
                .ToList();
        }

        private IDictionary<int, int> GetCountryDictionary(IList<Country> countryList)
        {
            var countryDictionary = new Dictionary<int, int>();
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

        private IList<StatisticReportInfo> GetDummyCountryList(
            int countryLength,
            IList<Country> countryList,
            bool isGrandTotal)
        {
            var dummyCountries = new List<StatisticReportInfo>();
            for (var i = 0; i < countryLength; i++)
            {
                dummyCountries.Add(new StatisticReportInfo
                {
                    CountryInfo = isGrandTotal ? countryList[i] : null
                });
            }

            return dummyCountries;
        }

        private StatisticReportInfo GetChartInfo(StatisticReportInfo statisticReport, IDictionary<int, int> countryDictionary)
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

        private void ExportStatisticReportBody(
            ref ExportExcelInfo exportExcelInfo,
            GetStatisticReportCriteria criteria,
            StatisticReportInfo statisticReport)
        {
            if (statisticReport == null || statisticReport.ChildReports.Count == 0)
            {
                exportExcelInfo.Worksheet = exportExcelInfo.ExcelFile.CreateSheet(Labels.StatisticReport_SheetName);
                ExportStatisticReportHeader(ref exportExcelInfo, criteria, statisticReport, null);
                NpoiExtensions.RenderNoInfoMessage(exportExcelInfo, exportExcelInfo.CurrentRow);
                return;
            }

            var countryLength = statisticReport.Total.ChildReports.Count;
            List<Country> countryList = new List<Country>();
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
            NpoiExtensions.SetTotalColorIndex(exportExcelInfo);
            NpoiExtensions.SetGrandTotalColorIndex(exportExcelInfo);

            var maxRowPerSheet = NpoiExtensions.MaxRowPerSheet;
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
                        normalRowWriter.WriteRow(exportExcelInfo, timeReport, dateReport, j);
                    }

                    totalRowWriter.WriteRow(exportExcelInfo, dateReport.Total);
                }

                firstGrandTotalRowWriter.WriteRow(exportExcelInfo, statisticReport.Total);
            }
        }

        private void ExportStatisticReportHeader(
            ref ExportExcelInfo exportExcelInfo,
            GetStatisticReportCriteria criteria,
            StatisticReportInfo statisticReport,
            IList<Country> countryList)
        {
            NpoiExtensions.SetHeaderColorIndex(exportExcelInfo);
            ICellStyle headerStyle = NpoiExtensions.GetHeaderStyle(exportExcelInfo.ExcelFile, new ExcelStyleParam());
            ICellStyle leftHeaderStyle = NpoiExtensions.GetHeaderStyle(exportExcelInfo.ExcelFile, new ExcelStyleParam { Alignment = CRMHorizontalAlignment.Left });

            var currentColumn = exportExcelInfo.StartColumn;
            exportExcelInfo.Worksheet.SetColumnWidth(currentColumn++, (statisticReport != null ? 160 : 160) * NpoiExtensions.FontWidth);
            exportExcelInfo.Worksheet.SetColumnWidth(currentColumn++, (statisticReport != null ? 100 : 160) * NpoiExtensions.FontWidth);
            for (var i = 0; i < exportExcelInfo.MaxColumnIndex - 1; i++)
            {
                exportExcelInfo.Worksheet.SetColumnWidth(currentColumn++, 120 * NpoiExtensions.FontWidth);
            }

            exportExcelInfo.Worksheet.SetColumnWidth(currentColumn++, (statisticReport != null ? 140 : 160) * NpoiExtensions.FontWidth);
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
            NpoiExtensions.RenderMergedCells(exportExcelInfo, cellRangeAddress, reportTitle, leftHeaderStyle);
            exportExcelInfo.CurrentRow += 2;

            NpoiExtensions.RenderEmptyRow(exportExcelInfo, exportExcelInfo.CurrentRow);
            exportExcelInfo.CurrentRow++;

            IRow row = exportExcelInfo.Worksheet.CreateRow(exportExcelInfo.CurrentRow);
            currentColumn = exportExcelInfo.StartColumn;
            NpoiExtensions.RenderCell(row, currentColumn, Labels.Date, headerStyle);
            currentColumn++;
            NpoiExtensions.RenderCell(row, currentColumn, Labels.Time, headerStyle);
            currentColumn++;
            for (var i = 0; i < countryList.Count; i++)
            {
                var country = countryList[i];
                NpoiExtensions.RenderCell(row, currentColumn, country.Name, headerStyle);
                currentColumn++;
            }

            NpoiExtensions.RenderCell(row, currentColumn, Labels.Total, headerStyle);
            currentColumn++;
            exportExcelInfo.CurrentRow++;
        }

        private void GetTimeReport(
            IList<StatisticReportEntity> timeGroup,
            IDictionary<int, int> countryDictionary,
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