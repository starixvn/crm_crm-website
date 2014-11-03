// --------------------------------------------------------------------------
// <copyright file="WagerReportService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.03.28">Created</change>
// </history>

namespace Fanex.CRM.Report.Services.Implements.WagerReport
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web;
    using Fanex.CRM.Infrastructure.Extension;
    using Fanex.CRM.Infrastructure.Logging;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Infrastructure.Models.Npoi;
    using Fanex.CRM.Report.Criteria.WagerReport;
    using Fanex.CRM.Report.Models.WagerReport;
    using Fanex.CRM.Report.Repositories.Implements;
    using Fanex.CRM.Report.Repositories.Interfaces;
    using Fanex.CRM.Report.Resources;
    using Fanex.CRM.Report.Services.Interfaces;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;

    /// <summary>
    /// WagerReportService service.
    /// </summary>
    public class WagerReportService : IWagerReportService
    {
        private const string WagerReportSessionKey = "Fanex.CRM.Report.WagerReport.WagerReportSessionKey_{0}";
        private readonly IWagerReportRepository _wagerRepository;

        public WagerReportService()
            : this(null)
        {
        }

        public WagerReportService(IWagerReportRepository wagerReportRepository)
        {
            _wagerRepository = wagerReportRepository ?? new WagerReportRepository();
        }

        /// <summary>
        /// Gets the wager report.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>WagerReport model.</returns>
        public WagerReportModel GetWagerReport(GetWagerReportCriteria criteria)
        {
            WagerReportModel report;
            string sessionKey = string.Format(WagerReportSessionKey, criteria.Year);
            try
            {
                report = criteria.IsCached
                    ? (WagerReportModel)SessionExtension.GetSession(HttpContext.Current.Session, sessionKey)
                    : null;
                if (report == null)
                {
                    report = ParseWagerReport(criteria);
                    report.ReportTitle = string.Format(Labels.WagerReportTitle, criteria.Year);

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
        /// Exports the wager report.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        public void ExportWagerReportExcel(GetWagerReportCriteria criteria)
        {
            criteria.IsCached = true;
            WagerReportModel report = GetWagerReport(criteria);
            string excelFileName = string.Format(Labels.WagerReportExcelName, criteria.Year);

            HSSFWorkbook workbook = new HSSFWorkbook();
            ExportExcelInfo exportExcelInfo = new ExportExcelInfo
            {
                ExcelFile = workbook,
                Worksheet = workbook.CreateSheet(excelFileName),
                StartRow = 0,
                StartColumn = 0,
                CurrentRow = 0,
                MaxColumnIndex = 3
            };

            ExportTitle(ref exportExcelInfo, report.ReportTitle);
            ExportHeader(ref exportExcelInfo);
            if (report.WagerByMonths == null || report.WagerByMonths.Count == 0)
            {
                NpoiExcelExtensions.RenderNoInfoMessage(ref exportExcelInfo, exportExcelInfo.CurrentRow);
            }
            else
            {
                NpoiExcelExtensions.SetColorAtIndex(
                    ref exportExcelInfo,
                    NpoiExcelExtensions.EvenRowColor,
                    NpoiExcelExtensions.EvenRowColorIndex);
                NpoiExcelExtensions.SetGrandTotalColorIndex(ref exportExcelInfo);

                ExportBody(ref exportExcelInfo, report.WagerByMonths);
                ExportFooter(ref exportExcelInfo, report.GrandTotal);
            }

            NpoiExcelExtensions.ResponseStreamExcel(exportExcelInfo.ExcelFile, excelFileName);
        }

        /// <summary>
        /// Parses the wager report.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The WagerReportModel.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        private WagerReportModel ParseWagerReport(GetWagerReportCriteria criteria)
        {
            Wager wager = null;
            WagerByMonth wagerByMonth = null;
            IList<WagerByMonth> wagerByMonths = new List<WagerByMonth>();
            IDictionary<string, Wager> grandTotalHash = new Dictionary<string, Wager> 
            {
                { Labels.Accept, new Wager { Name = Labels.Accept, Amount = 0 } },
                { Labels.Reject, new Wager { Name = Labels.Reject, Amount = 0 } },
                { Labels.VoidRefund, new Wager { Name = Labels.VoidRefund, Amount = 0 } },
            };
            int grandTotal = 0;

            var rawData = _wagerRepository.GetWagerReport(criteria)
                .GroupBy(w => w.Month);
            foreach (var month in rawData)
            {
                wagerByMonth = new WagerByMonth
                {
                    Month = month.Key.ToUpper(),
                    Wagers = new List<Wager>(),
                    Total = 0
                };

                foreach (var item in month)
                {
                    wager = new Wager
                    {
                        Name = item.SubCategory,
                        Amount = item.NoOfIssues
                    };

                    wagerByMonth.Total += wager.Amount;
                    wagerByMonth.Wagers.Add(wager);

                    if (grandTotalHash.ContainsKey(wager.Name))
                    {
                        grandTotalHash[wager.Name].Amount += wager.Amount;
                    }
                }

                wagerByMonths.Add(wagerByMonth);
                grandTotal += wagerByMonth.Total;
            }

            return new WagerReportModel
            {
                WagerByMonths = wagerByMonths,
                GrandTotal = new WagerByMonth
                {
                    Month = Labels.GrandTotal,
                    Wagers = grandTotalHash.Values.ToList(),
                    Total = grandTotal
                }
            };
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
            ISheet worksheet = exportExcelInfo.Worksheet;
            var columnWidths = new List<int>() { 200, 200, 200, 200 };
            exportExcelInfo.InitColumnWidthByFontWidth(columnWidths);

            ICellStyle headerStyle = NpoiExcelExtensions.HeaderStyle(exportExcelInfo.ExcelFile, new ExcelStyleParam());
            exportExcelInfo.RenderHeaderTitle(title, headerStyle);
        }

        /// <summary>
        /// Exports the header.
        /// </summary>
        /// <param name="exportExcelInfo">The export excel info.</param>
        private void ExportHeader(ref ExportExcelInfo exportExcelInfo)
        {
            int currentColumn = exportExcelInfo.StartColumn;
            IRow row = exportExcelInfo.Worksheet.CreateRow(exportExcelInfo.CurrentRow);
            ICellStyle headerStyle = NpoiExcelExtensions.HeaderStyle(exportExcelInfo.ExcelFile, new ExcelStyleParam());

            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, Labels.Month, headerStyle);
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, Labels.Wagers, headerStyle);
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, Labels.Amount, headerStyle);
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, Labels.Total, headerStyle);
            exportExcelInfo.CurrentRow++;
        }

        /// <summary>
        /// Exports the body.
        /// </summary>
        /// <param name="exportExcelInfo">The export excel info.</param>
        /// <param name="wagerByMonths">The wager by months.</param>
        private void ExportBody(ref ExportExcelInfo exportExcelInfo, IList<WagerByMonth> wagerByMonths)
        {
            bool isEven;
            ICellStyle monthCellStyle, wagersCellStyle, amountCellStyle, totalCellStyle;
            HSSFWorkbook workbook = exportExcelInfo.ExcelFile;
            int startColumn, currentColumn, currentRow, mergeRowNumber;
            int reportLength = wagerByMonths.Count;

            WagerByMonth wagerByMonth;
            for (int i = 0; i < reportLength; i++)
            {
                wagerByMonth = wagerByMonths[i];
                isEven = i % 2 != 0;
                currentRow = exportExcelInfo.CurrentRow;
                startColumn = exportExcelInfo.StartColumn;
                currentColumn = exportExcelInfo.CurrentColumn;
                mergeRowNumber = wagerByMonth.Wagers.Count;

                CellRangeAddress monthCellRange = new CellRangeAddress(
                    currentRow,
                    currentRow + mergeRowNumber - 1,
                    startColumn,
                    startColumn);
                monthCellStyle = GetMonthCellStyle(workbook, isEven);
                NpoiExcelExtensions.RenderMergedCells(exportExcelInfo, monthCellRange, wagerByMonth.Month, monthCellStyle);
                startColumn++;

                wagersCellStyle = GetWagersCellStyle(workbook, isEven);
                amountCellStyle = GetAmountCellStyle(workbook, isEven);
                for (int j = 0; j < mergeRowNumber; j++)
                {
                    currentColumn = startColumn;
                    IRow row = exportExcelInfo.Worksheet.GetRow(currentRow) ?? exportExcelInfo.Worksheet.CreateRow(currentRow);
                    NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, wagerByMonth.Wagers[j].Name, wagersCellStyle);
                    NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, wagerByMonth.Wagers[j].Amount, amountCellStyle);
                    currentRow++;
                }

                CellRangeAddress totalCellRange = new CellRangeAddress(
                    exportExcelInfo.CurrentRow,
                    exportExcelInfo.CurrentRow + mergeRowNumber - 1,
                    currentColumn,
                    currentColumn);
                totalCellStyle = GetTotalCellStyle(workbook, isEven);
                NpoiExcelExtensions.RenderMergedCells(exportExcelInfo, totalCellRange, wagerByMonth.Total, totalCellStyle);

                exportExcelInfo.CurrentRow = currentRow;
            }
        }

        /// <summary>
        /// Exports the footer.
        /// </summary>
        /// <param name="exportExcelInfo">The export excel info.</param>
        /// <param name="total">The total.</param>
        private void ExportFooter(ref ExportExcelInfo exportExcelInfo, WagerByMonth total)
        {
            int mergeRowNumber = total.Wagers.Count;
            int currentRow = exportExcelInfo.CurrentRow;
            int startColumn = exportExcelInfo.StartColumn;
            int currentColumn = exportExcelInfo.CurrentColumn;

            HSSFWorkbook workbook = exportExcelInfo.ExcelFile;
            ICellStyle monthCellStyle = GetGrandTotalMonthCellStyle(workbook);
            ICellStyle totalCellStyle = GetGrandTotalCellStyle(workbook);
            ICellStyle wagersCellStyle = null, amountCellStyle = null;

            CellRangeAddress grandTotalCellRange = new CellRangeAddress(
                    currentRow,
                    currentRow + mergeRowNumber - 1,
                    startColumn,
                    startColumn);
            NpoiExcelExtensions.RenderMergedCells(exportExcelInfo, grandTotalCellRange, total.Month, monthCellStyle);
            startColumn++;

            for (int i = 0; i < mergeRowNumber; i++)
            {
                currentColumn = startColumn;
                IRow row = exportExcelInfo.Worksheet.GetRow(currentRow) ?? exportExcelInfo.Worksheet.CreateRow(currentRow);
                wagersCellStyle = GetGrandTotalWagersCellStyle(workbook);
                amountCellStyle = GetGrandTotalAmountCellStyle(workbook);
                if (i == 0)
                {
                    NpoiExcelExtensions.SetBorderToFirstGrandTotalCell(wagersCellStyle);
                    NpoiExcelExtensions.SetBorderToFirstGrandTotalCell(amountCellStyle);
                }

                NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, total.Wagers[i].Name, wagersCellStyle);
                NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, total.Wagers[i].Amount, amountCellStyle);
                currentRow++;
            }

            CellRangeAddress totalCellRange = new CellRangeAddress(
                    exportExcelInfo.CurrentRow,
                    exportExcelInfo.CurrentRow + mergeRowNumber - 1,
                    currentColumn,
                    currentColumn);
            NpoiExcelExtensions.RenderMergedCells(exportExcelInfo, totalCellRange, total.Total, totalCellStyle);
        }

        private short GetRowColorIndex(bool isEven)
        {
            ////return isEven ? NpoiExcelExtensions.EvenRowColorIndex : NpoiExcelExtensions.BackgroundColorIndex;
            return NpoiExcelExtensions.BackgroundColorIndex;
        }

        private ICellStyle GetMonthCellStyle(HSSFWorkbook workbook, bool isEvent = true)
        {
            return NpoiExcelExtensions.TextStyle(
                workbook,
                GetRowColorIndex(isEvent),
                true,
                CRMHorizontalAlignment.Center);
        }

        private ICellStyle GetGrandTotalMonthCellStyle(HSSFWorkbook workbook)
        {
            return NpoiExcelExtensions.TextStyle(
                workbook,
                NpoiExcelExtensions.GrandTotalRowColorIndex,
                true,
                CRMHorizontalAlignment.Center,
                NpoiExcelExtensions.GrandTotalTextColorIndex);
        }

        private ICellStyle GetWagersCellStyle(HSSFWorkbook workbook, bool isEven = true)
        {
            return NpoiExcelExtensions.TextStyle(
                workbook,
                GetRowColorIndex(isEven),
                false,
                CRMHorizontalAlignment.Left);
        }

        private ICellStyle GetGrandTotalWagersCellStyle(HSSFWorkbook workbook)
        {
            return NpoiExcelExtensions.TextStyle(
                workbook,
                NpoiExcelExtensions.GrandTotalRowColorIndex,
                true,
                CRMHorizontalAlignment.Left,
                NpoiExcelExtensions.GrandTotalTextColorIndex);
        }

        private ICellStyle GetAmountCellStyle(HSSFWorkbook workbook, bool isEven = true)
        {
            return NpoiExcelExtensions.IntNumberStyle(
                workbook,
                GetRowColorIndex(isEven),
                false,
                CRMHorizontalAlignment.Right);
        }

        private ICellStyle GetGrandTotalAmountCellStyle(HSSFWorkbook workbook)
        {
            return NpoiExcelExtensions.IntNumberStyle(
                workbook,
                NpoiExcelExtensions.GrandTotalRowColorIndex,
                true,
                CRMHorizontalAlignment.Right,
                NpoiExcelExtensions.GrandTotalTextColorIndex);
        }

        private ICellStyle GetTotalCellStyle(HSSFWorkbook workbook, bool isEven = true)
        {
            return NpoiExcelExtensions.IntNumberStyle(
                workbook,
                GetRowColorIndex(isEven),
                true,
                CRMHorizontalAlignment.Right);
        }

        private ICellStyle GetGrandTotalCellStyle(HSSFWorkbook workbook)
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
