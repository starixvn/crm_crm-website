namespace Fanex.CRM.Report.Services.StatisticReport
{
    using Fanex.CRM.Common.Enumerations;
    using Fanex.CRM.Common.Extensions;
    using Fanex.CRM.Common.Models.Npoi;
    using Fanex.CRM.Report.Models.StatisticReport;
    using NPOI.SS.UserModel;

    public class TotalRowExcelWriter
    {
        private readonly ExportExcelInfo _exportExcelInfo;
        private readonly short _rowBackgroundColorIndex;
        private readonly bool _isBoldRow = true;

        public TotalRowExcelWriter()
        {
        }

        public TotalRowExcelWriter(ExportExcelInfo exportExcelInfo)
        {
            _exportExcelInfo = exportExcelInfo;
            _rowBackgroundColorIndex = NpoiExtensions.TotalRowColorIndex;

            IntStyle = NpoiExtensions.GetIntegerNumberStyle(
                _exportExcelInfo.ExcelFile,
                _rowBackgroundColorIndex,
                _isBoldRow,
                CRMHorizontalAlignment.Right);

            TextStyle = NpoiExtensions.GetTextStyle(
                _exportExcelInfo.ExcelFile,
                _rowBackgroundColorIndex,
                _isBoldRow,
                CRMHorizontalAlignment.Left);
            CenterTextStyle = NpoiExtensions.GetTextStyle(
                _exportExcelInfo.ExcelFile,
                _rowBackgroundColorIndex,
                _isBoldRow,
                CRMHorizontalAlignment.Center);
        }

        public ICellStyle IntStyle { get; set; }

        public ICellStyle TextStyle { get; set; }

        public ICellStyle CenterTextStyle { get; set; }

        public void WriteRow(ExportExcelInfo exportExcelInfo, StatisticReportInfo info)
        {
            var row = exportExcelInfo.Worksheet.GetRow(exportExcelInfo.CurrentRow) ?? exportExcelInfo.Worksheet.CreateRow(exportExcelInfo.CurrentRow);
            var currentColumn = exportExcelInfo.CurrentColumn;
            NpoiExtensions.RenderCell(row, currentColumn, info.ShortName, TextStyle);
            currentColumn++;
            NpoiExtensions.RenderCell(row, currentColumn, string.Empty, CenterTextStyle);
            currentColumn++;

            var countryLength = info.ChildReports.Count;
            for (var i = 0; i < countryLength; i++)
            {
                var countryReport = info.ChildReports[i];
                NpoiExtensions.RenderCell(row, currentColumn, countryReport.NumberOfIssues, IntStyle);
                currentColumn++;
            }

            NpoiExtensions.RenderCell(row, currentColumn, (int)info.NumberOfIssues, IntStyle);
            currentColumn++;
            exportExcelInfo.CurrentRow += 1;
        }
    }
}