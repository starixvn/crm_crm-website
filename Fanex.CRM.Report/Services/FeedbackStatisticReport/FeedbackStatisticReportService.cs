namespace Fanex.CRM.Report.Services.FeedbackStatisticReport
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CRM.Common.Enumerations;
    using Fanex.CRM.Common.Extensions;
    using Fanex.CRM.Common.Helpers;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Common.Models.Npoi;
    using Fanex.CRM.Report.Criteria.FeedbackStatisticReport;
    using Fanex.CRM.Report.Models.FeedbackStatisticReport;
    using Fanex.CRM.Report.Repositories.FeedbackStatisticReport;
    using Fanex.CRM.Report.Resources;
    using Fanex.CRM.Setting.Constants;
    using Fanex.CRM.Setting.Services;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;

    public class FeedbackStatisticReportService : IFeedbackStatisticReportService
    {
        private readonly IFeedbackStatisticReportRepository _feedbackStatisticRepository;
        private readonly ISettingService _settingService;

        private ICellStyle _feedbackDetailTextStyle;
        private ICellStyle _feedbackDetailPendingTextStyle;
        private ICellStyle _feedbackDetailClosedTextStyle;

        public FeedbackStatisticReportService()
            : this(null, null)
        {
        }

        public FeedbackStatisticReportService(IFeedbackStatisticReportRepository feedbackStatisticRepository, ISettingService settingService)
        {
            _feedbackStatisticRepository = feedbackStatisticRepository ?? new FeedbackStatisticReportRepository();
            _settingService = settingService ?? new SettingService();
        }

        public BaseModel[] GetFeedbackStatisticFilterType()
        {
            return _settingService.GetFeedbackStatisticFilterTypes();
        }

        public BaseModel[] GetFeedbackStatisticFilterTypeCN88()
        {
            return _settingService.GetFeedbackStatisticFilterTypesCN88();
        }

        public FeedbackStatistic GetFeedbackStatistic(GetFeedbackStatisticCriteria criteria)
        {
            var title = string.Format(Labels.FeedbackStatisticTitle, criteria.FromDate.ToShortFormatedDate(), criteria.ToDate.ToShortFormatedDate());

            criteria.FromDate = DateTimeUtilize.ChangeClientFromTimeToServerTime(criteria.FromDate);
            criteria.ToDate = DateTimeUtilize.ChangeClientToTimeToServerTime(criteria.ToDate);

            var report = _feedbackStatisticRepository.GetFeedbackStatistic(criteria);
            report.Title = title;

            report.TotalItems = report.FeedbackCategories.Count;
            if (report.TotalItems > 0 && !criteria.ExportExcel)
            {
                report.FeedbackCategories = report.FeedbackCategories.Skip(criteria.PageSize * (criteria.PageIndex - 1)).Take(criteria.PageSize).ToList();
            }

            return report;
        }

        public void ExportFeedbackStatistic(GetFeedbackStatisticCriteria criteria)
        {
            string excelFileName = string.Format(Labels.FeedbackStatisticExcelName, FormatReportDate(criteria.FromDate), FormatReportDate(criteria.ToDate));

            criteria.ExportExcel = true;
            FeedbackStatistic report = GetFeedbackStatistic(criteria);

            int maxColumnIndex = 0;
            if (report != null && report.TotalItems != 0)
            {
                maxColumnIndex += report.Usernames.Count;
            }

            HSSFWorkbook workbook = new HSSFWorkbook();
            ExportExcelInfo exportExcelInfo = new ExportExcelInfo
            {
                ExcelFile = workbook,
                Worksheet = workbook.CreateSheet(Labels.FeedbackStatisticSheetName),
                StartRow = 0,
                StartColumn = 0,
                CurrentRow = 0,
                MaxColumnIndex = maxColumnIndex
            };

            GenerateFeedbackStatisticTitle(exportExcelInfo, report.Title);
            GenerateFeedbackStatisticHeader(exportExcelInfo, report.Usernames);
            if (report == null || report.TotalItems == 0)
            {
                NpoiExtensions.RenderNoInfoMessage(exportExcelInfo, exportExcelInfo.CurrentRow);
            }
            else
            {
                NpoiExtensions.SetColorAtIndex(exportExcelInfo, NpoiExtensions.GroupBackgroundColor, NpoiExtensions.GroupBackgroundColorIndex);
                NpoiExtensions.SetGrandTotalColorIndex(exportExcelInfo);

                GenerateFeedbackStatisticBody(exportExcelInfo, report.FeedbackCategories);
                GenerateFeedbackStatisticFooter(exportExcelInfo, report.GrandTotals);
            }

            NpoiExtensions.ResponseStreamExcel(exportExcelInfo.ExcelFile, excelFileName);
        }

        public FeedbackStatisticDetail FeedbackStatisticDetail(GetFeedbackStatisticDetailCriteria criteria)
        {
            var title = string.Format(Labels.FeedbackStatisticDetailTitle, criteria.FromDate.ToShortFormatedDate(), criteria.ToDate.ToShortFormatedDate());
            criteria.FromDate = DateTimeUtilize.ChangeClientFromTimeToServerTime(criteria.FromDate);
            criteria.ToDate = DateTimeUtilize.ChangeClientToTimeToServerTime(criteria.ToDate);

            var feedbacks = GetFeedbackDetail(criteria);

            // pagination.
            int totalItems = feedbacks.Count;
            if (totalItems > 0 && !criteria.ExportExcel)
            {
                feedbacks = feedbacks.Skip(criteria.PageSize * (criteria.PageIndex - 1)).Take(criteria.PageSize).ToList();
            }

            return new FeedbackStatisticDetail
            {
                Title = title,
                TotalItems = totalItems,
                Feedbacks = feedbacks
            };
        }

        public void ExportFeedbackStatisticDetail(GetFeedbackStatisticDetailCriteria criteria, IEnumerable<Setting> mainCategories)
        {
            var excelFileName = string.Format(Labels.FeedbackStatisticDetailExcelName, FormatReportDate(criteria.FromDate), FormatReportDate(criteria.ToDate));

            criteria.ExportExcel = true;
            var report = FeedbackStatisticDetail(criteria);

            var workbook = new HSSFWorkbook();
            var exportExcelInfo = new ExportExcelInfo
            {
                ExcelFile = workbook,
                StartRow = 0,
                StartColumn = 0,
                CurrentRow = 0,
                MaxColumnIndex = 5
            };

            if (report == null || report.TotalItems == 0)
            {
                NpoiExtensions.RenderNoInfoMessage(exportExcelInfo, exportExcelInfo.CurrentRow);
            }
            else
            {
                var totalRow = report.TotalItems;
                var extraSheet = totalRow % NpoiExtensions.MaxRowPerSheet != 0 ? 1 : 0;
                var numberOfSheet = (totalRow / NpoiExtensions.MaxRowPerSheet) + extraSheet;

                for (int sheetIndex = 0; sheetIndex < numberOfSheet; sheetIndex++)
                {
                    var sheetName = numberOfSheet > 1 ?
                    string.Format("{0} - {1}", Labels.FeedbackStatisticDetailSheetName, sheetIndex + 1) :
                    Labels.FeedbackStatisticDetailSheetName;
                    exportExcelInfo.CreateSheet(sheetName);

                    var startIndex = sheetIndex * NpoiExtensions.MaxRowPerSheet;
                    var endIndex = startIndex + NpoiExtensions.MaxRowPerSheet;
                    if (endIndex > totalRow)
                    {
                        endIndex = totalRow;
                    }

                    exportExcelInfo.CurrentRow = 0;
                    GenerateFeedbackStatisticDetailTitle(exportExcelInfo, report.Title);
                    GenerateFeedbackStatisticDetailHeader(exportExcelInfo);
                    GenerateFeedbackStatisticDetailBody(exportExcelInfo, report.Feedbacks, mainCategories, startIndex, endIndex);
                }
            }

            NpoiExtensions.ResponseStreamExcel(exportExcelInfo.ExcelFile, excelFileName);
        }

        private static string FormatReportDate(DateTime date)
        {
            return date.ToString("MMddyyyy");
        }

        private IList<FeedbackDetail> GetFeedbackDetail(GetFeedbackStatisticDetailCriteria criteria)
        {
            var statusList = _settingService.GetAllStatuses(ViewTypeConstants.All);
            var statusHash = statusList.ToDictionary(k => k.Id, v => v.Name);
            statusHash.Add(1, Labels.Symbol_NotApply);

            IList<FeedbackDetail> feedbacks = new List<FeedbackDetail>();
            var rawdata = _feedbackStatisticRepository.GetFeedbackDetail(criteria);
            foreach (var feedback in rawdata)
            {
                feedback.Status = statusHash.ContainsKey(feedback.StatusId) ? statusHash[feedback.StatusId] : string.Empty;
                feedbacks.Add(feedback);
            }

            return feedbacks;
        }

        #region Excel - Feedback statistic

        private void GenerateFeedbackStatisticTitle(ExportExcelInfo exportExcelInfo, string title)
        {
            NpoiExtensions.SetHeaderColorIndex(exportExcelInfo);
            exportExcelInfo.ResetCurrentColumn();
            exportExcelInfo.SetColumnWidthByFontWidth(exportExcelInfo.CurrentColumn++, 300);
            for (int i = 0; i < exportExcelInfo.MaxColumnIndex; i++)
            {
                exportExcelInfo.SetColumnWidthByFontWidth(exportExcelInfo.CurrentColumn++, 120);
            }

            ICellStyle headerStyle = NpoiExtensions.GetHeaderStyle(exportExcelInfo.ExcelFile, new ExcelStyleParam { Alignment = ExcelHorizontalAlignment.Center });
            exportExcelInfo.RenderHeaderTitle(title, headerStyle);
        }

        private static void GenerateFeedbackStatisticHeader(ExportExcelInfo exportExcelInfo, IList<BaseModel> usernames)
        {
            int currentRow = exportExcelInfo.CurrentRow;
            int currentColumn = exportExcelInfo.StartColumn;
            ICellStyle headerStyle = NpoiExtensions.GetHeaderStyle(exportExcelInfo.ExcelFile, new ExcelStyleParam());

            IRow subCategoryRow = exportExcelInfo.Worksheet.CreateRow(currentRow);
            NpoiExtensions.RenderCell(subCategoryRow, currentColumn, Labels.SubCategory, headerStyle);
            currentColumn++;

            foreach (var item in usernames)
            {
                NpoiExtensions.RenderCell(subCategoryRow, currentColumn, item.Name, headerStyle);
                currentColumn++;
            }

            currentRow += 1;
            exportExcelInfo.CurrentRow = currentRow;
        }

        private static void GenerateFeedbackStatisticBody(ExportExcelInfo exportExcelInfo, IList<FeedbackCategory> feedbackCategories)
        {
            int currentColumn;
            int currentRow = exportExcelInfo.CurrentRow;
            ICellStyle categoryCellStyle = CategoryCellStyle(exportExcelInfo.ExcelFile);
            ICellStyle subCategoryCellStyle = SubCategoryCellStyle(exportExcelInfo.ExcelFile);
            ICellStyle resolveCellStyle = ResolveCellStyle(exportExcelInfo.ExcelFile);
            ICellStyle subTotalCellStyle = SubTotalCellStyle(exportExcelInfo.ExcelFile);
            ICellStyle subTotalResolveCellStyle = SubTotalResolveCellStyle(exportExcelInfo.ExcelFile);

            string categoryPattern = "{0} - {1} " + Labels.Items;
            const string subCategoryPattern = "{0} ({1})";

            foreach (var category in feedbackCategories)
            {
                int subCategoriesLen = category.FeedbackSubcategories.Count;
                currentColumn = exportExcelInfo.StartColumn;
                CellRangeAddress categoryCellRangeAddress = new CellRangeAddress(
                    currentRow,
                    currentRow,
                    exportExcelInfo.StartColumn,
                    exportExcelInfo.MaxColumnIndex);
                NpoiExtensions.RenderMergedCells(
                    exportExcelInfo,
                    categoryCellRangeAddress,
                    string.Format(categoryPattern, category.Name, subCategoriesLen),
                    categoryCellStyle);
                currentRow++;

                foreach (var subCategory in category.FeedbackSubcategories)
                {
                    currentColumn = exportExcelInfo.StartColumn;
                    IRow subCategoryRow = exportExcelInfo.Worksheet.CreateRow(currentRow);
                    NpoiExtensions.RenderCell(
                        subCategoryRow,
                        currentColumn,
                        string.Format(subCategoryPattern, subCategory.Name, subCategory.Total),
                        subCategoryCellStyle);
                    currentColumn++;

                    foreach (var resolve in subCategory.Resolves)
                    {
                        NpoiExtensions.RenderCell(subCategoryRow, currentColumn, resolve, resolveCellStyle);
                        currentColumn++;
                    }

                    currentRow++;
                }

                // Sub-Total.
                currentColumn = exportExcelInfo.StartColumn;
                IRow subTotalRow = exportExcelInfo.Worksheet.CreateRow(currentRow);
                NpoiExtensions.RenderCell(
                    subTotalRow,
                    currentColumn,
                    string.Format(subCategoryPattern, Labels.SubTotal, category.SubTotal.Total),
                    subTotalCellStyle);
                currentColumn++;

                foreach (var resolve in category.SubTotal.Resolves)
                {
                    NpoiExtensions.RenderCell(subTotalRow, currentColumn, resolve, subTotalResolveCellStyle);
                    currentColumn++;
                }

                exportExcelInfo.Worksheet.GroupRow(currentRow - subCategoriesLen, currentRow);
                exportExcelInfo.Worksheet.SetRowGroupCollapsed(currentRow - subCategoriesLen, false);
                currentRow++;
            }

            exportExcelInfo.CurrentRow = currentRow;
        }

        private static void GenerateFeedbackStatisticFooter(ExportExcelInfo exportExcelInfo, int[] grandTotals)
        {
            int currentRow = exportExcelInfo.CurrentRow;
            int currentColumn = exportExcelInfo.StartColumn;

            ICellStyle subCategoryCellStyle = TotalSubCategoryCellStyle(exportExcelInfo.ExcelFile);
            ICellStyle resolveCellStyle = GrandTotalResolveCellStyle(exportExcelInfo.ExcelFile);

            IRow subCategoryRow = exportExcelInfo.Worksheet.CreateRow(currentRow);
            NpoiExtensions.RenderCell(subCategoryRow, currentColumn, Labels.GrandTotal, subCategoryCellStyle);
            currentColumn++;

            foreach (var item in grandTotals)
            {
                NpoiExtensions.RenderCell(subCategoryRow, currentColumn, item, resolveCellStyle);
                currentColumn++;
            }
        }

        private static ICellStyle CategoryCellStyle(HSSFWorkbook workbook)
        {
            return NpoiExtensions.GetTextStyle(
                workbook,
                NpoiExtensions.GroupBackgroundColorIndex,
                true,
                ExcelHorizontalAlignment.Left,
                NpoiExtensions.GroupColorIndex);
        }

        private static ICellStyle SubCategoryCellStyle(HSSFWorkbook workbook)
        {
            return NpoiExtensions.GetTextStyle(
                workbook,
                NpoiExtensions.BackgroundColorIndex,
                false,
                ExcelHorizontalAlignment.Left);
        }

        private static ICellStyle ResolveCellStyle(HSSFWorkbook workbook)
        {
            return NpoiExtensions.GetIntegerNumberStyle(
                workbook,
                NpoiExtensions.BackgroundColorIndex,
                false,
                ExcelHorizontalAlignment.Center);
        }

        private static ICellStyle SubTotalCellStyle(HSSFWorkbook workbook)
        {
            return NpoiExtensions.GetTextStyle(
                workbook,
                NpoiExtensions.BackgroundColorIndex,
                true,
                ExcelHorizontalAlignment.Left);
        }

        private static ICellStyle SubTotalResolveCellStyle(HSSFWorkbook workbook)
        {
            return NpoiExtensions.GetIntegerNumberStyle(
                workbook,
                NpoiExtensions.BackgroundColorIndex,
                true,
                ExcelHorizontalAlignment.Center);
        }

        private static ICellStyle TotalSubCategoryCellStyle(HSSFWorkbook workbook)
        {
            return NpoiExtensions.GetTextStyle(
                workbook,
                NpoiExtensions.GrandTotalRowColorIndex,
                true,
                ExcelHorizontalAlignment.Left,
                NpoiExtensions.GrandTotalTextColorIndex);
        }

        private static ICellStyle GrandTotalResolveCellStyle(HSSFWorkbook workbook)
        {
            return NpoiExtensions.GetIntegerNumberStyle(
                workbook,
                NpoiExtensions.GrandTotalRowColorIndex,
                true,
                ExcelHorizontalAlignment.Center,
                NpoiExtensions.GrandTotalTextColorIndex);
        }

        #endregion Excel - Feedback statistic

        #region Excel - Feedback statistic detail

        private void GenerateFeedbackStatisticDetailTitle(ExportExcelInfo exportExcelInfo, string title)
        {
            int currentRow = exportExcelInfo.CurrentRow;
            int currentColumn = exportExcelInfo.StartColumn;

            NpoiExtensions.SetHeaderColorIndex(exportExcelInfo);
            ISheet worksheet = exportExcelInfo.Worksheet;
            worksheet.SetColumnWidth(currentColumn++, 370 * NpoiExtensions.FontWidth);
            worksheet.SetColumnWidth(currentColumn++, 150 * NpoiExtensions.FontWidth);
            worksheet.SetColumnWidth(currentColumn++, 250 * NpoiExtensions.FontWidth);
            worksheet.SetColumnWidth(currentColumn++, 250 * NpoiExtensions.FontWidth);
            worksheet.SetColumnWidth(currentColumn++, 100 * NpoiExtensions.FontWidth);
            worksheet.SetColumnWidth(currentColumn++, 150 * NpoiExtensions.FontWidth);

            CellRangeAddress cellRangeAddress = new CellRangeAddress(
                currentRow,
                currentRow + 1,
                exportExcelInfo.StartColumn,
                exportExcelInfo.MaxColumnIndex);
            ICellStyle headerStyle = NpoiExtensions.GetHeaderStyle(exportExcelInfo.ExcelFile, new ExcelStyleParam { Alignment = ExcelHorizontalAlignment.Center });
            NpoiExtensions.RenderMergedCells(exportExcelInfo, cellRangeAddress, title, headerStyle);
            currentRow += 2;

            NpoiExtensions.RenderEmptyRow(exportExcelInfo, currentRow);
            exportExcelInfo.CurrentRow = ++currentRow;
        }

        private static void GenerateFeedbackStatisticDetailHeader(ExportExcelInfo exportExcelInfo)
        {
            int currentRow = exportExcelInfo.CurrentRow;
            int currentColumn = exportExcelInfo.StartColumn;
            ICellStyle headerStyle = NpoiExtensions.GetHeaderStyle(exportExcelInfo.ExcelFile, new ExcelStyleParam());

            IRow headerRow = exportExcelInfo.Worksheet.CreateRow(currentRow);
            NpoiExtensions.RenderCell(headerRow, currentColumn, Labels.Feedback, headerStyle);
            currentColumn++;
            NpoiExtensions.RenderCell(headerRow, currentColumn, Labels.Customer, headerStyle);
            currentColumn++;
            NpoiExtensions.RenderCell(headerRow, currentColumn, Labels.Category, headerStyle);
            currentColumn++;
            NpoiExtensions.RenderCell(headerRow, currentColumn, Labels.Status, headerStyle);
            currentColumn++;
            NpoiExtensions.RenderCell(headerRow, currentColumn, Labels.RelatedCustomer, headerStyle);

            exportExcelInfo.CurrentRow = ++currentRow;
        }

        private void GenerateFeedbackStatisticDetailBody(
            ExportExcelInfo exportExcelInfo,
            IList<FeedbackDetail> feedbacks,
            IEnumerable<Setting> mainCategories,
            int startIndex,
            int endIndex)
        {
            const string FeedbackPattern = "{0}\nFeedback date {1}\nBy {2} on {3}\nSolving Duration: {4}{5}";
            int currentColumn;
            int currentRow = exportExcelInfo.CurrentRow;
            ICellStyle textCellStyle;
            IRow row;
            string reviewer;

            InitializeTextStyles(exportExcelInfo.ExcelFile);

            for (int i = startIndex; i < endIndex; i++)
            {
                var feedback = feedbacks[i];
                currentColumn = exportExcelInfo.StartColumn;
                textCellStyle = GetFeedbackDetailTextStyle(feedback.Status);

                reviewer = !string.IsNullOrWhiteSpace(feedback.ReviewerName)
                    ? string.Format("\nReviewed by {0} on {1}", feedback.ReviewerName, feedback.SolveDateString)
                    : string.Empty;
                row = exportExcelInfo.Worksheet.CreateRow(currentRow);
                row.Height = 1200;

                NpoiExtensions.RenderCell(
                    row,
                    currentColumn,
                    string.Format(FeedbackPattern, feedback.Subject, feedback.FeedbackDateString, feedback.CreatorName, feedback.CreateDateString, feedback.DurationTime, reviewer),
                                    textCellStyle);

                var categoryContents = NameMapping.GetCategoryContentWithoutSettingType(feedback.Settings, mainCategories);

                currentColumn++;
                NpoiExtensions.RenderCell(row, currentColumn, feedback.Username, textCellStyle);
                currentColumn++;
                NpoiExtensions.RenderCell(row, currentColumn, String.Join("\n\n", categoryContents), textCellStyle);
                currentColumn++;
                NpoiExtensions.RenderCell(row, currentColumn, feedback.Status, textCellStyle);
                currentColumn++;
                NpoiExtensions.RenderCell(
                    row,
                    currentColumn,
                    feedback.IsRelated ? feedback.Username : feedback.RelatedCustomer,
                    textCellStyle);

                currentRow++;
            }
        }

        private ICellStyle GetFeedbackDetailTextStyle(string status)
        {
            ICellStyle textCellStyle;
            if (string.Equals(status, "closed", StringComparison.OrdinalIgnoreCase))
            {
                textCellStyle = _feedbackDetailClosedTextStyle;
            }
            else if (string.Equals(status, "pending", StringComparison.OrdinalIgnoreCase))
            {
                textCellStyle = _feedbackDetailPendingTextStyle;
            }
            else
            {
                textCellStyle = _feedbackDetailTextStyle;
            }

            return textCellStyle;
        }

        private void InitializeTextStyles(HSSFWorkbook workbook)
        {
            _feedbackDetailTextStyle = NpoiExtensions.GetTextStyle(
                workbook,
                NpoiExtensions.BackgroundColorIndex,
                false,
                ExcelHorizontalAlignment.Left);
            _feedbackDetailPendingTextStyle = NpoiExtensions.GetTextStyle(
                  workbook,
                  NpoiExtensions.PendingStatusColorIndex,
                  false,
                  ExcelHorizontalAlignment.Left);
            _feedbackDetailClosedTextStyle = NpoiExtensions.GetTextStyle(
                workbook,
                NpoiExtensions.CloseStatusColorIndex,
                false,
                ExcelHorizontalAlignment.Left);

            _feedbackDetailTextStyle.WrapText = true;
            _feedbackDetailPendingTextStyle.WrapText = true;
            _feedbackDetailClosedTextStyle.WrapText = true;
        }

        #endregion Excel - Feedback statistic detail
    }
}