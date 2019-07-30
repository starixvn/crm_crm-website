namespace Fanex.CRM.Common.Models.Npoi
{
    using System.Collections.Generic;
    using Fanex.CRM.Common.Enumerations;
    using Fanex.CRM.Common.Extensions;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;

    public class ExportExcelInfo
    {
        public HSSFWorkbook ExcelFile { get; set; }

        public ISheet Worksheet { get; set; }

        public int StartRow { get; set; }

        public int StartColumn { get; set; }

        public int CurrentRow { get; set; }

        public int CurrentColumn { get; set; }

        public int MaxColumnIndex { get; set; }

        public ICellStyle TextStyle { get; set; }

        public IRow Row { get; set; }

        public void SetColumnWidthByFontWidth(int columnIndex, int width)
        {
            Worksheet.SetColumnWidth(columnIndex, width * NpoiExtensions.FontWidth);
        }

        public void InitColumnWidthByFontWidth(List<int> columnWidths)
        {
            var columnIndex = StartColumn;
            var columnLength = columnWidths.Count;
            for (var i = 0; i < columnLength; i++)
            {
                Worksheet.SetColumnWidth(columnIndex++, columnWidths[i] * NpoiExtensions.FontWidth);
            }
        }

        public void ResetCurrentColumn()
        {
            CurrentColumn = StartColumn;
        }

        public void RenderCell(object cellValue, ICellStyle cellStyle)
        {
            ICell cell = Row.GetCell(CurrentColumn) ?? Row.CreateCell(CurrentColumn);
            NpoiExtensions.SetCellValue(cell, cellValue);
            cellStyle.VerticalAlignment = NpoiExtensions.MappingVerticalAlignmentType(ExcelVerticalAlignment.Center);
            cell.CellStyle = cellStyle;
            CurrentColumn += 1;
        }

        public void CreateSheet(string sheetName)
        {
            Worksheet = ExcelFile.CreateSheet(sheetName);
        }

        public void CreateCurrentRow()
        {
            Row = Worksheet.CreateRow(CurrentRow);
        }

        public CellRangeAddress GetCellRangeBySpanning(int rowSpan, int colSpan)
        {
            return new CellRangeAddress(CurrentRow, CurrentRow + rowSpan, StartColumn, StartColumn + colSpan);
        }

        public void RenderHeaderTitle(string title, ICellStyle headerStyle)
        {
            var exportExcelInfo = this;
            var cellRangeAddress = new CellRangeAddress(CurrentRow, CurrentRow + 1, StartColumn, MaxColumnIndex);
            NpoiExtensions.RenderMergedCells(exportExcelInfo, cellRangeAddress, title, headerStyle);
            CurrentRow += 2;

            NpoiExtensions.RenderEmptyRow(exportExcelInfo, CurrentRow);
            CurrentRow++;
        }

        public CellRangeAddress GetCellRangeBySpanning(int rowSpan)
        {
            return new CellRangeAddress(CurrentRow, CurrentRow + rowSpan, StartColumn, MaxColumnIndex);
        }
    }
}