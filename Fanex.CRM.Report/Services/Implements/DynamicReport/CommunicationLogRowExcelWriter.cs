// --------------------------------------------------------------------------
// <copyright file="CommunicationLogRowExcelWriter.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Services.Implements.DynamicReport
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.CRM.Infrastructure.Extension;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Infrastructure.Models.Npoi;
    using Fanex.CRM.Report.Resources;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;

    /// <summary>
    /// Communication Log Row Excel Writer.
    /// </summary>
    public class CommunicationLogRowExcelWriter
    {
        private ICellStyle _intStyle;
        private ICellStyle _textStyle;
        private ICellStyle _centerTextStyle;
        private ExportExcelInfoNPOI _exportExcelInfo;
        private short _rowBackgroundColorIndex;

        public CommunicationLogRowExcelWriter(
            ExportExcelInfoNPOI exportExcelInfo,
            short rowBackgroundColorIndex)
        {
            _exportExcelInfo = exportExcelInfo;
            _rowBackgroundColorIndex = rowBackgroundColorIndex;

            _intStyle = NpoiExcelExtensions.IntNumberStyle(
                _exportExcelInfo.ExcelFile,
                rowBackgroundColorIndex,
                false,
                HorizontalAlignment.Left);
            _textStyle = NpoiExcelExtensions.TextStyle(
                _exportExcelInfo.ExcelFile,
                rowBackgroundColorIndex,
                false,
                CRMHorizontalAlignment.Left);
            _centerTextStyle = NpoiExcelExtensions.TextStyle(
                _exportExcelInfo.ExcelFile,
                rowBackgroundColorIndex,
                false,
                CRMHorizontalAlignment.Center);
        }

        /// <summary>
        /// Exports the dynamic report row.
        /// </summary>
        /// <param name="exportExcelInfo">The export excel information.</param>
        /// <param name="info">The information.</param>
        /// <param name="infoIndex">Index of the information.</param>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public void WriteRow(
            ref ExportExcelInfoNPOI exportExcelInfo,
            DynamicReportInfo info,
            int infoIndex)
        {
            CellRangeAddress cellRangeAddress = new CellRangeAddress(
                exportExcelInfo.CurrentRow,
                exportExcelInfo.CurrentRow + 1,
                exportExcelInfo.CurrentColumn,
                exportExcelInfo.CurrentColumn);
            NpoiExcelExtensions.RenderMergedCells(
                ref exportExcelInfo,
                cellRangeAddress,
                infoIndex + 1,
                _intStyle);
            exportExcelInfo.CurrentColumn++;

            cellRangeAddress = new CellRangeAddress(
                exportExcelInfo.CurrentRow,
                exportExcelInfo.CurrentRow + 1,
                exportExcelInfo.CurrentColumn,
                exportExcelInfo.CurrentColumn);
            NpoiExcelExtensions.RenderMergedCells(
                ref exportExcelInfo,
                cellRangeAddress,
                info.RunningNumber,
                _textStyle);
            exportExcelInfo.CurrentColumn++;

            cellRangeAddress = new CellRangeAddress(
                exportExcelInfo.CurrentRow,
                exportExcelInfo.CurrentRow + 1,
                exportExcelInfo.CurrentColumn,
                exportExcelInfo.CurrentColumn);
            NpoiExcelExtensions.RenderMergedCells(
                ref exportExcelInfo,
                cellRangeAddress,
                info.PeriodRange.FromDate.ToShortFormatedDate() + "\n" + info.PeriodRange.FromDate.ToLongTimeString(),
                _centerTextStyle);
            exportExcelInfo.CurrentColumn++;

            cellRangeAddress = new CellRangeAddress(
                exportExcelInfo.CurrentRow,
                exportExcelInfo.CurrentRow + 1,
                exportExcelInfo.CurrentColumn,
                exportExcelInfo.CurrentColumn);
            NpoiExcelExtensions.RenderMergedCells(
                ref exportExcelInfo,
                cellRangeAddress,
                info.MainCategory.Name + "\n" + info.SubCategory.Name,
                _textStyle);
            exportExcelInfo.CurrentColumn++;

            cellRangeAddress = new CellRangeAddress(
                exportExcelInfo.CurrentRow,
                exportExcelInfo.CurrentRow + 1,
                exportExcelInfo.CurrentColumn,
                exportExcelInfo.CurrentColumn);
            NpoiExcelExtensions.RenderMergedCells(
                ref exportExcelInfo,
                cellRangeAddress,
                info.Subject,
                _textStyle);
            exportExcelInfo.CurrentColumn++;

            cellRangeAddress = new CellRangeAddress(
                exportExcelInfo.CurrentRow,
                exportExcelInfo.CurrentRow + 1,
                exportExcelInfo.CurrentColumn,
                exportExcelInfo.CurrentColumn);
            NpoiExcelExtensions.RenderMergedCells(
                ref exportExcelInfo,
                cellRangeAddress,
                info.StatusName,
                _centerTextStyle);
            exportExcelInfo.CurrentColumn++;

            cellRangeAddress = new CellRangeAddress(
                exportExcelInfo.CurrentRow,
                exportExcelInfo.CurrentRow + 1,
                exportExcelInfo.CurrentColumn,
                exportExcelInfo.CurrentColumn);
            NpoiExcelExtensions.RenderMergedCells(
                ref exportExcelInfo,
                cellRangeAddress,
                string.Format(Labels.Format_Duration, info.Duration.Hours, info.Duration.Minutes, info.Duration.Seconds),
                _centerTextStyle);
            exportExcelInfo.CurrentColumn++;

            cellRangeAddress = new CellRangeAddress(
                exportExcelInfo.CurrentRow,
                exportExcelInfo.CurrentRow + 1,
                exportExcelInfo.CurrentColumn,
                exportExcelInfo.CurrentColumn);
            NpoiExcelExtensions.RenderMergedCells(
                ref exportExcelInfo,
                cellRangeAddress,
                info.Handler,
                _textStyle);
            exportExcelInfo.CurrentColumn++;

            cellRangeAddress = new CellRangeAddress(
                exportExcelInfo.CurrentRow,
                exportExcelInfo.CurrentRow + 1,
                exportExcelInfo.CurrentColumn,
                exportExcelInfo.CurrentColumn);
            NpoiExcelExtensions.RenderMergedCells(
                ref exportExcelInfo,
                cellRangeAddress,
                info.Reviewer,
                _textStyle);
            exportExcelInfo.CurrentColumn++;
            exportExcelInfo.CurrentRow += 2;
        }
    }
}
