// --------------------------------------------------------------------------
// <copyright file="NormalRowExcelWriter.cs" company="Nexcel Solutions Vietnam">
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
    /// Normal Row Excel Writer.
    /// </summary>
    public class NormalRowExcelWriter
    {
        private ICellStyle _intStyle;
        private ICellStyle _textStyle;
        private ICellStyle _centerTextStyle;
        private ICellStyle _centerIntStyle;
        private ExportExcelInfoNPOI _exportExcelInfo;
        private short _rowBackgroundColorIndex;

        public NormalRowExcelWriter(ExportExcelInfoNPOI exportExcelInfo)
        {
            _exportExcelInfo = exportExcelInfo;
            _rowBackgroundColorIndex = NpoiExcelExtensions.BackgroundColorIndex;

            _intStyle = NpoiExcelExtensions.IntNumberStyle(
               _exportExcelInfo.ExcelFile,
               _rowBackgroundColorIndex,
               false,
               HorizontalAlignment.RIGHT);
            _centerIntStyle = NpoiExcelExtensions.IntNumberStyle(
                _exportExcelInfo.ExcelFile,
                _rowBackgroundColorIndex,
                false,
                HorizontalAlignment.CENTER);

            _textStyle = NpoiExcelExtensions.TextStyle(
                 _exportExcelInfo.ExcelFile,
                 _rowBackgroundColorIndex,
                 false,
                 NpoiExcelExtensions.Alignment.Left);
            _centerTextStyle = NpoiExcelExtensions.TextStyle(
                _exportExcelInfo.ExcelFile,
                _rowBackgroundColorIndex,
                false,
                NpoiExcelExtensions.Alignment.Center);
        }

        /// <summary>
        /// Exports the dynamic report row.
        /// </summary>
        /// <param name="exportExcelInfo">The export excel information.</param>
        /// <param name="info">The information.</param>
        /// <param name="groupInfo">The group information.</param>
        /// <param name="infoIndex">Index of the information.</param>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public void WriteRow(
            ref ExportExcelInfoNPOI exportExcelInfo,
            SettlementReportInfo info,
            SettlementReportInfo groupInfo,
            int infoIndex)
        {
            CellRangeAddress cellRangeAddress;
            var row = exportExcelInfo.Worksheet.GetRow(exportExcelInfo.CurrentRow) ?? exportExcelInfo.Worksheet.CreateRow(exportExcelInfo.CurrentRow);
            var currentColumn = exportExcelInfo.CurrentColumn;

            if (infoIndex == 0)
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

            if (infoIndex == 0)
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
