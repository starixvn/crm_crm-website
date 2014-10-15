// --------------------------------------------------------------------------
// <copyright file="ExportExcelInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Infrastructure.Models.Npoi
{
    using System.Collections.Generic;
    using ExcelUtil.Spreadsheet;
    using Fanex.CRM.Infrastructure.Extension;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;

    /// <summary>
    /// Base Model.
    /// </summary>
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
            Worksheet.SetColumnWidth(columnIndex, width * NpoiExcelExtensions.FontWidth);
        }

        public void InitColumnWidthByFontWidth(List<int> columnWidths)
        {
            var columnIndex = StartColumn;
            var columnLength = columnWidths.Count;
            for (var i = 0; i < columnLength; i++)
            {
                Worksheet.SetColumnWidth(columnIndex++, columnWidths[i] * NpoiExcelExtensions.FontWidth);
            }
        }

        public void ResetCurrentColumn()
        {
            CurrentColumn = StartColumn;
        }

        /// <summary>
        /// Renders the cell.
        /// </summary>
        /// <param name="cellValue">The cell value.</param>
        /// <param name="cellStyle">The cell style.</param>
        public void RenderCell(object cellValue, ICellStyle cellStyle)
        {
            ICell cell = Row.GetCell(CurrentColumn) ?? Row.CreateCell(CurrentColumn);
            NpoiExcelExtensions.SetCellValue(cell, cellValue);
            cellStyle.VerticalAlignment = NpoiExcelExtensions.MappingVerticalAlignmentType(CRMVerticalAlignment.Center);
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
    }
}
