// --------------------------------------------------------------------------
// <copyright file="NpoiExcelExtensions.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Infrastructure.Extension
{
    using System;
    using System.Drawing;
    using System.Web;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Infrastructure.Resources;
    using NPOI.HSSF.UserModel;
    using NPOI.HSSF.Util;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class NpoiExcelExtensions
    {
        public const int FontWidth = 30;
        public const int FontHeight = 20;
        public const int FontSize = 8;
        public const int MaxRowPerSheet = 20000;
        public const string FontName = "Tahoma";

        public static readonly Color BackGroundColor = Color.White;
        public static readonly Color HeaderTitleColor = Color.FromArgb(60, 94, 165);
        public static readonly Color EvenRowColor = Color.FromArgb(228, 242, 253);
        public static readonly Color OddRowColor = Color.White;
        public static readonly Color SubTotalRowColor = Color.FromArgb(194, 217, 236);
        public static readonly Color TotalRowColor = Color.FromArgb(244, 244, 190);
        public static readonly Color GrandTotalRowColor = Color.FromArgb(219, 225, 187);
        public static readonly Color GrandTotalSeparatorColor = Color.FromArgb(255, 102, 0);
        public static readonly Color RevenueLicenseeColor = Color.FromArgb(213, 132, 52);
        public static readonly Color ParentColor = Color.FromArgb(194, 222, 236);
        public static readonly Color ChildColor = Color.FromArgb(194, 226, 236);
        public static readonly Color NoteInformationCellColor = Color.FromArgb(181, 196, 208);
        public static readonly Color BorderColor = Color.Gray;

        public static readonly short HeaderTitleColorIndex = (short)HSSFColor.PINK.index;
        public static readonly short BorderColorIndex = (short)HSSFColor.GREY_80_PERCENT.index;
        public static readonly short BackgroundColorIndex = (short)HSSFColor.WHITE.index;
        public static readonly short EvenRowColorIndex = (short)HSSFColor.CORAL.index;
        public static readonly short SubTotalRowColorIndex = (short)HSSFColor.DARK_TEAL.index;
        public static readonly short TotalRowColorIndex = (short)HSSFColor.DARK_RED.index;
        public static readonly short GrandTotalRowColorIndex = (short)HSSFColor.DARK_YELLOW.index;
        public static readonly short GrandTotalSeparatorColorIndex = (short)HSSFColor.CORNFLOWER_BLUE.index;

        private const string IntNumberFormat = "#,##0";
        private const string TwoDecimalDigitNumber = "##,##0.00";
        private const string NegativeNumberFormat = "[>=0]#,##0{0}_);[RED][<0]-#,##0{1}";
        private const string NormalNumberFormat = "#,##0{0}_);-#,##0{1}";
        private const string PositiveNumberFormat = "[RED][>0]#,##0{0}_);[<0]-#,##0{1};#,##0{2}";

        public enum Alignment
        {
            /// <summary>
            /// Align left.
            /// </summary>
            Left,

            /// <summary>
            /// Align center.
            /// </summary>
            Center,

            /// <summary>
            /// Align Right.
            /// </summary>
            Right,
        }

        public static void SetHeaderColorIndex(ref ExportExcelInfoNPOI exportExcelInfo)
        {
            NpoiExcelExtensions.SetColorAtIndex(
                ref exportExcelInfo,
                HeaderTitleColor,
                HeaderTitleColorIndex);
        }

        public static void SetSubTotalColorIndex(ref ExportExcelInfoNPOI exportExcelInfo)
        {
            NpoiExcelExtensions.SetColorAtIndex(
                ref exportExcelInfo,
                SubTotalRowColor,
                SubTotalRowColorIndex);
        }

        public static void SetTotalColorIndex(ref ExportExcelInfoNPOI exportExcelInfo)
        {
            NpoiExcelExtensions.SetColorAtIndex(
                ref exportExcelInfo,
                TotalRowColor,
                TotalRowColorIndex);
        }

        public static void SetGrandTotalColorIndex(ref ExportExcelInfoNPOI exportExcelInfo)
        {
            NpoiExcelExtensions.SetColorAtIndex(
                ref exportExcelInfo,
                GrandTotalRowColor,
                GrandTotalRowColorIndex);
        }

        public static void SetGrandTotalSeparatorColorIndex(ref ExportExcelInfoNPOI exportExcelInfo)
        {
            NpoiExcelExtensions.SetColorAtIndex(
                ref exportExcelInfo,
                GrandTotalSeparatorColor,
                GrandTotalSeparatorColorIndex);
        }

        /// <summary>
        /// Style for header.
        /// </summary>
        /// <param name="excelFile">The excel file.</param>
        /// <param name="excelStyleParam">The excel style parameter.</param>
        /// <returns>
        /// The cell style.
        /// </returns>
        public static ICellStyle HeaderStyle(
            HSSFWorkbook excelFile,
            ExcelStyleParamNPOI excelStyleParam)
        {
            ICellStyle cellStyle = excelFile.CreateCellStyle();
            IFont font = excelFile.CreateFont();
            font.Boldweight = (short)FontBoldWeight.BOLD;
            font.Color = HSSFColor.WHITE.index;
            font.FontHeightInPoints = FontSize;
            font.FontName = FontName;

            cellStyle.FillForegroundColor = (short)HeaderTitleColorIndex;
            cellStyle.FillPattern = FillPatternType.SOLID_FOREGROUND;
            cellStyle.Alignment = excelStyleParam.Alignment ?? HorizontalAlignment.CENTER;
            cellStyle.VerticalAlignment = VerticalAlignment.CENTER;
            cellStyle.SetFont(font);
            cellStyle = SetBorderToCell(cellStyle);
            return cellStyle;
        }

        /// <summary>
        /// Style for text.
        /// </summary>
        /// <param name="excelFile">The excel file.</param>
        /// <param name="cellColor">The color of cell.</param>
        /// <param name="isBold">Check bold.</param>
        /// <param name="align">Align text(value is right, left or center).</param>
        /// <param name="fontColor">The color of font.</param>
        /// <returns>The cell style.</returns>
        public static ICellStyle TextStyle(
            HSSFWorkbook excelFile,
            short cellColor,
            bool isBold,
            Alignment align,
            short fontColor = 8)
        {
            ICellStyle cellStyle = excelFile.CreateCellStyle();
            IFont font = excelFile.CreateFont();
            font.FontHeightInPoints = FontSize;
            font.FontName = FontName;
            font.Color = fontColor;

            if (isBold)
            {
                font.Boldweight = (short)FontBoldWeight.BOLD;
            }

            cellStyle.FillForegroundColor = cellColor;
            cellStyle.FillPattern = FillPatternType.SOLID_FOREGROUND;
            switch (align)
            {
                case Alignment.Right:
                    cellStyle.Alignment = HorizontalAlignment.RIGHT;
                    break;
                case Alignment.Left:
                    cellStyle.Alignment = HorizontalAlignment.LEFT;
                    break;
                case Alignment.Center:
                    cellStyle.Alignment = HorizontalAlignment.CENTER;
                    break;
            }

            cellStyle.VerticalAlignment = VerticalAlignment.CENTER;
            cellStyle.SetFont(font);
            cellStyle = SetBorderToCell(cellStyle);
            return cellStyle;
        }

        /// <summary>
        /// Creates the decimal number style.
        /// </summary>
        /// <param name="excelFile">The excel file.</param>
        /// <param name="backgroundColorIndex">Index of the background color.</param>
        /// <param name="textAlign">The text align.</param>
        /// <param name="isBold">If set to <c>true</c> [is bold].</param>
        /// <param name="isNegative">If set to <c>true</c> [is negative].</param>
        /// <param name="isPositive">If set to <c>true</c> [is positive].</param>
        /// <returns>Decimal number style.</returns>
        public static ICellStyle CreateDecimalNumberStyle(
            HSSFWorkbook excelFile,
            short backgroundColorIndex,
            HorizontalAlignment textAlign,
            bool isBold,
            bool isNegative,
            bool isPositive)
        {
            var decimalNumberStyle = excelFile.CreateCellStyle();
            IDataFormat dataFormat = excelFile.CreateDataFormat();
            IFont font = excelFile.CreateFont();
            font.FontHeightInPoints = NpoiExcelExtensions.FontSize;
            font.FontName = NpoiExcelExtensions.FontName;

            if (isBold)
            {
                font.Boldweight = (short)FontBoldWeight.BOLD;
            }

            if (isNegative)
            {
                decimalNumberStyle.DataFormat = dataFormat.GetFormat(NegativeNumberFormat);
            }
            else if (isPositive)
            {
                decimalNumberStyle.DataFormat = dataFormat.GetFormat(PositiveNumberFormat);
            }
            else
            {
                decimalNumberStyle.DataFormat = dataFormat.GetFormat(NormalNumberFormat);
            }

            if (backgroundColorIndex != HSSFColor.WHITE.index)
            {
                decimalNumberStyle.FillForegroundColor = backgroundColorIndex;
            }
            else
            {
                decimalNumberStyle.FillForegroundColor = HSSFColor.WHITE.index;
            }

            decimalNumberStyle.FillPattern = FillPatternType.SOLID_FOREGROUND;
            decimalNumberStyle.Alignment = textAlign;
            decimalNumberStyle.VerticalAlignment = VerticalAlignment.CENTER;
            decimalNumberStyle.SetFont(font);
            SetBorderToCell(decimalNumberStyle);
            return decimalNumberStyle;
        }

        /// <summary>
        /// Style for number value.
        /// </summary>
        /// <param name="excelFile">The excel file.</param>
        /// <param name="cellColor">The cell color.</param>
        /// <param name="isBold">Check bold.</param>
        /// <param name="alignment">The alignment.</param>
        /// <param name="fontColor">The font color.</param>
        /// <returns>
        /// The cell style.
        /// </returns>
        public static ICellStyle IntNumberStyle(
            HSSFWorkbook excelFile,
            short cellColor,
            bool isBold,
            HorizontalAlignment alignment,
            short fontColor = 8)
        {
            ICellStyle cellStyle = excelFile.CreateCellStyle();
            IFont font = excelFile.CreateFont();
            IDataFormat dataFormat = excelFile.CreateDataFormat();
            font.FontHeightInPoints = FontSize;
            font.FontName = FontName;
            font.Color = fontColor;

            if (isBold)
            {
                font.Boldweight = (short)FontBoldWeight.BOLD;
            }

            cellStyle.FillForegroundColor = cellColor;
            cellStyle.FillPattern = FillPatternType.SOLID_FOREGROUND;
            cellStyle.Alignment = alignment;
            cellStyle.VerticalAlignment = VerticalAlignment.CENTER;
            cellStyle.DataFormat = dataFormat.GetFormat(IntNumberFormat);
            cellStyle.SetFont(font);
            cellStyle = SetBorderToCell(cellStyle);
            return cellStyle;
        }

        /// <summary>
        /// Render cell.
        /// </summary>
        /// <param name="exportExcelInfo">The export excel information.</param>
        /// <param name="cellRangeAddress">The cell range address.</param>
        /// <param name="cellValue">The label of cell.</param>
        /// <param name="cellStyle">The style of cell.</param>
        public static void RenderMergedCells(
            ref ExportExcelInfoNPOI exportExcelInfo,
            CellRangeAddress cellRangeAddress,
            object cellValue,
            ICellStyle cellStyle)
        {
            IRow row;
            ICell cell;

            exportExcelInfo.Worksheet.AddMergedRegion(cellRangeAddress);
            for (var i = cellRangeAddress.FirstRow; i <= cellRangeAddress.LastRow; i++)
            {
                row = exportExcelInfo.Worksheet.GetRow(i) ?? exportExcelInfo.Worksheet.CreateRow(i);
                for (var j = cellRangeAddress.FirstColumn; j <= cellRangeAddress.LastColumn; j++)
                {
                    cell = row.GetCell(j) ?? row.CreateCell(j);
                    if (cellValue is string)
                    {
                        cell.SetCellValue((string)cellValue);
                    }
                    else if (cellValue is int)
                    {
                        cell.SetCellValue((int)cellValue);
                    }
                    else if (cellValue is long)
                    {
                        cell.SetCellValue((long)cellValue);
                    }
                    else if (cellValue is double)
                    {
                        cell.SetCellValue((double)cellValue);
                    }
                    else if (cellValue is decimal)
                    {
                        cell.SetCellValue(Convert.ToDouble(cellValue));
                    }
                    else if (cellValue is bool)
                    {
                        cell.SetCellValue((bool)cellValue);
                    }
                    else if (cellValue is System.DateTime)
                    {
                        cell.SetCellValue((System.DateTime)cellValue);
                    }

                    cell.CellStyle = cellStyle;
                }
            }
        }

        /// <summary>
        /// Renders the empty row.
        /// </summary>
        /// <param name="exportExcelInfo">The export excel information.</param>
        /// <param name="rowIndex">Index of the row.</param>
        public static void RenderEmptyRow(ref ExportExcelInfoNPOI exportExcelInfo, int rowIndex)
        {
            CellRangeAddress cellRangeAddress = new CellRangeAddress(
                rowIndex,
                rowIndex,
                exportExcelInfo.StartColumn,
                exportExcelInfo.MaxColumnIndex);
            var textStyle = NpoiExcelExtensions.TextStyle(
                exportExcelInfo.ExcelFile,
                BackgroundColorIndex,
                false,
                Alignment.Center);
            RenderMergedCells(ref exportExcelInfo, cellRangeAddress, string.Empty, textStyle);
        }

        /// <summary>
        /// Renders the no information message.
        /// </summary>
        /// <param name="exportExcelInfo">The export excel information.</param>
        /// <param name="currentRow">The current row.</param>
        public static void RenderNoInfoMessage(ref ExportExcelInfoNPOI exportExcelInfo, int currentRow)
        {
            CellRangeAddress cellRangeAddress = new CellRangeAddress(
                currentRow,
                currentRow,
                exportExcelInfo.StartColumn,
                exportExcelInfo.MaxColumnIndex);
            var textStyle = NpoiExcelExtensions.TextStyle(
                exportExcelInfo.ExcelFile,
                BackgroundColorIndex,
                true,
                Alignment.Center);

            RenderMergedCells(
                ref exportExcelInfo,
                cellRangeAddress,
                Labels.ResultMessage_NoInfo,
                textStyle);
        }

        /// <summary>
        /// Renders the cell.
        /// </summary>
        /// <param name="row">The row of report.</param>
        /// <param name="currentColumn">The current column.</param>
        /// <param name="cellValue">The cell value.</param>
        /// <param name="cellStyle">The cell style.</param>
        public static void RenderCell(
            ref IRow row,
            ref int currentColumn,
            object cellValue,
            ICellStyle cellStyle)
        {
            ICell cell = row.GetCell(currentColumn) ?? row.CreateCell(currentColumn);
            if (cellValue is string)
            {
                cell.SetCellValue((string)cellValue);
            }
            else if (cellValue is decimal)
            {
                cell.SetCellValue(Convert.ToDouble(cellValue));
            }
            else if (cellValue is int)
            {
                cell.SetCellValue(Convert.ToInt32(cellValue));
            }

            cell.CellStyle = cellStyle;
            currentColumn += 1;
        }

        public static void ResponseStreamExcel(this IWorkbook workbook, string fileName)
        {
            var response = HttpContext.Current.Response;
            response.Clear();
            response.ContentType = "application/vnd.ms-excel";
            response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.xls", fileName));
            workbook.Write(response.OutputStream);
        }

        /// <summary>
        /// Set border to cell.
        /// </summary>
        /// <param name="cellStyle">The cell style.</param>
        /// <returns>The style of cell.</returns>
        public static ICellStyle SetBorderToCell(ICellStyle cellStyle)
        {
            cellStyle.BorderBottom = BorderStyle.THIN;
            cellStyle.BorderLeft = BorderStyle.THIN;
            cellStyle.BorderRight = BorderStyle.THIN;
            cellStyle.BorderTop = BorderStyle.THIN;

            cellStyle.BottomBorderColor = (short)BorderColorIndex;
            cellStyle.LeftBorderColor = (short)BorderColorIndex;
            cellStyle.RightBorderColor = (short)BorderColorIndex;
            cellStyle.TopBorderColor = (short)BorderColorIndex;

            return cellStyle;
        }

        /// <summary>
        /// Set border to first grand total cell.
        /// </summary>
        /// <param name="cellStyle">The cell style.</param>
        /// <returns>
        /// The style of cell.
        /// </returns>
        public static ICellStyle SetBorderToFirstGrandTotalCell(ICellStyle cellStyle)
        {
            cellStyle.BorderBottom = BorderStyle.THIN;
            cellStyle.BorderLeft = BorderStyle.THIN;
            cellStyle.BorderRight = BorderStyle.THIN;
            cellStyle.BorderTop = BorderStyle.THIN;

            cellStyle.BottomBorderColor = (short)BorderColorIndex;
            cellStyle.LeftBorderColor = (short)BorderColorIndex;
            cellStyle.RightBorderColor = (short)BorderColorIndex;
            cellStyle.TopBorderColor = (short)GrandTotalSeparatorColorIndex;

            return cellStyle;
        }

        public static void SetColorAtIndex(
            ref ExportExcelInfoNPOI exportExcelInfo,
            Color color,
            short colorIndex)
        {
            HSSFPalette palette = exportExcelInfo.ExcelFile.GetCustomPalette();
            palette.SetColorAtIndex(
                colorIndex,
                color.R,
                color.G,
                color.B);
        }

        public static string StandardizeExcelReportName(string excelReportName)
        {
            return excelReportName.Replace(" ", "_").Replace("/", "_");
        }
    }
}
