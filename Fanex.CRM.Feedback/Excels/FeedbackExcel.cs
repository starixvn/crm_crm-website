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
    using System.Linq;
    using System.Text.RegularExpressions;
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
        private string _title;
        private IEnumerable<FeedbackDetail> _feedbacks;

        public FeedbackExcel(IEnumerable<FeedbackDetail> feedbacks, string title, int columnsCount = 5)
            : base(Regex.Replace(title, @"\W+", "_"))
        {
            _feedbacks = feedbacks;
            _title = title;
            ExportExcelInfo = new ExportExcelInfo
            {
                ExcelFile = WorkBook,
                StartRow = 0,
                StartColumn = 0,
                CurrentRow = 0,
                MaxColumnIndex = columnsCount - 1
            };

            SetStatusColorIndex();
        }

        public ExportExcelInfo ExportExcelInfo { get; private set; }

        public FeedbackRowExcelWriter NormalRowWriter { get; set; }

        public FeedbackRowExcelWriter ClosedRowWriter { get; set; }

        public FeedbackRowExcelWriter CheckedRowWriter { get; set; }

        public FeedbackRowExcelWriter PendingRowWriter { get; set; }

        protected readonly short ClosedBackgroundColorIndex = (short)HSSFColor.DarkTeal.Index;
        protected readonly short CheckedBackgroundColorIndex = (short)HSSFColor.LightOrange.Index;
        protected readonly short PendingBackgroundColorIndex = (short)HSSFColor.Orange.Index;

        public override void BuildWorkBook()
        {
            GenerateBodyData();

            NpoiExcelExtensions.ResponseStreamExcel(ExportExcelInfo.ExcelFile, ExcelFileName);
        }

        protected virtual void SetColumnsWidth()
        {
            ExportExcelInfo.ResetCurrentColumn();
            ExportExcelInfo.SetColumnWidthByFontWidth(ExportExcelInfo.CurrentColumn++, 400);
            ExportExcelInfo.SetColumnWidthByFontWidth(ExportExcelInfo.CurrentColumn++, 150);
            ExportExcelInfo.SetColumnWidthByFontWidth(ExportExcelInfo.CurrentColumn++, 350);
            ExportExcelInfo.SetColumnWidthByFontWidth(ExportExcelInfo.CurrentColumn++, 100);
            ExportExcelInfo.SetColumnWidthByFontWidth(ExportExcelInfo.CurrentColumn++, 200);
        }

        private void GenerateTitle()
        {
            int currentRowIndex = ExportExcelInfo.CurrentRow;

            NpoiExcelExtensions.SetHeaderColorIndex(ExportExcelInfo);
            var titleStyle = NpoiExcelExtensions.GetHeaderStyle(ExportExcelInfo.ExcelFile, new ExcelStyleParam());
            ExportExcelInfo.RenderHeaderTitle(_title, titleStyle);
            currentRowIndex += 2;

            NpoiExcelExtensions.RenderEmptyRow(ExportExcelInfo, currentRowIndex);
            ExportExcelInfo.CurrentRow = currentRowIndex + 1;
        }

        protected virtual void GenerateHeader()
        {
            int currentColumnIndex = ExportExcelInfo.StartColumn;

            var headerStyle = NpoiExcelExtensions.GetHeaderStyle(ExportExcelInfo.ExcelFile, new ExcelStyleParam());
            headerStyle.WrapText = true;

            var row = ExportExcelInfo.Worksheet.CreateRow(ExportExcelInfo.CurrentRow);
            NpoiExcelExtensions.RenderCell(row, currentColumnIndex++, Labels.Feedback, headerStyle);
            NpoiExcelExtensions.RenderCell(row, currentColumnIndex++, Labels.Customer, headerStyle);
            NpoiExcelExtensions.RenderCell(row, currentColumnIndex++, Labels.Category, headerStyle);
            NpoiExcelExtensions.RenderCell(row, currentColumnIndex++, Labels.Status, headerStyle);
            NpoiExcelExtensions.RenderCell(row, currentColumnIndex++, Labels.RelatedCustomer, headerStyle);

            ExportExcelInfo.CurrentRow++;
        }

        protected virtual void InitWriter()
        {
            NormalRowWriter = new FeedbackRowExcelWriter(ExportExcelInfo, NpoiExcelExtensions.BackgroundColorIndex);
            ClosedRowWriter = new FeedbackRowExcelWriter(ExportExcelInfo, ClosedBackgroundColorIndex);
            CheckedRowWriter = new FeedbackRowExcelWriter(ExportExcelInfo, CheckedBackgroundColorIndex);
            PendingRowWriter = new FeedbackRowExcelWriter(ExportExcelInfo, PendingBackgroundColorIndex);
        }

        private void GenerateBodyData()
        {
            InitWriter();
            var totalRows = _feedbacks.Count();
            var maxRowPerSheet = NpoiExcelExtensions.MaxRowPerSheet;
            var numberOfSheet = (totalRows / maxRowPerSheet) + (totalRows % maxRowPerSheet != 0 ? 1 : 0);

            for (var sheetIndex = 0; sheetIndex < numberOfSheet; sheetIndex++)
            {
                var sheetName = numberOfSheet > 1 ?
                    string.Format("{0} - {1}", ExcelFileName, sheetIndex + 1) :
                    ExcelFileName;
                ExportExcelInfo.CreateSheet(sheetName);

                ExportExcelInfo.CurrentRow = 0;
                CreateSheetHeader();

                var startIndex = sheetIndex * maxRowPerSheet;
                var endIndex = startIndex + maxRowPerSheet;               
                for (var i = startIndex; i < endIndex && i < totalRows; i++)
                {
                    var feedback = _feedbacks.ElementAt(i);
                    if (string.Equals(feedback.Status, Labels.Closed, StringComparison.OrdinalIgnoreCase))
                    {
                        ClosedRowWriter.WriteRow(ExportExcelInfo, feedback);
                    }
                    else if (string.Equals(feedback.Status, Labels.Checked, StringComparison.OrdinalIgnoreCase) ||
                        string.Equals(feedback.Status, Labels.Solved, StringComparison.OrdinalIgnoreCase))
                    {
                        CheckedRowWriter.WriteRow(ExportExcelInfo, feedback);
                    }
                    else if (string.Equals(feedback.Status, Labels.Pending, StringComparison.OrdinalIgnoreCase))
                    {
                        PendingRowWriter.WriteRow(ExportExcelInfo, feedback);
                    }
                    else
                    {
                        NormalRowWriter.WriteRow(ExportExcelInfo, feedback);
                    }
                }
            }
        }

        private void CreateSheetHeader()
        {
            SetColumnsWidth();
            GenerateTitle();
            GenerateHeader();
        }

        private void SetStatusColorIndex()
        {
            var closedBackgroundColor = Color.FromArgb(221, 221, 221);
            NpoiExcelExtensions.SetColorAtIndex(ExportExcelInfo, closedBackgroundColor, ClosedBackgroundColorIndex);

            var checkedBackgroundColor = Color.FromArgb(242, 242, 242);
            NpoiExcelExtensions.SetColorAtIndex(ExportExcelInfo, checkedBackgroundColor, CheckedBackgroundColorIndex);

            var pendingBackgroundColor = Color.FromArgb(240, 237, 196);
            NpoiExcelExtensions.SetColorAtIndex(
                ExportExcelInfo, pendingBackgroundColor, PendingBackgroundColorIndex);
        }
    }
}