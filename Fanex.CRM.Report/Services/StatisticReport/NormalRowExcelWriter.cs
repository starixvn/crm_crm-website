namespace Fanex.CRM.Report.Services.StatisticReport
{
    using Fanex.CRM.Common.Extensions;
    using Fanex.CRM.Common.Models.Npoi;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Report.Models.StatisticReport;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;
    using System.Diagnostics.CodeAnalysis;

    public class NormalRowExcelWriter
    {
        private readonly ICellStyle _intStyle;
        private readonly ICellStyle _textStyle;
        private readonly ICellStyle _centerTextStyle;
        private readonly ICellStyle _boldIntStyle;
        private readonly ExportExcelInfo _exportExcelInfo;
        private readonly short _rowBackgroundColorIndex;

        public NormalRowExcelWriter(ExportExcelInfo exportExcelInfo)
        {
            _exportExcelInfo = exportExcelInfo;
            _rowBackgroundColorIndex = NpoiExtensions.BackgroundColorIndex;

            _intStyle = NpoiExtensions.GetIntegerNumberStyle(
               _exportExcelInfo.ExcelFile,
               _rowBackgroundColorIndex,
               false,
               CRMHorizontalAlignment.Right);
            _boldIntStyle = NpoiExtensions.GetIntegerNumberStyle(
               _exportExcelInfo.ExcelFile,
               _rowBackgroundColorIndex,
               true,
               CRMHorizontalAlignment.Right);
            _textStyle = NpoiExtensions.GetTextStyle(_exportExcelInfo.ExcelFile, _rowBackgroundColorIndex, false, CRMHorizontalAlignment.Left);
            _centerTextStyle = NpoiExtensions.GetTextStyle(_exportExcelInfo.ExcelFile, _rowBackgroundColorIndex, false, CRMHorizontalAlignment.Center);
        }

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public void WriteRow(
            ExportExcelInfo exportExcelInfo,
            StatisticReportInfo info,
            StatisticReportInfo groupInfo,
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
                NpoiExtensions.RenderMergedCells(
                    exportExcelInfo,
                    cellRangeAddress,
                    groupInfo.ShortName,
                    _centerTextStyle);
            }

            currentColumn++;
            NpoiExtensions.RenderCell(row, currentColumn, info.ShortName, _centerTextStyle);
            currentColumn++;
            var countryLength = info.ChildReports.Count;
            for (var i = 0; i < countryLength; i++)
            {
                var countryReport = info.ChildReports[i];
                if (countryReport.HasData)
                {
                    NpoiExtensions.RenderCell(row, currentColumn, countryReport.NumberOfIssues, _intStyle);
                    currentColumn++;
                }
                else
                {
                    NpoiExtensions.RenderCell(row, currentColumn, string.Empty, _textStyle);
                    currentColumn++;
                }
            }

            NpoiExtensions.RenderCell(row, currentColumn, info.NumberOfIssues, _boldIntStyle);
            currentColumn++;
            exportExcelInfo.CurrentRow += 1;
        }
    }
}
