namespace Fanex.CRM.Common.Extensions
{
    using System;
    using System.Drawing;
    using System.Web;
    using Fanex.Common;
    using Fanex.CRM.Common.Models.Npoi;
    using Fanex.CRM.Common.Resources;
    using Fanex.CRM.Infrastructure.Models;
    using NPOI.HSSF.UserModel;
    using NPOI.HSSF.Util;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;

    public static class NpoiExtensions
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
        public static readonly Color TotalColumnBackgroundColor = Color.FromArgb(240, 237, 196);

        public static readonly short HeaderTitleColorIndex = HSSFColor.Pink.Index;
        public static readonly short BorderColorIndex = HSSFColor.Grey80Percent.Index;
        public static readonly short BackgroundColorIndex = HSSFColor.White.Index;
        public static readonly short EvenRowColorIndex = HSSFColor.Coral.Index;
        public static readonly short SubTotalRowColorIndex = HSSFColor.DarkTeal.Index;
        public static readonly short TotalRowColorIndex = HSSFColor.DarkRed.Index;
        public static readonly short GrandTotalRowColorIndex = HSSFColor.DarkYellow.Index;
        public static readonly short GrandTotalSeparatorColorIndex = HSSFColor.CornflowerBlue.Index;
        public static readonly short GroupColorIndex = HSSFColor.Blue.Index;
        public static readonly short GroupBackgroundColorIndex = HSSFColor.DarkGreen.Index;
        public static readonly short GrandTotalTextColorIndex = HSSFColor.White.Index;
        public static readonly short PendingStatusColorIndex = HSSFColor.LightYellow.Index;
        public static readonly short CloseStatusColorIndex = HSSFColor.Grey25Percent.Index;
        public static readonly short TotalColumnBackgroundIndex = HSSFColor.Yellow.Index;

        private const string IntNumberFormat = "#,##0";
        private const string NegativeNumberFormat = "[>=0]#,##0{0}_);[RED][<0]-#,##0{1}";
        private const string NormalNumberFormat = "#,##0{0}_);-#,##0{1}";
        private const string PositiveNumberFormat = "[RED][>0]#,##0{0}_);[<0]-#,##0{1};#,##0{2}";

        public static void SetHeaderColorIndex(ExportExcelInfo exportExcelInfo)
        {
            NpoiExtensions.SetColorAtIndex(exportExcelInfo, HeaderTitleColor, HeaderTitleColorIndex);
        }

        public static void SetSubTotalColorIndex(ExportExcelInfo exportExcelInfo)
        {
            NpoiExtensions.SetColorAtIndex(exportExcelInfo, SubTotalRowColor, SubTotalRowColorIndex);
        }

        public static void SetTotalColorIndex(ExportExcelInfo exportExcelInfo)
        {
            NpoiExtensions.SetColorAtIndex(exportExcelInfo, TotalRowColor, TotalRowColorIndex);
        }

        public static void SetGrandTotalColorIndex(ExportExcelInfo exportExcelInfo)
        {
            NpoiExtensions.SetColorAtIndex(exportExcelInfo, GrandTotalRowColor, GrandTotalRowColorIndex);
        }

        public static void SetGrandTotalSeparatorColorIndex(ExportExcelInfo exportExcelInfo)
        {
            NpoiExtensions.SetColorAtIndex(exportExcelInfo, GrandTotalSeparatorColor, GrandTotalSeparatorColorIndex);
        }

        public static void SetTotalColumnBackgroundColorIndex(ExportExcelInfo exportExcelInfo)
        {
            NpoiExtensions.SetColorAtIndex(exportExcelInfo, TotalColumnBackgroundColor, TotalColumnBackgroundIndex);
        }

        public static ICellStyle GetHeaderStyle(
            HSSFWorkbook excelFile,
            ExcelStyleParam excelStyleParam)
        {
            ICellStyle cellStyle = excelFile.CreateCellStyle();
            IFont font = excelFile.CreateFont();
            font.Boldweight = (short)FontBoldWeight.Bold;
            font.Color = HSSFColor.White.Index;
            font.FontHeightInPoints = FontSize;
            font.FontName = FontName;

            cellStyle.FillForegroundColor = HeaderTitleColorIndex;
            cellStyle.FillPattern = FillPattern.SolidForeground;
            cellStyle.Alignment = excelStyleParam.Alignment != null ?
                MappingHorizontalAlignmentType(excelStyleParam.Alignment) :
                MappingHorizontalAlignmentType(CRMHorizontalAlignment.Center);
            cellStyle.VerticalAlignment = MappingVerticalAlignmentType(CRMVerticalAlignment.Center);
            cellStyle.SetFont(font);
            cellStyle = SetBorderToCell(cellStyle);
            return cellStyle;
        }

        public static ICellStyle GetTextStyle(
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
            font.FontHeightInPoints = NpoiExtensions.FontSize;
            font.FontName = NpoiExtensions.FontName;

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

        public static ICellStyle GetIntegerNumberStyle(
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
                        SetCellValue(cell, cellValue);
                    }
                }
            }
        }

        public static void RenderNoInfoMessage(ExportExcelInfo exportExcelInfo, int currentRow)
        {
            CellRangeAddress cellRangeAddress = new CellRangeAddress(
                currentRow,
                currentRow,
                exportExcelInfo.StartColumn,
                exportExcelInfo.MaxColumnIndex);
            var textStyle = NpoiExtensions.GetTextStyle(exportExcelInfo.ExcelFile, BackgroundColorIndex, true, CRMHorizontalAlignment.Center);

            RenderMergedCells(exportExcelInfo, cellRangeAddress, Labels.ResultMessage_NoInfo, textStyle);
        }

        public static void RenderCell(IRow row, int currentColumn, object cellValue, ICellStyle cellStyle)
        {
            ICell cell = row.GetCell(currentColumn) ?? row.CreateCell(currentColumn);
            SetCellValue(cell, cellValue);
            cellStyle.VerticalAlignment = MappingVerticalAlignmentType(CRMVerticalAlignment.Center);
            cell.CellStyle = cellStyle;
            currentColumn += 1;
        }

        public static void SetCellValue(ICell cell, object cellValue)
        {
            if (cellValue is string)
            {
                cell.SetCellValue(Converter.ToString2(cellValue));
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

        public static ICellStyle SetBorderToCell(ICellStyle cellStyle)
        {
            cellStyle.BorderBottom = BorderStyle.Thin;
            cellStyle.BorderLeft = BorderStyle.Thin;
            cellStyle.BorderRight = BorderStyle.Thin;
            cellStyle.BorderTop = BorderStyle.Thin;

            cellStyle.BottomBorderColor = BorderColorIndex;
            cellStyle.LeftBorderColor = BorderColorIndex;
            cellStyle.RightBorderColor = BorderColorIndex;
            cellStyle.TopBorderColor = BorderColorIndex;

            return cellStyle;
        }

        public static ICellStyle SetBorderToFirstGrandTotalCell(ICellStyle cellStyle)
        {
            cellStyle.BorderBottom = BorderStyle.Thin;
            cellStyle.BorderLeft = BorderStyle.Thin;
            cellStyle.BorderRight = BorderStyle.Thin;
            cellStyle.BorderTop = BorderStyle.Thin;

            cellStyle.BottomBorderColor = BorderColorIndex;
            cellStyle.LeftBorderColor = BorderColorIndex;
            cellStyle.RightBorderColor = BorderColorIndex;
            cellStyle.TopBorderColor = GrandTotalSeparatorColorIndex;

            return cellStyle;
        }

        public static void SetColorAtIndex(ExportExcelInfo exportExcelInfo, Color color, short colorIndex)
        {
            HSSFPalette palette = exportExcelInfo.ExcelFile.GetCustomPalette();
            palette.SetColorAtIndex(colorIndex, color.R, color.G, color.B);
        }

        public static string StandardizeExcelReportName(string excelReportName)
        {
            return excelReportName.Replace(" ", "_").Replace("/", "_");
        }

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

        public static void SetGroupBackgroundColorIndex(ExportExcelInfo exportExcelInfo)
        {
            NpoiExtensions.SetColorAtIndex(exportExcelInfo, GroupBackgroundColor, GroupBackgroundColorIndex);
        }

        public static void RenderEmptyRow(ExportExcelInfo exportExcelInfo, int rowIndex)
        {
            CellRangeAddress cellRangeAddress = new CellRangeAddress(
                rowIndex,
                rowIndex,
                exportExcelInfo.StartColumn,
                exportExcelInfo.MaxColumnIndex);
            var textStyle = NpoiExtensions.GetTextStyle(exportExcelInfo.ExcelFile, BackgroundColorIndex, false, CRMHorizontalAlignment.Center);
            RenderMergedCells(exportExcelInfo, cellRangeAddress, string.Empty, textStyle);
        }

        public static ICellStyle GetIntegerNumberStyleGrandTotal(HSSFWorkbook excelFile)
        {
            return GetIntegerNumberStyle(excelFile, GrandTotalRowColorIndex, true, CRMHorizontalAlignment.Right, GrandTotalTextColorIndex);
        }

        public static ICellStyle GetTextStyleGrandTotal(HSSFWorkbook excelFile)
        {
            return GetTextStyle(excelFile, GrandTotalRowColorIndex, true, CRMHorizontalAlignment.Left, GrandTotalTextColorIndex);
        }
    }
}