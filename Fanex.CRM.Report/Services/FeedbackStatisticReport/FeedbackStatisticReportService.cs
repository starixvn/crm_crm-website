// --------------------------------------------------------------------------
// <copyright file="FeedbackStatisticReportService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.10.10">Created</change>
// </history>

namespace Fanex.CRM.Report.Services.FeedbackStatisticReport
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Fanex.CRM.Common.Extensions;
    using Fanex.CRM.Common.Helpers;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Common.Models.Npoi;
    using Fanex.CRM.Infrastructure.Extensions;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Report.Criteria.FeedbackStatisticReport;
    using Fanex.CRM.Report.Models.FeedbackStatisticReport;
    using Fanex.CRM.Report.Repositories.FeedbackStatisticReport;
    using Fanex.CRM.Report.Resources;
    using Fanex.CRM.Setting.Constants;
    using Fanex.CRM.Setting.Services;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;

    /// <summary>
    /// Feedback statistic report service.
    /// </summary>
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

        /// <summary>
        /// Gets the feedback statistic.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The Feedback statistic object.</returns>
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

        /// <summary>
        /// Exports the feedback statistic.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
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
                NpoiExcelExtensions.RenderNoInfoMessage(exportExcelInfo, exportExcelInfo.CurrentRow);
            }
            else
            {
                NpoiExcelExtensions.SetColorAtIndex(exportExcelInfo, NpoiExcelExtensions.GroupBackgroundColor, NpoiExcelExtensions.GroupBackgroundColorIndex);
                NpoiExcelExtensions.SetGrandTotalColorIndex(exportExcelInfo);

                GenerateFeedbackStatisticBody(exportExcelInfo, report.FeedbackCategories);
                GenerateFeedbackStatisticFooter(exportExcelInfo, report.GrandTotals);
            }

            NpoiExcelExtensions.ResponseStreamExcel(exportExcelInfo.ExcelFile, excelFileName);
        }

        /// <summary>
        /// Feedbacks the statistic detail.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The Feedback statistic detail report.</returns>
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

        /// <summary>
        /// Exports the feedback statistic detail.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        public void ExportFeedbackStatisticDetail(GetFeedbackStatisticDetailCriteria criteria)
        {
            string excelFileName = string.Format(Labels.FeedbackStatisticDetailExcelName, FormatReportDate(criteria.FromDate), FormatReportDate(criteria.ToDate));

            criteria.ExportExcel = true;
            FeedbackStatisticDetail report = FeedbackStatisticDetail(criteria);

            HSSFWorkbook workbook = new HSSFWorkbook();
            ExportExcelInfo exportExcelInfo = new ExportExcelInfo
            {
                ExcelFile = workbook,
                StartRow = 0,
                StartColumn = 0,
                CurrentRow = 0,
                MaxColumnIndex = 5
            };

            if (report == null || report.TotalItems == 0)
            {
                NpoiExcelExtensions.RenderNoInfoMessage(exportExcelInfo, exportExcelInfo.CurrentRow);
            }
            else
            {
                var totalRow = report.TotalItems;
                var maxRowPerSheet = NpoiExcelExtensions.MaxRowPerSheet;
                var extraSheet = totalRow % maxRowPerSheet != 0 ? 1 : 0;
                var numberOfSheet = (totalRow / maxRowPerSheet) + extraSheet;

                for (int sheetIndex = 0; sheetIndex < numberOfSheet; sheetIndex++)
                {
                    var sheetName = numberOfSheet > 1 ?
                    string.Format("{0} - {1}", Labels.FeedbackStatisticDetailSheetName, sheetIndex + 1) :
                    Labels.FeedbackStatisticDetailSheetName;
                    exportExcelInfo.CreateSheet(sheetName);

                    var startIndex = sheetIndex * maxRowPerSheet;
                    var endIndex = startIndex + maxRowPerSheet;
                    if (endIndex > totalRow)
                    {
                        endIndex = totalRow;
                    }

                    exportExcelInfo.CurrentRow = 0;
                    GenerateFeedbackStatisticDetailTitle(exportExcelInfo, report.Title);
                    GenerateFeedbackStatisticDetailHeader(exportExcelInfo);

                    GenerateFeedbackStatisticDetailBody(exportExcelInfo, report.Feedbacks, startIndex, endIndex);
                }
            }

            NpoiExcelExtensions.ResponseStreamExcel(exportExcelInfo.ExcelFile, excelFileName);
        }

        private string FormatReportDate(DateTime date)
        {
            string pattern = "MMddyyyy";
            return date.ToString(pattern);
        }

        /// <summary>
        /// Gets the feedback detail.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The feedback detail list.</returns>
        private IList<FeedbackDetail> GetFeedbackDetail(GetFeedbackStatisticDetailCriteria criteria)
        {
            var categoryHash = new Dictionary<int, string>();
            var subcategoryHash = new Dictionary<int, string>();
            var categoies = _settingService.GetAllMainCategories();
            foreach (var category in categoies)
            {
                if (!categoryHash.ContainsKey(category.Id))
                {
                    categoryHash.Add(category.Id, category.Name);
                }

                foreach (var subcategory in category.ChildSettings)
                {
                    if (!subcategoryHash.ContainsKey(subcategory.Id))
                    {
                        subcategoryHash.Add(subcategory.Id, subcategory.Name);
                    }
                }
            }

            var statusList = _settingService.GetAllStatuses(ViewTypeConstants.All);
            var statusHash = statusList.ToDictionary(k => k.Id, v => v.Name);
            statusHash.Add(1, Labels.Symbol_NotApply);

            IList<FeedbackDetail> feedbacks = new List<FeedbackDetail>();
            var rawdata = _feedbackStatisticRepository.GetFeedbackDetail(criteria);
            foreach (var feedback in rawdata)
            {
                feedback.MainCategory = categoryHash[feedback.MainCategoryId];
                feedback.SubCategory = subcategoryHash.ContainsKey(feedback.SubCategoryId) ? subcategoryHash[feedback.SubCategoryId] : Labels.NA;
                feedback.Status = statusHash.ContainsKey(feedback.StatusId) ? statusHash[feedback.StatusId] : string.Empty;
                feedbacks.Add(feedback);
            }

            return feedbacks;
        }

        #region Excel - Feedback statistic

        /// <summary>
        /// Generates the feedback statistic title.
        /// </summary>
        /// <param name="exportExcelInfo">The export excel info.</param>
        /// <param name="title">The title.</param>
        private void GenerateFeedbackStatisticTitle(ExportExcelInfo exportExcelInfo, string title)
        {
            NpoiExcelExtensions.SetHeaderColorIndex(exportExcelInfo);
            exportExcelInfo.ResetCurrentColumn();
            exportExcelInfo.SetColumnWidthByFontWidth(exportExcelInfo.CurrentColumn++, 300);
            for (int i = 0; i < exportExcelInfo.MaxColumnIndex; i++)
            {
                exportExcelInfo.SetColumnWidthByFontWidth(exportExcelInfo.CurrentColumn++, 120);
            }

            ICellStyle headerStyle = NpoiExcelExtensions.GetHeaderStyle(exportExcelInfo.ExcelFile, new ExcelStyleParam() { Alignment = CRMHorizontalAlignment.Center });
            exportExcelInfo.RenderHeaderTitle(title, headerStyle);
        }

        /// <summary>
        /// Generates the feedback statistic header.
        /// </summary>
        /// <param name="exportExcelInfo">The export excel info.</param>
        /// <param name="usernames">The usernames.</param>
        private void GenerateFeedbackStatisticHeader(ExportExcelInfo exportExcelInfo, IList<BaseModel> usernames)
        {
            int currentRow = exportExcelInfo.CurrentRow;
            int currentColumn = exportExcelInfo.StartColumn;
            ICellStyle headerStyle = NpoiExcelExtensions.GetHeaderStyle(exportExcelInfo.ExcelFile, new ExcelStyleParam());

            IRow subCategoryRow = exportExcelInfo.Worksheet.CreateRow(currentRow);
            NpoiExcelExtensions.RenderCell(subCategoryRow, currentColumn, Labels.SubCategory, headerStyle);
            currentColumn++;

            foreach (var item in usernames)
            {
                NpoiExcelExtensions.RenderCell(subCategoryRow, currentColumn, item.Name, headerStyle);
                currentColumn++;
            }

            currentRow += 1;
            exportExcelInfo.CurrentRow = currentRow;
        }

        /// <summary>
        /// Generates the feedback statistic body.
        /// </summary>
        /// <param name="exportExcelInfo">The export excel info.</param>
        /// <param name="feedbackCategories">The feedback categories.</param>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        private void GenerateFeedbackStatisticBody(ExportExcelInfo exportExcelInfo, IList<FeedbackCategory> feedbackCategories)
        {
            int currentColumn;
            int currentRow = exportExcelInfo.CurrentRow;
            ICellStyle categoryCellStyle = CategoryCellStyle(exportExcelInfo.ExcelFile);
            ICellStyle subCategoryCellStyle = SubCategoryCellStyle(exportExcelInfo.ExcelFile);
            ICellStyle resolveCellStyle = ResolveCellStyle(exportExcelInfo.ExcelFile);
            ICellStyle subTotalCellStyle = SubTotalCellStyle(exportExcelInfo.ExcelFile);
            ICellStyle subTotalResolveCellStyle = SubTotalResolveCellStyle(exportExcelInfo.ExcelFile);

            string categoryPattern = "{0} - {1} " + Labels.Items;
            string subCategoryPattern = "{0} ({1})";

            foreach (var category in feedbackCategories)
            {
                int subCategoriesLen = category.FeedbackSubcategories.Count;
                currentColumn = exportExcelInfo.StartColumn;
                CellRangeAddress categoryCellRangeAddress = new CellRangeAddress(
                    currentRow,
                    currentRow,
                    exportExcelInfo.StartColumn,
                    exportExcelInfo.MaxColumnIndex);
                NpoiExcelExtensions.RenderMergedCells(
                    exportExcelInfo,
                    categoryCellRangeAddress,
                    string.Format(categoryPattern, category.Name, subCategoriesLen),
                    categoryCellStyle);
                currentRow++;

                foreach (var subCategory in category.FeedbackSubcategories)
                {
                    currentColumn = exportExcelInfo.StartColumn;
                    IRow subCategoryRow = exportExcelInfo.Worksheet.CreateRow(currentRow);
                    NpoiExcelExtensions.RenderCell(
                        subCategoryRow,
                        currentColumn,
                        string.Format(subCategoryPattern, subCategory.Name, subCategory.Total),
                        subCategoryCellStyle);
                    currentColumn++;

                    foreach (var resolve in subCategory.Resolves)
                    {
                        NpoiExcelExtensions.RenderCell(subCategoryRow, currentColumn, resolve, resolveCellStyle);
                        currentColumn++;
                    }

                    currentRow++;
                }

                // Sub-Total.
                currentColumn = exportExcelInfo.StartColumn;
                IRow subTotalRow = exportExcelInfo.Worksheet.CreateRow(currentRow);
                NpoiExcelExtensions.RenderCell(
                    subTotalRow,
                    currentColumn,
                    string.Format(subCategoryPattern, Labels.SubTotal, category.SubTotal.Total),
                    subTotalCellStyle);
                currentColumn++;

                foreach (var resolve in category.SubTotal.Resolves)
                {
                    NpoiExcelExtensions.RenderCell(subTotalRow, currentColumn, resolve, subTotalResolveCellStyle);
                    currentColumn++;
                }

                exportExcelInfo.Worksheet.GroupRow(currentRow - subCategoriesLen, currentRow);
                exportExcelInfo.Worksheet.SetRowGroupCollapsed(currentRow - subCategoriesLen, false);
                currentRow++;
            }

            exportExcelInfo.CurrentRow = currentRow;
        }

        /// <summary>
        /// Generates the feedback statistic footer.
        /// </summary>
        /// <param name="exportExcelInfo">The export excel info.</param>
        /// <param name="grandTotals">The grand totals.</param>
        private void GenerateFeedbackStatisticFooter(ExportExcelInfo exportExcelInfo, int[] grandTotals)
        {
            int currentRow = exportExcelInfo.CurrentRow;
            int currentColumn = exportExcelInfo.StartColumn;

            ICellStyle subCategoryCellStyle = TotalSubCategoryCellStyle(exportExcelInfo.ExcelFile);
            ICellStyle resolveCellStyle = GrandTotalResolveCellStyle(exportExcelInfo.ExcelFile);

            IRow subCategoryRow = exportExcelInfo.Worksheet.CreateRow(currentRow);
            NpoiExcelExtensions.RenderCell(subCategoryRow, currentColumn, Labels.GrandTotal, subCategoryCellStyle);
            currentColumn++;

            foreach (var item in grandTotals)
            {
                NpoiExcelExtensions.RenderCell(subCategoryRow, currentColumn, item, resolveCellStyle);
                currentColumn++;
            }
        }

        /// <summary>
        /// Categories the cell style.
        /// </summary>
        /// <param name="workbook">The workbook.</param>
        /// <returns>The cell style of excel.</returns>
        private ICellStyle CategoryCellStyle(HSSFWorkbook workbook)
        {
            return NpoiExcelExtensions.GetTextStyle(
                workbook,
                NpoiExcelExtensions.GroupBackgroundColorIndex,
                true,
                CRMHorizontalAlignment.Left,
                NpoiExcelExtensions.GroupColorIndex);
        }

        private ICellStyle SubCategoryCellStyle(HSSFWorkbook workbook)
        {
            return NpoiExcelExtensions.GetTextStyle(
                workbook,
                NpoiExcelExtensions.BackgroundColorIndex,
                false,
                CRMHorizontalAlignment.Left);
        }

        private ICellStyle ResolveCellStyle(HSSFWorkbook workbook)
        {
            return NpoiExcelExtensions.GetIntegerNumberStyle(
                workbook,
                NpoiExcelExtensions.BackgroundColorIndex,
                false,
                CRMHorizontalAlignment.Center);
        }

        private ICellStyle SubTotalCellStyle(HSSFWorkbook workbook)
        {
            return NpoiExcelExtensions.GetTextStyle(
                workbook,
                NpoiExcelExtensions.BackgroundColorIndex,
                true,
                CRMHorizontalAlignment.Left);
        }

        private ICellStyle SubTotalResolveCellStyle(HSSFWorkbook workbook)
        {
            return NpoiExcelExtensions.GetIntegerNumberStyle(
                workbook,
                NpoiExcelExtensions.BackgroundColorIndex,
                true,
                CRMHorizontalAlignment.Center);
        }

        private ICellStyle TotalSubCategoryCellStyle(HSSFWorkbook workbook)
        {
            return NpoiExcelExtensions.GetTextStyle(
                workbook,
                NpoiExcelExtensions.GrandTotalRowColorIndex,
                true,
                CRMHorizontalAlignment.Left,
                NpoiExcelExtensions.GrandTotalTextColorIndex);
        }

        private ICellStyle GrandTotalResolveCellStyle(HSSFWorkbook workbook)
        {
            return NpoiExcelExtensions.GetIntegerNumberStyle(
                workbook,
                NpoiExcelExtensions.GrandTotalRowColorIndex,
                true,
                CRMHorizontalAlignment.Center,
                NpoiExcelExtensions.GrandTotalTextColorIndex);
        }

        #endregion Excel - Feedback statistic

        #region Excel - Feedback statistic detail

        /// <summary>
        /// Generates the feedback statistic detail title.
        /// </summary>
        /// <param name="exportExcelInfo">The export excel info.</param>
        /// <param name="title">The title.</param>
        private void GenerateFeedbackStatisticDetailTitle(ExportExcelInfo exportExcelInfo, string title)
        {
            int currentRow = exportExcelInfo.CurrentRow;
            int currentColumn = exportExcelInfo.StartColumn;

            NpoiExcelExtensions.SetHeaderColorIndex(exportExcelInfo);
            ISheet worksheet = exportExcelInfo.Worksheet;
            worksheet.SetColumnWidth(currentColumn++, 370 * NpoiExcelExtensions.FontWidth);
            worksheet.SetColumnWidth(currentColumn++, 150 * NpoiExcelExtensions.FontWidth);
            worksheet.SetColumnWidth(currentColumn++, 250 * NpoiExcelExtensions.FontWidth);
            worksheet.SetColumnWidth(currentColumn++, 250 * NpoiExcelExtensions.FontWidth);
            worksheet.SetColumnWidth(currentColumn++, 100 * NpoiExcelExtensions.FontWidth);
            worksheet.SetColumnWidth(currentColumn++, 150 * NpoiExcelExtensions.FontWidth);

            CellRangeAddress cellRangeAddress = new CellRangeAddress(
                currentRow,
                currentRow + 1,
                exportExcelInfo.StartColumn,
                exportExcelInfo.MaxColumnIndex);
            ICellStyle headerStyle = NpoiExcelExtensions.GetHeaderStyle(exportExcelInfo.ExcelFile, new ExcelStyleParam() { Alignment = CRMHorizontalAlignment.Center });
            NpoiExcelExtensions.RenderMergedCells(exportExcelInfo, cellRangeAddress, title, headerStyle);
            currentRow += 2;

            NpoiExcelExtensions.RenderEmptyRow(exportExcelInfo, currentRow);
            exportExcelInfo.CurrentRow = ++currentRow;
        }

        /// <summary>
        /// Generates the feedback statistic detail header.
        /// </summary>
        /// <param name="exportExcelInfo">The export excel info.</param>
        private void GenerateFeedbackStatisticDetailHeader(ExportExcelInfo exportExcelInfo)
        {
            int currentRow = exportExcelInfo.CurrentRow;
            int currentColumn = exportExcelInfo.StartColumn;
            ICellStyle headerStyle = NpoiExcelExtensions.GetHeaderStyle(exportExcelInfo.ExcelFile, new ExcelStyleParam());

            IRow headerRow = exportExcelInfo.Worksheet.CreateRow(currentRow);
            NpoiExcelExtensions.RenderCell(headerRow, currentColumn, Labels.Feedback, headerStyle);
            currentColumn++;
            NpoiExcelExtensions.RenderCell(headerRow, currentColumn, Labels.Customer, headerStyle);
            currentColumn++;
            NpoiExcelExtensions.RenderCell(headerRow, currentColumn, Labels.Category, headerStyle);
            currentColumn++;
            NpoiExcelExtensions.RenderCell(headerRow, currentColumn, Labels.SubCategory, headerStyle);
            currentColumn++;
            NpoiExcelExtensions.RenderCell(headerRow, currentColumn, Labels.Status, headerStyle);
            currentColumn++;
            NpoiExcelExtensions.RenderCell(headerRow, currentColumn, Labels.RelatedCustomer, headerStyle);

            exportExcelInfo.CurrentRow = ++currentRow;
        }

        /// <summary>
        /// Generates the feedback statistic detail body.
        /// </summary>
        /// <param name="exportExcelInfo">The export excel info.</param>
        /// <param name="feedbacks">The feedbacks.</param>
        private void GenerateFeedbackStatisticDetailBody(ExportExcelInfo exportExcelInfo, IList<FeedbackDetail> feedbacks, int startIndex, int endIndex)
        {
            int currentColumn;
            int currentRow = exportExcelInfo.CurrentRow;
            ICellStyle textCellStyle;

            IRow row;
            string reviewer;
            string feedbackPattern = "{0}\nFeedback date {1}\nBy {2} on {3}\nSolving Duration: {4}{5}";
            InitializeTextStyles(exportExcelInfo.ExcelFile);

            for (int i = startIndex; i < endIndex; i++)
            {
                currentColumn = exportExcelInfo.StartColumn;
                textCellStyle = GetFeedbackDetailTextStyle(feedbacks[i].Status);

                reviewer = !string.IsNullOrWhiteSpace(feedbacks[i].ReviewerName)
                    ? string.Format("\nReviewed by {0} on {1}", feedbacks[i].ReviewerName, feedbacks[i].SolveDateString)
                    : string.Empty;
                row = exportExcelInfo.Worksheet.CreateRow(currentRow);
                row.Height = 1200;
                NpoiExcelExtensions.RenderCell(
                    row,
                    currentColumn,
                    string.Format(feedbackPattern, feedbacks[i].Subject, feedbacks[i].FeedbackDateString, feedbacks[i].CreatorName, feedbacks[i].CreateDateString, feedbacks[i].DurationTime, reviewer),
                                    textCellStyle);
                currentColumn++;
                NpoiExcelExtensions.RenderCell(row, currentColumn, feedbacks[i].Username, textCellStyle);
                currentColumn++;
                NpoiExcelExtensions.RenderCell(row, currentColumn, feedbacks[i].MainCategory, textCellStyle);
                currentColumn++;
                NpoiExcelExtensions.RenderCell(row, currentColumn, feedbacks[i].SubCategory, textCellStyle);
                currentColumn++;
                NpoiExcelExtensions.RenderCell(row, currentColumn, feedbacks[i].Status, textCellStyle);
                currentColumn++;
                NpoiExcelExtensions.RenderCell(
                    row,
                    currentColumn,
                    feedbacks[i].IsRelated ? feedbacks[i].Username : feedbacks[i].RelatedCustomer,
                    textCellStyle);

                currentRow++;
            }
        }

        /// <summary>
        /// Feedbacks the detail text style.
        /// </summary>
        /// <param name="workbook">The workbook.</param>
        /// <param name="status">The status.</param>
        /// <returns>The cell style of Feedback detail.</returns>
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
            _feedbackDetailTextStyle = NpoiExcelExtensions.GetTextStyle(
                workbook,
                NpoiExcelExtensions.BackgroundColorIndex,
                false,
                CRMHorizontalAlignment.Left);
            _feedbackDetailPendingTextStyle = NpoiExcelExtensions.GetTextStyle(
                  workbook,
                  NpoiExcelExtensions.PendingStatusColorIndex,
                  false,
                  CRMHorizontalAlignment.Left);
            _feedbackDetailClosedTextStyle = NpoiExcelExtensions.GetTextStyle(
                workbook,
                NpoiExcelExtensions.CloseStatusColorIndex,
                false,
                CRMHorizontalAlignment.Left);

            _feedbackDetailTextStyle.WrapText = true;
            _feedbackDetailPendingTextStyle.WrapText = true;
            _feedbackDetailClosedTextStyle.WrapText = true;
        }

        #endregion Excel - Feedback statistic detail
    }
}