// --------------------------------------------------------------------------
// <copyright file="FeedbackStatisticReportService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.10.10">Created</change>
// </history>

namespace Fanex.CRM.Report.Services.Implements.FeedbackStatisticReport
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web;
    using Fanex.CRM.Infrastructure.Core;
    using Fanex.CRM.Infrastructure.Extension;
    using Fanex.CRM.Infrastructure.Logging;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Infrastructure.Models.Npoi;
    using Fanex.CRM.Report.Criteria.FeedbackStatisticReport;
    using Fanex.CRM.Report.Models.FeedbackStatisticReport;
    using Fanex.CRM.Report.Repositories.Implements;
    using Fanex.CRM.Report.Repositories.Interfaces;
    using Fanex.CRM.Report.Resources;
    using Fanex.CRM.Report.Services.Interfaces;
    using Fanex.CRM.Setting.Constants;
    using Fanex.CRM.Setting.Criteria;
    using Fanex.CRM.Setting.Services.Implements;
    using Fanex.CRM.Setting.Services.Interfaces;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;

    /// <summary>
    /// Feedback statistic report service.
    /// </summary>
    public class FeedbackStatisticReportService : IFeedbackStatisticReportService
    {
        // Session keys.
        private const string FeedbackStatisticSessionKey = "Fanex.CRM.Report.Services.FeedbackStatistics";
        private const string FeedbackStatisticDetailSessionKey = "Fanex.CRM.Report.Services.FeedbackStatisticsDetail";

        private readonly IFeedbackStatisticReportRepository _feedbackStatisticRepository;
        private readonly ISettingService _settingService;

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
            return _settingService.GetFeedbackStatisticFilterType();
        }

        /// <summary>
        /// Gets the feedback statistic.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The Feedback statistic object.</returns>
        public FeedbackStatistic GetFeedbackStatistic(GetFeedbackStatisticCriteria criteria)
        {
            string key = FeedbackStatisticSessionKey;
            FeedbackStatistic report = null;
            if (criteria.FromCache)
            {
                report = (FeedbackStatistic)SessionExtension.GetSession(HttpContext.Current.Session, key);
            }

            if (report == null)
            {
                string title = string.Format(Labels.FeedbackStatisticTitle, criteria.FromDate.ToShortFormatedDate(), criteria.ToDate.ToShortFormatedDate());
                
                // Get data from DB.
                criteria.FromDate = DateTimeUtilize.ChangeClientFromTimeToServerTime(criteria.FromDate);
                criteria.ToDate = DateTimeUtilize.ChangeClientToTimeToServerTime(criteria.ToDate);
                try
                {
                    report = _feedbackStatisticRepository.GetFeedbackStatistic(criteria);
                    report.Title = title;

                    SessionExtension.AddSession(HttpContext.Current.Session, key, report);
                }
                catch (Exception ex)
                {
                    LogExtensions.ExceptionHandler(ex);
                    throw;
                }
            }

            // pagination.
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
            criteria.ExportExcel = true;
            FeedbackStatistic report = GetFeedbackStatistic(criteria);

            int maxColumnIndex = 0;
            if (report != null && report.TotalItems != 0)
            {
                maxColumnIndex += report.Usernames.Count;
            }

            string excelFileName = string.Format(Labels.FeedbackStatisticExcelName, FormatReportDate(criteria.FromDate), FormatReportDate(criteria.ToDate));
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
                NpoiExcelExtensions.RenderNoInfoMessage(ref exportExcelInfo, exportExcelInfo.CurrentRow);
            }
            else
            {
                NpoiExcelExtensions.SetColorAtIndex(
                    ref exportExcelInfo,
                    NpoiExcelExtensions.GroupBackgroundColor,
                    NpoiExcelExtensions.GroupBackgroundColorIndex);
                NpoiExcelExtensions.SetGrandTotalColorIndex(ref exportExcelInfo);

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
            string key = FeedbackStatisticDetailSessionKey;
            string title = string.Format(Labels.FeedbackStatisticDetailTitle, criteria.FromDate.ToShortFormatedDate(), criteria.ToDate.ToShortFormatedDate());
            IList<FeedbackDetail> feedbacks = null;
            if (criteria.FromCache)
            {
                feedbacks = (IList<FeedbackDetail>)SessionExtension.GetSession(HttpContext.Current.Session, key);
            }

            if (feedbacks == null)
            {
                // Get data from DB.
                criteria.FromDate = DateTimeUtilize.ChangeClientFromTimeToServerTime(criteria.FromDate);
                criteria.ToDate = DateTimeUtilize.ChangeClientToTimeToServerTime(criteria.ToDate);
                try
                {
                    feedbacks = GetFeedbackDetail(criteria);
                    SessionExtension.AddSession(HttpContext.Current.Session, key, feedbacks);
                }
                catch (Exception ex)
                {
                    LogExtensions.ExceptionHandler(ex);
                    throw;
                }
            }
            
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
            criteria.ExportExcel = true;
            FeedbackStatisticDetail report = FeedbackStatisticDetail(criteria);

            string excelFileName = string.Format(Labels.FeedbackStatisticDetailExcelName, FormatReportDate(criteria.FromDate), FormatReportDate(criteria.ToDate));
            HSSFWorkbook workbook = new HSSFWorkbook();
            ExportExcelInfo exportExcelInfo = new ExportExcelInfo
            {
                ExcelFile = workbook,
                Worksheet = workbook.CreateSheet(Labels.FeedbackStatisticDetailSheetName),
                StartRow = 0,
                StartColumn = 0,
                CurrentRow = 0,
                MaxColumnIndex = 5
            };

            GenerateFeedbackStatisticDetailTitle(exportExcelInfo, report.Title);
            GenerateFeedbackStatisticDetailHeader(exportExcelInfo);
            if (report == null || report.TotalItems == 0)
            {
                NpoiExcelExtensions.RenderNoInfoMessage(ref exportExcelInfo, exportExcelInfo.CurrentRow);
            }
            else
            {
                GenerateFeedbackStatisticDetailBody(exportExcelInfo, report.Feedbacks);
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
            var categoies = _settingService.GetAllMainCategory();
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

            var statusList = _settingService.GetAllStatus(new GetAllStatusCriteria { ViewType = ViewTypeConstants.All });
            var statusHash = statusList.ToDictionary(k => k.Id, v => v.Name);
            statusHash.Add(1, Labels.Symbol_NotApply);

            IList<FeedbackDetail> feedbacks = new List<FeedbackDetail>();
            var rawdata = _feedbackStatisticRepository.GetFeedbackDetail(criteria);
            foreach (var feedback in rawdata)
            {
                feedback.MainCategory = categoryHash[feedback.MainCategoryId];
                feedback.SubCategory = subcategoryHash[feedback.SubCategoryId];
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
            int currentRow = exportExcelInfo.CurrentRow;
            int currentColumn = exportExcelInfo.StartColumn;

            NpoiExcelExtensions.SetHeaderColorIndex(ref exportExcelInfo);
            ISheet worksheet = exportExcelInfo.Worksheet;
            worksheet.SetColumnWidth(currentColumn++, 300 * NpoiExcelExtensions.FontWidth);
            for (int i = 0; i < exportExcelInfo.MaxColumnIndex; i++)
            {
                worksheet.SetColumnWidth(currentColumn++, 120 * NpoiExcelExtensions.FontWidth);
            }

            CellRangeAddress cellRangeAddress = new CellRangeAddress(
                currentRow,
                currentRow + 1,
                exportExcelInfo.StartColumn,
                exportExcelInfo.MaxColumnIndex);
            ICellStyle headerStyle = NpoiExcelExtensions.HeaderStyle(exportExcelInfo.ExcelFile, new ExcelStyleParam() { Alignment = CRMHorizontalAlignment.Center });
            NpoiExcelExtensions.RenderMergedCells(ref exportExcelInfo, cellRangeAddress, title, headerStyle);
            currentRow += 2;

            NpoiExcelExtensions.RenderEmptyRow(ref exportExcelInfo, currentRow);
            currentRow++;

            exportExcelInfo.CurrentRow = currentRow;
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
            ICellStyle headerStyle = NpoiExcelExtensions.HeaderStyle(exportExcelInfo.ExcelFile, new ExcelStyleParam());

            IRow subCategoryRow = exportExcelInfo.Worksheet.CreateRow(currentRow);
            NpoiExcelExtensions.RenderCell(ref subCategoryRow, ref currentColumn, Labels.SubCategory, headerStyle);

            foreach (var item in usernames)
            {
                NpoiExcelExtensions.RenderCell(ref subCategoryRow, ref currentColumn, item.Name, headerStyle);
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
                    ref exportExcelInfo, 
                    categoryCellRangeAddress,
                    string.Format(categoryPattern, category.Name, subCategoriesLen),
                    categoryCellStyle);
                currentRow++;

                foreach (var subCategory in category.FeedbackSubcategories)
                {
                    currentColumn = exportExcelInfo.StartColumn;
                    IRow subCategoryRow = exportExcelInfo.Worksheet.CreateRow(currentRow);
                    NpoiExcelExtensions.RenderCell(
                        ref subCategoryRow, 
                        ref currentColumn, 
                        string.Format(subCategoryPattern, subCategory.Name, subCategory.Total), 
                        subCategoryCellStyle);

                    foreach (var resolve in subCategory.Resolves)
                    {
                        NpoiExcelExtensions.RenderCell(ref subCategoryRow, ref currentColumn, resolve, resolveCellStyle);    
                    }

                    currentRow++;
                }

                // Sub-Total.
                currentColumn = exportExcelInfo.StartColumn;
                IRow subTotalRow = exportExcelInfo.Worksheet.CreateRow(currentRow);
                NpoiExcelExtensions.RenderCell(
                    ref subTotalRow, 
                    ref currentColumn,
                    string.Format(subCategoryPattern, Labels.SubTotal, category.SubTotal.Total), 
                    subTotalCellStyle);

                foreach (var resolve in category.SubTotal.Resolves)
                {
                    NpoiExcelExtensions.RenderCell(ref subTotalRow, ref currentColumn, resolve, subTotalResolveCellStyle);        
                }
                //// ---------- End of Sub-Total. ----------

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
            NpoiExcelExtensions.RenderCell(ref subCategoryRow, ref currentColumn, Labels.GrandTotal, subCategoryCellStyle);

            foreach (var item in grandTotals)
            {
                NpoiExcelExtensions.RenderCell(ref subCategoryRow, ref currentColumn, item, resolveCellStyle);    
            }
        }

        /// <summary>
        /// Categories the cell style.
        /// </summary>
        /// <param name="workbook">The workbook.</param>
        /// <returns>The cell style of excel.</returns>
        private ICellStyle CategoryCellStyle(HSSFWorkbook workbook)
        {
            ICellStyle cellStyle = NpoiExcelExtensions.TextStyle(
                workbook,
                NpoiExcelExtensions.GroupBackgroundColorIndex,
                true,
                CRMHorizontalAlignment.Left);
            var font = workbook.CreateFont();
            font.FontHeightInPoints = NpoiExcelExtensions.FontSize;
            font.FontName = NpoiExcelExtensions.FontName;
            font.Color = NpoiExcelExtensions.GroupColorIndex;
            font.Boldweight = (short)FontBoldWeight.Bold;
            cellStyle.SetFont(font);
            NpoiExcelExtensions.SetBorderToCell(cellStyle);

            return cellStyle;
        }

        private ICellStyle SubCategoryCellStyle(HSSFWorkbook workbook)
        {
            return NpoiExcelExtensions.TextStyle(
                workbook,
                NpoiExcelExtensions.BackgroundColorIndex,
                false,
                CRMHorizontalAlignment.Left);
        }

        private ICellStyle ResolveCellStyle(HSSFWorkbook workbook)
        {
            return NpoiExcelExtensions.IntNumberStyle(
                workbook,
                NpoiExcelExtensions.BackgroundColorIndex,
                false,
                CRMHorizontalAlignment.Center);
        }

        private ICellStyle SubTotalCellStyle(HSSFWorkbook workbook)
        {
            return NpoiExcelExtensions.TextStyle(
                workbook,
                NpoiExcelExtensions.BackgroundColorIndex,
                true,
                CRMHorizontalAlignment.Left);
        }

        private ICellStyle SubTotalResolveCellStyle(HSSFWorkbook workbook)
        {
            return NpoiExcelExtensions.IntNumberStyle(
                workbook,
                NpoiExcelExtensions.BackgroundColorIndex,
                true,
                CRMHorizontalAlignment.Center);
        }

        private ICellStyle TotalSubCategoryCellStyle(HSSFWorkbook workbook)
        {
            return NpoiExcelExtensions.TextStyle(
                workbook,
                NpoiExcelExtensions.GrandTotalRowColorIndex,
                true,
                CRMHorizontalAlignment.Left,
                NpoiExcelExtensions.GrandTotalTextColorIndex);
        }

        private ICellStyle GrandTotalResolveCellStyle(HSSFWorkbook workbook)
        {
            return NpoiExcelExtensions.IntNumberStyle(
                workbook,
                NpoiExcelExtensions.GrandTotalRowColorIndex,
                true,
                CRMHorizontalAlignment.Center,
                NpoiExcelExtensions.GrandTotalTextColorIndex);
        }

        #endregion

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

            NpoiExcelExtensions.SetHeaderColorIndex(ref exportExcelInfo);
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
            ICellStyle headerStyle = NpoiExcelExtensions.HeaderStyle(exportExcelInfo.ExcelFile, new ExcelStyleParam() { Alignment = CRMHorizontalAlignment.Center });
            NpoiExcelExtensions.RenderMergedCells(ref exportExcelInfo, cellRangeAddress, title, headerStyle);
            currentRow += 2;

            NpoiExcelExtensions.RenderEmptyRow(ref exportExcelInfo, currentRow);
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
            ICellStyle headerStyle = NpoiExcelExtensions.HeaderStyle(exportExcelInfo.ExcelFile, new ExcelStyleParam());

            IRow headerRow = exportExcelInfo.Worksheet.CreateRow(currentRow);
            NpoiExcelExtensions.RenderCell(ref headerRow, ref currentColumn, Labels.Feedback, headerStyle);
            NpoiExcelExtensions.RenderCell(ref headerRow, ref currentColumn, Labels.Customer, headerStyle);
            NpoiExcelExtensions.RenderCell(ref headerRow, ref currentColumn, Labels.Category, headerStyle);
            NpoiExcelExtensions.RenderCell(ref headerRow, ref currentColumn, Labels.SubCategory, headerStyle);
            NpoiExcelExtensions.RenderCell(ref headerRow, ref currentColumn, Labels.Status, headerStyle);
            NpoiExcelExtensions.RenderCell(ref headerRow, ref currentColumn, Labels.RelatedCustomer, headerStyle);

            exportExcelInfo.CurrentRow = ++currentRow;
        }

        /// <summary>
        /// Generates the feedback statistic detail body.
        /// </summary>
        /// <param name="exportExcelInfo">The export excel info.</param>
        /// <param name="feedbacks">The feedbacks.</param>
        private void GenerateFeedbackStatisticDetailBody(ExportExcelInfo exportExcelInfo, IList<FeedbackDetail> feedbacks)
        {
            int currentColumn;
            int currentRow = exportExcelInfo.CurrentRow;
            ICellStyle textCellStyle;

            IRow row;
            string reviewer;
            string feedbackPattern = "{0}\nFeedback date {1}\nBy {2} on {3}\nSolving Duration: {4}{5}";
            foreach (var feedback in feedbacks)
            {
                currentColumn = exportExcelInfo.StartColumn;
                textCellStyle = FeedbackDetailTextStyle(exportExcelInfo.ExcelFile, feedback.Status);

                reviewer = !string.IsNullOrWhiteSpace(feedback.ReviewerName)
                    ? string.Format("\nReviewed by {0} on {1}", feedback.ReviewerName, feedback.SolveDateString)
                    : string.Empty;
                row = exportExcelInfo.Worksheet.CreateRow(currentRow);
                NpoiExcelExtensions.RenderCell(
                    ref row,
                    ref currentColumn,
                    string.Format(feedbackPattern, feedback.Subject, feedback.FeedbackDateString, feedback.CreatorName, feedback.CreateDateString, feedback.DurationTime, reviewer),
                    textCellStyle);
                NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, feedback.Username, textCellStyle);
                NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, feedback.MainCategory, textCellStyle);
                NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, feedback.SubCategory, textCellStyle);
                NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, feedback.Status, textCellStyle);
                NpoiExcelExtensions.RenderCell(
                    ref row, 
                    ref currentColumn, 
                    feedback.IsRelated ? feedback.Username : feedback.RelatedCustomer,
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
        private ICellStyle FeedbackDetailTextStyle(HSSFWorkbook workbook, string status)
        {
            ICellStyle textCellStyle;
            if (string.Equals(status, "closed", StringComparison.OrdinalIgnoreCase))
            {
                textCellStyle = FeedbackDetailClosedTextStyle(workbook);    
            }
            else if (string.Equals(status, "pending", StringComparison.OrdinalIgnoreCase))
            {
                textCellStyle = FeedbackDetailPendingTextStyle(workbook);    
            }
            else
            {
                textCellStyle = FeedbackDetailTextStyle(workbook);
            }

            textCellStyle.WrapText = true;
            return textCellStyle;
        }

        private ICellStyle FeedbackDetailTextStyle(HSSFWorkbook workbook)
        {
            return NpoiExcelExtensions.TextStyle(
                workbook,
                NpoiExcelExtensions.BackgroundColorIndex,
                false,
                CRMHorizontalAlignment.Left);
        }

        private ICellStyle FeedbackDetailPendingTextStyle(HSSFWorkbook workbook)
        {
            return NpoiExcelExtensions.TextStyle(
                workbook,
                NpoiExcelExtensions.PendingStatusColorIndex,
                false,
                CRMHorizontalAlignment.Left);
        }

        private ICellStyle FeedbackDetailClosedTextStyle(HSSFWorkbook workbook)
        {
            return NpoiExcelExtensions.TextStyle(
                workbook,
                NpoiExcelExtensions.CloseStatusColorIndex,
                false,
                CRMHorizontalAlignment.Left);
        }

        #endregion
    }
}
