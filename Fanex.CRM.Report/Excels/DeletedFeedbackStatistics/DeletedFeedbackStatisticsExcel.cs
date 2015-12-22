// -----------------------------------------------------------------------
// <copyright file="DeletedFeedbackStatisticsExcel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.Report.Excels.DeletedFeedbackStatistics
{
    using Fanex.CRM.Common.Excel;
    using Fanex.CRM.Common.Extensions;
    using Fanex.CRM.Common.Models.Npoi;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Report.Models.DeletedFeedbackStatistics;
    using Fanex.CRM.Report.Resources;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;
    using NPOI.HSSF.Util;

    public class DeletedFeedbackStatisticsExcel : BaseExcel
    {
        private readonly DeletedFeedbackStatisticsViewModel _deletedFeedbackStatisticsReport;

        private ExportExcelInfo exportExcelInfo;

        public DeletedFeedbackStatisticsExcel(DeletedFeedbackStatisticsViewModel deletedFeedbackStatisticsReport, string excelFileName)
            : base(excelFileName)
        {
            _deletedFeedbackStatisticsReport = deletedFeedbackStatisticsReport;

            exportExcelInfo = new ExportExcelInfo
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

            NpoiExcelExtensions.ResponseStreamExcel(exportExcelInfo.ExcelFile, ExcelFileName);
        }

        private void SetColumnsWidth()
        {
            exportExcelInfo.ResetCurrentColumn();
            exportExcelInfo.SetColumnWidthByFontWidth(exportExcelInfo.CurrentColumn++, 150);
            exportExcelInfo.SetColumnWidthByFontWidth(exportExcelInfo.CurrentColumn++, 150);
            exportExcelInfo.SetColumnWidthByFontWidth(exportExcelInfo.CurrentColumn++, 150);
            exportExcelInfo.SetColumnWidthByFontWidth(exportExcelInfo.CurrentColumn++, 350);
            exportExcelInfo.SetColumnWidthByFontWidth(exportExcelInfo.CurrentColumn++, 280);
            exportExcelInfo.SetColumnWidthByFontWidth(exportExcelInfo.CurrentColumn++, 150);
            exportExcelInfo.SetColumnWidthByFontWidth(exportExcelInfo.CurrentColumn++, 150);
        }

        private void GenerateTitle(string title)
        {
            int currentRowIndex = exportExcelInfo.CurrentRow;

            NpoiExcelExtensions.SetHeaderColorIndex(exportExcelInfo);
            var titleStyle = NpoiExcelExtensions.GetHeaderStyle(exportExcelInfo.ExcelFile, new ExcelStyleParam());
            exportExcelInfo.RenderHeaderTitle(title, titleStyle);
            currentRowIndex += 2;

            NpoiExcelExtensions.RenderEmptyRow(exportExcelInfo, currentRowIndex);
            exportExcelInfo.CurrentRow = currentRowIndex + 1;
        }

        private void GenerateHeader()
        {
            int currentColumnIndex = exportExcelInfo.StartColumn;

            var headerStyle = NpoiExcelExtensions.GetHeaderStyle(exportExcelInfo.ExcelFile, new ExcelStyleParam());
            headerStyle.WrapText = true;

            var row = exportExcelInfo.Worksheet.CreateRow(exportExcelInfo.CurrentRow);
            NpoiExcelExtensions.RenderCell(row, currentColumnIndex++, Labels.Date, headerStyle);
            NpoiExcelExtensions.RenderCell(row, currentColumnIndex++, Labels.Handler, headerStyle);
            NpoiExcelExtensions.RenderCell(row, currentColumnIndex++, Labels.DeletedBy, headerStyle);
            NpoiExcelExtensions.RenderCell(row, currentColumnIndex++, Labels.Feedback, headerStyle);
            NpoiExcelExtensions.RenderCell(row, currentColumnIndex++, Labels.Category, headerStyle);
            NpoiExcelExtensions.RenderCell(row, currentColumnIndex++, Labels.Customer, headerStyle);
            NpoiExcelExtensions.RenderCell(row, currentColumnIndex++, Labels.RelatedCustomer, headerStyle);

            exportExcelInfo.CurrentRow++;
        }

        private void GenerateBodyData()
        {
            var currentRowIndex = exportExcelInfo.CurrentRow;
            int currentColumnIndex;

            NpoiExcelExtensions.SetGroupBackgroundColorIndex(exportExcelInfo);
            var groupStyle = GetGroupedHandlerStyle();
            
            var feedbackInfoStyle = NpoiExcelExtensions.GetTextStyle(exportExcelInfo.ExcelFile, NpoiExcelExtensions.BackgroundColorIndex, false, CRMHorizontalAlignment.Left);

            var groupedHandlers = _deletedFeedbackStatisticsReport.DeletedFeedbackByHandlers;
            foreach (var handler in groupedHandlers)
            {
                var groupedHandlerRowValue = string.Format("{0} ({1})", handler.Handler, handler.GrandTotal);
                var cellRangeAddress = new CellRangeAddress(currentRowIndex, currentRowIndex, 0, exportExcelInfo.MaxColumnIndex);
                NpoiExcelExtensions.RenderMergedCells(exportExcelInfo, cellRangeAddress, groupedHandlerRowValue, groupStyle);
                currentRowIndex++;

                foreach (var feedback in handler.DeletedFeedbacks)
                {
                    currentColumnIndex = exportExcelInfo.StartColumn;
                    var feedbackRow = exportExcelInfo.Worksheet.CreateRow(currentRowIndex);
                    feedbackRow.Height = 500;
                    NpoiExcelExtensions.RenderCell(feedbackRow, currentColumnIndex++, feedback.CreateDateString, feedbackInfoStyle);
                    NpoiExcelExtensions.RenderCell(feedbackRow, currentColumnIndex++, feedback.Handler, feedbackInfoStyle);
                    NpoiExcelExtensions.RenderCell(feedbackRow, currentColumnIndex++, feedback.Deletor, feedbackInfoStyle);
                    NpoiExcelExtensions.RenderCell(feedbackRow, currentColumnIndex++, feedback.Subject, feedbackInfoStyle);

                    var categoryCellValue = string.Format("{0}\n{1}", feedback.MainCategory, feedback.SubCategory);
                    NpoiExcelExtensions.RenderCell(feedbackRow, currentColumnIndex++, categoryCellValue, feedbackInfoStyle);

                    NpoiExcelExtensions.RenderCell(feedbackRow, currentColumnIndex++, feedback.Customer, feedbackInfoStyle);
                    NpoiExcelExtensions.RenderCell(feedbackRow, currentColumnIndex++, feedback.RelatedCustomer, feedbackInfoStyle);
                    currentRowIndex++;
                }

                exportExcelInfo.CurrentRow = currentRowIndex;
            }
        }

        private ICellStyle GetGroupedHandlerStyle()
        {
            var cellStyle = NpoiExcelExtensions.GetTextStyle(exportExcelInfo.ExcelFile, NpoiExcelExtensions.GroupBackgroundColorIndex, false, CRMHorizontalAlignment.Left);
            var font = exportExcelInfo.ExcelFile.CreateFont();
            font.FontHeightInPoints = NpoiExcelExtensions.FontSize;
            font.FontName = NpoiExcelExtensions.FontName;
            font.Color = (short)HSSFColor.Blue.Index;
            font.Boldweight = (short)FontBoldWeight.Bold;

            cellStyle.SetFont(font);

            return cellStyle;
        }

        private void GenerateFooterData()
        {
            var currentRowIndex = exportExcelInfo.CurrentRow;
            NpoiExcelExtensions.SetGrandTotalColorIndex(exportExcelInfo);
            var footerStyle = NpoiExcelExtensions.GetTextStyleGrandTotal(exportExcelInfo.ExcelFile);
            var grandTotalCellValue = string.Format("{0} ({1})", Labels.Grand_Total, _deletedFeedbackStatisticsReport.GrandTotal);
            var cellRangeAddress = new CellRangeAddress(currentRowIndex, currentRowIndex, 0, exportExcelInfo.MaxColumnIndex);
            NpoiExcelExtensions.RenderMergedCells(exportExcelInfo, cellRangeAddress, grandTotalCellValue, footerStyle);
        }
    }
}
