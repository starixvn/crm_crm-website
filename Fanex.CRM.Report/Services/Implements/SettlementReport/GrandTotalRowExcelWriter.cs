// --------------------------------------------------------------------------
// <copyright file="FirstGrandTotalRowExcelWriter.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Services.Implements.SettlementReport
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.CRM.Infrastructure.Extension;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Report.Models.SettlementReport;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;

    /// <summary>
    /// First Grand Total Row Excel Writer.
    /// </summary>
    public class GrandTotalRowExcelWriter
    {
        private ICellStyle _intStyle;
        private ICellStyle _textStyle;
        private ICellStyle _centerTextStyle;
        private ExportExcelInfoNPOI _exportExcelInfo;
        private short _rowBackgroundColorIndex;
        private ICellStyle _centerIntStyle;
        private bool _isFirstRow;

        public GrandTotalRowExcelWriter(ExportExcelInfoNPOI exportExcelInfo, bool isFirstRow)
        {
            _exportExcelInfo = exportExcelInfo;
            _isFirstRow = isFirstRow;
            _rowBackgroundColorIndex = NpoiExcelExtensions.GrandTotalRowColorIndex;

            _intStyle = NpoiExcelExtensions.IntNumberStyle(
                _exportExcelInfo.ExcelFile,
                _rowBackgroundColorIndex,
                true,
                HorizontalAlignment.RIGHT);
            _textStyle = NpoiExcelExtensions.TextStyle(
                _exportExcelInfo.ExcelFile,
                _rowBackgroundColorIndex,
                true,
                NpoiExcelExtensions.Alignment.Left);
            _centerTextStyle = NpoiExcelExtensions.TextStyle(
                _exportExcelInfo.ExcelFile,
                _rowBackgroundColorIndex,
                true,
                NpoiExcelExtensions.Alignment.Center);
            _centerIntStyle = NpoiExcelExtensions.IntNumberStyle(
                _exportExcelInfo.ExcelFile,
                _rowBackgroundColorIndex,
                true,
                HorizontalAlignment.CENTER);

            if (isFirstRow)
            {
                NpoiExcelExtensions.SetBorderToFirstGrandTotalCell(_intStyle);
                NpoiExcelExtensions.SetBorderToFirstGrandTotalCell(_textStyle);
                NpoiExcelExtensions.SetBorderToFirstGrandTotalCell(_centerTextStyle);
                NpoiExcelExtensions.SetBorderToFirstGrandTotalCell(_centerIntStyle);
            }
        }

        /// <summary>
        /// Exports the dynamic report row.
        /// </summary>
        /// <param name="exportExcelInfo">The export excel information.</param>
        /// <param name="info">The information.</param>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public void WriteRow(
            ref ExportExcelInfoNPOI exportExcelInfo,
            SettlementReportInfo info,
            SettlementReportInfo groupInfo)
        {
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
                    ref exportExcelInfo,
                    cellRangeAddress,
                    groupInfo.Name,
                    _centerTextStyle);
            }

            currentColumn++;

            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, info.Name, _textStyle);
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, info.NumberOfIssues, _intStyle);

            if (_isFirstRow)
            {
                cellRangeAddress = new CellRangeAddress(
                    exportExcelInfo.CurrentRow,
                    exportExcelInfo.CurrentRow + groupInfo.ChildReports.Count - 1,
                    currentColumn,
                    currentColumn);
                NpoiExcelExtensions.RenderMergedCells(
                    ref exportExcelInfo,
                    cellRangeAddress,
                    groupInfo.NumberOfIssues,
                    _centerIntStyle);
            }

            exportExcelInfo.CurrentRow += 1;
        }
    }
}
