// --------------------------------------------------------------------------
// <copyright file="CasinoReportService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.07.30">Created</change>
// </history>

namespace Fanex.CRM.Report.Services.CasinoReport
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web;
    using Fanex.CRM.Common.Extensions;
    using Fanex.CRM.Common.Helpers;
    using Fanex.CRM.Common.Models.Npoi;
    using Fanex.CRM.Infrastructure.Extensions;
    using Fanex.CRM.Infrastructure.Helpers;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Report.Criteria.CasinoReport;
    using Fanex.CRM.Report.Entities.CasinoReport;
    using Fanex.CRM.Report.Models.CasinoReport;
    using Fanex.CRM.Report.Repositories.CasinoReport;
    using Fanex.CRM.Report.Resources;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;

    /// <summary>
    /// Casino Report Service.
    /// </summary>
    public class CasinoReportService : ICasinoReportService
    {
        private const int CasinoDailyType = 1;
        private const int CasinoWeeklyType = 3;

        // Casino daily summary.
        private const string CasinoDailySummarySessionKey = "Fanex.CRM.Report.Services.CasinoReport.CasinoDailySummary";
        private const string CasinoDailyDetailsSessionKey = "Fanex.CRM.Report.Services.CasinoReport.CasinoDailyDetails";

        // Casino daily summary.
        private const string CasinoWeeklySummarySessionKey = "Fanex.CRM.Report.Services.CasinoReport.CasinoWeeklySummary";
        private const string CasinoWeeklyDetailSessionKey = "Fanex.CRM.Report.Services.CasinoReport.CasinoWeeklyDetail";

        private readonly ICasinoReportRepository _casinoReportRepository;

        public CasinoReportService()
            : this(null)
        {
        }

        public CasinoReportService(ICasinoReportRepository casinoReportRepository)
        {
            _casinoReportRepository = casinoReportRepository ?? new CasinoReportRepository();
        }

        /// <summary>
        /// Gets the casino daily summaries.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>
        /// The Casino Daily Summary object.
        /// </returns>
        public CasinoDailySummary GetCasinoDailySummary(GetCasinoSummaryCriteria criteria)
        {
            string key = CasinoDailySummarySessionKey;
            CasinoDailySummary report = null;
            if (criteria.FromCache)
            {
                report = (CasinoDailySummary)SessionExtension.GetSession(HttpContext.Current.Session, key);
            }

            if (report == null)
            {
                string title = string.Format(Labels.CasinoDailySummaryTitle, criteria.FromDate.ToShortFormatedDate(), criteria.ToDate.ToShortFormatedDate());

                // Get data from DB.
                criteria.FromDate = DateTimeUtilize.ChangeClientFromTimeToServerTime(criteria.FromDate);
                criteria.ToDate = DateTimeUtilize.ChangeClientToTimeToServerTime(criteria.ToDate);
                try
                {
                    report = _casinoReportRepository.GetCasinoDailySummary(criteria);
                    report.Title = title;

                    SessionExtension.AddSession(HttpContext.Current.Session, key, report);
                }
                catch (Exception ex)
                {
                    LogExtensions.ExceptionHandler(ex);
                    throw;
                }
            }

            return report;
        }

        /// <summary>
        /// Exports the get casino daily summaries.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        public void ExportCasinoDailySummary(GetCasinoSummaryCriteria criteria)
        {
            criteria.FromCache = true;
            CasinoDailySummary report = GetCasinoDailySummary(criteria);
            int maxColumnIndex = 0;
            if (report != null && report.CategoryCasinoGroups != null && report.CategoryCasinoGroups.Count > 0)
            {
                maxColumnIndex = report.CategoryCasinoGroups[0].CasinoGroups.Count;
                maxColumnIndex *= report.CategoryCasinoGroups[0].CasinoGroups[0].Communications.Count;
            }

            string excelFileName = string.Format(Labels.CasinoSummaryExcelName, FormatReportDate(criteria.FromDate), FormatReportDate(criteria.ToDate));
            HSSFWorkbook workbook = new HSSFWorkbook();
            ExportExcelInfo exportExcelInfo = new ExportExcelInfo
            {
                ExcelFile = workbook,
                Worksheet = workbook.CreateSheet(excelFileName),
                StartRow = 0,
                StartColumn = 0,
                CurrentRow = 0,
                MaxColumnIndex = maxColumnIndex
            };

            GenerateCasinoDailySummaryTitle(exportExcelInfo, report.Title);
            GenerateCasinoDailySummaryHeader(exportExcelInfo, report.CategoryCasinoGroups);
            if (maxColumnIndex == 0)
            {
                NpoiExcelExtensions.RenderNoInfoMessage(exportExcelInfo, exportExcelInfo.CurrentRow);
            }
            else
            {
                NpoiExcelExtensions.SetColorAtIndex(exportExcelInfo, NpoiExcelExtensions.EvenRowColor, NpoiExcelExtensions.EvenRowColorIndex);
                NpoiExcelExtensions.SetGrandTotalColorIndex(exportExcelInfo);

                GenerateCasinoDailySummaryBody(exportExcelInfo, report.CategoryCasinoGroups);
                GenerateCasinoDailySummaryFooter(exportExcelInfo, report.Total);
            }

            NpoiExcelExtensions.ResponseStreamExcel(exportExcelInfo.ExcelFile, excelFileName);
        }

        /// <summary>
        /// Gets the casino daily details.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The Casino Daily Detail object.</returns>
        public CasinoDetail GetCasinoDailyDetail(GetCasinoDetailCriteria criteria)
        {
            string key = CasinoDailyDetailsSessionKey;
            IList<CasinoDetailEntity> casinoDailyDetails = null;
            if (criteria.FromCache)
            {
                casinoDailyDetails = (IList<CasinoDetailEntity>)SessionExtension.GetSession(HttpContext.Current.Session, key);
            }

            string title = string.Format(Labels.CasinoDailyDetailTitle, criteria.FromDate.ToShortFormatedDate(), criteria.ToDate.ToShortFormatedDate());
            if (casinoDailyDetails == null)
            {
                // Get data from DB.
                criteria.FromDate = DateTimeUtilize.ChangeClientFromTimeToServerTime(criteria.FromDate);
                criteria.ToDate = DateTimeUtilize.ChangeClientToTimeToServerTime(criteria.ToDate);
                try
                {
                    casinoDailyDetails = _casinoReportRepository.GetCasinoDetails(criteria);
                    SessionExtension.AddSession(HttpContext.Current.Session, key, casinoDailyDetails);
                }
                catch (Exception ex)
                {
                    LogExtensions.ExceptionHandler(ex);
                    throw;
                }
            }

            // Pagination.
            int totalItems = casinoDailyDetails != null ? casinoDailyDetails.Count : 0;
            if (totalItems > 0)
            {
                int page = criteria.PageIndex - 1;
                return new CasinoDetail
                {
                    Title = title,
                    TotalItems = totalItems,
                    CasinoDetails = criteria.FullReport ? casinoDailyDetails : casinoDailyDetails.Skip(criteria.ItemsPerPage * page).Take(criteria.ItemsPerPage).ToList(),
                    CurrentPage = criteria.PageIndex,
                    ItemsPerPage = criteria.ItemsPerPage
                };
            }

            return new CasinoDetail { Title = title };
        }

        /// <summary>
        /// Exports the casino daily detail.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        public void ExportCasinoDailyDetail(GetCasinoDetailCriteria criteria)
        {
            criteria.FromCache = criteria.FullReport = true;
            CasinoDetail report = GetCasinoDailyDetail(criteria);

            int maxColumnIndex = 18;
            string excelFileName = string.Format(Labels.CasinoDetailExcelName, FormatReportDate(criteria.FromDate), FormatReportDate(criteria.ToDate));
            HSSFWorkbook workbook = new HSSFWorkbook();
            ExportExcelInfo exportExcelInfo = new ExportExcelInfo
            {
                ExcelFile = workbook,
                Worksheet = workbook.CreateSheet(excelFileName),
                StartRow = 0,
                StartColumn = 0,
                CurrentRow = 0,
                MaxColumnIndex = maxColumnIndex
            };

            GenerateCasinoDetailTitle(exportExcelInfo, report.Title.ToUpper());
            GenerateCasinoDetailHeader(exportExcelInfo);
            if (report.CasinoDetails == null || report.CasinoDetails.Count == 0)
            {
                NpoiExcelExtensions.RenderNoInfoMessage(exportExcelInfo, exportExcelInfo.CurrentRow);
            }
            else
            {
                NpoiExcelExtensions.SetColorAtIndex(exportExcelInfo, NpoiExcelExtensions.EvenRowColor, NpoiExcelExtensions.EvenRowColorIndex);
                NpoiExcelExtensions.SetGrandTotalColorIndex(exportExcelInfo);

                GenerateCasinoDetailBody(exportExcelInfo, report.CasinoDetails);
            }

            NpoiExcelExtensions.ResponseStreamExcel(exportExcelInfo.ExcelFile, excelFileName);
        }

        /// <summary>
        /// Gets the casino weekly summary.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The Casino Weekly Summary object.</returns>
        public CasinoWeeklySummary GetCasinoWeeklySummary(GetCasinoSummaryCriteria criteria)
        {
            string key = CasinoWeeklySummarySessionKey;
            CasinoWeeklySummary report = null;
            if (criteria.FromCache)
            {
                report = (CasinoWeeklySummary)SessionExtension.GetSession(HttpContext.Current.Session, key);
            }

            if (report == null)
            {
                string title = string.Format(Labels.CasinoWeeklySummaryTitle, criteria.FromDate.ToShortFormatedDate(), criteria.ToDate.ToShortFormatedDate());

                // Get data from DB.
                criteria.FromDate = DateTimeUtilize.ChangeClientFromTimeToServerTime(criteria.FromDate);
                criteria.ToDate = DateTimeUtilize.ChangeClientToTimeToServerTime(criteria.ToDate);
                criteria.ReportType = CasinoWeeklyType;
                try
                {
                    report = _casinoReportRepository.GetCasinoWeeklySummary(criteria);
                    report.Title = title;

                    SessionExtension.AddSession(HttpContext.Current.Session, key, report);
                }
                catch (Exception ex)
                {
                    LogExtensions.ExceptionHandler(ex);
                    throw;
                }
            }

            return report;
        }

        /// <summary>
        /// Exports the casino weekly summary.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        public void ExportCasinoWeeklySummary(GetCasinoSummaryCriteria criteria)
        {
            criteria.FromCache = true;
            CasinoWeeklySummary report = GetCasinoWeeklySummary(criteria);

            int maxColumnIndex = 0;
            if (report != null && report.GameTypes != null)
            {
                maxColumnIndex += report.GameTypes.Count;
                maxColumnIndex++; // Grand Total column.
            }

            string excelFileName = string.Format(Labels.CasinoSummaryExcelName, FormatReportDate(criteria.FromDate), FormatReportDate(criteria.ToDate));
            HSSFWorkbook workbook = new HSSFWorkbook();
            ExportExcelInfo exportExcelInfo = new ExportExcelInfo
            {
                ExcelFile = workbook,
                Worksheet = workbook.CreateSheet(excelFileName),
                StartRow = 0,
                StartColumn = 0,
                CurrentRow = 0,
                MaxColumnIndex = maxColumnIndex
            };

            GenerateCasinoWeeklySummaryTitle(exportExcelInfo, report.Title);
            GenerateCasinoWeeklySummaryHeader(exportExcelInfo, report.GameTypes);
            if (report.WeeklyCategories == null || report.WeeklyCategories.Count == 0)
            {
                NpoiExcelExtensions.RenderNoInfoMessage(exportExcelInfo, exportExcelInfo.CurrentRow);
            }
            else
            {
                NpoiExcelExtensions.SetColorAtIndex(exportExcelInfo, NpoiExcelExtensions.EvenRowColor, NpoiExcelExtensions.EvenRowColorIndex);
                NpoiExcelExtensions.SetGrandTotalColorIndex(exportExcelInfo);

                GenerateCasinoWeeklySummaryBody(exportExcelInfo, report.WeeklyCategories);
                GenerateCasinoWeeklySummaryFooter(exportExcelInfo, report.GrandTotal);
            }

            NpoiExcelExtensions.ResponseStreamExcel(exportExcelInfo.ExcelFile, excelFileName);
        }

        /// <summary>
        /// Gets the casino weekly detail.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The casino detail object.</returns>
        public CasinoDetail GetCasinoWeeklyDetail(GetCasinoDetailCriteria criteria)
        {
            string key = CasinoWeeklyDetailSessionKey;
            IList<CasinoDetailEntity> casinoDetails = null;
            if (criteria.FromCache)
            {
                casinoDetails = (IList<CasinoDetailEntity>)SessionExtension.GetSession(HttpContext.Current.Session, key);
            }

            string title = string.Format(Labels.CasinoWeeklyDetailTitle, criteria.FromDate.ToShortFormatedDate(), criteria.ToDate.ToShortFormatedDate());
            if (casinoDetails == null)
            {
                // Get data from DB.
                criteria.FromDate = DateTimeUtilize.ChangeClientFromTimeToServerTime(criteria.FromDate);
                criteria.ToDate = DateTimeUtilize.ChangeClientToTimeToServerTime(criteria.ToDate);
                try
                {
                    casinoDetails = _casinoReportRepository.GetCasinoDetails(criteria);
                    SessionExtension.AddSession(HttpContext.Current.Session, key, casinoDetails);
                }
                catch (Exception ex)
                {
                    LogExtensions.ExceptionHandler(ex);
                    throw;
                }
            }

            // Pagination.
            int totalItems = casinoDetails != null ? casinoDetails.Count : 0;
            if (totalItems > 0)
            {
                int page = criteria.PageIndex - 1;
                return new CasinoDetail
                {
                    Title = title,
                    TotalItems = totalItems,
                    CasinoDetails = casinoDetails.Skip(criteria.ItemsPerPage * page).Take(criteria.ItemsPerPage).ToList(),
                    CurrentPage = criteria.PageIndex,
                    ItemsPerPage = criteria.ItemsPerPage
                };
            }

            return new CasinoDetail { Title = title };
        }

        #region Excel - Casino Daily Statistics

        /// <summary>
        /// Generates the casino daily summary title.
        /// </summary>
        /// <param name="exportExcelInfo">The export excel info.</param>
        /// <param name="title">The title.</param>
        private void GenerateCasinoDailySummaryTitle(ExportExcelInfo exportExcelInfo, string title)
        {
            int currentRow = exportExcelInfo.CurrentRow;
            int currentColumn = exportExcelInfo.StartColumn;

            NpoiExcelExtensions.SetHeaderColorIndex(exportExcelInfo);
            ISheet worksheet = exportExcelInfo.Worksheet;
            worksheet.SetColumnWidth(currentColumn++, 280 * NpoiExcelExtensions.FontWidth);

            CellRangeAddress cellRangeAddress = new CellRangeAddress(
                currentRow,
                currentRow + 1,
                exportExcelInfo.StartColumn,
                exportExcelInfo.MaxColumnIndex);
            ICellStyle headerStyle = NpoiExcelExtensions.HeaderStyle(exportExcelInfo.ExcelFile, new ExcelStyleParam() { Alignment = CRMHorizontalAlignment.Left });
            NpoiExcelExtensions.RenderMergedCells(exportExcelInfo, cellRangeAddress, title, headerStyle);
            currentRow += 2;

            NpoiExcelExtensions.RenderEmptyRow(exportExcelInfo, currentRow);
            currentRow++;

            exportExcelInfo.CurrentRow = currentRow;
        }

        /// <summary>
        /// Generates the casino daily summary header.
        /// </summary>
        /// <param name="exportExcelInfo">The export excel info.</param>
        /// <param name="categoryCasinoGroups">The category casino groups.</param>
        private void GenerateCasinoDailySummaryHeader(ExportExcelInfo exportExcelInfo, IList<CategoryCasinoGroup> categoryCasinoGroups)
        {
            int currentRow = exportExcelInfo.CurrentRow;
            int currentCasinoColumn = exportExcelInfo.StartColumn;

            IRow commRow = exportExcelInfo.Worksheet.CreateRow(currentRow + 1);
            ICellStyle headerStyle = NpoiExcelExtensions.HeaderStyle(exportExcelInfo.ExcelFile, new ExcelStyleParam());

            CellRangeAddress categoryCellRangeAddress = new CellRangeAddress(currentRow, currentRow + 1, currentCasinoColumn, currentCasinoColumn);
            NpoiExcelExtensions.RenderMergedCells(exportExcelInfo, categoryCellRangeAddress, Labels.Categories, headerStyle);
            currentCasinoColumn++;

            string casinoGroupName;
            int commLeng;
            IList<CommunicationValue> communications;
            CellRangeAddress casinoGrpCellRangeAddress;
            int currentCommColumn = exportExcelInfo.StartColumn + 1;
            foreach (var casinoGroup in categoryCasinoGroups[0].CasinoGroups)
            {
                casinoGroupName = casinoGroup.ResourceName;

                communications = casinoGroup.Communications;
                commLeng = communications.Count;

                casinoGrpCellRangeAddress = new CellRangeAddress(currentRow, currentRow, currentCasinoColumn, currentCasinoColumn + commLeng - 1);
                NpoiExcelExtensions.RenderMergedCells(exportExcelInfo, casinoGrpCellRangeAddress, casinoGroupName, headerStyle);
                currentCasinoColumn += commLeng;

                foreach (var comm in communications)
                {
                    NpoiExcelExtensions.RenderCell(commRow, currentCommColumn, comm.Name, headerStyle);
                    currentCommColumn++;
                }
            }

            currentRow += 2;
            exportExcelInfo.CurrentRow = currentRow;
        }

        /// <summary>
        /// Generates the casino daily summary body.
        /// </summary>
        /// <param name="exportExcelInfo">The export excel info.</param>
        /// <param name="categoryCasinoGroups">The category casino groups.</param>
        private void GenerateCasinoDailySummaryBody(ExportExcelInfo exportExcelInfo, IList<CategoryCasinoGroup> categoryCasinoGroups)
        {
            int currentRow = exportExcelInfo.CurrentRow;
            int currentColumn = exportExcelInfo.CurrentColumn;
            ISheet worksheet = exportExcelInfo.Worksheet;

            IRow row;
            ICellStyle categoryCellStyle = CategoryCellStyle(exportExcelInfo.ExcelFile);
            ICellStyle communicationCellStyle = CommunicationCellStyle(exportExcelInfo.ExcelFile);
            foreach (var category in categoryCasinoGroups)
            {
                row = worksheet.CreateRow(currentRow);
                NpoiExcelExtensions.RenderCell(row, currentColumn, category.Category, categoryCellStyle);
                currentColumn++;
                foreach (var casino in category.CasinoGroups)
                {
                    foreach (var comm in casino.Communications)
                    {
                        NpoiExcelExtensions.RenderCell(row, currentColumn, comm.Value, communicationCellStyle);
                        currentColumn++;
                    }
                }

                currentRow++;
                currentColumn = exportExcelInfo.CurrentColumn;
            }

            exportExcelInfo.CurrentRow = currentRow;
        }

        /// <summary>
        /// Generates the casino daily summary footer.
        /// </summary>
        /// <param name="exportExcelInfo">The export excel info.</param>
        /// <param name="total">The total.</param>
        private void GenerateCasinoDailySummaryFooter(ExportExcelInfo exportExcelInfo, CategoryCasinoGroup total)
        {
            int currentRow = exportExcelInfo.CurrentRow;
            int currentColumn = exportExcelInfo.StartColumn;

            ICellStyle categoryCellStyle = TotalCategoryCellStyle(exportExcelInfo.ExcelFile);
            ICellStyle communicationCellStyle = TotalCommunicationCellStyle(exportExcelInfo.ExcelFile);
            ICellStyle grandTotalCommunicationCellStyle = GrandTotalCommunicationCellStyle(exportExcelInfo.ExcelFile);

            CellRangeAddress categoryCellRangeAddress = new CellRangeAddress(currentRow, currentRow + 1, currentColumn, currentColumn);
            NpoiExcelExtensions.RenderMergedCells(exportExcelInfo, categoryCellRangeAddress, total.Category, categoryCellStyle);
            currentColumn++;

            // Total.
            IRow totalRow = exportExcelInfo.Worksheet.GetRow(currentRow);
            foreach (var casino in total.CasinoGroups)
            {
                foreach (var comm in casino.Communications)
                {
                    NpoiExcelExtensions.RenderCell(totalRow, currentColumn, comm.Value, communicationCellStyle);
                    currentColumn++;
                }
            }

            // Grand Total.
            currentRow++;
            currentColumn = exportExcelInfo.StartColumn + 1;

            int commLeng;
            CellRangeAddress casinoCellRangeAddress;
            foreach (var casino in total.CasinoGroups)
            {
                commLeng = casino.Communications.Count;
                casinoCellRangeAddress = new CellRangeAddress(currentRow, currentRow, currentColumn, currentColumn + commLeng - 1);
                NpoiExcelExtensions.RenderMergedCells(exportExcelInfo, casinoCellRangeAddress, casino.GrandTotal, grandTotalCommunicationCellStyle);
                currentColumn += commLeng;
            }
        }

        private string FormatReportDate(DateTime date)
        {
            string pattern = "MMddyyyy";
            return date.ToString(pattern);
        }

        private ICellStyle CategoryCellStyle(HSSFWorkbook workbook)
        {
            return NpoiExcelExtensions.TextStyle(
                workbook,
                NpoiExcelExtensions.BackgroundColorIndex,
                false,
                CRMHorizontalAlignment.Left);
        }

        private ICellStyle TotalCategoryCellStyle(HSSFWorkbook workbook)
        {
            return NpoiExcelExtensions.TextStyle(
                workbook,
                NpoiExcelExtensions.GrandTotalRowColorIndex,
                true,
                CRMHorizontalAlignment.Left,
                NpoiExcelExtensions.GrandTotalTextColorIndex);
        }

        private ICellStyle CommunicationCellStyle(HSSFWorkbook workbook)
        {
            return NpoiExcelExtensions.IntNumberStyle(
                workbook,
                NpoiExcelExtensions.BackgroundColorIndex,
                false,
                CRMHorizontalAlignment.Right);
        }

        private ICellStyle TotalCommunicationCellStyle(HSSFWorkbook workbook)
        {
            return NpoiExcelExtensions.IntNumberStyle(
                workbook,
                NpoiExcelExtensions.GrandTotalRowColorIndex,
                true,
                CRMHorizontalAlignment.Right,
                NpoiExcelExtensions.GrandTotalTextColorIndex);
        }

        private ICellStyle GrandTotalCommunicationCellStyle(HSSFWorkbook workbook)
        {
            return NpoiExcelExtensions.IntNumberStyle(
                workbook,
                NpoiExcelExtensions.GrandTotalRowColorIndex,
                true,
                CRMHorizontalAlignment.Center,
                NpoiExcelExtensions.GrandTotalTextColorIndex);
        }

        private ICellStyle TextCenterCellStyle(HSSFWorkbook workbook)
        {
            return NpoiExcelExtensions.TextStyle(
                workbook,
                NpoiExcelExtensions.BackgroundColorIndex,
                false,
                CRMHorizontalAlignment.Center);
        }

        #endregion

        #region Excel - Casino Weekly Statistics

        /// <summary>
        /// Generates the casino weekly summary title.
        /// </summary>
        /// <param name="exportExcelInfo">The export excel info.</param>
        /// <param name="title">The title.</param>
        private void GenerateCasinoWeeklySummaryTitle(ExportExcelInfo exportExcelInfo, string title)
        {
            NpoiExcelExtensions.SetHeaderColorIndex(exportExcelInfo);
            exportExcelInfo.ResetCurrentColumn();
            exportExcelInfo.SetColumnWidthByFontWidth(exportExcelInfo.CurrentColumn++, 280);
            for (int i = 0; i < exportExcelInfo.MaxColumnIndex; i++)
            {
                exportExcelInfo.SetColumnWidthByFontWidth(exportExcelInfo.CurrentColumn++, 120);
            }

            ICellStyle headerStyle = NpoiExcelExtensions.HeaderStyle(exportExcelInfo.ExcelFile, new ExcelStyleParam() { Alignment = CRMHorizontalAlignment.Center });
            exportExcelInfo.RenderHeaderTitle(title, headerStyle);
        }

        /// <summary>
        /// Generates the casino weekly summary header.
        /// </summary>
        /// <param name="exportExcelInfo">The export excel info.</param>
        /// <param name="gameTypes">The game types.</param>
        private void GenerateCasinoWeeklySummaryHeader(ExportExcelInfo exportExcelInfo, IList<string> gameTypes)
        {
            int currentRow = exportExcelInfo.CurrentRow;
            int currentColumn = exportExcelInfo.StartColumn;

            ICellStyle headerStyle = NpoiExcelExtensions.HeaderStyle(exportExcelInfo.ExcelFile, new ExcelStyleParam());
            IRow row = exportExcelInfo.Worksheet.CreateRow(currentRow);

            NpoiExcelExtensions.RenderCell(row, currentColumn, Labels.Category, headerStyle);
            currentColumn++;
            foreach (var item in gameTypes)
            {
                NpoiExcelExtensions.RenderCell(row, currentColumn, item, headerStyle);
                currentColumn++;
            }

            // Grand Total.
            NpoiExcelExtensions.RenderCell(row, currentColumn, Labels.Grand_Total, headerStyle);
            currentColumn++;

            currentRow += 1;
            exportExcelInfo.CurrentRow = currentRow;
        }

        /// <summary>
        /// Generates the casino weekly summary body.
        /// </summary>
        /// <param name="exportExcelInfo">The export excel info.</param>
        /// <param name="weeklyCategories">The weekly categories.</param>
        private void GenerateCasinoWeeklySummaryBody(ExportExcelInfo exportExcelInfo, IList<CasinoWeeklyCategory> weeklyCategories)
        {
            exportExcelInfo.ResetCurrentColumn();
            int currentRow = exportExcelInfo.CurrentRow;
            int currentColumn = exportExcelInfo.CurrentColumn;
            ISheet worksheet = exportExcelInfo.Worksheet;

            IRow row;
            ICellStyle categoryCellStyle = CategoryCellStyle(exportExcelInfo.ExcelFile);
            ICellStyle gameValueCellStyle = CommunicationCellStyle(exportExcelInfo.ExcelFile);
            foreach (var category in weeklyCategories)
            {
                row = worksheet.CreateRow(currentRow);
                NpoiExcelExtensions.RenderCell(row, currentColumn, category.Name, categoryCellStyle);
                currentColumn++;
                foreach (var game in category.GameTypeValues)
                {
                    NpoiExcelExtensions.RenderCell(row, currentColumn, game, gameValueCellStyle);
                    currentColumn++;
                }

                // Grand Total.
                NpoiExcelExtensions.RenderCell(row, currentColumn, category.GrandTotal, gameValueCellStyle);
                currentColumn++;

                currentRow++;
                currentColumn = exportExcelInfo.StartColumn;
            }

            exportExcelInfo.CurrentRow = currentRow;
        }

        /// <summary>
        /// Generates the casino weekly summary footer.
        /// </summary>
        /// <param name="exportExcelInfo">The export excel info.</param>
        /// <param name="grandTotal">The grand total.</param>
        private void GenerateCasinoWeeklySummaryFooter(ExportExcelInfo exportExcelInfo, CasinoWeeklyCategory grandTotal)
        {
            int currentRow = exportExcelInfo.CurrentRow;
            int currentColumn = exportExcelInfo.CurrentColumn;
            ISheet worksheet = exportExcelInfo.Worksheet;

            ICellStyle categoryCellStyle = TotalCategoryCellStyle(exportExcelInfo.ExcelFile);
            ICellStyle gameValueCellStyle = TotalCommunicationCellStyle(exportExcelInfo.ExcelFile);

            IRow row = worksheet.CreateRow(currentRow);
            NpoiExcelExtensions.RenderCell(row, currentColumn, grandTotal.Name, categoryCellStyle);
            currentColumn++;
            foreach (var game in grandTotal.GameTypeValues)
            {
                NpoiExcelExtensions.RenderCell(row, currentColumn, game, gameValueCellStyle);
                currentColumn++;
            }

            // Grand Total.
            NpoiExcelExtensions.RenderCell(row, currentColumn, grandTotal.GrandTotal, gameValueCellStyle);
            currentColumn++;
        }

        #endregion

        #region Excel - Casino Detail

        /// <summary>
        /// Generates the casino detail title.
        /// </summary>
        /// <param name="exportExcelInfo">The export excel info.</param>
        /// <param name="title">The title.</param>
        private void GenerateCasinoDetailTitle(ExportExcelInfo exportExcelInfo, string title)
        {
            int currentRow = exportExcelInfo.CurrentRow;
            int currentColumn = exportExcelInfo.StartColumn;

            NpoiExcelExtensions.SetHeaderColorIndex(exportExcelInfo);
            ISheet worksheet = exportExcelInfo.Worksheet;
            worksheet.SetColumnWidth(currentColumn++, 100 * NpoiExcelExtensions.FontWidth);
            worksheet.SetColumnWidth(currentColumn++, 100 * NpoiExcelExtensions.FontWidth);
            worksheet.SetColumnWidth(currentColumn++, 100 * NpoiExcelExtensions.FontWidth);
            worksheet.SetColumnWidth(currentColumn++, 100 * NpoiExcelExtensions.FontWidth);
            worksheet.SetColumnWidth(currentColumn++, 150 * NpoiExcelExtensions.FontWidth);
            worksheet.SetColumnWidth(currentColumn++, 120 * NpoiExcelExtensions.FontWidth);
            worksheet.SetColumnWidth(currentColumn++, 120 * NpoiExcelExtensions.FontWidth);
            worksheet.SetColumnWidth(currentColumn++, 120 * NpoiExcelExtensions.FontWidth);
            worksheet.SetColumnWidth(currentColumn++, 80 * NpoiExcelExtensions.FontWidth);
            worksheet.SetColumnWidth(currentColumn++, 250 * NpoiExcelExtensions.FontWidth);
            worksheet.SetColumnWidth(currentColumn++, 120 * NpoiExcelExtensions.FontWidth);
            worksheet.SetColumnWidth(currentColumn++, 250 * NpoiExcelExtensions.FontWidth);
            worksheet.SetColumnWidth(currentColumn++, 250 * NpoiExcelExtensions.FontWidth);
            worksheet.SetColumnWidth(currentColumn++, 350 * NpoiExcelExtensions.FontWidth);
            worksheet.SetColumnWidth(currentColumn++, 350 * NpoiExcelExtensions.FontWidth);
            worksheet.SetColumnWidth(currentColumn++, 100 * NpoiExcelExtensions.FontWidth);
            worksheet.SetColumnWidth(currentColumn++, 100 * NpoiExcelExtensions.FontWidth);
            worksheet.SetColumnWidth(currentColumn++, 150 * NpoiExcelExtensions.FontWidth);
            worksheet.SetColumnWidth(currentColumn++, 150 * NpoiExcelExtensions.FontWidth);

            CellRangeAddress cellRangeAddress = new CellRangeAddress(
                currentRow,
                currentRow + 1,
                exportExcelInfo.StartColumn,
                exportExcelInfo.MaxColumnIndex);
            ICellStyle headerStyle = NpoiExcelExtensions.HeaderStyle(exportExcelInfo.ExcelFile, new ExcelStyleParam() { Alignment = CRMHorizontalAlignment.Center });
            NpoiExcelExtensions.RenderMergedCells(exportExcelInfo, cellRangeAddress, title, headerStyle);
            currentRow += 2;

            NpoiExcelExtensions.RenderEmptyRow(exportExcelInfo, currentRow);
            currentRow++;

            exportExcelInfo.CurrentRow = currentRow;
        }

        /// <summary>
        /// Generates the casino detail header.
        /// </summary>
        /// <param name="exportExcelInfo">The export excel info.</param>
        private void GenerateCasinoDetailHeader(ExportExcelInfo exportExcelInfo)
        {
            int currentRow = exportExcelInfo.CurrentRow;
            int currentColumn = exportExcelInfo.StartColumn;

            IRow row = exportExcelInfo.Worksheet.CreateRow(currentRow);
            ICellStyle headerStyle = NpoiExcelExtensions.HeaderStyle(exportExcelInfo.ExcelFile, new ExcelStyleParam());
            NpoiExcelExtensions.RenderCell(row, currentColumn, Labels.Date, headerStyle);
            currentColumn++;
            NpoiExcelExtensions.RenderCell(row, currentColumn, Labels.StartTime, headerStyle);
            currentColumn++;
            NpoiExcelExtensions.RenderCell(row, currentColumn, Labels.EndTime, headerStyle);
            currentColumn++;
            NpoiExcelExtensions.RenderCell(row, currentColumn, Labels.Duration, headerStyle);
            currentColumn++;
            NpoiExcelExtensions.RenderCell(row, currentColumn, Labels.Customer, headerStyle);
            currentColumn++;
            NpoiExcelExtensions.RenderCell(row, currentColumn, Labels.Site, headerStyle);
            currentColumn++;
            NpoiExcelExtensions.RenderCell(row, currentColumn, Labels.TypeOfContact, headerStyle);
            currentColumn++;
            NpoiExcelExtensions.RenderCell(row, currentColumn, Labels.ProductName, headerStyle);
            currentColumn++;
            NpoiExcelExtensions.RenderCell(row, currentColumn, Labels.Ext, headerStyle);
            currentColumn++;
            NpoiExcelExtensions.RenderCell(row, currentColumn, Labels.Contact, headerStyle);
            currentColumn++;
            NpoiExcelExtensions.RenderCell(row, currentColumn, Labels.Country, headerStyle);
            currentColumn++;
            NpoiExcelExtensions.RenderCell(row, currentColumn, Labels.Category, headerStyle);
            currentColumn++;
            NpoiExcelExtensions.RenderCell(row, currentColumn, Labels.SubCategory, headerStyle);
            currentColumn++;
            NpoiExcelExtensions.RenderCell(row, currentColumn, Labels.Description, headerStyle);
            currentColumn++;
            NpoiExcelExtensions.RenderCell(row, currentColumn, Labels.Solution, headerStyle);
            currentColumn++;
            NpoiExcelExtensions.RenderCell(row, currentColumn, Labels.Status, headerStyle);
            currentColumn++;
            NpoiExcelExtensions.RenderCell(row, currentColumn, Labels.Response, headerStyle);
            currentColumn++;
            NpoiExcelExtensions.RenderCell(row, currentColumn, Labels.HandleBy, headerStyle);
            currentColumn++;
            NpoiExcelExtensions.RenderCell(row, currentColumn, Labels.CreationTime, headerStyle);
            currentColumn++;

            exportExcelInfo.CurrentRow = ++currentRow;
        }

        /// <summary>
        /// Generates the casino detail body.
        /// </summary>
        /// <param name="exportExcelInfo">The export excel info.</param>
        /// <param name="casinoDetails">The casino details.</param>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private void GenerateCasinoDetailBody(ExportExcelInfo exportExcelInfo, IList<CasinoDetailEntity> casinoDetails)
        {
            int currentRow = exportExcelInfo.CurrentRow;
            ISheet worksheet = exportExcelInfo.Worksheet;
            ICellStyle textLeftCellStyle = CategoryCellStyle(exportExcelInfo.ExcelFile);
            ICellStyle textCenterCellStyle = TextCenterCellStyle(exportExcelInfo.ExcelFile);
            IRow row;
            int currentColumn;
            foreach (var item in casinoDetails)
            {
                currentColumn = exportExcelInfo.CurrentColumn;
                row = worksheet.CreateRow(currentRow);
                NpoiExcelExtensions.RenderCell(row, currentColumn, item.FeedbackDateString, textCenterCellStyle);
                currentColumn++;
                NpoiExcelExtensions.RenderCell(row, currentColumn, item.FeedbackTimeString, textCenterCellStyle);
                currentColumn++;
                NpoiExcelExtensions.RenderCell(row, currentColumn, item.ResolutionTimeString, textCenterCellStyle);
                currentColumn++;
                NpoiExcelExtensions.RenderCell(row, currentColumn, item.Duration, textCenterCellStyle);
                currentColumn++;
                NpoiExcelExtensions.RenderCell(row, currentColumn, item.UserName, textLeftCellStyle);
                currentColumn++;
                NpoiExcelExtensions.RenderCell(row, currentColumn, item.SiteName, textLeftCellStyle);
                currentColumn++;
                NpoiExcelExtensions.RenderCell(row, currentColumn, item.Communication, textLeftCellStyle);
                currentColumn++;
                NpoiExcelExtensions.RenderCell(row, currentColumn, item.Product, textLeftCellStyle);
                currentColumn++;
                NpoiExcelExtensions.RenderCell(row, currentColumn, item.Station, textCenterCellStyle);
                currentColumn++;
                NpoiExcelExtensions.RenderCell(row, currentColumn, HtmlHelper.ReplaceHtmlTag(item.Contact), textLeftCellStyle);
                currentColumn++;
                NpoiExcelExtensions.RenderCell(row, currentColumn, item.CountryName, textLeftCellStyle);
                currentColumn++;
                NpoiExcelExtensions.RenderCell(row, currentColumn, item.MainCategory, textLeftCellStyle);
                currentColumn++;
                NpoiExcelExtensions.RenderCell(row, currentColumn, item.SubCategory, textLeftCellStyle);
                currentColumn++;
                NpoiExcelExtensions.RenderCell(row, currentColumn, HtmlHelper.ReplaceHtmlTag(item.Description), textLeftCellStyle);
                currentColumn++;
                NpoiExcelExtensions.RenderCell(row, currentColumn, HtmlHelper.ReplaceHtmlTag(item.Solution), textLeftCellStyle);
                currentColumn++;
                NpoiExcelExtensions.RenderCell(row, currentColumn, item.Status, textCenterCellStyle);
                currentColumn++;
                NpoiExcelExtensions.RenderCell(row, currentColumn, item.Response, textCenterCellStyle);
                currentColumn++;
                NpoiExcelExtensions.RenderCell(row, currentColumn, item.CreatorName, textLeftCellStyle);
                currentColumn++;
                NpoiExcelExtensions.RenderCell(row, currentColumn, item.CreateDateString, textCenterCellStyle);
                currentRow++;
            }
        }

        #endregion
    }
}
