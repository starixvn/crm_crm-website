// --------------------------------------------------------------------------
// <copyright file="SummaryReportService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.04.08">Created</change>
// </history>

namespace Fanex.CRM.Report.Services.Implements.SummaryReport
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web;
    using Fanex.CRM.Infrastructure.Extension;
    using Fanex.CRM.Infrastructure.Logging;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Report.Criteria.SummaryReport;
    using Fanex.CRM.Report.Models.SummaryReport;
    using Fanex.CRM.Report.Repositories.Implements;
    using Fanex.CRM.Report.Repositories.Interfaces;
    using Fanex.CRM.Report.Resources;
    using Fanex.CRM.Report.Services.Interfaces;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;

    /// <summary>
    /// SummaryReportService class.
    /// </summary>
    public class SummaryReportService : ISummaryReportService
    {
        private const string SummaryReportSessionKey = "Fanex.CRM.Report.SummaryReport.SummaryReportSessionKey_{0}";
        private readonly ISummaryReportRepository _summaryRepository;

        public SummaryReportService()
            : this(null)
        {
        }

        public SummaryReportService(ISummaryReportRepository summaryRepository)
        {
            _summaryRepository = summaryRepository ?? new SummaryReportRepository();
        }

        /// <summary>
        /// Gets the summary report.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The SummaryReportModel object.</returns>
        public SummaryReportModel GetSummaryReport(GetSummaryReportCriteria criteria)
        {
            SummaryReportModel report = null;
            string sessionKey = string.Format(SummaryReportSessionKey, criteria.Year);
            try
            {
                report = criteria.IsCached
                    ? (SummaryReportModel)SessionExtension.GetSession(HttpContext.Current.Session, sessionKey)
                    : null;
                if (report == null)
                {
                    report = ParseSummaryReport(criteria);
                    report.ReportTitle = string.Format(Labels.SummaryReportTitle, criteria.Year);

                    // Add to session.
                    if (report != null)
                    {
                        SessionExtension.AddSession(HttpContext.Current.Session, sessionKey, report);
                    }
                }

                return report;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        /// <summary>
        /// Exports the summary report excel.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        public void ExportSummaryReportExcel(GetSummaryReportCriteria criteria)
        {
            criteria.IsCached = true;
            SummaryReportModel report = GetSummaryReport(criteria);
            string excelFileName = string.Format(Labels.SummaryReportExcelName, criteria.Year);

            try
            {
                HSSFWorkbook workbook = new HSSFWorkbook();
                ExportExcelInfoNPOI exportExcelInfo = new ExportExcelInfoNPOI
                {
                    ExcelFile = workbook,
                    Worksheet = workbook.CreateSheet(excelFileName),
                    StartRow = 0,
                    StartColumn = 0,
                    CurrentRow = 0,
                    MaxColumnIndex = report.Months.Count
                };

                ExportTitle(ref exportExcelInfo, report.ReportTitle);
                ExportHeader(ref exportExcelInfo, report.Months);
                if (report.CategorySummaries == null || report.CategorySummaries.Count == 0)
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
                    NpoiExcelExtensions.SetGrandTotalSeparatorColorIndex(ref exportExcelInfo);

                    ExportBody(ref exportExcelInfo, report.CategorySummaries);
                    ExportFooter(ref exportExcelInfo, report.GrandTotals);
                }

                NpoiExcelExtensions.ResponseStreamExcel(exportExcelInfo.ExcelFile, excelFileName);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        /// <summary>
        /// Parses the summary report.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The SummaryReportModel object.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        private SummaryReportModel ParseSummaryReport(GetSummaryReportCriteria criteria)
        {
            string month;
            int numberOfIssues;
            IList<CategorySummary> subCategorySummaries;
            CategorySummary category, subCategory;
            IList<CategorySummary> categorySummaries = new List<CategorySummary>();

            var rawData = _summaryRepository.GetSummaryReport(criteria).ToList();

            // Get Month.
            var monthData = rawData.Select(s => s.Month.ToUpper()).Distinct().ToList();

            IDictionary<string, Summary> categoryTotal, summaryHash;
            IDictionary<string, Summary> grandTotal = InitializeSummaries(monthData);

            // Group by MainCategory.
            var mainCategoryGroup = rawData.GroupBy(s => new { MainCategoryId = s.MainCategoryId, MainCategory = s.MainCategory });
            foreach (var mainKey in mainCategoryGroup)
            {
                categoryTotal = InitializeSummaries(monthData);
                subCategorySummaries = new List<CategorySummary>();
                category = new CategorySummary
                {
                    Id = mainKey.Key.MainCategoryId,
                    Name = mainKey.Key.MainCategory,
                    SubCategories = subCategorySummaries
                };

                // Group by SubCategory.
                var subCategoryGroup = mainKey.GroupBy(s => new { SubCategoryId = s.SubCategoryId, SubCategory = s.SubCategory });
                foreach (var subKey in subCategoryGroup)
                {
                    summaryHash = InitializeSummaries(monthData);
                    subCategory = new CategorySummary
                    {
                        Id = subKey.Key.SubCategoryId,
                        Name = subKey.Key.SubCategory
                    };

                    foreach (var summary in subKey)
                    {
                        month = summary.Month.ToUpper();
                        numberOfIssues = summary.NoOfIssues;
                        summaryHash[month].Amount = numberOfIssues;

                        // Total of Category.
                        categoryTotal[month].Amount += numberOfIssues;

                        // Grand Total.
                        grandTotal[month].Amount += numberOfIssues;
                    }

                    subCategory.Summaries = summaryHash.Values.ToList();
                    subCategorySummaries.Add(subCategory);
                }

                category.Summaries = categoryTotal.Values.ToList();
                categorySummaries.Add(category);
            }

            return new SummaryReportModel
            {
                CategorySummaries = categorySummaries,
                GrandTotals = grandTotal.Values.ToList(),
                Months = monthData
            };
        }

        private IDictionary<string, Summary> InitializeSummaries(IList<string> months)
        {
            IDictionary<string, Summary> summaries = new Dictionary<string, Summary>();
            foreach (var item in months)
            {
                summaries.Add(item, new Summary { Month = item, Amount = 0 });
            }

            return summaries;
        }

        #region Export excel helper

        /// <summary>
        /// Exports the title.
        /// </summary>
        /// <param name="exportExcelInfo">The export excel info.</param>
        /// <param name="title">The title.</param>
        private void ExportTitle(ref ExportExcelInfoNPOI exportExcelInfo, string title)
        {
            int currentColumn = exportExcelInfo.StartColumn;
            int currentRow = exportExcelInfo.CurrentRow;
            int monthColWidth = 80 * NpoiExcelExtensions.FontWidth;

            NpoiExcelExtensions.SetHeaderColorIndex(ref exportExcelInfo);
            ISheet worksheet = exportExcelInfo.Worksheet;
            worksheet.SetColumnWidth(currentColumn++, 300 * NpoiExcelExtensions.FontWidth);
            worksheet.SetColumnWidth(currentColumn++, monthColWidth);
            worksheet.SetColumnWidth(currentColumn++, monthColWidth);
            worksheet.SetColumnWidth(currentColumn++, monthColWidth);
            worksheet.SetColumnWidth(currentColumn++, monthColWidth);
            worksheet.SetColumnWidth(currentColumn++, monthColWidth);
            worksheet.SetColumnWidth(currentColumn++, monthColWidth);
            worksheet.SetColumnWidth(currentColumn++, monthColWidth);
            worksheet.SetColumnWidth(currentColumn++, monthColWidth);
            worksheet.SetColumnWidth(currentColumn++, monthColWidth);
            worksheet.SetColumnWidth(currentColumn++, monthColWidth);
            worksheet.SetColumnWidth(currentColumn++, monthColWidth);
            worksheet.SetColumnWidth(currentColumn++, monthColWidth);

            CellRangeAddress cellRangeAddress = new CellRangeAddress(
                currentRow,
                currentRow + 1,
                exportExcelInfo.StartColumn,
                exportExcelInfo.MaxColumnIndex);
            ICellStyle headerStyle = NpoiExcelExtensions.HeaderStyle(exportExcelInfo.ExcelFile, new ExcelStyleParamNPOI());
            NpoiExcelExtensions.RenderMergedCells(ref exportExcelInfo, cellRangeAddress, title, headerStyle);
            currentRow += 2;

            NpoiExcelExtensions.RenderEmptyRow(ref exportExcelInfo, currentRow);
            currentRow++;

            exportExcelInfo.CurrentRow = currentRow;
        }

        /// <summary>
        /// Exports the header.
        /// </summary>
        /// <param name="exportExcelInfo">The export excel info.</param>
        /// <param name="months">The months.</param>
        private void ExportHeader(ref ExportExcelInfoNPOI exportExcelInfo, IList<string> months)
        {
            int currentColumn = exportExcelInfo.StartColumn;
            IRow row = exportExcelInfo.Worksheet.CreateRow(exportExcelInfo.CurrentRow);
            ICellStyle headerStyle = NpoiExcelExtensions.HeaderStyle(exportExcelInfo.ExcelFile, new ExcelStyleParamNPOI());

            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, Labels.Categories, headerStyle);
            foreach (string item in months)
            {
                NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, item, headerStyle);
            }
            
            exportExcelInfo.CurrentRow++;
        }

        /// <summary>
        /// Exports the body.
        /// </summary>
        /// <param name="exportExcelInfo">The export excel info.</param>
        /// <param name="categorySummaries">The category summaries.</param>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        private void ExportBody(ref ExportExcelInfoNPOI exportExcelInfo, IList<CategorySummary> categorySummaries)
        {
            HSSFWorkbook workbook = exportExcelInfo.ExcelFile;
            ICellStyle categoryCellStyle = GetCategoryCellStyle(workbook);
            ICellStyle categoryTotalCellStyle = GetCategoryTotalCellStyle(workbook);
            ICellStyle subCategoryCellStyle = GetSubCategoryCellStyle(workbook);
            ICellStyle monthCellStyle = GetMonthCellStyle(workbook);

            int currentColumn, currentRow = exportExcelInfo.CurrentRow;
            CategorySummary category, subCategory;
            Summary summay;
            IRow categoryRow, subCategortRow;
            int reportLength = categorySummaries.Count;
            int categoryTotalLen, subCategoriesLen, monthLen;
            for (int i = 0; i < reportLength; i++)
            {
                category = categorySummaries[i];
                currentColumn = exportExcelInfo.CurrentColumn;

                categoryRow = exportExcelInfo.Worksheet.GetRow(currentRow) ?? exportExcelInfo.Worksheet.CreateRow(currentRow);
                categoryRow.Height *= 2;
                NpoiExcelExtensions.RenderCell(ref categoryRow, ref currentColumn, category.Name, categoryCellStyle);

                categoryTotalLen = category.Summaries.Count;
                for (int l = 0; l < categoryTotalLen; l++)
                {
                    summay = category.Summaries[l];
                    if (summay.Amount == 0)
                    {
                        NpoiExcelExtensions.RenderCell(ref categoryRow, ref currentColumn, string.Empty, categoryTotalCellStyle);
                    }
                    else
                    {
                        NpoiExcelExtensions.RenderCell(ref categoryRow, ref currentColumn, summay.Amount, categoryTotalCellStyle);
                    }
                }

                currentRow++;
                subCategoriesLen = category.SubCategories.Count;
                for (int j = 0; j < subCategoriesLen; j++)
                {
                    currentColumn = exportExcelInfo.CurrentColumn;
                    subCategory = category.SubCategories[j];

                    subCategortRow = exportExcelInfo.Worksheet.GetRow(currentRow) ?? exportExcelInfo.Worksheet.CreateRow(currentRow);
                    NpoiExcelExtensions.RenderCell(ref subCategortRow, ref currentColumn, subCategory.Name, subCategoryCellStyle);

                    monthLen = subCategory.Summaries.Count;
                    for (int k = 0; k < monthLen; k++)
                    {
                        summay = subCategory.Summaries[k];
                        if (summay.Amount == 0)
                        {
                            NpoiExcelExtensions.RenderCell(ref subCategortRow, ref currentColumn, string.Empty, monthCellStyle);   
                        }
                        else
                        {
                            NpoiExcelExtensions.RenderCell(ref subCategortRow, ref currentColumn, summay.Amount, monthCellStyle);   
                        }
                    }

                    currentRow++;
                }

                exportExcelInfo.Worksheet.GroupRow(currentRow - subCategoriesLen, currentRow);
                exportExcelInfo.Worksheet.SetRowGroupCollapsed(currentRow - subCategoriesLen, false);
            }

            exportExcelInfo.CurrentRow = currentRow;
        }

        /// <summary>
        /// Exports the footer.
        /// </summary>
        /// <param name="exportExcelInfo">The export excel info.</param>
        /// <param name="grandTotals">The total.</param>
        private void ExportFooter(ref ExportExcelInfoNPOI exportExcelInfo, IList<Summary> grandTotals)
        {
            int currentRow = exportExcelInfo.CurrentRow;
            int currentColumn = exportExcelInfo.CurrentColumn;

            HSSFWorkbook workbook = exportExcelInfo.ExcelFile;
            ICellStyle monthCellStyle = GetGrandTotalMonthCellStyle(workbook);
            NpoiExcelExtensions.SetBorderToFirstGrandTotalCell(monthCellStyle);

            ICellStyle totalCellStyle = GetGrandTotalCellStyle(workbook);
            NpoiExcelExtensions.SetBorderToFirstGrandTotalCell(totalCellStyle);

            IRow row = exportExcelInfo.Worksheet.GetRow(currentRow) ?? exportExcelInfo.Worksheet.CreateRow(currentRow);
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, Labels.GrandTotal, totalCellStyle);

            Summary summary;
            int totalLen = grandTotals.Count;
            for (int i = 0; i < totalLen; i++)
            {
                summary = grandTotals[i];
                if (summary.Amount == 0)
                {
                    NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, string.Empty, monthCellStyle);
                }
                else
                {
                    NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, summary.Amount, monthCellStyle);
                }
            }
        }

        /// <summary>
        /// Gets the category cell style.
        /// </summary>
        /// <param name="workbook">The workbook.</param>
        /// <returns>ICellStyle object.</returns>
        private ICellStyle GetCategoryCellStyle(HSSFWorkbook workbook)
        {
            ICellStyle cellStyle = NpoiExcelExtensions.TextStyle(
                workbook,
                NpoiExcelExtensions.GroupBackgroundColorIndex,
                true,
                NpoiExcelExtensions.Alignment.Left);
            var font = workbook.CreateFont();
            font.FontHeightInPoints = NpoiExcelExtensions.FontSize;
            font.FontName = NpoiExcelExtensions.FontName;
            font.Color = NpoiExcelExtensions.GroupColorIndex;
            font.Boldweight = (short)FontBoldWeight.BOLD;
            cellStyle.SetFont(font);
            NpoiExcelExtensions.SetBorderToCell(cellStyle);

            return cellStyle;
        }

        /// <summary>
        /// Gets the category total cell style.
        /// </summary>
        /// <param name="workbook">The workbook.</param>
        /// <returns>ICellStyle object.</returns>
        private ICellStyle GetCategoryTotalCellStyle(HSSFWorkbook workbook)
        {
            ICellStyle cellStyle = NpoiExcelExtensions.IntNumberStyle(
                workbook,
                NpoiExcelExtensions.GroupBackgroundColorIndex,
                true,
                HorizontalAlignment.RIGHT);
            var font = workbook.CreateFont();
            font.FontHeightInPoints = NpoiExcelExtensions.FontSize;
            font.FontName = NpoiExcelExtensions.FontName;
            font.Color = NpoiExcelExtensions.GroupColorIndex;
            font.Boldweight = (short)FontBoldWeight.BOLD;
            cellStyle.SetFont(font);
            NpoiExcelExtensions.SetBorderToCell(cellStyle);

            return cellStyle;
        }

        private ICellStyle GetGrandTotalCellStyle(HSSFWorkbook workbook)
        {
            return NpoiExcelExtensions.TextStyle(
                workbook,
                NpoiExcelExtensions.GrandTotalRowColorIndex,
                true,
                NpoiExcelExtensions.Alignment.Left);
        }

        private ICellStyle GetSubCategoryCellStyle(HSSFWorkbook workbook)
        {
            return NpoiExcelExtensions.TextStyle(
                workbook,
                NpoiExcelExtensions.BackgroundColorIndex,
                false,
                NpoiExcelExtensions.Alignment.Left);
        }

        private ICellStyle GetMonthCellStyle(HSSFWorkbook workbook)
        {
            return NpoiExcelExtensions.IntNumberStyle(
                workbook,
                NpoiExcelExtensions.BackgroundColorIndex,
                false,
                HorizontalAlignment.RIGHT);
        }

        private ICellStyle GetGrandTotalMonthCellStyle(HSSFWorkbook workbook)
        {
            return NpoiExcelExtensions.IntNumberStyle(
                workbook,
                NpoiExcelExtensions.GrandTotalRowColorIndex,
                true,
                HorizontalAlignment.RIGHT);
        }

        #endregion
    }
}
