// --------------------------------------------------------------------------
// <copyright file="SummaryReportService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// <change who="Kan.Nguyen" date="2014.04.08">Created</change>
// </history>

namespace Fanex.CRM.Report.Services.SummaryReport
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web;
    using Fanex.CRM.Common.Extensions;
    using Fanex.CRM.Common.Helpers;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Common.Models.Npoi;
    using Fanex.CRM.Common.Services;
    using Fanex.CRM.Infrastructure.Extensions;
    using Fanex.CRM.Infrastructure.Helpers;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Report.Criteria.SummaryReport;
    using Fanex.CRM.Report.Models.SummaryReport;
    using Fanex.CRM.Report.Repositories.SummaryReport;
    using Fanex.CRM.Report.Resources;
    using Fanex.CRM.Setting.Services;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;

    /// <summary>
    /// SummaryReportService class.
    /// </summary>
    public class SummaryReportService : ISummaryReportService
    {
        private const string SummaryReportSessionKey = "Fanex.CRM.Report.SummaryReport.SummaryReportSessionKey_{0}";
        private readonly ISummaryReportRepository _summaryRepository;
        private readonly ISettingService _settingServices;

        public SummaryReportService()
            : this(null, null)
        {
        }

        public SummaryReportService(ISummaryReportRepository summaryRepository, ISettingService settingServices)
        {
            _summaryRepository = summaryRepository ?? new SummaryReportRepository();
            _settingServices = settingServices ?? new SettingService();
        }

        /// <summary>
        /// Gets all settings.
        /// </summary>
        /// <returns>The object with all settings.</returns>
        public dynamic AllSettings()
        {
            var minReportDate = AppSetting.MinReportDate.Year;
            var currentYear = DateTimeUtilize.CurrentClientTime().Year;
            var yearRange = new List<SettingInfo>();
            for (int i = minReportDate; i <= currentYear; i++)
            {
                yearRange.Add(new SettingInfo { Id = i, Value = i.ToString() });
            }

            var siteTypes = _settingServices.AllSiteType();
            var categories = _settingServices.AllMainCategory();
            return new
            {
                YearRange = yearRange,
                SiteTypes = siteTypes,
                Categories = categories
            };
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
                    //// Add to session.
                    SessionExtension.AddSession(HttpContext.Current.Session, sessionKey, report);
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
                ExportExcelInfo exportExcelInfo = new ExportExcelInfo
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
            if (string.IsNullOrWhiteSpace(criteria.Categories))
            {
                return new SummaryReportModel();
            }

            string month;
            int numberOfIssues;
            IList<CategorySummary> subCategorySummaries;
            CategorySummary category, subCategory;
            IList<CategorySummary> categorySummaries = new List<CategorySummary>();

            // Filter.
            int[] categoryIds = criteria.Categories.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(c => int.Parse(c))
            .ToArray();
            var rawData = _summaryRepository.GetSummaryReport(criteria)
            .Where(c => categoryIds.Contains(c.MainCategoryId))
            .ToList();

            // Get Month.
            var monthData = rawData.OrderBy(s => s.M).Select(s => s.Month.ToUpper()).Distinct().ToList();

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
        private void ExportTitle(ref ExportExcelInfo exportExcelInfo, string title)
        {
            NpoiExcelExtensions.SetHeaderColorIndex(ref exportExcelInfo);
            var columnWidths = new List<int>() { 300 };
            columnWidths.AddRange(Enumerable.Repeat(80, 12));
            exportExcelInfo.InitColumnWidthByFontWidth(columnWidths);

            ICellStyle headerStyle = NpoiExcelExtensions.HeaderStyle(exportExcelInfo.ExcelFile, new ExcelStyleParam());
            exportExcelInfo.RenderHeaderTitle(title, headerStyle);
        }

        /// <summary>
        /// Exports the header.
        /// </summary>
        /// <param name="exportExcelInfo">The export excel info.</param>
        /// <param name="months">The months.</param>
        private void ExportHeader(ref ExportExcelInfo exportExcelInfo, IList<string> months)
        {
            int currentColumn = exportExcelInfo.StartColumn;
            IRow row = exportExcelInfo.Worksheet.CreateRow(exportExcelInfo.CurrentRow);
            ICellStyle headerStyle = NpoiExcelExtensions.HeaderStyle(exportExcelInfo.ExcelFile, new ExcelStyleParam());

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
        private void ExportBody(ref ExportExcelInfo exportExcelInfo, IList<CategorySummary> categorySummaries)
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
        private void ExportFooter(ref ExportExcelInfo exportExcelInfo, IList<Summary> grandTotals)
        {
            int currentRow = exportExcelInfo.CurrentRow;
            int currentColumn = exportExcelInfo.CurrentColumn;

            HSSFWorkbook workbook = exportExcelInfo.ExcelFile;
            ICellStyle monthCellStyle = GetGrandTotalMonthCellStyle(workbook);

            ICellStyle totalCellStyle = GetGrandTotalCellStyle(workbook);

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
            return NpoiExcelExtensions.TextStyle(
                workbook,
                NpoiExcelExtensions.GroupBackgroundColorIndex,
                true,
                CRMHorizontalAlignment.Left,
                NpoiExcelExtensions.GroupColorIndex);
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
            CRMHorizontalAlignment.Right,
            NpoiExcelExtensions.GrandTotalTextColorIndex);
            var font = workbook.CreateFont();
            font.FontHeightInPoints = NpoiExcelExtensions.FontSize;
            font.FontName = NpoiExcelExtensions.FontName;
            font.Color = NpoiExcelExtensions.GroupColorIndex;
            font.Boldweight = (short)FontBoldWeight.Bold;
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
            CRMHorizontalAlignment.Left,
            NpoiExcelExtensions.GrandTotalTextColorIndex);
        }

        private ICellStyle GetSubCategoryCellStyle(HSSFWorkbook workbook)
        {
            return NpoiExcelExtensions.TextStyle(
            workbook,
            NpoiExcelExtensions.BackgroundColorIndex,
            false,
            CRMHorizontalAlignment.Left);
        }

        private ICellStyle GetMonthCellStyle(HSSFWorkbook workbook)
        {
            return NpoiExcelExtensions.IntNumberStyle(
            workbook,
            NpoiExcelExtensions.BackgroundColorIndex,
            false,
            CRMHorizontalAlignment.Right);
        }

        private ICellStyle GetGrandTotalMonthCellStyle(HSSFWorkbook workbook)
        {
            return NpoiExcelExtensions.IntNumberStyle(
            workbook,
            NpoiExcelExtensions.GrandTotalRowColorIndex,
            true,
            CRMHorizontalAlignment.Right,
            NpoiExcelExtensions.GrandTotalTextColorIndex);
        }

        #endregion
    }
}