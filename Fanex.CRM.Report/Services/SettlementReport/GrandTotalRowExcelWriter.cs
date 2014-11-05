// --------------------------------------------------------------------------
// <copyright file="GrandTotalRowExcelWriter.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Services.SettlementReport
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.CRM.Common.Extensions;
    using Fanex.CRM.Common.Models.Npoi;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Report.Models.SettlementReport;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;

    /// <summary>
    /// Grand Total Row Excel Writer.
    /// </summary>
    public class GrandTotalRowExcelWriter : SettlementReportExportExcelService
    {
        private ICellStyle _intStyle;
        private ICellStyle _textStyle;
        private ICellStyle _centerTextStyle;
        private ExportExcelInfo _exportExcelInfo;
        private short _rowBackgroundColorIndex;
        private ICellStyle _centerIntStyle;
        private bool _isFirstRow;

        public GrandTotalRowExcelWriter(ExportExcelInfo exportExcelInfo, bool isFirstRow)
        {
            _exportExcelInfo = exportExcelInfo;
            _isFirstRow = isFirstRow;
            _rowBackgroundColorIndex = NpoiExcelExtensions.GrandTotalRowColorIndex;

            _intStyle = NpoiExcelExtensions.IntNumberStyleGrandTotal(_exportExcelInfo.ExcelFile);
            _textStyle = NpoiExcelExtensions.TextStyleGrandTotal(_exportExcelInfo.ExcelFile);
            _centerTextStyle = NpoiExcelExtensions.TextStyle(
                _exportExcelInfo.ExcelFile,
                _rowBackgroundColorIndex,
                true,
                CRMHorizontalAlignment.Center,
                NpoiExcelExtensions.GrandTotalTextColorIndex);
            _centerIntStyle = NpoiExcelExtensions.IntNumberStyle(
                _exportExcelInfo.ExcelFile,
                _rowBackgroundColorIndex,
                true,
                CRMHorizontalAlignment.Center,
                NpoiExcelExtensions.GrandTotalTextColorIndex);
        }

        /// <summary>
        /// Exports the dynamic report row.
        /// </summary>
        /// <param name="exportExcelInfo">The export excel information.</param>
        /// <param name="info">The information.</param>
        /// <param name="groupInfo">The group information.</param>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
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
                NpoiExcelExtensions.RenderMergedCells(
                    exportExcelInfo,
                    cellRangeAddress,
                    groupInfo.Name,
                    _centerTextStyle);
            }

            currentColumn++;

            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, info.Name, _textStyle);
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, info.NumberOfIssues, _intStyle);

            if (_isFirstRow)
            {
                exportExcelInfo.CurrentColumn = currentColumn;
                ExportColumn(exportExcelInfo, groupInfo.ChildReports.Count, groupInfo.NumberOfIssues, _centerIntStyle);
            }

            exportExcelInfo.CurrentRow += 1;
        }
    }
}
