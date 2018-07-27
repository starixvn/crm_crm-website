namespace Fanex.CRM.Report.Services.SettlementReport
{
    using Fanex.CRM.Common.Extensions;
    using Fanex.CRM.Common.Models.Npoi;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Report.Models.SettlementReport;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;
    using System.Diagnostics.CodeAnalysis;

    public class OddEvenRowExcelWriter : SettlementReportExportExcelService
    {
        private readonly ICellStyle _intStyle;
        private readonly ICellStyle _textStyle;
        private readonly ICellStyle _centerTextStyle;
        private readonly ICellStyle _centerIntStyle;
        private readonly ExportExcelInfo _exportExcelInfo;
        private readonly short _rowBackgroundColorIndex;

        public OddEvenRowExcelWriter(ExportExcelInfo exportExcelInfo, bool isEvenRow)
        {
            _exportExcelInfo = exportExcelInfo;
            _rowBackgroundColorIndex = isEvenRow ?
                NpoiExtensions.EvenRowColorIndex :
                NpoiExtensions.BackgroundColorIndex;

            _intStyle = NpoiExtensions.GetIntegerNumberStyle(
               _exportExcelInfo.ExcelFile,
               _rowBackgroundColorIndex,
               false,
               CRMHorizontalAlignment.Right);
            _centerIntStyle = NpoiExtensions.GetIntegerNumberStyle(
                _exportExcelInfo.ExcelFile,
                _rowBackgroundColorIndex,
                false,
                CRMHorizontalAlignment.Center);

            _textStyle = NpoiExtensions.GetTextStyle(_exportExcelInfo.ExcelFile, _rowBackgroundColorIndex, false, CRMHorizontalAlignment.Left);
            _centerTextStyle = NpoiExtensions.GetTextStyle(_exportExcelInfo.ExcelFile, _rowBackgroundColorIndex, false, CRMHorizontalAlignment.Center);
        }

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
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
                ExportColumn(exportExcelInfo, groupInfo.ChildReports.Count, groupInfo.Name.ToUpper(), _centerTextStyle);
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
