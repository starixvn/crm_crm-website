namespace Fanex.CRM.Report.Services.SettlementReport
{
    using Fanex.CRM.Common.Enumerations;
    using Fanex.CRM.Common.Extensions;
    using Fanex.CRM.Common.Models.Npoi;
    using Fanex.CRM.Report.Models.SettlementReport;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;

    public class OddEvenRowExcelWriter : SettlementReportExportExcelService
    {
        private readonly ICellStyle _intStyle;
        private readonly ICellStyle _textStyle;
        private readonly ICellStyle _centerTextStyle;
        private readonly ICellStyle _centerIntStyle;

        public OddEvenRowExcelWriter(ExportExcelInfo exportExcelInfo, bool isEvenRow)
        {
            ExportExcelInfo _exportExcelInfo = exportExcelInfo;
            short _rowBackgroundColorIndex = isEvenRow ?
                NpoiExtensions.EvenRowColorIndex :
                NpoiExtensions.BackgroundColorIndex;

            _intStyle = NpoiExtensions.GetIntegerNumberStyle(
               _exportExcelInfo.ExcelFile,
               _rowBackgroundColorIndex,
               false,
               ExcelHorizontalAlignment.Right);
            _centerIntStyle = NpoiExtensions.GetIntegerNumberStyle(
                _exportExcelInfo.ExcelFile,
                _rowBackgroundColorIndex,
                false,
                ExcelHorizontalAlignment.Center);

            _textStyle = NpoiExtensions.GetTextStyle(_exportExcelInfo.ExcelFile, _rowBackgroundColorIndex, false, ExcelHorizontalAlignment.Left);
            _centerTextStyle = NpoiExtensions.GetTextStyle(_exportExcelInfo.ExcelFile, _rowBackgroundColorIndex, false, ExcelHorizontalAlignment.Center);
        }

        public void WriteRow(
            ExportExcelInfo exportExcelInfo,
            SettlementReportInfo info,
            SettlementReportInfo groupInfo,
            int infoIndex)
        {
            CellRangeAddress cellRangeAddress;
            var row = exportExcelInfo.Worksheet.GetRow(exportExcelInfo.CurrentRow) ?? exportExcelInfo.Worksheet.CreateRow(exportExcelInfo.CurrentRow);
            var currentColumn = exportExcelInfo.CurrentColumn;

            if (infoIndex == 0)
            {
                ExportColumn(exportExcelInfo, groupInfo.ChildReports.Count, groupInfo.Name.ToUpperInvariant(), _centerTextStyle);
            }

            currentColumn++;
            NpoiExtensions.RenderCell(row, currentColumn, info.Name, _textStyle);
            currentColumn++;
            NpoiExtensions.RenderCell(row, currentColumn, info.NumberOfIssues, _intStyle);
            currentColumn++;

            if (infoIndex == 0)
            {
                cellRangeAddress = new CellRangeAddress(
                    exportExcelInfo.CurrentRow,
                    exportExcelInfo.CurrentRow + groupInfo.ChildReports.Count - 1,
                    currentColumn,
                    currentColumn);
                NpoiExtensions.RenderMergedCells(
                    exportExcelInfo,
                    cellRangeAddress,
                    groupInfo.NumberOfIssues,
                    _centerIntStyle);
            }

            exportExcelInfo.CurrentRow += 1;
        }
    }
}