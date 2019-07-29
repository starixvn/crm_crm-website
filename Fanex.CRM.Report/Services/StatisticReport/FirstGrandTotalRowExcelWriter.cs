namespace Fanex.CRM.Report.Services.StatisticReport
{
    using Fanex.CRM.Common.Enumerations;
    using Fanex.CRM.Common.Extensions;
    using Fanex.CRM.Common.Models.Npoi;

    public class FirstGrandTotalRowExcelWriter : TotalRowExcelWriter
    {
        private readonly ExportExcelInfo _exportExcelInfo;
        private readonly short _rowBackgroundColorIndex;

        public FirstGrandTotalRowExcelWriter()
        {
        }

        public FirstGrandTotalRowExcelWriter(ExportExcelInfo exportExcelInfo)
        {
            _exportExcelInfo = exportExcelInfo;
            _rowBackgroundColorIndex = NpoiExtensions.GrandTotalRowColorIndex;

            IntStyle = NpoiExtensions.GetIntegerNumberStyle(
               _exportExcelInfo.ExcelFile,
               _rowBackgroundColorIndex,
               true,
               CRMHorizontalAlignment.Right,
               NpoiExtensions.GrandTotalTextColorIndex);
            TextStyle = NpoiExtensions.GetTextStyle(
                 _exportExcelInfo.ExcelFile,
                 _rowBackgroundColorIndex,
                 true,
                 CRMHorizontalAlignment.Left,
                 NpoiExtensions.GrandTotalTextColorIndex);
            CenterTextStyle = NpoiExtensions.GetTextStyle(
                _exportExcelInfo.ExcelFile,
                _rowBackgroundColorIndex,
                true,
                CRMHorizontalAlignment.Center,
                NpoiExtensions.GrandTotalTextColorIndex);
        }
    }
}