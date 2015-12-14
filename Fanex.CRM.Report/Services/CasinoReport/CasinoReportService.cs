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
    using Fanex.CRM.Common.Extensions;
    using Fanex.CRM.Common.Helpers;
    using Fanex.CRM.Common.Models.Npoi;
    using Fanex.CRM.Infrastructure.Extensions;
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

        private readonly ICasinoReportRepository _casinoReportRepository;

        public CasinoReportService()
            : this(null)
        {
        }

        public CasinoReportService(ICasinoReportRepository casinoReportRepository)
        {
            _casinoReportRepository = casinoReportRepository ?? new CasinoReportRepository();
        }

        public CasinoDailySummary GetCasinoDailySummary(GetCasinoSummaryCriteria criteria)
        {
            var title = string.Format(Labels.CasinoDailySummaryTitle, criteria.FromDate.ToShortFormatedDate(), criteria.ToDate.ToShortFormatedDate());
            criteria.FromDate = DateTimeUtilize.ChangeClientFromTimeToServerTime(criteria.FromDate);
            criteria.ToDate = DateTimeUtilize.ChangeClientToTimeToServerTime(criteria.ToDate);
            criteria.ReportType = CasinoDailyType;
            var data = _casinoReportRepository.GetCasinoDailySummary(criteria);
            data.Title = title;
            return data;
        }

        public void ExportCasinoDailySummary(GetCasinoSummaryCriteria criteria)
        {
            var excelFileName = string.Format(Labels.CasinoSummaryExcelName, FormatReportDate(criteria.FromDate), FormatReportDate(criteria.ToDate));
            var data = GetCasinoDailySummary(criteria);
            int maxColumnIndex = 0;
            if (data.CategoryCasinoGroups.Count > 0)
            {
                maxColumnIndex = data.CategoryCasinoGroups[0].CasinoGroups.Count;
                maxColumnIndex *= data.CategoryCasinoGroups[0].CasinoGroups[0].Communications.Count;
            }

            var workbook = new HSSFWorkbook();
            var exportExcelInfo = new ExportExcelInfo
            {
                ExcelFile = workbook,
                Worksheet = workbook.CreateSheet(excelFileName),
                StartRow = 0,
                StartColumn = 0,
                CurrentRow = 0,
                MaxColumnIndex = maxColumnIndex
            };

            GenerateCasinoDailySummaryTitle(exportExcelInfo, data.Title);
            GenerateCasinoDailySummaryHeader(exportExcelInfo, data.CategoryCasinoGroups);
            if (maxColumnIndex == 0)
            {
                NpoiExcelExtensions.RenderNoInfoMessage(exportExcelInfo, exportExcelInfo.CurrentRow);
            }
            else
            {
                NpoiExcelExtensions.SetColorAtIndex(exportExcelInfo, NpoiExcelExtensions.EvenRowColor, NpoiExcelExtensions.EvenRowColorIndex);
                NpoiExcelExtensions.SetGrandTotalColorIndex(exportExcelInfo);

                GenerateCasinoDailySummaryBody(exportExcelInfo, data.CategoryCasinoGroups);
                GenerateCasinoDailySummaryFooter(exportExcelInfo, data.Total);
            }

            NpoiExcelExtensions.ResponseStreamExcel(exportExcelInfo.ExcelFile, excelFileName);
        }

        public CasinoDetail GetCasinoDailyDetail(GetCasinoDetailCriteria criteria)
        {
            var titleFormat = criteria.ReportType == CasinoWeeklyType ?
                Labels.CasinoWeeklyDetailTitle :
                Labels.CasinoDailyDetailTitle;
            var title = string.Format(titleFormat, criteria.FromDate.ToShortFormatedDate(), criteria.ToDate.ToShortFormatedDate());

            criteria.FromDate = DateTimeUtilize.ChangeClientFromTimeToServerTime(criteria.FromDate);
            criteria.ToDate = DateTimeUtilize.ChangeClientToTimeToServerTime(criteria.ToDate);

            var casinoDailyDetails = _casinoReportRepository.GetCasinoDetails(criteria);
            
            int totalItems = casinoDailyDetails.Count();
            if (totalItems > 0)
            {
                return new CasinoDetail
                {
                    Title = title,
                    TotalItems = totalItems,
                    CasinoDetails = criteria.FullReport ?
                        casinoDailyDetails :
                        casinoDailyDetails.Skip(criteria.ItemsPerPage * criteria.PageIndex).Take(criteria.ItemsPerPage).ToList(),
                    CurrentPage = criteria.PageIndex,
                    ItemsPerPage = criteria.ItemsPerPage
                };
            }

            return new CasinoDetail { Title = title };
        }

        public void ExportCasinoDailyDetail(GetCasinoDetailCriteria criteria)
        {
            criteria.FullReport = true;

            string excelFileName = string.Format(
                criteria.ReportType == CasinoWeeklyType ? Labels.CasinoWeeklyExcelName : Labels.CasinoDetailExcelName,
                FormatReportDate(criteria.FromDate),
                FormatReportDate(criteria.ToDate));

            var data = GetCasinoDailyDetail(criteria);

            int maxColumnIndex = 19;
            var workbook = new HSSFWorkbook();
            var exportExcelInfo = new ExportExcelInfo
            {
                ExcelFile = workbook,
                Worksheet = workbook.CreateSheet(excelFileName),
                StartRow = 0,
                StartColumn = 0,
                CurrentRow = 0,
                MaxColumnIndex = maxColumnIndex
            };

            GenerateCasinoDetailTitle(exportExcelInfo, data.Title.ToUpper());
            GenerateCasinoDetailHeader(exportExcelInfo);
            if (data.CasinoDetails == null || data.CasinoDetails.Count() == 0)
            {
                NpoiExcelExtensions.RenderNoInfoMessage(exportExcelInfo, exportExcelInfo.CurrentRow);
            }
            else
            {
                NpoiExcelExtensions.SetColorAtIndex(exportExcelInfo, NpoiExcelExtensions.EvenRowColor, NpoiExcelExtensions.EvenRowColorIndex);
                NpoiExcelExtensions.SetGrandTotalColorIndex(exportExcelInfo);

                GenerateCasinoDetailBody(exportExcelInfo, data.CasinoDetails);
            }

            NpoiExcelExtensions.ResponseStreamExcel(exportExcelInfo.ExcelFile, excelFileName);
        }

        public CasinoWeeklySummary GetCasinoWeeklySummary(GetCasinoSummaryCriteria criteria)
        {
            var title = string.Format(Labels.CasinoWeeklySummaryTitle, criteria.FromDate.ToShortFormatedDate(), criteria.ToDate.ToShortFormatedDate());
            criteria.FromDate = DateTimeUtilize.ChangeClientFromTimeToServerTime(criteria.FromDate);
            criteria.ToDate = DateTimeUtilize.ChangeClientToTimeToServerTime(criteria.ToDate);
            criteria.ReportType = CasinoWeeklyType;
            var data = _casinoReportRepository.GetCasinoWeeklySummary(criteria);
            data.Title = title;
            return data;
        }

        public void ExportCasinoWeeklySummary(GetCasinoSummaryCriteria criteria)
        {
            var excelFileName = string.Format(Labels.CasinoSummaryExcelName, FormatReportDate(criteria.FromDate), FormatReportDate(criteria.ToDate));

            var data = GetCasinoWeeklySummary(criteria);

            int maxColumnIndex = data.GameTypes.Count;
            maxColumnIndex++; // Grand Total column.

            var workbook = new HSSFWorkbook();
            var exportExcelInfo = new ExportExcelInfo
            {
                ExcelFile = workbook,
                Worksheet = workbook.CreateSheet(excelFileName),
                StartRow = 0,
                StartColumn = 0,
                CurrentRow = 0,
                MaxColumnIndex = maxColumnIndex
            };

            GenerateCasinoWeeklySummaryTitle(exportExcelInfo, data.Title);
            GenerateCasinoWeeklySummaryHeader(exportExcelInfo, data.GameTypes);
            if (data.WeeklyCategories.Count == 0)
            {
                NpoiExcelExtensions.RenderNoInfoMessage(exportExcelInfo, exportExcelInfo.CurrentRow);
            }
            else
            {
                NpoiExcelExtensions.SetColorAtIndex(exportExcelInfo, NpoiExcelExtensions.EvenRowColor, NpoiExcelExtensions.EvenRowColorIndex);
                NpoiExcelExtensions.SetGrandTotalColorIndex(exportExcelInfo);

                GenerateCasinoWeeklySummaryBody(exportExcelInfo, data.WeeklyCategories);
                GenerateCasinoWeeklySummaryFooter(exportExcelInfo, data.GrandTotal);
            }

            NpoiExcelExtensions.ResponseStreamExcel(exportExcelInfo.ExcelFile, excelFileName);
        }

        public CasinoDetail GetCasinoWeeklyDetail(GetCasinoDetailCriteria criteria)
        {
            var title = string.Format(Labels.CasinoWeeklyDetailTitle, criteria.FromDate.ToShortFormatedDate(), criteria.ToDate.ToShortFormatedDate());
            criteria.FromDate = DateTimeUtilize.ChangeClientFromTimeToServerTime(criteria.FromDate);
            criteria.ToDate = DateTimeUtilize.ChangeClientToTimeToServerTime(criteria.ToDate);

            var data = _casinoReportRepository.GetCasinoDetails(criteria);

            int totalItems = data.Count();
            if (totalItems > 0)
            {
                return new CasinoDetail
                {
                    Title = title,
                    TotalItems = totalItems,
                    CasinoDetails = data.Skip(criteria.ItemsPerPage * criteria.PageIndex).Take(criteria.ItemsPerPage).ToList(),
                    CurrentPage = criteria.PageIndex,
                    ItemsPerPage = criteria.ItemsPerPage
                };
            }

            return new CasinoDetail { Title = title };
        }

        #region Excel - Casino Daily Statistics

        private static void GenerateCasinoDailySummaryTitle(ExportExcelInfo exportExcelInfo, string title)
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
            ICellStyle headerStyle = NpoiExcelExtensions.GetHeaderStyle(exportExcelInfo.ExcelFile, new ExcelStyleParam() { Alignment = CRMHorizontalAlignment.Left });
            NpoiExcelExtensions.RenderMergedCells(exportExcelInfo, cellRangeAddress, title, headerStyle);
            currentRow += 2;

            NpoiExcelExtensions.RenderEmptyRow(exportExcelInfo, currentRow);
            currentRow++;

            exportExcelInfo.CurrentRow = currentRow;
        }

        private static void GenerateCasinoDailySummaryHeader(ExportExcelInfo exportExcelInfo, IList<CategoryCasinoGroup> categoryCasinoGroups)
        {
            int currentRow = exportExcelInfo.CurrentRow;
            int currentCasinoColumn = exportExcelInfo.StartColumn;

            IRow commRow = exportExcelInfo.Worksheet.CreateRow(currentRow + 1);
            ICellStyle headerStyle = NpoiExcelExtensions.GetHeaderStyle(exportExcelInfo.ExcelFile, new ExcelStyleParam());

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
        private static void GenerateCasinoDailySummaryBody(ExportExcelInfo exportExcelInfo, IList<CategoryCasinoGroup> categoryCasinoGroups)
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
        private static void GenerateCasinoDailySummaryFooter(ExportExcelInfo exportExcelInfo, CategoryCasinoGroup total)
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

        private static string FormatReportDate(DateTime date)
        {
            return date.ToString("MMddyyyy");
        }

        private static ICellStyle CategoryCellStyle(HSSFWorkbook workbook)
        {
            return NpoiExcelExtensions.GetTextStyle(
                workbook,
                NpoiExcelExtensions.BackgroundColorIndex,
                false,
                CRMHorizontalAlignment.Left);
        }

        private static ICellStyle TotalCategoryCellStyle(HSSFWorkbook workbook)
        {
            return NpoiExcelExtensions.GetTextStyle(
                workbook,
                NpoiExcelExtensions.GrandTotalRowColorIndex,
                true,
                CRMHorizontalAlignment.Left,
                NpoiExcelExtensions.GrandTotalTextColorIndex);
        }

        private static ICellStyle CommunicationCellStyle(HSSFWorkbook workbook)
        {
            return NpoiExcelExtensions.GetIntegerNumberStyle(
                workbook,
                NpoiExcelExtensions.BackgroundColorIndex,
                false,
                CRMHorizontalAlignment.Right);
        }

        private static ICellStyle TotalCommunicationCellStyle(HSSFWorkbook workbook)
        {
            return NpoiExcelExtensions.GetIntegerNumberStyle(
                workbook,
                NpoiExcelExtensions.GrandTotalRowColorIndex,
                true,
                CRMHorizontalAlignment.Right,
                NpoiExcelExtensions.GrandTotalTextColorIndex);
        }

        private static ICellStyle GrandTotalCommunicationCellStyle(HSSFWorkbook workbook)
        {
            return NpoiExcelExtensions.GetIntegerNumberStyle(
                workbook,
                NpoiExcelExtensions.GrandTotalRowColorIndex,
                true,
                CRMHorizontalAlignment.Center,
                NpoiExcelExtensions.GrandTotalTextColorIndex);
        }

        private static ICellStyle TextCenterCellStyle(HSSFWorkbook workbook)
        {
            return NpoiExcelExtensions.GetTextStyle(
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
        private static void GenerateCasinoWeeklySummaryTitle(ExportExcelInfo exportExcelInfo, string title)
        {
            NpoiExcelExtensions.SetHeaderColorIndex(exportExcelInfo);
            exportExcelInfo.ResetCurrentColumn();
            exportExcelInfo.SetColumnWidthByFontWidth(exportExcelInfo.CurrentColumn++, 280);
            for (int i = 0; i < exportExcelInfo.MaxColumnIndex; i++)
            {
                exportExcelInfo.SetColumnWidthByFontWidth(exportExcelInfo.CurrentColumn++, 120);
            }

            ICellStyle headerStyle = NpoiExcelExtensions.GetHeaderStyle(exportExcelInfo.ExcelFile, new ExcelStyleParam() { Alignment = CRMHorizontalAlignment.Center });
            exportExcelInfo.RenderHeaderTitle(title, headerStyle);
        }

        /// <summary>
        /// Generates the casino weekly summary header.
        /// </summary>
        /// <param name="exportExcelInfo">The export excel info.</param>
        /// <param name="gameTypes">The game types.</param>
        private static void GenerateCasinoWeeklySummaryHeader(ExportExcelInfo exportExcelInfo, IList<string> gameTypes)
        {
            int currentRow = exportExcelInfo.CurrentRow;
            int currentColumn = exportExcelInfo.StartColumn;

            ICellStyle headerStyle = NpoiExcelExtensions.GetHeaderStyle(exportExcelInfo.ExcelFile, new ExcelStyleParam());
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
        private static void GenerateCasinoWeeklySummaryBody(ExportExcelInfo exportExcelInfo, IList<CasinoWeeklyCategory> weeklyCategories)
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
        private static void GenerateCasinoWeeklySummaryFooter(ExportExcelInfo exportExcelInfo, CasinoWeeklyCategory grandTotal)
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
        private static void GenerateCasinoDetailTitle(ExportExcelInfo exportExcelInfo, string title)
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
            ICellStyle headerStyle = NpoiExcelExtensions.GetHeaderStyle(exportExcelInfo.ExcelFile, new ExcelStyleParam() { Alignment = CRMHorizontalAlignment.Center });
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
        private static void GenerateCasinoDetailHeader(ExportExcelInfo exportExcelInfo)
        {
            int currentRow = exportExcelInfo.CurrentRow;
            int currentColumn = exportExcelInfo.StartColumn;

            IRow row = exportExcelInfo.Worksheet.CreateRow(currentRow);
            ICellStyle headerStyle = NpoiExcelExtensions.GetHeaderStyle(exportExcelInfo.ExcelFile, new ExcelStyleParam());
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
            NpoiExcelExtensions.RenderCell(row, currentColumn, Labels.Role, headerStyle);
            currentColumn++;
            NpoiExcelExtensions.RenderCell(row, currentColumn, Labels.ContactInfo, headerStyle);
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
        private static void GenerateCasinoDetailBody(ExportExcelInfo exportExcelInfo, IEnumerable<CasinoDetailEntity> casinoDetails)
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
                NpoiExcelExtensions.RenderCell(row, currentColumn, item.Role, textCenterCellStyle);
                currentColumn++;
                NpoiExcelExtensions.RenderCell(row, currentColumn, HtmlHelper.RemoveHtmlTags(item.Contact), textLeftCellStyle);
                currentColumn++;
                NpoiExcelExtensions.RenderCell(row, currentColumn, item.CountryName, textLeftCellStyle);
                currentColumn++;
                NpoiExcelExtensions.RenderCell(row, currentColumn, item.MainCategory, textLeftCellStyle);
                currentColumn++;
                NpoiExcelExtensions.RenderCell(row, currentColumn, item.SubCategory, textLeftCellStyle);
                currentColumn++;
                NpoiExcelExtensions.RenderCell(row, currentColumn, HtmlHelper.RemoveHtmlTags(item.Description), textLeftCellStyle);
                currentColumn++;
                NpoiExcelExtensions.RenderCell(row, currentColumn, HtmlHelper.RemoveHtmlTags(item.Solution), textLeftCellStyle);
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
