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

    public class FeedbackExcel : BaseExcel
    {
        private readonly string _title;
        private readonly IEnumerable<FeedbackDetail> _feedbacks;
        private readonly IEnumerable<Setting> _mainCategories;

        public FeedbackExcel(IEnumerable<FeedbackDetail> feedbacks, IEnumerable<Setting> mainCategories, string title, int columnsCount = 5)
            : base(Regex.Replace(title, @"\W+", "_"))
        {
            _feedbacks = feedbacks;
            _mainCategories = mainCategories;
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

        protected readonly short ClosedBackgroundColorIndex = HSSFColor.DarkTeal.Index;
        protected readonly short CheckedBackgroundColorIndex = HSSFColor.LightOrange.Index;
        protected readonly short PendingBackgroundColorIndex = HSSFColor.Orange.Index;

        public override void BuildWorkBook()
        {
            GenerateBodyData();

            NpoiExtensions.ResponseStreamExcel(ExportExcelInfo.ExcelFile, ExcelFileName);
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

            NpoiExtensions.SetHeaderColorIndex(ExportExcelInfo);
            var titleStyle = NpoiExtensions.GetHeaderStyle(ExportExcelInfo.ExcelFile, new ExcelStyleParam());
            ExportExcelInfo.RenderHeaderTitle(_title, titleStyle);
            currentRowIndex += 2;

            NpoiExtensions.RenderEmptyRow(ExportExcelInfo, currentRowIndex);
            ExportExcelInfo.CurrentRow = currentRowIndex + 1;
        }

        protected virtual void GenerateHeader()
        {
            int currentColumnIndex = ExportExcelInfo.StartColumn;

            var headerStyle = NpoiExtensions.GetHeaderStyle(ExportExcelInfo.ExcelFile, new ExcelStyleParam());
            headerStyle.WrapText = true;

            var row = ExportExcelInfo.Worksheet.CreateRow(ExportExcelInfo.CurrentRow);
            NpoiExtensions.RenderCell(row, currentColumnIndex++, Labels.Feedback, headerStyle);
            NpoiExtensions.RenderCell(row, currentColumnIndex++, Labels.Customer, headerStyle);
            NpoiExtensions.RenderCell(row, currentColumnIndex++, Labels.Category, headerStyle);
            NpoiExtensions.RenderCell(row, currentColumnIndex++, Labels.Status, headerStyle);
            NpoiExtensions.RenderCell(row, currentColumnIndex++, Labels.RelatedCustomer, headerStyle);

            ExportExcelInfo.CurrentRow++;
        }

        protected virtual void InitWriter()
        {
            NormalRowWriter = new FeedbackRowExcelWriter(ExportExcelInfo, NpoiExtensions.BackgroundColorIndex);
            ClosedRowWriter = new FeedbackRowExcelWriter(ExportExcelInfo, ClosedBackgroundColorIndex);
            CheckedRowWriter = new FeedbackRowExcelWriter(ExportExcelInfo, CheckedBackgroundColorIndex);
            PendingRowWriter = new FeedbackRowExcelWriter(ExportExcelInfo, PendingBackgroundColorIndex);
        }

        private void GenerateBodyData()
        {
            InitWriter();
            var totalRows = _feedbacks.Count();
            var numberOfSheet = (totalRows / NpoiExtensions.MaxRowPerSheet) + (totalRows % NpoiExtensions.MaxRowPerSheet != 0 ? 1 : 0);

            for (var sheetIndex = 0; sheetIndex < numberOfSheet; sheetIndex++)
            {
                var sheetName = numberOfSheet > 1 ?
                    string.Format("{0} - {1}", ExcelFileName, sheetIndex + 1) :
                    ExcelFileName;
                ExportExcelInfo.CreateSheet(sheetName);

                ExportExcelInfo.CurrentRow = 0;
                CreateSheetHeader();

                var startIndex = sheetIndex * NpoiExtensions.MaxRowPerSheet;
                var endIndex = startIndex + NpoiExtensions.MaxRowPerSheet;
                for (var i = startIndex; i < endIndex && i < totalRows; i++)
                {
                    var feedback = _feedbacks.ElementAt(i);
                    if (string.Equals(feedback.Status, Labels.Closed, StringComparison.OrdinalIgnoreCase))
                    {
                        ClosedRowWriter.WriteRow(ExportExcelInfo, feedback, _mainCategories);
                    }
                    else if (string.Equals(feedback.Status, Labels.Checked, StringComparison.OrdinalIgnoreCase) ||
                        string.Equals(feedback.Status, Labels.Solved, StringComparison.OrdinalIgnoreCase))
                    {
                        CheckedRowWriter.WriteRow(ExportExcelInfo, feedback, _mainCategories);
                    }
                    else if (string.Equals(feedback.Status, Labels.Pending, StringComparison.OrdinalIgnoreCase))
                    {
                        PendingRowWriter.WriteRow(ExportExcelInfo, feedback, _mainCategories);
                    }
                    else
                    {
                        NormalRowWriter.WriteRow(ExportExcelInfo, feedback, _mainCategories);
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
            NpoiExtensions.SetColorAtIndex(ExportExcelInfo, closedBackgroundColor, ClosedBackgroundColorIndex);

            var checkedBackgroundColor = Color.FromArgb(242, 242, 242);
            NpoiExtensions.SetColorAtIndex(ExportExcelInfo, checkedBackgroundColor, CheckedBackgroundColorIndex);

            var pendingBackgroundColor = Color.FromArgb(240, 237, 196);
            NpoiExtensions.SetColorAtIndex(
                ExportExcelInfo, pendingBackgroundColor, PendingBackgroundColorIndex);
        }
    }
}