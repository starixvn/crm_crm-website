// -----------------------------------------------------------------------
// <copyright file="FeedbackRowExcelWriter.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.Feedback.Excels
{
    using Fanex.CRM.Common.Extensions;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Common.Models.Npoi;
    using Fanex.CRM.Infrastructure.Models;
    using NPOI.SS.UserModel;
    using System;
    using System.Collections.Generic;
    using System.Linq;

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

        public virtual void WriteRow(ExportExcelInfo exportExcelInfo, FeedbackDetail feedback, IEnumerable<Setting> mainCategories)
        {
            int columnIndex = exportExcelInfo.StartColumn;
            var feedbackRow = exportExcelInfo.Worksheet.CreateRow(exportExcelInfo.CurrentRow);
            feedbackRow.Height = 1200;

            var categoryContents = BuildCategoryContent(feedback, mainCategories);
            var categoryCellValue = String.Join("\n\n", categoryContents);

            var feedbackCellValue = GetFeedbackCellValue(feedback);
            NpoiExtensions.RenderCell(feedbackRow, columnIndex++, feedbackCellValue, _cellStyle);
            NpoiExtensions.RenderCell(feedbackRow, columnIndex++, feedback.Username, _cellStyle);
            NpoiExtensions.RenderCell(feedbackRow, columnIndex++, categoryCellValue, _cellStyle);
            NpoiExtensions.RenderCell(feedbackRow, columnIndex++, feedback.Status, _cellStyle);
            NpoiExtensions.RenderCell(feedbackRow, columnIndex++, feedback.RelatedCustomer, _cellStyle);

            exportExcelInfo.CurrentRow++;
        }

        protected string[] BuildCategoryContent(FeedbackDetail feedback, IEnumerable<Setting> mainCategories)
        {
            return feedback.Settings
                    .Where(i => i.SettingType == "Category")
                   .GroupBy(s => s.SettingIdLv1)
                   .Select(s =>
                   {
                       var con = GetMainCategoryName(s.Key, mainCategories);
                       var subIds = s.Select(e => e.SettingIdLv2).ToList();
                       foreach (var id in subIds)
                       {
                           con = con + "\n  " + GetSubCategoryName(s.Key, id, mainCategories);
                       }

                       return con;
                   })
                   .ToArray();
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

        private string GetMainCategoryName(int mainCategoryId, IEnumerable<Setting> mainCategories)
        {
            var mainCategory = mainCategories.FirstOrDefault(c => c.Id == mainCategoryId);
            return mainCategory == null ? string.Empty : mainCategory.Name;
        }

        private string GetSubCategoryName(int mainCategoryId, int subCategoryId, IEnumerable<Setting> mainCategories)
        {
            var mainCategory = mainCategories.FirstOrDefault(c => c.Id == mainCategoryId);
            var child = mainCategory == null ? null : mainCategory.ChildSettings.FirstOrDefault(c => c.Id == subCategoryId);
            return child == null ? string.Empty : child.Name;
        }
    }
}
