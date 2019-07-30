namespace Fanex.CRM.Report.Excels.DeletedFeedbackStatistics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CRM.Common.Enumerations;
    using Fanex.CRM.Common.Excel;
    using Fanex.CRM.Common.Extensions;
    using Fanex.CRM.Common.Models.Npoi;
    using Fanex.CRM.Report.Models.DeletedFeedbackStatistics;
    using Fanex.CRM.Report.Resources;
    using NPOI.HSSF.Util;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;

    public class DeletedFeedbackStatisticsExcel : BaseExcel
    {
        private readonly DeletedFeedbackStatisticsViewModel _deletedFeedbackStatisticsReport;
        private readonly IEnumerable<Common.Models.Setting> _mainCategories;
        private readonly ExportExcelInfo _exportExcelInfo;

        public DeletedFeedbackStatisticsExcel(
            DeletedFeedbackStatisticsViewModel deletedFeedbackStatisticsReport,
            string excelFileName,
            IEnumerable<Common.Models.Setting> mainCategories)
            : base(excelFileName)
        {
            _deletedFeedbackStatisticsReport = deletedFeedbackStatisticsReport;
            _mainCategories = mainCategories;
            _exportExcelInfo = new ExportExcelInfo
            {
                ExcelFile = WorkBook,
                Worksheet = WorkBook.CreateSheet(ExcelFileName),
                StartRow = 0,
                StartColumn = 0,
                CurrentRow = 0,
                MaxColumnIndex = 6
            };
        }

        public override void BuildWorkBook()
        {
            SetColumnsWidth();
            GenerateTitle(_deletedFeedbackStatisticsReport.Name);

            GenerateHeader();
            GenerateBodyData();
            GenerateFooterData();

            NpoiExtensions.ResponseStreamExcel(_exportExcelInfo.ExcelFile, ExcelFileName);
        }

        private void SetColumnsWidth()
        {
            _exportExcelInfo.ResetCurrentColumn();
            _exportExcelInfo.SetColumnWidthByFontWidth(_exportExcelInfo.CurrentColumn++, 150);
            _exportExcelInfo.SetColumnWidthByFontWidth(_exportExcelInfo.CurrentColumn++, 150);
            _exportExcelInfo.SetColumnWidthByFontWidth(_exportExcelInfo.CurrentColumn++, 150);
            _exportExcelInfo.SetColumnWidthByFontWidth(_exportExcelInfo.CurrentColumn++, 350);
            _exportExcelInfo.SetColumnWidthByFontWidth(_exportExcelInfo.CurrentColumn++, 280);
            _exportExcelInfo.SetColumnWidthByFontWidth(_exportExcelInfo.CurrentColumn++, 150);
            _exportExcelInfo.SetColumnWidthByFontWidth(_exportExcelInfo.CurrentColumn++, 150);
        }

        private void GenerateTitle(string title)
        {
            int currentRowIndex = _exportExcelInfo.CurrentRow;

            NpoiExtensions.SetHeaderColorIndex(_exportExcelInfo);
            var titleStyle = NpoiExtensions.GetHeaderStyle(_exportExcelInfo.ExcelFile, new ExcelStyleParam());
            _exportExcelInfo.RenderHeaderTitle(title, titleStyle);
            currentRowIndex += 2;

            NpoiExtensions.RenderEmptyRow(_exportExcelInfo, currentRowIndex);
            _exportExcelInfo.CurrentRow = currentRowIndex + 1;
        }

        private void GenerateHeader()
        {
            int currentColumnIndex = _exportExcelInfo.StartColumn;

            var headerStyle = NpoiExtensions.GetHeaderStyle(_exportExcelInfo.ExcelFile, new ExcelStyleParam());
            headerStyle.WrapText = true;

            var row = _exportExcelInfo.Worksheet.CreateRow(_exportExcelInfo.CurrentRow);
            NpoiExtensions.RenderCell(row, currentColumnIndex++, Labels.Date, headerStyle);
            NpoiExtensions.RenderCell(row, currentColumnIndex++, Labels.Handler, headerStyle);
            NpoiExtensions.RenderCell(row, currentColumnIndex++, Labels.DeletedBy, headerStyle);
            NpoiExtensions.RenderCell(row, currentColumnIndex++, Labels.Feedback, headerStyle);
            NpoiExtensions.RenderCell(row, currentColumnIndex++, Labels.Category, headerStyle);
            NpoiExtensions.RenderCell(row, currentColumnIndex++, Labels.Customer, headerStyle);
            NpoiExtensions.RenderCell(row, currentColumnIndex, Labels.RelatedCustomer, headerStyle);

            _exportExcelInfo.CurrentRow++;
        }

        private void GenerateBodyData()
        {
            var currentRowIndex = _exportExcelInfo.CurrentRow;
            int currentColumnIndex;

            NpoiExtensions.SetGroupBackgroundColorIndex(_exportExcelInfo);
            var groupStyle = GetGroupedHandlerStyle();

            var feedbackStyle = NpoiExtensions.GetTextStyle(_exportExcelInfo.ExcelFile, NpoiExtensions.BackgroundColorIndex, false, ExcelHorizontalAlignment.Left);
            feedbackStyle.WrapText = true;

            var feedbackGroups = _deletedFeedbackStatisticsReport.DeletedFeedbackByHandlers;
            foreach (var group in feedbackGroups)
            {
                var groupedHandlerRowValue = string.Format("{0} ({1})", group.Handler, group.DeletedFeedbacks.Count());
                var cellRangeAddress = new CellRangeAddress(currentRowIndex, currentRowIndex, 0, _exportExcelInfo.MaxColumnIndex);
                NpoiExtensions.RenderMergedCells(_exportExcelInfo, cellRangeAddress, groupedHandlerRowValue, groupStyle);
                currentRowIndex++;

                foreach (var feedback in group.DeletedFeedbacks)
                {
                    currentColumnIndex = _exportExcelInfo.StartColumn;
                    var feedbackRow = _exportExcelInfo.Worksheet.CreateRow(currentRowIndex);
                    feedbackRow.Height = 600;

                    var categoryContents = NameMapping.GetCategoryContent(feedback.Settings, _mainCategories);

                    NpoiExtensions.RenderCell(feedbackRow, currentColumnIndex++, feedback.CreateDateString, feedbackStyle);
                    NpoiExtensions.RenderCell(feedbackRow, currentColumnIndex++, feedback.Handler, feedbackStyle);
                    NpoiExtensions.RenderCell(feedbackRow, currentColumnIndex++, feedback.Deletor, feedbackStyle);
                    NpoiExtensions.RenderCell(feedbackRow, currentColumnIndex++, feedback.Subject, feedbackStyle);

                    NpoiExtensions.RenderCell(feedbackRow, currentColumnIndex++, String.Join("\n\n", categoryContents), feedbackStyle);

                    NpoiExtensions.RenderCell(feedbackRow, currentColumnIndex++, feedback.Customer, feedbackStyle);
                    NpoiExtensions.RenderCell(feedbackRow, currentColumnIndex, feedback.RelatedCustomer, feedbackStyle);
                    currentRowIndex++;
                }

                _exportExcelInfo.CurrentRow = currentRowIndex;
            }
        }

        private ICellStyle GetGroupedHandlerStyle()
        {
            var cellStyle = NpoiExtensions.GetTextStyle(_exportExcelInfo.ExcelFile, NpoiExtensions.GroupBackgroundColorIndex, false, ExcelHorizontalAlignment.Left);
            var font = _exportExcelInfo.ExcelFile.CreateFont();
            font.FontHeightInPoints = NpoiExtensions.FontSize;
            font.FontName = NpoiExtensions.FontName;
            font.Color = HSSFColor.Blue.Index;
            font.Boldweight = (short)FontBoldWeight.Bold;

            cellStyle.SetFont(font);

            return cellStyle;
        }

        private void GenerateFooterData()
        {
            var currentRowIndex = _exportExcelInfo.CurrentRow;
            NpoiExtensions.SetGrandTotalColorIndex(_exportExcelInfo);
            var footerStyle = NpoiExtensions.GetTextStyleGrandTotal(_exportExcelInfo.ExcelFile);
            var grandTotalCellValue = string.Format("{0} ({1})", Labels.Grand_Total, _deletedFeedbackStatisticsReport.GrandTotal);
            var cellRangeAddress = new CellRangeAddress(currentRowIndex, currentRowIndex, 0, _exportExcelInfo.MaxColumnIndex);
            NpoiExtensions.RenderMergedCells(_exportExcelInfo, cellRangeAddress, grandTotalCellValue, footerStyle);
        }
    }
}