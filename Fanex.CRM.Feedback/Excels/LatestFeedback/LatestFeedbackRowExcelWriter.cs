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
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class LatestFeedbackRowExcelWriter : FeedbackRowExcelWriter
    {
        public LatestFeedbackRowExcelWriter(ExportExcelInfo exportExcelInfo, short rowBackgroundColorIndex)
            : base(exportExcelInfo, rowBackgroundColorIndex)
        {
        }

        public override void WriteRow(ExportExcelInfo exportExcelInfo, FeedbackDetail feedback, IEnumerable<Setting> mainCategories)
        {
            int columnIndex = exportExcelInfo.StartColumn;
            var feedbackRow = exportExcelInfo.Worksheet.CreateRow(exportExcelInfo.CurrentRow);
            feedbackRow.Height = 1200;

            var categoryContents = BuildCategoryContent(feedback, mainCategories);
            var categoryCellValue = String.Join("\n\n", categoryContents);

            var feedbackCellValue = GetFeedbackCellValue(feedback);
            NpoiExtensions.RenderCell(feedbackRow, columnIndex++, feedbackCellValue, _cellStyle);
            NpoiExtensions.RenderCell(feedbackRow, columnIndex++, categoryCellValue, _cellStyle);
            NpoiExtensions.RenderCell(feedbackRow, columnIndex++, feedback.Status, _cellStyle);
            NpoiExtensions.RenderCell(feedbackRow, columnIndex++, feedback.RelatedCustomer, _cellStyle);

            exportExcelInfo.CurrentRow++;
        }
    }
}
