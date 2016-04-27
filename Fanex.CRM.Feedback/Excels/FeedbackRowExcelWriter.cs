// -----------------------------------------------------------------------
// <copyright file="FeedbackRowExcelWriter.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.Feedback.Excels
{
    using System.Text;
    using Fanex.CRM.Common.Extensions;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Common.Models.Npoi;
    using Fanex.CRM.Infrastructure.Models;
    using NPOI.SS.UserModel;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class FeedbackRowExcelWriter
    {
        private ExportExcelInfo _exportExcelInfo;

        public FeedbackRowExcelWriter(ExportExcelInfo exportExcelInfo, short rowBackgroundColorIndex)
        {
            _exportExcelInfo = exportExcelInfo;

            _cellStyle = NpoiExtensions.GetTextStyle(
                _exportExcelInfo.ExcelFile,
                rowBackgroundColorIndex,
                false,
                CRMHorizontalAlignment.Left);
            _cellStyle.WrapText = true;
        }

        public ICellStyle _cellStyle { get; private set; }

        public virtual void WriteRow(ExportExcelInfo exportExcelInfo, FeedbackDetail feedback)
        {
            int columnIndex = exportExcelInfo.StartColumn;
            var feedbackRow = exportExcelInfo.Worksheet.CreateRow(exportExcelInfo.CurrentRow);
            feedbackRow.Height = 1200;

            var feedbackCellValue = GetFeedbackCellValue(feedback);
            NpoiExtensions.RenderCell(feedbackRow, columnIndex++, feedbackCellValue, _cellStyle);

            NpoiExtensions.RenderCell(feedbackRow, columnIndex++, feedback.Username, _cellStyle);

            var categoryCellValue = GetCategoryCellValue(feedback);
            NpoiExtensions.RenderCell(feedbackRow, columnIndex++, categoryCellValue, _cellStyle);

            NpoiExtensions.RenderCell(feedbackRow, columnIndex++, feedback.Status, _cellStyle);
            NpoiExtensions.RenderCell(feedbackRow, columnIndex++, feedback.RelatedCustomer, _cellStyle);

            exportExcelInfo.CurrentRow++;
        }

        protected string GetFeedbackCellValue(FeedbackDetail feedback)
        {
            var cellValue = string.Format(
                "{0}\n{1}\n{2}\n{3}", 
                feedback.Subject,
                "Feedback date " + feedback.FeedbackDateString,
                "By " + feedback.CreatorName + " on " + feedback.CreateDateString,
                "Solving Duration: " + feedback.DurationTime);

            if (!string.IsNullOrEmpty(feedback.ReviewerName))
            {
                var reviewText = string.Format("\nReviewed by {0} on {1}", feedback.ReviewerName, feedback.SolveDateString);
                return cellValue + reviewText;
            }

            return cellValue;
        }

        protected string GetCategoryCellValue(FeedbackDetail feedback)
        {
            return string.Format("{0}\n{1}", feedback.MainCategory, feedback.SubCategory);
        }
    }
}
