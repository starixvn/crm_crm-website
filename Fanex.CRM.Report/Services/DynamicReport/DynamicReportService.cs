namespace Fanex.CRM.Report.Services.DynamicReport
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using Common.Services;
    using Fanex.CRM.Common.Enumerations;
    using Fanex.CRM.Common.Extensions;
    using Fanex.CRM.Common.Helpers;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Common.Models.Npoi;
    using Fanex.CRM.Report.Criteria.DynamicReport;
    using Fanex.CRM.Report.Models;
    using Fanex.CRM.Report.Repositories.DynamicReport;
    using Fanex.CRM.Report.Resources;
    using Fanex.CRM.Setting.Constants;
    using Fanex.CRM.Setting.Models;
    using Fanex.CRM.Setting.Services;
    using NPOI.HSSF.UserModel;
    using NPOI.HSSF.Util;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;

    public class DynamicReportService : IDynamicReportService
    {
        private readonly short _closedBackgroundColorIndex = HSSFColor.DarkTeal.Index;
        private readonly short _checkedBackgroundColorIndex = HSSFColor.LightOrange.Index;
        private readonly short _pendingBackgroundColorIndex = HSSFColor.Orange.Index;

        private readonly IDynamicReportRepository _dynamicReportRepository;
        private readonly ISettingService _serviceSetting;

        private ICellStyle periodGroupTitleStyle;

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

        public IEnumerable<Setting> GetAllPeriodTypes()
        {
            var types = new List<Setting>();
            types.Add(new Setting
            {
                Id = PeriodTypeConstants.Daily,
                Name = Labels.Daily
            });
            types.Add(new Setting
            {
                Id = PeriodTypeConstants.Weekly,
                Name = Labels.Weekly
            });
            types.Add(new Setting
            {
                Id = PeriodTypeConstants.Monthly,
                Name = Labels.Monthly
            });
            types.Add(new Setting
            {
                Id = PeriodTypeConstants.Yearly,
                Name = Labels.Yearly
            });

            return types;
        }

        public IEnumerable<Product> GetAllSubProducts()
        {
            var subProducts = _serviceSetting.GetAllSubProducts();
            var notApplyItem = subProducts.FirstOrDefault(x => x.Id == SettingConstants.NotApply);

            if (notApplyItem == null)
            {
                subProducts.Insert(
                    0,
                    new Product
                    {
                        Id = SettingConstants.NotApply,
                        Value = Labels.All
                    });
            }
            else
            {
                notApplyItem.Value = Labels.All;
            }

            return subProducts;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            var products = _serviceSetting.GetAllProducts();
            var itemProductAll = CreateItemAll();

            products.Insert(0, itemProductAll);

            return products;
        }

        private static Product CreateItemAll()
        {
            return new Product
            {
                Id = 0,
                Value = Labels.All
            };
        }

        public IEnumerable<SiteInfo> GetAllSites()
        {
            return _serviceSetting.GetAllSites();
        }

        public IEnumerable<BaseModel> GetAllStatuses()
        {
            var rawData = _serviceSetting.GetAllStatuses(ViewTypeConstants.CommunicationTool)
                .Where(s => !string.Equals(s.Name, "Deleted", StringComparison.OrdinalIgnoreCase))
                .ToList();

            rawData.Insert(
                0,
                new BaseModel
                {
                    Id = SettingConstants.NotApply,
                    Name = Labels.All
                });

            return rawData;
        }

        public IEnumerable<SettingRelation> GetAllResponses()
        {
            var data = _serviceSetting.GetAllResponses();
            data.Insert(
                0,
                new SettingRelation
                {
                    Id = SettingConstants.NotApply,
                    Value = Labels.All
                });

            return data;
        }

        public IEnumerable<SettingRelation> GetAllRoles()
        {
            var data = _serviceSetting.GetAllRoles();
            data.Insert(
                0,
                new SettingRelation
                {
                    Id = SettingConstants.NotApply,
                    Value = Labels.All
                });

            return data;
        }

        public IEnumerable<Setting> GetAllCommunicationTools()
        {
            var rawData = _serviceSetting.GetAllCommunicationTools();
            var item = rawData.FirstOrDefault(x => x.Id == SettingConstants.NotApply);

            if (item == null)
            {
                rawData.Insert(
                    0,
                    new Setting
                    {
                        Id = SettingConstants.NotApply,
                        Name = Labels.All
                    });
            }
            else
            {
                item.Name = Labels.All;
            }

            return rawData;
        }

        public IEnumerable<Setting> GetAllMainCategories()
        {
            var allMainCategory = new List<Setting>();
            var rawData = _serviceSetting.GetAllMainCategories();
            var allSubSettingOption = new Setting
            {
                Id = SettingConstants.NotApply,
                Name = Labels.All,
            };

            allMainCategory.Add(new Setting
            {
                Id = SettingConstants.NotApply,
                Name = Labels.All,
                ChildSettings = new List<Setting>
                    {
                       allSubSettingOption
                    }
            });

            foreach (var rawMainCategory in rawData)
            {
                rawMainCategory.ChildSettings.Insert(0, allSubSettingOption);
                allMainCategory.Add(rawMainCategory);
            }

            return allMainCategory;
        }

        public IEnumerable<SettingRelation> GetAllExtensions()
        {
            var extensions = new List<SettingRelation>();
            var rawData = _serviceSetting.GetAllExtensions();
            extensions.Add(new SettingRelation
            {
                Id = NAConstants.GetExtentionNA(AppSession.CurrentUser.UserRole),
                Value = Labels.All
            });

            foreach (var rawExtension in rawData)
            {
                if (rawExtension.Id == NAConstants.GetExtentionNA(AppSession.CurrentUser.UserRole))
                {
                    continue;
                }

                extensions.Add(rawExtension);
            }

            return extensions;
        }

        public DynamicReportInfo GetDynamicReport(GetDynamicReportCriteria criteria)
        {
            criteria.FromDate = DateTimeUtilize.ChangeClientFromTimeToServerTime(criteria.FromDate);
            criteria.ToDate = DateTimeUtilize.ChangeClientToTimeToServerTime(criteria.ToDate);
            var dynamicReport = GetDynamicReportData(criteria);

            var chartInfo = new DynamicReportInfo
            {
                Id = dynamicReport.Id,
                Name = dynamicReport.Name,
                ShortName = dynamicReport.ShortName,
                TotalChildReports = dynamicReport.TotalChildReports,
                ChildReports = new List<DynamicReportInfo>()
            };

            var childReportLength = dynamicReport.ChildReports.Count;
            for (var periodIndex = 0; periodIndex < childReportLength; periodIndex++)
            {
                var periodReport = dynamicReport.ChildReports[periodIndex];
                chartInfo.ChildReports.Add(new DynamicReportInfo
                {
                    Id = periodReport.Id,
                    Name = periodReport.Name,
                    ShortName = periodReport.ShortName,
                    TotalChildReports = periodReport.TotalChildReports
                });
            }

            return new DynamicReportInfo
            {
                Id = dynamicReport.Id,
                Name = dynamicReport.Name,
                ShortName = dynamicReport.ShortName,
                ChildReports = criteria.IsGetFullReport ?
                   dynamicReport.ChildReports :
                   GetPagingLogList(criteria, dynamicReport.ChildReports),
                TotalChildReports = dynamicReport.TotalChildReports,
                ChartInfo = chartInfo
            };
        }

        public void ExportDynamicReport(GetDynamicReportCriteria criteria, IEnumerable<Setting> mainCategories)
        {
            string reportName = string.Format(
                 Labels.DynamicReport_ExcelTitle,
                 SettingService.GetPeriodTypeName(criteria.ReportType),
                 criteria.FromDate.ToShortFormatedDateWithSlash().Replace("/", "_"),
                 criteria.ToDate.ToShortFormatedDateWithSlash().Replace("/", "_"));

            criteria.IsGetFullReport = true;
            var dynamicReport = GetDynamicReport(criteria);

            var exportExcelInfo = new ExportExcelInfo
            {
                ExcelFile = new HSSFWorkbook(),
                StartRow = 0,
                StartColumn = 0,
                CurrentRow = 0,
                MaxColumnIndex = 9
            };

            SetStatusColorIndex(ref exportExcelInfo);
            ExportDynamicReportBody(ref exportExcelInfo, criteria, dynamicReport, mainCategories);

            NpoiExtensions.ResponseStreamExcel(exportExcelInfo.ExcelFile, reportName);
        }

        private DynamicReportInfo GetDynamicReportData(GetDynamicReportCriteria criteria)
        {
            var rawData = _dynamicReportRepository.GetDynamicReport(criteria);
            DynamicReportInfo dynamicReport;
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
                    throw new ArgumentException("Invalid report type.");
            }

            dynamicReport.ReportTypeId = criteria.ReportType;
            dynamicReport.TotalChildReports = rawData.Count();
            return dynamicReport;
        }

        private DynamicReportInfo GetDynamicReportByYearly(
            IEnumerable<DynamicReportInfo> rawData,
            GetDynamicReportCriteria criteria)
        {
            var dynamicReport = new DynamicReportInfo
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

            if (!rawData.Any())
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

        private DynamicReportInfo GetDynamicReportByMonthly(
            IEnumerable<DynamicReportInfo> rawData,
            GetDynamicReportCriteria criteria)
        {
            var dynamicReport = new DynamicReportInfo
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

            if (!rawData.Any())
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

        private DynamicReportInfo GetDynamicReportByWeekly(
            IEnumerable<DynamicReportInfo> rawData,
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

            if (!rawData.Any())
            {
                return dynamicReport;
            }

            var periodGroups = rawData.GroupBy(data =>
                data.PeriodRange.FromDate.FirstDayOfWeek().ToShortFormatedDate() +
                " - " +
                data.PeriodRange.FromDate.LastDayOfWeek().ToShortFormatedDate());
            GetDynamicReportInfoByGroup(periodGroups, ref dynamicReport, Labels.Weekly);
            return dynamicReport;
        }

        private DynamicReportInfo GetDynamicReportByDaily(
            IEnumerable<DynamicReportInfo> rawData,
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

            if (!rawData.Any())
            {
                return dynamicReport;
            }

            var periodGroups = rawData.GroupBy(data => data.PeriodRange.FromDate.ToShortFormatedDate());
            GetDynamicReportInfoByGroup(periodGroups, ref dynamicReport, Labels.Daily);
            return dynamicReport;
        }

        private static void ExportDynamicReportHeader(
            ref ExportExcelInfo exportExcelInfo,
            GetDynamicReportCriteria criteria,
            DynamicReportInfo dynamicReport)
        {
            NpoiExtensions.SetHeaderColorIndex(exportExcelInfo);
            ICellStyle headerStyle = NpoiExtensions.GetHeaderStyle(
                exportExcelInfo.ExcelFile,
                new ExcelStyleParam());
            var currentColumn = exportExcelInfo.StartColumn;
            exportExcelInfo.Worksheet.SetColumnWidth(currentColumn++, 80 * NpoiExtensions.FontWidth);
            exportExcelInfo.Worksheet.SetColumnWidth(currentColumn++, 100 * NpoiExtensions.FontWidth);
            exportExcelInfo.Worksheet.SetColumnWidth(currentColumn++, 120 * NpoiExtensions.FontWidth);
            exportExcelInfo.Worksheet.SetColumnWidth(currentColumn++, 300 * NpoiExtensions.FontWidth);
            exportExcelInfo.Worksheet.SetColumnWidth(currentColumn++, 160 * NpoiExtensions.FontWidth);
            exportExcelInfo.Worksheet.SetColumnWidth(currentColumn++, 100 * NpoiExtensions.FontWidth);
            exportExcelInfo.Worksheet.SetColumnWidth(currentColumn++, 80 * NpoiExtensions.FontWidth);
            exportExcelInfo.Worksheet.SetColumnWidth(currentColumn++, 140 * NpoiExtensions.FontWidth);
            exportExcelInfo.Worksheet.SetColumnWidth(currentColumn++, 140 * NpoiExtensions.FontWidth);
            exportExcelInfo.Worksheet.SetColumnWidth(currentColumn++, 140 * NpoiExtensions.FontWidth);

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
            NpoiExtensions.RenderMergedCells(exportExcelInfo, cellRangeAddress, reportTitle, headerStyle);
            exportExcelInfo.CurrentRow += 2;

            NpoiExtensions.RenderEmptyRow(exportExcelInfo, exportExcelInfo.CurrentRow);
            exportExcelInfo.CurrentRow++;

            IRow row = exportExcelInfo.Worksheet.CreateRow(exportExcelInfo.CurrentRow);
            currentColumn = exportExcelInfo.StartColumn;
            NpoiExtensions.RenderCell(row, currentColumn, Labels.Symbol_No, headerStyle);
            currentColumn++;
            NpoiExtensions.RenderCell(row, currentColumn, Labels.Running_No, headerStyle);
            currentColumn++;
            NpoiExtensions.RenderCell(row, currentColumn, Labels.Feedback_Date, headerStyle);
            currentColumn++;
            NpoiExtensions.RenderCell(
                row,
                currentColumn,
                string.Format("{0}/{1}", Labels.Main_Category, Labels.Sub_Category),
                headerStyle);
            currentColumn++;
            NpoiExtensions.RenderCell(row, currentColumn, Labels.Subject, headerStyle);
            currentColumn++;
            NpoiExtensions.RenderCell(row, currentColumn, Labels.Status, headerStyle);
            currentColumn++;
            NpoiExtensions.RenderCell(row, currentColumn, Labels.Duration, headerStyle);
            currentColumn++;
            NpoiExtensions.RenderCell(row, currentColumn, Labels.Username, headerStyle);
            currentColumn++;
            NpoiExtensions.RenderCell(row, currentColumn, Labels.Handler, headerStyle);
            currentColumn++;
            NpoiExtensions.RenderCell(row, currentColumn, Labels.Reviewer, headerStyle);
            currentColumn++;
            exportExcelInfo.CurrentRow++;
        }

        private void ExportDynamicReportBody(
            ref ExportExcelInfo exportExcelInfo,
            GetDynamicReportCriteria criteria,
            DynamicReportInfo dynamicReport,
            IEnumerable<Setting> mainCategories)
        {
            if (dynamicReport == null || dynamicReport.ChildReports.Count == 0)
            {
                exportExcelInfo.Worksheet = exportExcelInfo.ExcelFile.CreateSheet(Labels.DynamicReport_SheetName);
                ExportDynamicReportHeader(ref exportExcelInfo, criteria, dynamicReport);
                NpoiExtensions.RenderNoInfoMessage(exportExcelInfo, exportExcelInfo.CurrentRow);
                return;
            }

            InitPeriodGroupTitleStyle(exportExcelInfo);

            CellRangeAddress cellRangeAddress;

            var normalRowWriter = new CommunicationLogRowExcelWriter(
                exportExcelInfo,
                NpoiExtensions.BackgroundColorIndex);
            var closedRowWriter = new CommunicationLogRowExcelWriter(
                exportExcelInfo,
                _closedBackgroundColorIndex);
            var checkedRowWriter = new CommunicationLogRowExcelWriter(
                exportExcelInfo,
                _checkedBackgroundColorIndex);
            var pendingRowWriter = new CommunicationLogRowExcelWriter(
                exportExcelInfo,
                _pendingBackgroundColorIndex);

            var totalPeriodGroupTitle = dynamicReport.ChildReports.Count;
            var totalItems = dynamicReport.TotalChildReports + totalPeriodGroupTitle;
            var numberOfSheet = (totalItems / NpoiExtensions.MaxRowPerSheet) + (totalItems % NpoiExtensions.MaxRowPerSheet != 0 ? 1 : 0);

            var mainCategoryCount = dynamicReport.ChildReports.Count;

            var currentIndexOfMainCategory = 0;
            var currentIndexOfDetailItem = 0;
            var currentExportedItemCount = 0;
            for (var sheetIndex = 0; sheetIndex < numberOfSheet; sheetIndex++)
            {
                var sheetName = numberOfSheet > 1 ?
                    string.Format("{0} - {1}", Labels.DynamicReport_SheetName, sheetIndex + 1) :
                    Labels.DynamicReport_SheetName;

                exportExcelInfo.Worksheet = exportExcelInfo.ExcelFile.CreateSheet(sheetName);
                exportExcelInfo.CurrentRow = 0;
                ExportDynamicReportHeader(ref exportExcelInfo, criteria, dynamicReport);

                currentExportedItemCount = 0;

                for (var i = currentIndexOfMainCategory; i < mainCategoryCount && currentExportedItemCount < NpoiExtensions.MaxRowPerSheet; i++)
                {
                    var periodReport = dynamicReport.ChildReports[i];
                    var childsLength = periodReport.ChildReports.Count;

                    exportExcelInfo.Worksheet.CreateRow(exportExcelInfo.CurrentRow);
                    exportExcelInfo.CurrentColumn = exportExcelInfo.StartColumn;
                    cellRangeAddress = new CellRangeAddress(
                        exportExcelInfo.CurrentRow,
                        exportExcelInfo.CurrentRow + 1,
                        exportExcelInfo.StartColumn,
                        exportExcelInfo.MaxColumnIndex);
                    NpoiExtensions.RenderMergedCells(
                        exportExcelInfo,
                        cellRangeAddress,
                        string.Format("{0} [{1}]", periodReport.Name, childsLength),
                        periodGroupTitleStyle);
                    exportExcelInfo.CurrentRow += 2;
                    currentExportedItemCount++;

                    for (var j = currentIndexOfDetailItem; j < childsLength && currentExportedItemCount <= NpoiExtensions.MaxRowPerSheet; j++)
                    {
                        exportExcelInfo.Worksheet.CreateRow(exportExcelInfo.CurrentRow);
                        exportExcelInfo.CurrentColumn = exportExcelInfo.StartColumn;
                        var info = periodReport.ChildReports[j];

                        if (string.Equals(info.StatusName, Labels.Closed, StringComparison.OrdinalIgnoreCase))
                        {
                            closedRowWriter.WriteRow(exportExcelInfo, info, j, mainCategories);
                        }
                        else if (string.Equals(info.StatusName, Labels.Checked, StringComparison.OrdinalIgnoreCase) ||
                            string.Equals(info.StatusName, Labels.Solved, StringComparison.OrdinalIgnoreCase))
                        {
                            checkedRowWriter.WriteRow(exportExcelInfo, info, j, mainCategories);
                        }
                        else if (string.Equals(info.StatusName, Labels.Pending, StringComparison.OrdinalIgnoreCase))
                        {
                            pendingRowWriter.WriteRow(exportExcelInfo, info, j, mainCategories);
                        }
                        else
                        {
                            normalRowWriter.WriteRow(exportExcelInfo, info, j, mainCategories);
                        }

                        currentIndexOfDetailItem++;
                        currentExportedItemCount++;
                    }

                    if (currentIndexOfDetailItem == childsLength)
                    {
                        currentIndexOfDetailItem = 0;
                        currentIndexOfMainCategory++;
                    }
                }
            }
        }

        private void InitPeriodGroupTitleStyle(ExportExcelInfo exportExcelInfo)
        {
            var periodGroupBackgroundColor = Color.FromArgb(215, 215, 215);
            const short periodGroupBackgroundColorIndex = HSSFColor.DarkYellow.Index;
            NpoiExtensions.SetColorAtIndex(exportExcelInfo, periodGroupBackgroundColor, periodGroupBackgroundColorIndex);
            periodGroupTitleStyle = NpoiExtensions.GetTextStyle(
                exportExcelInfo.ExcelFile,
                periodGroupBackgroundColorIndex,
                true,
                CRMHorizontalAlignment.Left);
            var font = exportExcelInfo.ExcelFile.CreateFont();
            font.FontHeightInPoints = NpoiExtensions.FontSize;
            font.FontName = NpoiExtensions.FontName;
            font.Color = HSSFColor.Blue.Index;
            font.Boldweight = (short)FontBoldWeight.Bold;
            periodGroupTitleStyle.SetFont(font);
            NpoiExtensions.SetBorderToCell(periodGroupTitleStyle);
        }

        private void SetStatusColorIndex(ref ExportExcelInfo exportExcelInfo)
        {
            var closedBackgroundColor = Color.FromArgb(221, 221, 221);
            NpoiExtensions.SetColorAtIndex(exportExcelInfo, closedBackgroundColor, _closedBackgroundColorIndex);

            var checkedBackgroundColor = Color.FromArgb(242, 242, 242);
            NpoiExtensions.SetColorAtIndex(exportExcelInfo, checkedBackgroundColor, _checkedBackgroundColorIndex);

            var pendingBackgroundColor = Color.FromArgb(240, 237, 196);
            NpoiExtensions.SetColorAtIndex(
                exportExcelInfo, pendingBackgroundColor, _pendingBackgroundColorIndex);
        }

        private List<DynamicReportInfo> GetPagingLogList(
            GetDynamicReportCriteria criteria,
            List<DynamicReportInfo> dynamicReportList)
        {
            List<DynamicReportInfo> pagingLogList = new List<DynamicReportInfo>();
            var childReportLength = dynamicReportList.Count;
            var startIndex = criteria.PageSize * criteria.PageIndex;
            var endIndex = startIndex + criteria.PageSize - 1;
            var currentIndex = 0;

            var startSplittingIndex = 0;
            var endSplittingIndex = 0;
            for (var i = 0; i < childReportLength && currentIndex <= endIndex; i++)
            {
                var periodGroup = dynamicReportList[i];
                startSplittingIndex = 0;
                endSplittingIndex = 0;

                if (currentIndex + periodGroup.ChildReports.Count - 1 < startIndex)
                {
                    currentIndex += periodGroup.ChildReports.Count;
                    continue;
                }

                if (currentIndex < startIndex)
                {
                    startSplittingIndex = startIndex - currentIndex;
                    currentIndex += startSplittingIndex;
                }
                else
                {
                    startSplittingIndex = 0;
                }

                if (currentIndex + periodGroup.ChildReports.Count - 1 <= endIndex)
                {
                    endSplittingIndex = periodGroup.ChildReports.Count - 1;
                }
                else
                {
                    endSplittingIndex = startSplittingIndex + (endIndex - currentIndex) <= periodGroup.ChildReports.Count ?
                        (endIndex - currentIndex) + startSplittingIndex :
                        periodGroup.ChildReports.Count - 1;
                }

                pagingLogList.Add(new DynamicReportInfo
                {
                    Name = periodGroup.Name,
                    ShortName = periodGroup.ShortName,
                    ChildReports = periodGroup.ChildReports.Skip(startSplittingIndex)
                        .Take(endSplittingIndex - startSplittingIndex + 1)
                        .ToList(),
                    TotalChildReports = periodGroup.ChildReports.Count,
                    StartChildIndex = startSplittingIndex
                });

                currentIndex += endSplittingIndex - startSplittingIndex + 1;
            }

            return pagingLogList;
        }

        private void GetDynamicReportInfoByGroup(
            IEnumerable<IGrouping<string, DynamicReportInfo>> periodGroups,
            ref DynamicReportInfo dynamicReport,
            string groupTypeName)
        {
            foreach (var periodGroup in periodGroups)
            {
                var periodReport = new DynamicReportInfo
                {
                    Name = string.Format(
                        Labels.DynamicReport_PeriodReport_Title,
                        groupTypeName,
                        periodGroup.Key),
                    ShortName = periodGroup.Key,
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
        }
    }
}