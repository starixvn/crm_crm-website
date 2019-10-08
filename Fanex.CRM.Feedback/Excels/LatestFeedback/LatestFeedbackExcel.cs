namespace Fanex.CRM.Feedback.Excels.LatestFeedback
{
    using Fanex.CRM.Common.Extensions;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Common.Models.Npoi;
    using Fanex.CRM.Feedback.Resources;
    using System.Collections.Generic;

    public class LatestFeedbackExcel : FeedbackExcel
    {
        private const int ColumnsCount = 5;

        public LatestFeedbackExcel(IEnumerable<FeedbackDetail> feedbacks, string excelFileName, IEnumerable<Setting> mainCategories)
            : base(feedbacks, mainCategories, excelFileName, ColumnsCount)
        {
        }

        protected override void SetColumnsWidth()
        {
            ExportExcelInfo.ResetCurrentColumn();
            ExportExcelInfo.SetColumnWidthByFontWidth(ExportExcelInfo.CurrentColumn++, 200);
            ExportExcelInfo.SetColumnWidthByFontWidth(ExportExcelInfo.CurrentColumn++, 400);
            ExportExcelInfo.SetColumnWidthByFontWidth(ExportExcelInfo.CurrentColumn++, 350);
            ExportExcelInfo.SetColumnWidthByFontWidth(ExportExcelInfo.CurrentColumn++, 100);
            ExportExcelInfo.SetColumnWidthByFontWidth(ExportExcelInfo.CurrentColumn++, 200);
        }

        protected override void GenerateHeader()
        {
            int currentColumnIndex = ExportExcelInfo.StartColumn;

            var headerStyle = NpoiExtensions.GetHeaderStyle(ExportExcelInfo.ExcelFile, new ExcelStyleParam());
            headerStyle.WrapText = true;

            var row = ExportExcelInfo.Worksheet.CreateRow(ExportExcelInfo.CurrentRow);
            NpoiExtensions.RenderCell(row, currentColumnIndex++, Labels.RunningNo, headerStyle);
            NpoiExtensions.RenderCell(row, currentColumnIndex++, Labels.Feedback, headerStyle);
            NpoiExtensions.RenderCell(row, currentColumnIndex++, Labels.Category, headerStyle);
            NpoiExtensions.RenderCell(row, currentColumnIndex++, Labels.Status, headerStyle);
            NpoiExtensions.RenderCell(row, currentColumnIndex++, Labels.RelatedCustomer, headerStyle);

            ExportExcelInfo.CurrentRow++;
        }

        protected override void InitWriter()
        {
            NormalRowWriter = new LatestFeedbackRowExcelWriter(ExportExcelInfo, NpoiExtensions.BackgroundColorIndex);
            ClosedRowWriter = new LatestFeedbackRowExcelWriter(ExportExcelInfo, ClosedBackgroundColorIndex);
            CheckedRowWriter = new LatestFeedbackRowExcelWriter(ExportExcelInfo, CheckedBackgroundColorIndex);
            PendingRowWriter = new LatestFeedbackRowExcelWriter(ExportExcelInfo, PendingBackgroundColorIndex);
        }
    }
}