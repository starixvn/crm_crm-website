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
    using Fanex.CRM.Report.Models.DeletedFeedbackStatistics;
    using Fanex.CRM.Report.Resources;
    using NPOI.SS.UserModel;

    public class DeletedFeedbackStatisticsExcel : BaseExcel
    {
        private readonly DeletedFeedbackStatisticsViewModel _deletedFeedbackStatisticsReport;

        public DeletedFeedbackStatisticsExcel(DeletedFeedbackStatisticsViewModel deletedFeedbackStatisticsReport, string excelFileName)
            : base(excelFileName)
        {
            _deletedFeedbackStatisticsReport = deletedFeedbackStatisticsReport;
        }

        public override void BuildWorkBook()
        {
            ExportExcelInfo exportExcelInfo = new ExportExcelInfo
            {
                ExcelFile = WorkBook,
                Worksheet = WorkBook.CreateSheet(ExcelFileName),
                StartRow = 0,
                StartColumn = 0,
                CurrentRow = 0,
                MaxColumnIndex = 7
            };

            SetColumnsWidth(exportExcelInfo);
            GenerateTitle(exportExcelInfo, _deletedFeedbackStatisticsReport.Name);

            GenerateHeader(exportExcelInfo);
            GenerateBodyData(exportExcelInfo);
            GenerateFooterData(exportExcelInfo);

            NpoiExcelExtensions.ResponseStreamExcel(exportExcelInfo.ExcelFile, ExcelFileName);
        }

        private void SetColumnsWidth(ExportExcelInfo exportExcelInfo)
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

        private void GenerateTitle(ExportExcelInfo exportExcelInfo, string title)
        {
            int currentRowIndex = exportExcelInfo.CurrentRow;

            NpoiExcelExtensions.SetHeaderColorIndex(exportExcelInfo);
            ICellStyle titleStyle = NpoiExcelExtensions.GetHeaderStyle(exportExcelInfo.ExcelFile, new ExcelStyleParam());
            exportExcelInfo.RenderHeaderTitle(title, titleStyle);
            currentRowIndex += 2;

            NpoiExcelExtensions.RenderEmptyRow(exportExcelInfo, currentRowIndex);
            exportExcelInfo.CurrentRow = currentRowIndex + 1;
        }

        private void GenerateHeader(ExportExcelInfo exportExcelInfo)
        {
            int currentRowIndex = exportExcelInfo.CurrentRow;
            int currentColumnIndex = exportExcelInfo.StartColumn;

            var headerStyle = NpoiExcelExtensions.GetHeaderStyle(exportExcelInfo.ExcelFile, new ExcelStyleParam());
            headerStyle.WrapText = true;

            var row = exportExcelInfo.Worksheet.CreateRow(currentRowIndex + 1);
            NpoiExcelExtensions.RenderCell(row, currentColumnIndex++, Labels.Date, headerStyle);
            NpoiExcelExtensions.RenderCell(row, currentColumnIndex++, Labels.Handler, headerStyle);
            NpoiExcelExtensions.RenderCell(row, currentColumnIndex++, Labels.DeletedBy, headerStyle);
            NpoiExcelExtensions.RenderCell(row, currentColumnIndex++, Labels.Feedback, headerStyle);
            NpoiExcelExtensions.RenderCell(row, currentColumnIndex++, Labels.Category, headerStyle);
            NpoiExcelExtensions.RenderCell(row, currentColumnIndex++, Labels.Customer, headerStyle);
            NpoiExcelExtensions.RenderCell(row, currentColumnIndex++, Labels.RelatedCustomer, headerStyle);

            exportExcelInfo.CurrentRow = currentRowIndex + 1;
        }

        private void GenerateBodyData(ExportExcelInfo exportExcelInfo)
        {
        }

        private void GenerateFooterData(ExportExcelInfo exportExcelInfo)
        {
        }

        private void GenerateRowValue(ExportExcelInfo exportExcelInfo, DeletedFeedback deletedFeedback, ICellStyle textStyle, ICellStyle numberStyle, ICellStyle totalCellStyle)
        {

        }
    }
}
