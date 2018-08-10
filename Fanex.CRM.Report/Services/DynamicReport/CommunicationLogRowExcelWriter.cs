namespace Fanex.CRM.Report.Services.DynamicReport
{
    using Fanex.CRM.Common.Extensions;
    using Fanex.CRM.Common.Models.Npoi;
    using Fanex.CRM.Infrastructure.Extensions;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Report.Models;
    using Fanex.CRM.Report.Resources;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    public class CommunicationLogRowExcelWriter
    {
        private readonly ICellStyle _textStyle;
        private readonly ICellStyle _centerIntStyle;
        private readonly ICellStyle _centerTextStyle;
        private readonly ExportExcelInfo _exportExcelInfo;

        public CommunicationLogRowExcelWriter(
            ExportExcelInfo exportExcelInfo,
            short rowBackgroundColorIndex)
        {
            _exportExcelInfo = exportExcelInfo;

            _textStyle = NpoiExtensions.GetTextStyle(
                _exportExcelInfo.ExcelFile,
                rowBackgroundColorIndex,
                false,
                CRMHorizontalAlignment.Left);
            _centerIntStyle = NpoiExtensions.GetIntegerNumberStyle(
                _exportExcelInfo.ExcelFile,
                rowBackgroundColorIndex,
                false,
                CRMHorizontalAlignment.Center);
            _centerTextStyle = NpoiExtensions.GetTextStyle(
                _exportExcelInfo.ExcelFile,
                rowBackgroundColorIndex,
                false,
                CRMHorizontalAlignment.Center);
        }

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public void WriteRow(
            ExportExcelInfo exportExcelInfo,
            DynamicReportInfo info,
            int infoIndex,
            IEnumerable<Common.Models.Setting> mainCategories)
        {
            var categoryContents = NameMapping.GetCategoryContentWithoutSettingType(info.Settings, mainCategories);
            var categoryCellValue = String.Join("\n\n", categoryContents);

            CellRangeAddress cellRangeAddress = new CellRangeAddress(
                exportExcelInfo.CurrentRow,
                exportExcelInfo.CurrentRow + 1,
                exportExcelInfo.CurrentColumn,
                exportExcelInfo.CurrentColumn);
            NpoiExtensions.RenderMergedCells(exportExcelInfo, cellRangeAddress, infoIndex + 1, _centerIntStyle);
            exportExcelInfo.CurrentColumn++;

            cellRangeAddress = new CellRangeAddress(
                exportExcelInfo.CurrentRow,
                exportExcelInfo.CurrentRow + 1,
                exportExcelInfo.CurrentColumn,
                exportExcelInfo.CurrentColumn);
            NpoiExtensions.RenderMergedCells(
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
            NpoiExtensions.RenderMergedCells(
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
            NpoiExtensions.RenderMergedCells(
                exportExcelInfo,
                cellRangeAddress,
                categoryCellValue,
                _textStyle);
            exportExcelInfo.CurrentColumn++;

            cellRangeAddress = new CellRangeAddress(
                exportExcelInfo.CurrentRow,
                exportExcelInfo.CurrentRow + 1,
                exportExcelInfo.CurrentColumn,
                exportExcelInfo.CurrentColumn);
            NpoiExtensions.RenderMergedCells(
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
            NpoiExtensions.RenderMergedCells(
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
            NpoiExtensions.RenderMergedCells(
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
            NpoiExtensions.RenderMergedCells(
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
            NpoiExtensions.RenderMergedCells(
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
            NpoiExtensions.RenderMergedCells(
                exportExcelInfo,
                cellRangeAddress,
                info.Reviewer,
                _textStyle);
            exportExcelInfo.CurrentColumn++;
            exportExcelInfo.CurrentRow += 2;
        }
    }
}
