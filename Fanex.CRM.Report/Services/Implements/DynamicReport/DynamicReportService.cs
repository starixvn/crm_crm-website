// --------------------------------------------------------------------------
// <copyright file="DynamicReportService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Services.Implements.DynamicReport
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Drawing;
    using System.Linq;
    using System.Web;
    using Fanex.CRM.Infrastructure.Common;
    using Fanex.CRM.Infrastructure.Core;
    using Fanex.CRM.Infrastructure.Extension;
    using Fanex.CRM.Infrastructure.Logging;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Infrastructure.Service.Implements;
    using Fanex.CRM.Report.Criteria.DynamicReport;
    using Fanex.CRM.Report.Models;
    using Fanex.CRM.Report.Repositories.Implements;
    using Fanex.CRM.Report.Repositories.Interfaces;
    using Fanex.CRM.Report.Resources;
    using Fanex.CRM.Report.Services.Interfaces;
    using Fanex.CRM.Setting.Constants;
    using Fanex.CRM.Setting.Criteria;
    using Fanex.CRM.Setting.Models;
    using Fanex.CRM.Setting.Services.Implements;
    using Fanex.CRM.Setting.Services.Interfaces;
    using NPOI.HSSF.UserModel;
    using NPOI.HSSF.Util;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;

    /// <summary>
    /// Dynamic Report Service.
    /// </summary>
    public class DynamicReportService : IDynamicReportService
    {
        public readonly string DynamicReportSessionKey = "Fanex.CRM.Report.DynamicReport.DynamicReportSessionKey_" +
            (AppSession.CurrentUser != null ? AppSession.CurrentUser.Id : 0);

        public readonly short ClosedBackgroundColorIndex = (short)HSSFColor.DARK_TEAL.index;

        public readonly short CheckedBackgroundColorIndex = (short)HSSFColor.LIGHT_ORANGE.index;

        public readonly short PendingBackgroundColorIndex = (short)HSSFColor.ORANGE.index;

        private IDynamicReportRepository _dynamicReportRepository;

        private ISettingService _serviceSetting;

        public DynamicReportService()
            : this(null, null)
        {
        }

        public DynamicReportService(
            IDynamicReportRepository dynamicReportRepository,
            ISettingService serviceSetting)
        {
            _dynamicReportRepository = dynamicReportRepository ?? new DynamicReportRepository();
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
                    Id = PeriodTypeConstants.Weekly,
                    Name = Labels.Weekly
                });
                allPeriodType.Add(new SettingInfo
                {
                    Id = PeriodTypeConstants.Monthly,
                    Name = Labels.Monthly
                });
                allPeriodType.Add(new SettingInfo
                {
                    Id = PeriodTypeConstants.Yearly,
                    Name = Labels.Yearly
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
        /// Gets all product.
        /// </summary>
        /// <returns>All product.</returns>      
        public List<SettingInfo> GetAllProduct()
        {
            try
            {
                var allProduct = new List<SettingInfo>();
                var rawData = _serviceSetting.GetAllProduct();
                allProduct.Add(new SettingInfo
                {
                    Id = SettingConstants.NotApply,
                    Value = Labels.All
                });

                foreach (var rawProduct in rawData)
                {
                    var product = rawProduct;
                    if (rawProduct.Id == SettingConstants.NotApply)
                    {
                        continue;
                    }

                    allProduct.Add(product);
                }

                return allProduct;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return new List<SettingInfo>();
        }

        /// <summary>
        /// Gets all site.
        /// </summary>
        /// <returns>All site value.</returns>
        public List<SiteInfo> GetAllSite()
        {
            try
            {
                var allSite = _serviceSetting.GetAllSite();
                return allSite;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return new List<SiteInfo>();
        }

        /// <summary>
        /// Gets all status.
        /// </summary>
        /// <returns>All status.</returns>
        public List<SettingInfo> GetAllStatus()
        {
            try
            {
                var allStatus = new List<SettingInfo>();
                var rawData = _serviceSetting.GetAllStatus(new GetAllStatusCriteria
                {
                    ViewType = ViewTypeConstants.CommunicationTool
                });

                allStatus.Add(new SettingInfo
                {
                    Id = SettingConstants.NotApply,
                    Name = Labels.All
                });

                foreach (var rawStatus in rawData)
                {
                    if (rawStatus.Name == Labels.Checked ||
                        rawStatus.Name == Labels.Solved ||
                        rawStatus.Name == Labels.Closed ||
                        rawStatus.Name == Labels.Pending)
                    {
                        allStatus.Add(rawStatus);
                    }
                }

                return allStatus;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return new List<SettingInfo>();
        }

        /// <summary>
        /// Gets all response.
        /// </summary>
        /// <returns>All response.</returns>
        public List<SettingInfo> GetAllResponse()
        {
            try
            {
                var allResponse = new List<SettingInfo>();
                var rawData = _serviceSetting.GetAllResponse();
                allResponse.Add(new SettingInfo
                {
                    Id = SettingConstants.NotApply,
                    Value = Labels.All
                });

                foreach (var rawResponse in rawData)
                {
                    allResponse.Add(rawResponse);
                }

                return allResponse;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return new List<SettingInfo>();
        }

        /// <summary>
        /// Gets all role.
        /// </summary>
        /// <returns>All role value.</returns>
        public List<SettingInfo> GetAllRole()
        {
            try
            {
                var allRole = new List<SettingInfo>();
                var rawData = _serviceSetting.GetAllRole();
                allRole.Add(new SettingInfo
                {
                    Id = SettingConstants.NotApply,
                    Value = Labels.All
                });

                foreach (var rawRole in rawData)
                {
                    allRole.Add(rawRole);
                }

                return allRole;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return new List<SettingInfo>();
        }

        /// <summary>
        /// Gets all communication tool.
        /// </summary>
        /// <returns>All communication tool.</returns>
        public List<SettingInfo> GetAllCommunicationTool()
        {
            try
            {
                var allCommunicationTool = new List<SettingInfo>();
                var rawData = _serviceSetting.GetAllCommunicationTool();
                allCommunicationTool.Add(new SettingInfo
                {
                    Id = SettingConstants.NotApply,
                    Name = Labels.All
                });

                foreach (var rawCommunicationTool in rawData)
                {
                    if (rawCommunicationTool.Id == CommunicationToolConstants.NotApply)
                    {
                        continue;
                    }

                    allCommunicationTool.Add(rawCommunicationTool);
                }

                return allCommunicationTool;
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
        public List<SettingInfo> GetAllMainCategory()
        {
            try
            {
                var allMainCategory = new List<SettingInfo>();
                var rawData = _serviceSetting.GetAllMainCategory();
                allMainCategory.Add(new SettingInfo
                {
                    Id = SettingConstants.NotApply,
                    Name = Labels.All,
                    ChildSettings = new List<SettingInfo>
                    {
                        new SettingInfo
                        {
                            Id = SettingConstants.NotApply,
                            Name = Labels.All,
                        }
                    }
                });

                foreach (var rawMainCategory in rawData)
                {
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
        /// Gets all extension.
        /// </summary>
        /// <returns>All extension.</returns>
        public List<SettingInfo> GetAllExtension()
        {
            try
            {
                var allExtension = new List<SettingInfo>();
                var rawData = _serviceSetting.GetAllExtension();
                allExtension.Add(new SettingInfo
                {
                    Id = ExtensionConstants.NotApply,
                    Value = Labels.All
                });

                foreach (var rawExtension in rawData)
                {
                    if (rawExtension.Id == ExtensionConstants.NotApply)
                    {
                        continue;
                    }

                    allExtension.Add(rawExtension);
                }

                return allExtension;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return new List<SettingInfo>();
        }

        /// <summary>
        /// Gets the dynamic report.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The dynamic report.</returns>      
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public DynamicReportInfo GetDynamicReport(GetDynamicReportCriteria criteria)
        {
            try
            {
                DynamicReportInfo dynamicReport = null;
                if (criteria.IsGetFromCached)
                {
                    dynamicReport = (DynamicReportInfo)SessionExtension.GetSession(HttpContext.Current.Session, DynamicReportSessionKey);
                }

                if (dynamicReport == null)
                {
                    criteria.FromDate = DateTimeUtilize.ChangeClientFromTimeToServerTime(criteria.FromDate);
                    criteria.ToDate = DateTimeUtilize.ChangeClientToTimeToServerTime(criteria.ToDate);
                    var rawData = _dynamicReportRepository.GetDynamicReport(criteria);
                    switch (criteria.ReportType)
                    {
                        case PeriodTypeConstants.Daily:
                            dynamicReport = GetDynamicReportByDaily(rawData, criteria);
                            break;
                        case PeriodTypeConstants.Weekly:
                            dynamicReport = GetDynamicReportByWeekly(rawData, criteria);
                            break;
                        case PeriodTypeConstants.Monthly:
                            dynamicReport = GetDynamicReportByMonthly(rawData, criteria);
                            break;
                        case PeriodTypeConstants.Yearly:
                            dynamicReport = GetDynamicReportByYearly(rawData, criteria);
                            break;
                        default:
                            break;
                    }

                    dynamicReport.ReportTypeId = criteria.ReportType;
                    SessionExtension.AddSession(HttpContext.Current.Session, DynamicReportSessionKey, dynamicReport);
                }

                return new DynamicReportInfo
                {
                    Id = dynamicReport.Id,
                    Name = dynamicReport.Name,
                    ShortName = dynamicReport.ShortName,
                    ChildReports = criteria.IsGetFullReport ?
                        dynamicReport.ChildReports :
                        dynamicReport.ChildReports.Skip(criteria.PageSize * criteria.PageIndex)
                             .Take(criteria.PageSize)
                             .ToList(),
                    TotalChildReports = dynamicReport.TotalChildReports
                };
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return null;
        }

        /// <summary>
        /// Exports the dynamic report.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        public void ExportDynamicReport(GetDynamicReportCriteria criteria)
        {
            try
            {
                criteria.FromDate = DateTimeUtilize.ChangeClientFromTimeToServerTime(criteria.FromDate);
                criteria.ToDate = DateTimeUtilize.ChangeClientToTimeToServerTime(criteria.ToDate);
                DynamicReportInfo dynamicReport = CacheService.Get<DynamicReportInfo>(DynamicReportSessionKey);
                criteria.IsGetFullReport = true;
                if (dynamicReport == null)
                {
                    dynamicReport = GetDynamicReport(criteria);
                }

                ExportExcelInfo exportExcelInfo = new ExportExcelInfo
                {
                    ExcelFile = new HSSFWorkbook(),
                    StartRow = 0,
                    StartColumn = 0,
                    CurrentRow = 0,
                    MaxColumnIndex = 8
                };

                SetStatusColorIndex(ref exportExcelInfo);
                ExportDynamicReportBody(ref exportExcelInfo, criteria, dynamicReport);
                string reportName = string.Format(
                     Labels.DynamicReport_ExcelTitle,
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
        /// Gets the dynamic report by yearly.
        /// </summary>
        /// <param name="rawData">The raw data.</param>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The dynamic report by yearly.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private DynamicReportInfo GetDynamicReportByYearly(
            List<DynamicReportInfo> rawData,
            GetDynamicReportCriteria criteria)
        {
            DynamicReportInfo dynamicReport = new DynamicReportInfo
            {
                Id = DateTimeUtilize.ChangeTimeZoneToClientDateTime(criteria.FromDate).Year.ToString(),
                ChildReports = new List<DynamicReportInfo>(),
                Name = string.Format(
                    Labels.DynamicReport_Title,
                    Labels.Yearly,
                    DateTimeUtilize.ChangeTimeZoneToClientDateTime(criteria.FromDate).ToShortFormatedDate(),
                    DateTimeUtilize.ChangeTimeZoneToClientDateTime(criteria.ToDate).ToShortFormatedDate()),
                ShortName = string.Format(
                    Labels.DynamicReport_ShortTitle,
                    DateTimeUtilize.ChangeTimeZoneToClientDateTime(criteria.FromDate).ToShortFormatedDate(),
                    DateTimeUtilize.ChangeTimeZoneToClientDateTime(criteria.ToDate).ToShortFormatedDate())
            };

            if (rawData == null ||
                rawData.Count == 0)
            {
                return dynamicReport;
            }

            var periodGroups = rawData.GroupBy(data => data.PeriodRange.FromDate.Year);
            foreach (var periodGroup in periodGroups)
            {
                var periodReport = new DynamicReportInfo
                {
                    Name = string.Format(
                        Labels.DynamicReport_PeriodReport_Title,
                        Labels.Yearly,
                        periodGroup.Key),
                    ShortName = periodGroup.Key.ToString(),
                    ChildReports = new List<DynamicReportInfo>(),
                    PeriodRange = new PeriodRange
                    {
                        FromDate = new DateTime(periodGroup.Key, 1, 1),
                        ToDate = new DateTime(periodGroup.Key, 12, 31)
                    }
                };

                periodReport.PeriodRange.FromDateString = periodReport.PeriodRange.FromDate.ToShortFormatedDate();
                periodReport.PeriodRange.ToDateString = periodReport.PeriodRange.ToDate.ToShortFormatedDate();
                foreach (var data in periodGroup)
                {
                    data.PeriodRange.FromDateString = data.PeriodRange.FromDate.ToLongFormatedDateTime();
                    periodReport.ChildReports.Add(data);
                }

                periodReport.TotalChildReports = periodReport.ChildReports.Count;
                dynamicReport.TotalChildReports += periodReport.TotalChildReports;
                dynamicReport.ChildReports.Add(periodReport);
            }

            return dynamicReport;
        }

        /// <summary>
        /// Gets the dynamic report by monthly.
        /// </summary>
        /// <param name="rawData">The raw data.</param>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The dynamic report by monthly.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private DynamicReportInfo GetDynamicReportByMonthly(
            List<DynamicReportInfo> rawData,
            GetDynamicReportCriteria criteria)
        {
            DynamicReportInfo dynamicReport = new DynamicReportInfo
            {
                Id = DateTimeUtilize.ChangeTimeZoneToClientDateTime(criteria.FromDate).ToMonthYear(),
                ChildReports = new List<DynamicReportInfo>(),
                Name = string.Format(
                    Labels.DynamicReport_Title,
                    Labels.Monthly,
                    DateTimeUtilize.ChangeTimeZoneToClientDateTime(criteria.FromDate).ToShortFormatedDate(),
                    DateTimeUtilize.ChangeTimeZoneToClientDateTime(criteria.ToDate).ToShortFormatedDate()),
                ShortName = string.Format(
                    Labels.DynamicReport_ShortTitle,
                    DateTimeUtilize.ChangeTimeZoneToClientDateTime(criteria.FromDate).ToShortFormatedDate(),
                    DateTimeUtilize.ChangeTimeZoneToClientDateTime(criteria.ToDate).ToShortFormatedDate())
            };

            if (rawData == null ||
                rawData.Count == 0)
            {
                return dynamicReport;
            }

            var periodGroups = rawData.GroupBy(data => new DateTime(data.PeriodRange.FromDate.Year, data.PeriodRange.FromDate.Month, 1));
            foreach (var periodGroup in periodGroups)
            {
                var periodReport = new DynamicReportInfo
                {
                    Name = string.Format(
                        Labels.DynamicReport_PeriodReport_Title,
                        Labels.Monthly,
                        periodGroup.Key.ToMonthYear()),
                    ShortName = periodGroup.Key.ToMonthYear(),
                    ChildReports = new List<DynamicReportInfo>(),
                    PeriodRange = new PeriodRange
                    {
                        FromDate = periodGroup.Key,
                        ToDate = periodGroup.Key.LastDayOfMonth()
                    }
                };

                periodReport.PeriodRange.FromDateString = periodReport.PeriodRange.FromDate.ToShortFormatedDate();
                periodReport.PeriodRange.ToDateString = periodReport.PeriodRange.ToDate.ToShortFormatedDate();
                foreach (var data in periodGroup)
                {
                    data.PeriodRange.FromDateString = data.PeriodRange.FromDate.ToLongFormatedDateTime();
                    periodReport.ChildReports.Add(data);
                }

                periodReport.TotalChildReports = periodReport.ChildReports.Count;
                dynamicReport.TotalChildReports += periodReport.TotalChildReports;
                dynamicReport.ChildReports.Add(periodReport);
            }

            return dynamicReport;
        }

        /// <summary>
        /// Gets the dynamic report by weekly.
        /// </summary>
        /// <param name="rawData">The raw data.</param>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The dynamic report by weekly.</returns>
        private DynamicReportInfo GetDynamicReportByWeekly(
            List<DynamicReportInfo> rawData,
            GetDynamicReportCriteria criteria)
        {
            DynamicReportInfo dynamicReport = new DynamicReportInfo
            {
                ChildReports = new List<DynamicReportInfo>(),
                Name = string.Format(
                    Labels.DynamicReport_Title,
                    Labels.Weekly,
                    DateTimeUtilize.ChangeTimeZoneToClientDateTime(criteria.FromDate).ToShortFormatedDate(),
                    DateTimeUtilize.ChangeTimeZoneToClientDateTime(criteria.ToDate).ToShortFormatedDate()),
                ShortName = string.Format(
                    Labels.DynamicReport_ShortTitle,
                    DateTimeUtilize.ChangeTimeZoneToClientDateTime(criteria.FromDate).ToShortFormatedDate(),
                    DateTimeUtilize.ChangeTimeZoneToClientDateTime(criteria.ToDate).ToShortFormatedDate())
            };

            if (rawData == null ||
                rawData.Count == 0)
            {
                return dynamicReport;
            }

            var periodGroups = rawData.GroupBy(data =>
                data.PeriodRange.FromDate.FirstDayOfWeek().ToShortFormatedDate() +
                " - " +
                data.PeriodRange.FromDate.LastDayOfWeek().ToShortFormatedDate());
            foreach (var periodGroup in periodGroups)
            {
                var periodReport = new DynamicReportInfo
                {
                    Name = string.Format(
                        Labels.DynamicReport_PeriodReport_Title,
                        Labels.Weekly,
                        periodGroup.Key),
                    ShortName = periodGroup.Key.ToString(),
                    ChildReports = new List<DynamicReportInfo>()
                };

                foreach (var data in periodGroup)
                {
                    data.PeriodRange.FromDateString = data.PeriodRange.FromDate.ToLongFormatedDateTime();
                    periodReport.ChildReports.Add(data);
                }

                periodReport.TotalChildReports = periodReport.ChildReports.Count;
                dynamicReport.TotalChildReports += periodReport.TotalChildReports;
                dynamicReport.ChildReports.Add(periodReport);
            }

            return dynamicReport;
        }

        /// <summary>
        /// Gets the dynamic report by daily.
        /// </summary>
        /// <param name="rawData">The raw data.</param>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The dynamic report by daily.</returns>
        private DynamicReportInfo GetDynamicReportByDaily(
            List<DynamicReportInfo> rawData,
            GetDynamicReportCriteria criteria)
        {
            DynamicReportInfo dynamicReport = new DynamicReportInfo
            {
                ChildReports = new List<DynamicReportInfo>(),
                Name = string.Format(
                    Labels.DynamicReport_Title,
                    Labels.Daily,
                    DateTimeUtilize.ChangeTimeZoneToClientDateTime(criteria.FromDate).ToShortFormatedDate(),
                    DateTimeUtilize.ChangeTimeZoneToClientDateTime(criteria.ToDate).ToShortFormatedDate()),
                ShortName = string.Format(
                    Labels.DynamicReport_ShortTitle,
                    DateTimeUtilize.ChangeTimeZoneToClientDateTime(criteria.FromDate).ToShortFormatedDate(),
                    DateTimeUtilize.ChangeTimeZoneToClientDateTime(criteria.ToDate).ToShortFormatedDate())
            };

            if (rawData == null ||
                rawData.Count == 0)
            {
                return dynamicReport;
            }

            var periodGroups = rawData.GroupBy(data => data.PeriodRange.FromDate.ToShortFormatedDate());
            foreach (var periodGroup in periodGroups)
            {
                var periodReport = new DynamicReportInfo
                {
                    Name = string.Format(
                        Labels.DynamicReport_PeriodReport_Title,
                        Labels.Daily,
                        periodGroup.Key),
                    ShortName = periodGroup.Key.ToString(),
                    ChildReports = new List<DynamicReportInfo>()
                };

                foreach (var data in periodGroup)
                {
                    data.PeriodRange.FromDateString = data.PeriodRange.FromDate.ToLongFormatedDateTime();
                    periodReport.ChildReports.Add(data);
                }

                periodReport.TotalChildReports = periodReport.ChildReports.Count;
                dynamicReport.TotalChildReports += periodReport.TotalChildReports;
                dynamicReport.ChildReports.Add(periodReport);
            }

            return dynamicReport;
        }

        /// <summary>
        /// Exports the dynamic report header.
        /// </summary>
        /// <param name="exportExcelInfo">The export excel information.</param>
        /// <param name="criteria">The criteria.</param>
        /// <param name="dynamicReport">The dynamic report.</param>
        private void ExportDynamicReportHeader(
            ref ExportExcelInfo exportExcelInfo,
            GetDynamicReportCriteria criteria,
            DynamicReportInfo dynamicReport)
        {
            ICellStyle headerStyle = NpoiExcelExtensions.HeaderStyle(exportExcelInfo.ExcelFile);
            var currentColumn = exportExcelInfo.StartColumn;
            exportExcelInfo.Worksheet.SetColumnWidth(currentColumn++, 80 * NpoiExcelExtensions.FontWidth);
            exportExcelInfo.Worksheet.SetColumnWidth(currentColumn++, 100 * NpoiExcelExtensions.FontWidth);
            exportExcelInfo.Worksheet.SetColumnWidth(currentColumn++, 120 * NpoiExcelExtensions.FontWidth);
            exportExcelInfo.Worksheet.SetColumnWidth(currentColumn++, 300 * NpoiExcelExtensions.FontWidth);
            exportExcelInfo.Worksheet.SetColumnWidth(currentColumn++, 160 * NpoiExcelExtensions.FontWidth);
            exportExcelInfo.Worksheet.SetColumnWidth(currentColumn++, 100 * NpoiExcelExtensions.FontWidth);
            exportExcelInfo.Worksheet.SetColumnWidth(currentColumn++, 80 * NpoiExcelExtensions.FontWidth);
            exportExcelInfo.Worksheet.SetColumnWidth(currentColumn++, 140 * NpoiExcelExtensions.FontWidth);
            exportExcelInfo.Worksheet.SetColumnWidth(currentColumn++, 140 * NpoiExcelExtensions.FontWidth);

            string reportTitle = string.Format(
                Labels.DynamicReport_TitleWithNumberOfLog,
                SettingService.GetPeriodTypeName(criteria.ReportType),
                DateTimeUtilize.ChangeTimeZoneToClientDateTime(criteria.FromDate).ToShortFormatedDate(),
                DateTimeUtilize.ChangeTimeZoneToClientDateTime(criteria.ToDate).ToShortFormatedDate(),
                dynamicReport != null ? "[" + dynamicReport.TotalChildReports + "]" : string.Empty);

            CellRangeAddress cellRangeAddress;
            cellRangeAddress = new CellRangeAddress(
                exportExcelInfo.CurrentRow,
                exportExcelInfo.CurrentRow + 1,
                exportExcelInfo.StartColumn,
                exportExcelInfo.MaxColumnIndex);
            NpoiExcelExtensions.RenderMergedCells(ref exportExcelInfo, cellRangeAddress, reportTitle, headerStyle);
            exportExcelInfo.CurrentRow += 2;

            NpoiExcelExtensions.RenderEmptyRow(ref exportExcelInfo, exportExcelInfo.CurrentRow);
            exportExcelInfo.CurrentRow++;

            IRow row = exportExcelInfo.Worksheet.CreateRow(exportExcelInfo.CurrentRow);
            currentColumn = exportExcelInfo.StartColumn;
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, Labels.Symbol_No, headerStyle);
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, Labels.Running_No, headerStyle);
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, Labels.Feedback_Date, headerStyle);
            NpoiExcelExtensions.RenderCell(
                ref row,
                ref currentColumn,
                string.Format("{0}/{1}", Labels.Main_Category, Labels.Sub_Category),
                headerStyle);
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, Labels.Subject, headerStyle);
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, Labels.Status, headerStyle);
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, Labels.Duration, headerStyle);
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, Labels.Handled_By, headerStyle);
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, Labels.Reviewed_By, headerStyle);
            exportExcelInfo.CurrentRow++;
        }

        /// <summary>
        /// Exports the dynamic report body.
        /// </summary>
        /// <param name="exportExcelInfo">The export excel information.</param>
        /// <param name="criteria">The criteria.</param>
        /// <param name="dynamicReport">The dynamic report.</param>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private void ExportDynamicReportBody(
            ref ExportExcelInfo exportExcelInfo,
            GetDynamicReportCriteria criteria,
            DynamicReportInfo dynamicReport)
        {
            if (dynamicReport == null || dynamicReport.ChildReports.Count == 0)
            {
                exportExcelInfo.Worksheet = exportExcelInfo.ExcelFile.CreateSheet(Labels.DynamicReport_SheetName);
                ExportDynamicReportHeader(ref exportExcelInfo, criteria, dynamicReport);
                NpoiExcelExtensions.RenderNoInfoMessage(ref exportExcelInfo, exportExcelInfo.CurrentRow);
                return;
            }

            var periodGroupBackgroundColor = Color.FromArgb(215, 215, 215);
            var periodGroupBackgroundColorIndex = (short)HSSFColor.DARK_YELLOW.index;
            NpoiExcelExtensions.SetColorAtIndex(
                ref exportExcelInfo,
                periodGroupBackgroundColor,
                periodGroupBackgroundColorIndex);
            ICellStyle periodGroupTitleStyle = NpoiExcelExtensions.TextStyle(
                exportExcelInfo.ExcelFile,
                periodGroupBackgroundColorIndex,
                true,
                NpoiExcelExtensions.Alignment.Left);
            var font = exportExcelInfo.ExcelFile.CreateFont();
            font.FontHeightInPoints = NpoiExcelExtensions.FontSize;
            font.FontName = NpoiExcelExtensions.FontName;
            font.Color = (short)HSSFColor.BLUE.index;
            font.Boldweight = (short)FontBoldWeight.BOLD;
            periodGroupTitleStyle.SetFont(font);
            NpoiExcelExtensions.SetBorderToCell(periodGroupTitleStyle);

            IRow row;
            CellRangeAddress cellRangeAddress;
            var childReportLength = dynamicReport.ChildReports.Count;

            var normalRowWriter = new CommunicationLogRowExcelWriter(
                exportExcelInfo,
                NpoiExcelExtensions.BackgroundColorIndex);
            var closedRowWriter = new CommunicationLogRowExcelWriter(
                exportExcelInfo,
                ClosedBackgroundColorIndex);
            var checkedRowWriter = new CommunicationLogRowExcelWriter(
                exportExcelInfo,
                CheckedBackgroundColorIndex);
            var pendingRowWriter = new CommunicationLogRowExcelWriter(
                exportExcelInfo,
                PendingBackgroundColorIndex);

            var maxRowPerSheet = NpoiExcelExtensions.MaxRowPerSheet;
            var numberOfSheet = (childReportLength / maxRowPerSheet) +
                (childReportLength % maxRowPerSheet != 0 ? 1 : 0);
            for (var sheetIndex = 0; sheetIndex < numberOfSheet; sheetIndex++)
            {
                var sheetName = numberOfSheet > 1 ?
                    string.Format("{0} - {1}", Labels.DynamicReport_SheetName, sheetIndex + 1) :
                    Labels.DynamicReport_SheetName;

                exportExcelInfo.Worksheet = exportExcelInfo.ExcelFile.CreateSheet(sheetName);
                exportExcelInfo.CurrentRow = 0;
                ExportDynamicReportHeader(ref exportExcelInfo, criteria, dynamicReport);
                var startIndex = sheetIndex * maxRowPerSheet;
                var endIndex = startIndex + maxRowPerSheet;
                for (var i = startIndex; i < endIndex && i < childReportLength; i++)
                {
                    var periodReport = dynamicReport.ChildReports[i];
                    var childsLength = periodReport.ChildReports.Count;

                    row = exportExcelInfo.Worksheet.CreateRow(exportExcelInfo.CurrentRow);
                    exportExcelInfo.CurrentColumn = exportExcelInfo.StartColumn;
                    cellRangeAddress = new CellRangeAddress(
                        exportExcelInfo.CurrentRow,
                        exportExcelInfo.CurrentRow + 1,
                        exportExcelInfo.StartColumn,
                        exportExcelInfo.MaxColumnIndex);
                    NpoiExcelExtensions.RenderMergedCells(
                        ref exportExcelInfo,
                        cellRangeAddress,
                        string.Format("{0} [{1}]", periodReport.Name, childsLength),
                        periodGroupTitleStyle);
                    exportExcelInfo.CurrentRow += 2;
                    for (var j = 0; j < childsLength; j++)
                    {
                        row = exportExcelInfo.Worksheet.CreateRow(exportExcelInfo.CurrentRow);
                        exportExcelInfo.CurrentColumn = exportExcelInfo.StartColumn;
                        var info = periodReport.ChildReports[j];

                        if (string.Equals(info.StatusName, Labels.Closed, StringComparison.OrdinalIgnoreCase))
                        {
                            closedRowWriter.WriteRow(ref exportExcelInfo, info, j);
                        }
                        else if (string.Equals(info.StatusName, Labels.Checked, StringComparison.OrdinalIgnoreCase) ||
                            string.Equals(info.StatusName, Labels.Solved, StringComparison.OrdinalIgnoreCase))
                        {
                            checkedRowWriter.WriteRow(ref exportExcelInfo, info, j);
                        }
                        else if (string.Equals(info.StatusName, Labels.Pending, StringComparison.OrdinalIgnoreCase))
                        {
                            pendingRowWriter.WriteRow(ref exportExcelInfo, info, j);
                        }
                        else
                        {
                            normalRowWriter.WriteRow(ref exportExcelInfo, info, j);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Sets the index of the status color.
        /// </summary>
        /// <param name="exportExcelInfo">The export excel information.</param>
        private void SetStatusColorIndex(ref ExportExcelInfo exportExcelInfo)
        {
            var closedBackgroundColor = Color.FromArgb(221, 221, 221);
            NpoiExcelExtensions.SetColorAtIndex(
                ref exportExcelInfo,
                closedBackgroundColor,
                ClosedBackgroundColorIndex);

            var checkedBackgroundColor = Color.FromArgb(242, 242, 242);
            NpoiExcelExtensions.SetColorAtIndex(
                ref exportExcelInfo,
                checkedBackgroundColor,
                CheckedBackgroundColorIndex);

            var pendingBackgroundColor = Color.FromArgb(240, 237, 196);
            NpoiExcelExtensions.SetColorAtIndex(
                ref exportExcelInfo,
                pendingBackgroundColor,
                PendingBackgroundColorIndex);
        }
    }
}
