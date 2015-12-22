// -----------------------------------------------------------------------
// <copyright file="FeedbackExcel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.Feedback.Excels
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using Fanex.CRM.Common.Excel;
    using Fanex.CRM.Common.Extensions;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Common.Models.Npoi;
    using Fanex.CRM.Feedback.Resources;
    using NPOI.HSSF.Util;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class FeedbackExcel : BaseExcel
    {
        private readonly short _closedBackgroundColorIndex = (short)HSSFColor.DarkTeal.Index;
        private readonly short _checkedBackgroundColorIndex = (short)HSSFColor.LightOrange.Index;
        private readonly short _pendingBackgroundColorIndex = (short)HSSFColor.Orange.Index;

        private string _title;
        private IEnumerable<FeedbackDetail> _feedbacks;
        private ExportExcelInfo exportExcelInfo;

        public FeedbackExcel(IEnumerable<FeedbackDetail> feedbacks, string excelFileName)
            : base(excelFileName)
        {
            _feedbacks = feedbacks;
            _title = excelFileName;
            exportExcelInfo = new ExportExcelInfo
            {
                ExcelFile = WorkBook,
                Worksheet = WorkBook.CreateSheet(ExcelFileName),
                StartRow = 0,
                StartColumn = 0,
                CurrentRow = 0,
                MaxColumnIndex = 4
            };

            SetStatusColorIndex();
        }

        public override void BuildWorkBook()
        {
            SetColumnsWidth();
            GenerateTitle();

            GenerateHeader();
            GenerateBodyData();

            NpoiExcelExtensions.ResponseStreamExcel(exportExcelInfo.ExcelFile, ExcelFileName);
        }

        private void SetColumnsWidth()
        {
            exportExcelInfo.ResetCurrentColumn();
            exportExcelInfo.SetColumnWidthByFontWidth(exportExcelInfo.CurrentColumn++, 400);
            exportExcelInfo.SetColumnWidthByFontWidth(exportExcelInfo.CurrentColumn++, 150);
            exportExcelInfo.SetColumnWidthByFontWidth(exportExcelInfo.CurrentColumn++, 350);
            exportExcelInfo.SetColumnWidthByFontWidth(exportExcelInfo.CurrentColumn++, 100);
            exportExcelInfo.SetColumnWidthByFontWidth(exportExcelInfo.CurrentColumn++, 200);
        }

        private void GenerateTitle()
        {
            int currentRowIndex = exportExcelInfo.CurrentRow;

            NpoiExcelExtensions.SetHeaderColorIndex(exportExcelInfo);
            var titleStyle = NpoiExcelExtensions.GetHeaderStyle(exportExcelInfo.ExcelFile, new ExcelStyleParam());
            exportExcelInfo.RenderHeaderTitle(_title, titleStyle);
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
            NpoiExcelExtensions.RenderCell(row, currentColumnIndex++, Labels.Feedback, headerStyle);
            NpoiExcelExtensions.RenderCell(row, currentColumnIndex++, Labels.Customer, headerStyle);
            NpoiExcelExtensions.RenderCell(row, currentColumnIndex++, Labels.Category, headerStyle);
            NpoiExcelExtensions.RenderCell(row, currentColumnIndex++, Labels.Status, headerStyle);
            NpoiExcelExtensions.RenderCell(row, currentColumnIndex++, Labels.RelatedCustomer, headerStyle);

            exportExcelInfo.CurrentRow++;
        }

        private void GenerateBodyData()
        {
            var normalRowWriter = new FeedbackRowExcelWriter(exportExcelInfo, NpoiExcelExtensions.BackgroundColorIndex);
            var closedRowWriter = new FeedbackRowExcelWriter(exportExcelInfo, _closedBackgroundColorIndex);
            var checkedRowWriter = new FeedbackRowExcelWriter(exportExcelInfo, _checkedBackgroundColorIndex);
            var pendingRowWriter = new FeedbackRowExcelWriter(exportExcelInfo, _pendingBackgroundColorIndex);

            foreach (var feedback in _feedbacks)
            {
                if (string.Equals(feedback.Status, Labels.Closed, StringComparison.OrdinalIgnoreCase))
                {
                    closedRowWriter.WriteRow(exportExcelInfo, feedback);
                }
                else if (string.Equals(feedback.Status, Labels.Checked, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(feedback.Status, Labels.Solved, StringComparison.OrdinalIgnoreCase))
                {
                    checkedRowWriter.WriteRow(exportExcelInfo, feedback);
                }
                else if (string.Equals(feedback.Status, Labels.Pending, StringComparison.OrdinalIgnoreCase))
                {
                    pendingRowWriter.WriteRow(exportExcelInfo, feedback);
                }
                else
                {
                    normalRowWriter.WriteRow(exportExcelInfo, feedback);
                }
            }
        }

        private void SetStatusColorIndex()
        {
            var closedBackgroundColor = Color.FromArgb(221, 221, 221);
            NpoiExcelExtensions.SetColorAtIndex(exportExcelInfo, closedBackgroundColor, _closedBackgroundColorIndex);

            var checkedBackgroundColor = Color.FromArgb(242, 242, 242);
            NpoiExcelExtensions.SetColorAtIndex(exportExcelInfo, checkedBackgroundColor, _checkedBackgroundColorIndex);

            var pendingBackgroundColor = Color.FromArgb(240, 237, 196);
            NpoiExcelExtensions.SetColorAtIndex(
                exportExcelInfo, pendingBackgroundColor, _pendingBackgroundColorIndex);
        }
    }
}
