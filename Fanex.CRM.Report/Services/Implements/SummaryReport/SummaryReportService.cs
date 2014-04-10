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

        private IList<Summary> InitializeSummaries
        {
            get
            {
                return new List<Summary>
                {
                    new Summary { Month = Labels.JAN, Amount = 0 },
                    new Summary { Month = Labels.FEB, Amount = 0 },
                    new Summary { Month = Labels.MAR, Amount = 0 },
                    new Summary { Month = Labels.APR, Amount = 0 },
                    new Summary { Month = Labels.MAY, Amount = 0 },
                    new Summary { Month = Labels.JUN, Amount = 0 },
                    new Summary { Month = Labels.JUL, Amount = 0 },
                    new Summary { Month = Labels.AUG, Amount = 0 },
                    new Summary { Month = Labels.SEP, Amount = 0 },
                    new Summary { Month = Labels.OCT, Amount = 0 },
                    new Summary { Month = Labels.NOV, Amount = 0 },
                    new Summary { Month = Labels.DEC, Amount = 0 }
                };
            }
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

            HSSFWorkbook workbook = new HSSFWorkbook();
            ExportExcelInfoNPOI exportExcelInfo = new ExportExcelInfoNPOI
            {
                ExcelFile = workbook,
                Worksheet = workbook.CreateSheet(excelFileName),
                StartRow = 0,
                StartColumn = 0,
                CurrentRow = 0,
                MaxColumnIndex = 12
            };

            ExportTitle(ref exportExcelInfo, report.ReportTitle);
            ExportHeader(ref exportExcelInfo);
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
                ExportFooter(ref exportExcelInfo, report.GrandTotal);
            }

            NpoiExcelExtensions.ResponseStreamExcel(exportExcelInfo.ExcelFile, excelFileName);
        }

        /// <summary>
        /// Parses the summary report.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The SummaryReportModel object.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        private SummaryReportModel ParseSummaryReport(GetSummaryReportCriteria criteria)
        {
            IList<CategorySummary> categorySummaries = new List<CategorySummary>();
            SubCategorySummary grandTotal = new SubCategorySummary 
            {
                SubCategoryName = Labels.GrandTotal,
                Summaries = InitializeSummaries
            };

            // Group by MainCategory.
            var rawData = _summaryRepository.GetSummaryReport(criteria)
                .GroupBy(s => new { MainCategoryId = s.MainCategoryId, MainCategory = s.MainCategory });
            foreach (var groupKey in rawData)
            {
                CategorySummary categorySummary = new CategorySummary
                {
                    CategoryId = groupKey.Key.MainCategoryId,
                    CategoryName = groupKey.Key.MainCategory,
                    Total = InitializeSummaries
                };

                categorySummaries.Add(categorySummary);

                IDictionary<int, IList<Summary>> subCategoryHash = new Dictionary<int, IList<Summary>>();
                IList<SubCategorySummary> subCategorySummaries = new List<SubCategorySummary>();
                foreach (var item in groupKey)
                {
                    IList<Summary> summaries;
                    Summary summary = new Summary { Month = item.Month.ToUpper(), Amount = item.NoOfIssues }; 
                    SubCategorySummary subCategorySummary = new SubCategorySummary
                    {
                        SubCategoryId = item.SubCategoryId,
                        SubCategoryName = item.SubCategory
                    };

                    int monthNo = item.M - 1;
                    if (subCategoryHash.TryGetValue(subCategorySummary.SubCategoryId, out summaries))
                    {
                        summaries[monthNo] = summary;
                    }
                    else
                    {
                        summaries = InitializeSummaries;
                        summaries[monthNo] = summary;
                        subCategoryHash.Add(subCategorySummary.SubCategoryId, summaries);

                        subCategorySummary.Summaries = summaries;
                        subCategorySummaries.Add(subCategorySummary);
                    }

                    // Total of Category.
                    categorySummary.Total[monthNo].Amount += summary.Amount;

                    // Grand Total
                    grandTotal.Summaries[monthNo].Amount += summary.Amount;
                }

                categorySummary.SubCategorySummaries = subCategorySummaries;
            }

            return new SummaryReportModel 
            {
                CategorySummaries = categorySummaries,
                GrandTotal = grandTotal
            };
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
        private void ExportHeader(ref ExportExcelInfoNPOI exportExcelInfo)
        {
            int currentColumn = exportExcelInfo.StartColumn;
            IRow row = exportExcelInfo.Worksheet.CreateRow(exportExcelInfo.CurrentRow);
            ICellStyle headerStyle = NpoiExcelExtensions.HeaderStyle(exportExcelInfo.ExcelFile, new ExcelStyleParamNPOI());

            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, Labels.Categories, headerStyle);
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, Labels.JAN, headerStyle);
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, Labels.FEB, headerStyle);
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, Labels.MAR, headerStyle);
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, Labels.APR, headerStyle);
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, Labels.MAY, headerStyle);
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, Labels.JUN, headerStyle);
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, Labels.JUL, headerStyle);
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, Labels.AUG, headerStyle);
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, Labels.SEP, headerStyle);
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, Labels.OCT, headerStyle);
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, Labels.NOV, headerStyle);
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, Labels.DEC, headerStyle);
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

            int startColumn, currentColumn, currentRow = exportExcelInfo.CurrentRow;
            CategorySummary categorySummary;
            SubCategorySummary subCategorySummary;
            Summary summay;
            CellRangeAddress categoryRow;
            IRow subCategortRow;
            int reportLength = categorySummaries.Count;
            int categoryTotalLen, subCategoriesLen, monthLen;
            for (int i = 0; i < reportLength; i++)
            {
                categorySummary = categorySummaries[i];
                currentColumn = exportExcelInfo.CurrentColumn;
                startColumn = exportExcelInfo.StartColumn;

                categoryRow = new CellRangeAddress(currentRow, currentRow + 1, startColumn, startColumn);
                startColumn++;

                NpoiExcelExtensions.RenderMergedCells(ref exportExcelInfo, categoryRow, categorySummary.CategoryName, categoryCellStyle);

                categoryTotalLen = categorySummary.Total.Count;
                for (int l = 0; l < categoryTotalLen; l++)
                {
                    categoryRow = new CellRangeAddress(currentRow, currentRow + 1, startColumn, startColumn);
                    startColumn++;
                    
                    summay = categorySummary.Total[l];
                    if (summay.Amount == 0)
                    {
                        NpoiExcelExtensions.RenderMergedCells(ref exportExcelInfo, categoryRow, string.Empty, categoryTotalCellStyle);
                    }
                    else
                    {
                        NpoiExcelExtensions.RenderMergedCells(ref exportExcelInfo, categoryRow, summay.Amount, categoryTotalCellStyle);
                    }
                }

                currentRow += 2;
                subCategoriesLen = categorySummary.SubCategorySummaries.Count;
                for (int j = 0; j < subCategoriesLen; j++)
                {
                    currentColumn = exportExcelInfo.CurrentColumn;
                    subCategorySummary = categorySummary.SubCategorySummaries[j];

                    subCategortRow = exportExcelInfo.Worksheet.GetRow(currentRow) ?? exportExcelInfo.Worksheet.CreateRow(currentRow);
                    NpoiExcelExtensions.RenderCell(ref subCategortRow, ref currentColumn, subCategorySummary.SubCategoryName, subCategoryCellStyle);

                    monthLen = subCategorySummary.Summaries.Count;
                    for (int k = 0; k < monthLen; k++)
                    {
                        summay = subCategorySummary.Summaries[k];
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
            }

            exportExcelInfo.CurrentRow = currentRow;
        }

        /// <summary>
        /// Exports the footer.
        /// </summary>
        /// <param name="exportExcelInfo">The export excel info.</param>
        /// <param name="total">The total.</param>
        private void ExportFooter(ref ExportExcelInfoNPOI exportExcelInfo, SubCategorySummary total)
        {
            int currentRow = exportExcelInfo.CurrentRow;
            int currentColumn = exportExcelInfo.CurrentColumn;

            HSSFWorkbook workbook = exportExcelInfo.ExcelFile;
            ICellStyle monthCellStyle = GetGrandTotalMonthCellStyle(workbook);
            NpoiExcelExtensions.SetBorderToFirstGrandTotalCell(monthCellStyle);

            ICellStyle totalCellStyle = GetGrandTotalCellStyle(workbook);
            NpoiExcelExtensions.SetBorderToFirstGrandTotalCell(totalCellStyle);

            IRow row = exportExcelInfo.Worksheet.GetRow(currentRow) ?? exportExcelInfo.Worksheet.CreateRow(currentRow);
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, total.SubCategoryName, totalCellStyle);

            Summary summary;
            int totalLen = total.Summaries.Count;
            for (int i = 0; i < totalLen; i++)
            {
                summary = total.Summaries[i];
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
