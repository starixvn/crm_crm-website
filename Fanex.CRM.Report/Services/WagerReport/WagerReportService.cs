namespace Fanex.CRM.Report.Services.WagerReport
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CRM.Common.Enumerations;
    using Fanex.CRM.Common.Extensions;
    using Fanex.CRM.Common.Models.Npoi;
    using Fanex.CRM.Common.Services;
    using Fanex.CRM.Report.Criteria.WagerReport;
    using Fanex.CRM.Report.Entities.WagerReport;
    using Fanex.CRM.Report.Models.WagerReport;
    using Fanex.CRM.Report.Resources;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;

    public class WagerReportService : IWagerReportService
    {
        private readonly IBaseService baseService;

        public WagerReportService() : this(null)
        {
        }

        public WagerReportService(IBaseService baseService)
        {
            this.baseService = baseService ?? new BaseService();
        }

        public WagerReportModel GetWagerReport(GetWagerReportCriteria criteria)
        {
            var report = ParseWagerReport(criteria);
            report.ReportTitle = string.Format(Labels.WagerReportTitle, criteria.Year);

            return report;
        }

        public void ExportWagerReportExcel(GetWagerReportCriteria criteria)
        {
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
                NpoiExtensions.RenderNoInfoMessage(exportExcelInfo, exportExcelInfo.CurrentRow);
            }
            else
            {
                NpoiExtensions.SetColorAtIndex(exportExcelInfo, NpoiExtensions.EvenRowColor, NpoiExtensions.EvenRowColorIndex);
                NpoiExtensions.SetGrandTotalColorIndex(exportExcelInfo);

                ExportBody(ref exportExcelInfo, report.WagerByMonths);
                ExportFooter(ref exportExcelInfo, report.GrandTotal);
            }

            NpoiExtensions.ResponseStreamExcel(exportExcelInfo.ExcelFile, excelFileName);
        }

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

            var rawData = baseService.GetData<WagerRawData>(criteria)
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

        private static void ExportTitle(ref ExportExcelInfo exportExcelInfo, string title)
        {
            NpoiExtensions.SetHeaderColorIndex(exportExcelInfo);
            var columnWidths = new List<int> { 200, 200, 200, 200 };
            exportExcelInfo.InitColumnWidthByFontWidth(columnWidths);

            ICellStyle headerStyle = NpoiExtensions.GetHeaderStyle(exportExcelInfo.ExcelFile, new ExcelStyleParam());
            exportExcelInfo.RenderHeaderTitle(title, headerStyle);
        }

        private static void ExportHeader(ref ExportExcelInfo exportExcelInfo)
        {
            int currentColumn = exportExcelInfo.StartColumn;
            IRow row = exportExcelInfo.Worksheet.CreateRow(exportExcelInfo.CurrentRow);
            ICellStyle headerStyle = NpoiExtensions.GetHeaderStyle(exportExcelInfo.ExcelFile, new ExcelStyleParam());

            NpoiExtensions.RenderCell(row, currentColumn, Labels.Month, headerStyle);
            currentColumn++;
            NpoiExtensions.RenderCell(row, currentColumn, Labels.Wagers, headerStyle);
            currentColumn++;
            NpoiExtensions.RenderCell(row, currentColumn, Labels.Amount, headerStyle);
            currentColumn++;
            NpoiExtensions.RenderCell(row, currentColumn, Labels.Total, headerStyle);
            currentColumn++;
            exportExcelInfo.CurrentRow++;
        }

        private static void ExportBody(ref ExportExcelInfo exportExcelInfo, IList<WagerByMonth> wagerByMonths)
        {
            ICellStyle monthCellStyle, wagersCellStyle, amountCellStyle, totalCellStyle;
            HSSFWorkbook workbook = exportExcelInfo.ExcelFile;
            int startColumn, currentColumn, currentRow, mergeRowNumber;
            int reportLength = wagerByMonths.Count;

            WagerByMonth wagerByMonth;
            for (int i = 0; i < reportLength; i++)
            {
                wagerByMonth = wagerByMonths[i];
                currentRow = exportExcelInfo.CurrentRow;
                startColumn = exportExcelInfo.StartColumn;
                currentColumn = exportExcelInfo.CurrentColumn;
                mergeRowNumber = wagerByMonth.Wagers.Count;

                CellRangeAddress monthCellRange = new CellRangeAddress(
                    currentRow,
                    currentRow + mergeRowNumber - 1,
                    startColumn,
                    startColumn);
                monthCellStyle = GetMonthCellStyle(workbook);
                NpoiExtensions.RenderMergedCells(exportExcelInfo, monthCellRange, wagerByMonth.Month, monthCellStyle);
                startColumn++;

                wagersCellStyle = GetWagersCellStyle(workbook);
                amountCellStyle = GetAmountCellStyle(workbook);
                for (int j = 0; j < mergeRowNumber; j++)
                {
                    currentColumn = startColumn;
                    IRow row = exportExcelInfo.Worksheet.GetRow(currentRow) ?? exportExcelInfo.Worksheet.CreateRow(currentRow);
                    NpoiExtensions.RenderCell(row, currentColumn, wagerByMonth.Wagers[j].Name, wagersCellStyle);
                    currentColumn++;
                    NpoiExtensions.RenderCell(row, currentColumn, wagerByMonth.Wagers[j].Amount, amountCellStyle);
                    currentColumn++;

                    currentRow++;
                }

                CellRangeAddress totalCellRange = new CellRangeAddress(
                    exportExcelInfo.CurrentRow,
                    exportExcelInfo.CurrentRow + mergeRowNumber - 1,
                    currentColumn,
                    currentColumn);
                totalCellStyle = GetTotalCellStyle(workbook);
                NpoiExtensions.RenderMergedCells(exportExcelInfo, totalCellRange, wagerByMonth.Total, totalCellStyle);

                exportExcelInfo.CurrentRow = currentRow;
            }
        }

        private static void ExportFooter(ref ExportExcelInfo exportExcelInfo, WagerByMonth total)
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
            NpoiExtensions.RenderMergedCells(exportExcelInfo, grandTotalCellRange, total.Month, monthCellStyle);
            startColumn++;

            for (int i = 0; i < mergeRowNumber; i++)
            {
                currentColumn = startColumn;
                IRow row = exportExcelInfo.Worksheet.GetRow(currentRow) ?? exportExcelInfo.Worksheet.CreateRow(currentRow);
                wagersCellStyle = GetGrandTotalWagersCellStyle(workbook);
                amountCellStyle = GetGrandTotalAmountCellStyle(workbook);

                NpoiExtensions.RenderCell(row, currentColumn, total.Wagers[i].Name, wagersCellStyle);
                currentColumn++;
                NpoiExtensions.RenderCell(row, currentColumn, total.Wagers[i].Amount, amountCellStyle);
                currentColumn++;

                currentRow++;
            }

            CellRangeAddress totalCellRange = new CellRangeAddress(
                    exportExcelInfo.CurrentRow,
                    exportExcelInfo.CurrentRow + mergeRowNumber - 1,
                    currentColumn,
                    currentColumn);
            NpoiExtensions.RenderMergedCells(exportExcelInfo, totalCellRange, total.Total, totalCellStyle);
        }

        private static short GetRowColorIndex()
        {
            return NpoiExtensions.BackgroundColorIndex;
        }

        private static ICellStyle GetMonthCellStyle(HSSFWorkbook workbook)
        {
            return NpoiExtensions.GetTextStyle(
                workbook,
                GetRowColorIndex(),
                true,
                CRMHorizontalAlignment.Center);
        }

        private static ICellStyle GetGrandTotalMonthCellStyle(HSSFWorkbook workbook)
        {
            return NpoiExtensions.GetTextStyle(
                workbook,
                NpoiExtensions.GrandTotalRowColorIndex,
                true,
                CRMHorizontalAlignment.Center,
                NpoiExtensions.GrandTotalTextColorIndex);
        }

        private static ICellStyle GetWagersCellStyle(HSSFWorkbook workbook)
        {
            return NpoiExtensions.GetTextStyle(
                workbook,
                GetRowColorIndex(),
                false,
                CRMHorizontalAlignment.Left);
        }

        private static ICellStyle GetGrandTotalWagersCellStyle(HSSFWorkbook workbook)
        {
            return NpoiExtensions.GetTextStyle(
                workbook,
                NpoiExtensions.GrandTotalRowColorIndex,
                true,
                CRMHorizontalAlignment.Left,
                NpoiExtensions.GrandTotalTextColorIndex);
        }

        private static ICellStyle GetAmountCellStyle(HSSFWorkbook workbook)
        {
            return NpoiExtensions.GetIntegerNumberStyle(
                workbook,
                GetRowColorIndex(),
                false,
                CRMHorizontalAlignment.Right);
        }

        private static ICellStyle GetGrandTotalAmountCellStyle(HSSFWorkbook workbook)
        {
            return NpoiExtensions.GetIntegerNumberStyle(
                workbook,
                NpoiExtensions.GrandTotalRowColorIndex,
                true,
                CRMHorizontalAlignment.Right,
                NpoiExtensions.GrandTotalTextColorIndex);
        }

        private static ICellStyle GetTotalCellStyle(HSSFWorkbook workbook)
        {
            return NpoiExtensions.GetIntegerNumberStyle(
                workbook,
                GetRowColorIndex(),
                true,
                CRMHorizontalAlignment.Right);
        }

        private static ICellStyle GetGrandTotalCellStyle(HSSFWorkbook workbook)
        {
            return NpoiExtensions.GetIntegerNumberStyle(
                workbook,
                NpoiExtensions.GrandTotalRowColorIndex,
                true,
                CRMHorizontalAlignment.Right,
                NpoiExtensions.GrandTotalTextColorIndex);
        }

        #endregion Export excel helper
    }
}