// -----------------------------------------------------------------------
// <copyright file="LatestFeedbackExcel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.Feedback.Excels.LatestFeedback
{
    using System.Collections.Generic;
    using Fanex.CRM.Common.Extensions;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Common.Models.Npoi;
    using Fanex.CRM.Feedback.Resources;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class LatestFeedbackExcel : FeedbackExcel
    {
        private const int ColumnsCount = 4;

        public LatestFeedbackExcel(IEnumerable<FeedbackDetail> feedbacks, string excelFileName)
            : base(feedbacks, excelFileName, ColumnsCount)
        {
        }

        protected override void SetColumnsWidth()
        {
            ExportExcelInfo.ResetCurrentColumn();
            ExportExcelInfo.SetColumnWidthByFontWidth(ExportExcelInfo.CurrentColumn++, 400);
            ExportExcelInfo.SetColumnWidthByFontWidth(ExportExcelInfo.CurrentColumn++, 350);
            ExportExcelInfo.SetColumnWidthByFontWidth(ExportExcelInfo.CurrentColumn++, 100);
            ExportExcelInfo.SetColumnWidthByFontWidth(ExportExcelInfo.CurrentColumn++, 200);
        }

        protected override void GenerateHeader()
        {
            int currentColumnIndex = ExportExcelInfo.StartColumn;

            var headerStyle = NpoiExcelExtensions.GetHeaderStyle(ExportExcelInfo.ExcelFile, new ExcelStyleParam());
            headerStyle.WrapText = true;

            var row = ExportExcelInfo.Worksheet.CreateRow(ExportExcelInfo.CurrentRow);
            NpoiExcelExtensions.RenderCell(row, currentColumnIndex++, Labels.Feedback, headerStyle);
            NpoiExcelExtensions.RenderCell(row, currentColumnIndex++, Labels.Category, headerStyle);
            NpoiExcelExtensions.RenderCell(row, currentColumnIndex++, Labels.Status, headerStyle);
            NpoiExcelExtensions.RenderCell(row, currentColumnIndex++, Labels.RelatedCustomer, headerStyle);

            ExportExcelInfo.CurrentRow++;
        }

        protected override void InitWriter()
        {
            NormalRowWriter = new LatestFeedbackRowExcelWriter(ExportExcelInfo, NpoiExcelExtensions.BackgroundColorIndex);
            ClosedRowWriter = new LatestFeedbackRowExcelWriter(ExportExcelInfo, ClosedBackgroundColorIndex);
            CheckedRowWriter = new LatestFeedbackRowExcelWriter(ExportExcelInfo, CheckedBackgroundColorIndex);
            PendingRowWriter = new LatestFeedbackRowExcelWriter(ExportExcelInfo, PendingBackgroundColorIndex);
        }
    }
}