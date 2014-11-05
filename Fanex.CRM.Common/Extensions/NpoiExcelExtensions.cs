// --------------------------------------------------------------------------
// <copyright file="NpoiExcelExtensions.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Common.Extensions
{
    using System;
    using System.Drawing;
    using System.Web;
    using Fanex.CRM.Common.Models.Npoi;
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
        public static readonly Color TotalRowColor = Color.FromArgb(227, 227, 227);
        public static readonly Color GrandTotalRowColor = Color.FromArgb(60, 94, 165);
        public static readonly Color GrandTotalSeparatorColor = Color.FromArgb(255, 102, 0);
        public static readonly Color RevenueLicenseeColor = Color.FromArgb(213, 132, 52);
        public static readonly Color NoteInformationCellColor = Color.FromArgb(181, 196, 208);
        public static readonly Color BorderColor = Color.Gray;
        public static readonly Color GroupBackgroundColor = Color.FromArgb(215, 215, 215);

        public static readonly short HeaderTitleColorIndex = (short)HSSFColor.Pink.Index;
        public static readonly short BorderColorIndex = (short)HSSFColor.Grey80Percent.Index;
        public static readonly short BackgroundColorIndex = (short)HSSFColor.White.Index;
        public static readonly short EvenRowColorIndex = (short)HSSFColor.Coral.Index;
        public static readonly short SubTotalRowColorIndex = (short)HSSFColor.DarkTeal.Index;
        public static readonly short TotalRowColorIndex = (short)HSSFColor.DarkRed.Index;
        public static readonly short GrandTotalRowColorIndex = (short)HSSFColor.DarkYellow.Index;
        public static readonly short GrandTotalSeparatorColorIndex = (short)HSSFColor.CornflowerBlue.Index;
        public static readonly short GroupColorIndex = (short)HSSFColor.Blue.Index;
        public static readonly short GroupBackgroundColorIndex = (short)HSSFColor.DarkGreen.Index;
        public static readonly short GrandTotalTextColorIndex = (short)HSSFColor.White.Index;
        public static readonly short PendingStatusColorIndex = (short)HSSFColor.LightYellow.Index;
        public static readonly short CloseStatusColorIndex = (short)HSSFColor.Grey25Percent.Index;

        private const string IntNumberFormat = "#,##0";
        private const string TwoDecimalDigitNumber = "##,##0.00";
        private const string NegativeNumberFormat = "[>=0]#,##0{0}_);[RED][<0]-#,##0{1}";
        private const string NormalNumberFormat = "#,##0{0}_);-#,##0{1}";
        private const string PositiveNumberFormat = "[RED][>0]#,##0{0}_);[<0]-#,##0{1};#,##0{2}";

        public static void SetHeaderColorIndex(ref ExportExcelInfo exportExcelInfo)
        {
            NpoiExcelExtensions.SetColorAtIndex(
                ref exportExcelInfo,
                HeaderTitleColor,
                HeaderTitleColorIndex);
        }

        public static void SetSubTotalColorIndex(ref ExportExcelInfo exportExcelInfo)
        {
            NpoiExcelExtensions.SetColorAtIndex(
                ref exportExcelInfo,
                SubTotalRowColor,
                SubTotalRowColorIndex);
        }

        public static void SetTotalColorIndex(ref ExportExcelInfo exportExcelInfo)
        {
            NpoiExcelExtensions.SetColorAtIndex(
                ref exportExcelInfo,
                TotalRowColor,
                TotalRowColorIndex);
        }

        public static void SetGrandTotalColorIndex(ref ExportExcelInfo exportExcelInfo)
        {
            NpoiExcelExtensions.SetColorAtIndex(
                ref exportExcelInfo,
                GrandTotalRowColor,
                GrandTotalRowColorIndex);
        }

        public static void SetGrandTotalSeparatorColorIndex(ref ExportExcelInfo exportExcelInfo)
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
            ExcelStyleParam excelStyleParam)
        {
            ICellStyle cellStyle = excelFile.CreateCellStyle();
            IFont font = excelFile.CreateFont();
            font.Boldweight = (short)FontBoldWeight.Bold;
            font.Color = HSSFColor.White.Index;
            font.FontHeightInPoints = FontSize;
            font.FontName = FontName;

            cellStyle.FillForegroundColor = (short)HeaderTitleColorIndex;
            cellStyle.FillPattern = FillPattern.SolidForeground;
            cellStyle.Alignment = excelStyleParam.Alignment != null ?
                MappingHorizontalAlignmentType(excelStyleParam.Alignment) :
                MappingHorizontalAlignmentType(CRMHorizontalAlignment.Center);
            cellStyle.VerticalAlignment = MappingVerticalAlignmentType(CRMVerticalAlignment.Center);
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
            CRMHorizontalAlignment align,
            short fontColor = 8)
        {
            ICellStyle cellStyle = excelFile.CreateCellStyle();
            IFont font = excelFile.CreateFont();
            font.FontHeightInPoints = FontSize;
            font.FontName = FontName;
            font.Color = fontColor;

            if (isBold)
            {
                font.Boldweight = (short)FontBoldWeight.Bold;
            }

            cellStyle.FillForegroundColor = cellColor;
            cellStyle.FillPattern = FillPattern.SolidForeground;
            cellStyle.Alignment = MappingHorizontalAlignmentType(align);
            cellStyle.VerticalAlignment = MappingVerticalAlignmentType(CRMVerticalAlignment.Center);
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
            CRMHorizontalAlignment textAlign,
            bool isBold,
            bool isNegative,
            bool isPositive)
        {
            var decimalNumberStyle = excelFile.CreateCellStyle();
            IFont font = excelFile.CreateFont();
            font.FontHeightInPoints = NpoiExcelExtensions.FontSize;
            font.FontName = NpoiExcelExtensions.FontName;

            if (isBold)
            {
                font.Boldweight = (short)FontBoldWeight.Bold;
            }

            decimalNumberStyle.DataFormat = GetNumerFormat(excelFile, isNegative, isPositive);
            decimalNumberStyle.FillForegroundColor = backgroundColorIndex;
            decimalNumberStyle.FillPattern = FillPattern.SolidForeground;
            decimalNumberStyle.Alignment = MappingHorizontalAlignmentType(textAlign);
            decimalNumberStyle.VerticalAlignment = MappingVerticalAlignmentType(CRMVerticalAlignment.Center);
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
            CRMHorizontalAlignment alignment,
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
                font.Boldweight = (short)FontBoldWeight.Bold;
            }

            cellStyle.FillForegroundColor = cellColor;
            cellStyle.FillPattern = FillPattern.SolidForeground;
            cellStyle.Alignment = MappingHorizontalAlignmentType(alignment);
            cellStyle.VerticalAlignment = MappingVerticalAlignmentType(CRMVerticalAlignment.Center);
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
            ExportExcelInfo exportExcelInfo,
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
                    if (i == cellRangeAddress.FirstRow ||
                        i == cellRangeAddress.LastRow ||
                        j == cellRangeAddress.FirstColumn ||
                        j == cellRangeAddress.LastColumn)
                    {
                        cell.CellStyle = cellStyle;
                    }

                    if (i == cellRangeAddress.FirstRow && j == cellRangeAddress.FirstColumn)
                    {
                        SetCellValue(ref cell, cellValue);
                    }
                }
            }
        }

        /// <summary>
        /// Renders the no information message.
        /// </summary>
        /// <param name="exportExcelInfo">The export excel information.</param>
        /// <param name="currentRow">The current row.</param>
        public static void RenderNoInfoMessage(ref ExportExcelInfo exportExcelInfo, int currentRow)
        {
            CellRangeAddress cellRangeAddress = new CellRangeAddress(
                currentRow,
                currentRow,
                exportExcelInfo.StartColumn,
                exportExcelInfo.MaxColumnIndex);
            var textStyle = NpoiExcelExtensions.TextStyle(exportExcelInfo.ExcelFile, BackgroundColorIndex, true, CRMHorizontalAlignment.Center);

            RenderMergedCells(exportExcelInfo, cellRangeAddress, Labels.ResultMessage_NoInfo, textStyle);
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
            SetCellValue(ref cell, cellValue);
            cellStyle.VerticalAlignment = MappingVerticalAlignmentType(CRMVerticalAlignment.Center);
            cell.CellStyle = cellStyle;
            currentColumn += 1;
        }

        /// <summary>
        /// Sets the cell value.
        /// </summary>
        /// <param name="cell">The target cell.</param>
        /// <param name="cellValue">The cell value.</param>
        public static void SetCellValue(
            ref ICell cell,
            object cellValue)
        {
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
            cellStyle.BorderBottom = BorderStyle.Thin;
            cellStyle.BorderLeft = BorderStyle.Thin;
            cellStyle.BorderRight = BorderStyle.Thin;
            cellStyle.BorderTop = BorderStyle.Thin;

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
            cellStyle.BorderBottom = BorderStyle.Thin;
            cellStyle.BorderLeft = BorderStyle.Thin;
            cellStyle.BorderRight = BorderStyle.Thin;
            cellStyle.BorderTop = BorderStyle.Thin;

            cellStyle.BottomBorderColor = (short)BorderColorIndex;
            cellStyle.LeftBorderColor = (short)BorderColorIndex;
            cellStyle.RightBorderColor = (short)BorderColorIndex;
            cellStyle.TopBorderColor = (short)GrandTotalSeparatorColorIndex;

            return cellStyle;
        }

        public static void SetColorAtIndex(ref ExportExcelInfo exportExcelInfo, Color color, short colorIndex)
        {
            HSSFPalette palette = exportExcelInfo.ExcelFile.GetCustomPalette();
            palette.SetColorAtIndex(colorIndex, color.R, color.G, color.B);
        }

        public static string StandardizeExcelReportName(string excelReportName)
        {
            return excelReportName.Replace(" ", "_").Replace("/", "_");
        }

        /// <summary>
        /// Mappings the type of the horizontal alignment.
        /// </summary>
        /// <param name="alignment">The alignment.</param>
        /// <returns>The NPOI horizontal alignment.</returns>
        public static HorizontalAlignment MappingHorizontalAlignmentType(CRMHorizontalAlignment? alignment)
        {
            if (alignment == null)
            {
                return HorizontalAlignment.Left;
            }

            switch (alignment)
            {
                case CRMHorizontalAlignment.Right:
                    return HorizontalAlignment.Right;
                case CRMHorizontalAlignment.Center:
                    return HorizontalAlignment.Center;
                case CRMHorizontalAlignment.Left:
                    return HorizontalAlignment.Left;
                default:
                    return HorizontalAlignment.Left;
            }
        }

        /// <summary>
        /// Mappings the type of the vertical alignment.
        /// </summary>
        /// <param name="alignment">The alignment.</param>
        /// <returns>The NPOI vertical alignment.</returns>
        public static VerticalAlignment MappingVerticalAlignmentType(CRMVerticalAlignment alignment)
        {
            // Vertical Alignment NPOI 2.0.1+ issue.
            switch (alignment)
            {
                case CRMVerticalAlignment.Top:
                    return VerticalAlignment.Bottom;
                case CRMVerticalAlignment.Center:
                    return VerticalAlignment.Top;
                case CRMVerticalAlignment.Bottom:
                    return VerticalAlignment.Bottom;
                case CRMVerticalAlignment.Justify:
                    return VerticalAlignment.Justify;
                case CRMVerticalAlignment.Distributed:
                    return VerticalAlignment.Distributed;
                default:
                    return VerticalAlignment.Top;
            }
        }

        /// <summary>
        /// Gets the number format.
        /// </summary>
        /// <param name="excelFile">The excel file.</param>
        /// <param name="isNegative">If set to <c>true</c> [is negative].</param>
        /// <param name="isPositive">If set to <c>true</c> [is positive].</param>
        /// <returns>The number format.</returns>
        public static short GetNumerFormat(HSSFWorkbook excelFile, bool isNegative, bool isPositive)
        {
            IDataFormat dataFormat = excelFile.CreateDataFormat();
            if (isNegative)
            {
                return dataFormat.GetFormat(NegativeNumberFormat);
            }

            if (isPositive)
            {
                return dataFormat.GetFormat(PositiveNumberFormat);
            }

            return dataFormat.GetFormat(NormalNumberFormat);
        }

        public static void SetHeaderColorIndex(ExportExcelInfo exportExcelInfo)
        {
            NpoiExcelExtensions.SetColorAtIndex(ref exportExcelInfo, HeaderTitleColor, HeaderTitleColorIndex);
        }

        public static void SetSubTotalColorIndex(ExportExcelInfo exportExcelInfo)
        {
            NpoiExcelExtensions.SetColorAtIndex(ref exportExcelInfo, SubTotalRowColor, SubTotalRowColorIndex);
        }

        public static void SetGroupBackgroundColorIndex(ExportExcelInfo exportExcelInfo)
        {
            NpoiExcelExtensions.SetColorAtIndex(ref exportExcelInfo, GroupBackgroundColor, GroupBackgroundColorIndex);
        }

        public static void SetGrandTotalSeparatorColorIndex(ExportExcelInfo exportExcelInfo)
        {
            NpoiExcelExtensions.SetColorAtIndex(ref exportExcelInfo, GrandTotalSeparatorColor, GrandTotalSeparatorColorIndex);
        }

        /// <summary>
        /// Sets the cell value.
        /// </summary>
        /// <param name="cell">The target cell.</param>
        /// <param name="cellValue">The cell value.</param>
        public static void SetCellValue(ICell cell, object cellValue)
        {
            if (cellValue.GetType() == typeof(string))
            {
                cell.SetCellValue((string)cellValue);
            }
            else if (cellValue.GetType() == typeof(int))
            {
                cell.SetCellValue((int)cellValue);
            }
            else if (cellValue.GetType() == typeof(long))
            {
                cell.SetCellValue((long)cellValue);
            }
            else if (cellValue.GetType() == typeof(double))
            {
                cell.SetCellValue((double)cellValue);
            }
            else if (cellValue.GetType() == typeof(decimal))
            {
                cell.SetCellValue(Convert.ToDouble(cellValue));
            }
            else if (cellValue.GetType() == typeof(bool))
            {
                cell.SetCellValue((bool)cellValue);
            }
            else if (cellValue.GetType() == typeof(DateTime))
            {
                cell.SetCellValue((System.DateTime)cellValue);
            }
        }

        /// <summary>
        /// Renders the empty row.
        /// </summary>
        /// <param name="exportExcelInfo">The export excel information.</param>
        /// <param name="rowIndex">Index of the row.</param>
        public static void RenderEmptyRow(ExportExcelInfo exportExcelInfo, int rowIndex)
        {
            CellRangeAddress cellRangeAddress = new CellRangeAddress(
                rowIndex,
                rowIndex,
                exportExcelInfo.StartColumn,
                exportExcelInfo.MaxColumnIndex);
            var textStyle = NpoiExcelExtensions.TextStyle(exportExcelInfo.ExcelFile, BackgroundColorIndex, false, CRMHorizontalAlignment.Center);
            RenderMergedCells(exportExcelInfo, cellRangeAddress, string.Empty, textStyle);
        }

        /// <summary>
        /// Renders the no information message.
        /// </summary>
        /// <param name="exportExcelInfo">The export excel information.</param>
        /// <param name="currentRow">The current row.</param>
        public static void RenderNoInfoMessage(ExportExcelInfo exportExcelInfo, int currentRow)
        {
            CellRangeAddress cellRangeAddress = new CellRangeAddress(currentRow, currentRow, exportExcelInfo.StartColumn, exportExcelInfo.MaxColumnIndex);
            var textStyle = NpoiExcelExtensions.TextStyle(exportExcelInfo.ExcelFile, BackgroundColorIndex, true, CRMHorizontalAlignment.Center);
            RenderMergedCells(exportExcelInfo, cellRangeAddress, Labels.ResultMessage_NoInfo, textStyle);
        }

        /// <summary>
        /// Style for number value.
        /// </summary>
        /// <param name="excelFile">The excel file.</param>
        /// <returns>
        /// The cell style.
        /// </returns>
        public static ICellStyle IntNumberStyleGrandTotal(HSSFWorkbook excelFile)
        {
            return IntNumberStyle(excelFile, GrandTotalRowColorIndex, true, CRMHorizontalAlignment.Right, GrandTotalTextColorIndex);
        }

        /// <summary>
        /// Style for text.
        /// </summary>
        /// <param name="excelFile">The excel file.</param>
        /// <returns>
        /// The cell style.
        /// </returns>
        public static ICellStyle TextStyleGrandTotal(HSSFWorkbook excelFile)
        {
            return TextStyle(excelFile, GrandTotalRowColorIndex, true, CRMHorizontalAlignment.Left, GrandTotalTextColorIndex);
        }
    }
}
