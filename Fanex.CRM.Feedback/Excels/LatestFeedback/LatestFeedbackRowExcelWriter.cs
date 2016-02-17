// -----------------------------------------------------------------------
// <copyright file="LatestFeedbackRowExcelWriter.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.Feedback.Excels.LatestFeedback
{
    using Fanex.CRM.Common.Extensions;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Common.Models.Npoi;
    using Fanex.CRM.Infrastructure.Models;
    using NPOI.SS.UserModel;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class LatestFeedbackRowExcelWriter : FeedbackRowExcelWriter
    {
        public LatestFeedbackRowExcelWriter(ExportExcelInfo exportExcelInfo, short rowBackgroundColorIndex)
            : base(exportExcelInfo, rowBackgroundColorIndex)
        {
        }

        public override void WriteRow(ExportExcelInfo exportExcelInfo, FeedbackDetail feedback)
        {
            int columnIndex = exportExcelInfo.StartColumn;
            var feedbackRow = exportExcelInfo.Worksheet.CreateRow(exportExcelInfo.CurrentRow);
            feedbackRow.Height = 1200;

            var feedbackCellValue = GetFeedbackCellValue(feedback);
            NpoiExtensions.RenderCell(feedbackRow, columnIndex++, feedbackCellValue, _cellStyle);

            var categoryCellValue = GetCategoryCellValue(feedback);
            NpoiExtensions.RenderCell(feedbackRow, columnIndex++, categoryCellValue, _cellStyle);

            NpoiExtensions.RenderCell(feedbackRow, columnIndex++, feedback.Status, _cellStyle);
            NpoiExtensions.RenderCell(feedbackRow, columnIndex++, feedback.RelatedCustomer, _cellStyle);

            exportExcelInfo.CurrentRow++;
        }
    }
}
