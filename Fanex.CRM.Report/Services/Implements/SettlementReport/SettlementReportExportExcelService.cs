// --------------------------------------------------------------------------
// <copyright file="SettlementReportExportExcelService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Services.Implements.SettlementReport
{
    using Fanex.CRM.Infrastructure.Extension;
    using Fanex.CRM.Infrastructure.Models.Npoi;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;

    /// <summary>
    /// Settlement Report Export Excel Service.
    /// </summary>
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
            NpoiExcelExtensions.RenderMergedCells(
                exportExcelInfo,
                cellRangeAddress,
                cellValue,
                cellStyle);
        }
    }
}
