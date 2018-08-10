namespace Fanex.CRM.Report.Services.SettlementReport
{
    using Fanex.CRM.Common.Extensions;
    using Fanex.CRM.Common.Models.Npoi;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Report.Models.SettlementReport;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;
    using System.Diagnostics.CodeAnalysis;

    public class GrandTotalRowExcelWriter : SettlementReportExportExcelService
    {
        private readonly ICellStyle _intStyle;
        private readonly ICellStyle _textStyle;
        private readonly ICellStyle _centerTextStyle;
        private readonly ExportExcelInfo _exportExcelInfo;
        private readonly short _rowBackgroundColorIndex;
        private readonly ICellStyle _centerIntStyle;
        private readonly bool _isFirstRow;

        public GrandTotalRowExcelWriter(ExportExcelInfo exportExcelInfo, bool isFirstRow)
        {
            _exportExcelInfo = exportExcelInfo;
            _isFirstRow = isFirstRow;
            _rowBackgroundColorIndex = NpoiExtensions.GrandTotalRowColorIndex;

            _intStyle = NpoiExtensions.GetIntegerNumberStyleGrandTotal(_exportExcelInfo.ExcelFile);
            _textStyle = NpoiExtensions.GetTextStyleGrandTotal(_exportExcelInfo.ExcelFile);
            _centerTextStyle = NpoiExtensions.GetTextStyle(
                _exportExcelInfo.ExcelFile,
                _rowBackgroundColorIndex,
                true,
                CRMHorizontalAlignment.Center,
                NpoiExtensions.GrandTotalTextColorIndex);
            _centerIntStyle = NpoiExtensions.GetIntegerNumberStyle(
                _exportExcelInfo.ExcelFile,
                _rowBackgroundColorIndex,
                true,
                CRMHorizontalAlignment.Center,
                NpoiExtensions.GrandTotalTextColorIndex);
        }

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public void WriteRow(
            ExportExcelInfo exportExcelInfo,
            SettlementReportInfo info,
            SettlementReportInfo groupInfo)
        {
            exportExcelInfo.ResetCurrentColumn();

            CellRangeAddress cellRangeAddress;
            var row = exportExcelInfo.Worksheet.GetRow(exportExcelInfo.CurrentRow) ?? exportExcelInfo.Worksheet.CreateRow(exportExcelInfo.CurrentRow);
            var currentColumn = exportExcelInfo.CurrentColumn;

            if (_isFirstRow)
            {
                cellRangeAddress = new CellRangeAddress(
               exportExcelInfo.CurrentRow,
               exportExcelInfo.CurrentRow + groupInfo.ChildReports.Count - 1,
               currentColumn,
               currentColumn);
                NpoiExtensions.RenderMergedCells(
                    exportExcelInfo,
                    cellRangeAddress,
                    groupInfo.Name,
                    _centerTextStyle);
            }

            currentColumn++;

            NpoiExtensions.RenderCell(row, currentColumn, info.Name, _textStyle);
            currentColumn++;
            NpoiExtensions.RenderCell(row, currentColumn, info.NumberOfIssues, _intStyle);
            currentColumn++;

            if (_isFirstRow)
            {
                exportExcelInfo.CurrentColumn = currentColumn;
                ExportColumn(exportExcelInfo, groupInfo.ChildReports.Count, groupInfo.NumberOfIssues, _centerIntStyle);
            }

            exportExcelInfo.CurrentRow += 1;
        }
    }
}
