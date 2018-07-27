namespace Fanex.CRM.Report.Services.SettlementReport
{
    using Fanex.CRM.Common.Extensions;
    using Fanex.CRM.Common.Models.Npoi;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;

    public class SettlementReportExportExcelService
    {
        /// <summary>
        /// Exports the column.
        /// </summary>
        /// <param name="exportExcelInfo">The export excel information.</param>
        /// <param name="rowSpan">The row span.</param>
        /// <param name="cellValue">The cell value.</param>
        /// <param name="cellStyle">The cell style.</param>
        public void ExportColumn(
            ExportExcelInfo exportExcelInfo,
            int rowSpan,
            object cellValue,
            ICellStyle cellStyle)
        {
            var cellRangeAddress = new CellRangeAddress(
                exportExcelInfo.CurrentRow,
                exportExcelInfo.CurrentRow + rowSpan - 1,
                exportExcelInfo.CurrentColumn,
                exportExcelInfo.CurrentColumn);
            NpoiExtensions.RenderMergedCells(
                exportExcelInfo,
                cellRangeAddress,
                cellValue,
                cellStyle);
        }
    }
}
