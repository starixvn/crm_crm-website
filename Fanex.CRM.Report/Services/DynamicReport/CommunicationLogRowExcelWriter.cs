// --------------------------------------------------------------------------
// <copyright file="CommunicationLogRowExcelWriter.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Services.DynamicReport
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.CRM.Common.Extensions;
    using Fanex.CRM.Common.Models.Npoi;
    using Fanex.CRM.Infrastructure.Extensions;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Report.Models;
    using Fanex.CRM.Report.Resources;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;

    /// <summary>
    /// Communication Log Row Excel Writer.
    /// </summary>
    public class CommunicationLogRowExcelWriter
    {
        private ICellStyle _textStyle;
        private ICellStyle _centerIntStyle;
        private ICellStyle _centerTextStyle;
        private ExportExcelInfo _exportExcelInfo;

        public CommunicationLogRowExcelWriter(
            ExportExcelInfo exportExcelInfo,
            short rowBackgroundColorIndex)
        {
            _exportExcelInfo = exportExcelInfo;

            _textStyle = NpoiExcelExtensions.GetTextStyle(
                _exportExcelInfo.ExcelFile,
                rowBackgroundColorIndex,
                false,
                CRMHorizontalAlignment.Left);
            _centerIntStyle = NpoiExcelExtensions.GetIntegerNumberStyle(
                _exportExcelInfo.ExcelFile,
                rowBackgroundColorIndex,
                false,
                CRMHorizontalAlignment.Center);
            _centerTextStyle = NpoiExcelExtensions.GetTextStyle(
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
        public void WriteRow(ExportExcelInfo exportExcelInfo, DynamicReportInfo info, int infoIndex)
        {
            CellRangeAddress cellRangeAddress = new CellRangeAddress(
                exportExcelInfo.CurrentRow,
                exportExcelInfo.CurrentRow + 1,
                exportExcelInfo.CurrentColumn,
                exportExcelInfo.CurrentColumn);
            NpoiExcelExtensions.RenderMergedCells(exportExcelInfo, cellRangeAddress, infoIndex + 1, _centerIntStyle);
            exportExcelInfo.CurrentColumn++;

            cellRangeAddress = new CellRangeAddress(
                exportExcelInfo.CurrentRow,
                exportExcelInfo.CurrentRow + 1,
                exportExcelInfo.CurrentColumn,
                exportExcelInfo.CurrentColumn);
            NpoiExcelExtensions.RenderMergedCells(
                exportExcelInfo,
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
                exportExcelInfo,
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
                exportExcelInfo,
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
                exportExcelInfo,
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
                exportExcelInfo,
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
                exportExcelInfo,
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
                exportExcelInfo,
                cellRangeAddress,
                info.Username,
                _textStyle);
            exportExcelInfo.CurrentColumn++;

            cellRangeAddress = new CellRangeAddress(
                exportExcelInfo.CurrentRow,
                exportExcelInfo.CurrentRow + 1,
                exportExcelInfo.CurrentColumn,
                exportExcelInfo.CurrentColumn);
            NpoiExcelExtensions.RenderMergedCells(
                exportExcelInfo,
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
                exportExcelInfo,
                cellRangeAddress,
                info.Reviewer,
                _textStyle);
            exportExcelInfo.CurrentColumn++;
            exportExcelInfo.CurrentRow += 2;
        }
    }
}
