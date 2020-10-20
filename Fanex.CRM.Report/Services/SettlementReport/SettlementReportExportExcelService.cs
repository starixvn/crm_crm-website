namespace Fanex.CRM.Report.Services.SettlementReport
{
    using Fanex.CRM.Common.Extensions;
    using Fanex.CRM.Common.Models.Npoi;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;

    public class SettlementReportExportExcelService
    {
        protected SettlementReportExportExcelService() { }

        public static void ExportColumn(
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