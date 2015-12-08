// -----------------------------------------------------------------------
// <copyright file="LogStatisticExcel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.Report.Excels.LogStatistic
{
    using System.Linq;
    using Fanex.CRM.Common.Excel;
    using Fanex.CRM.Common.Extensions;
    using Fanex.CRM.Common.Models.Npoi;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Report.Models.LogStatistic;
    using Fanex.CRM.Report.Resources;
    using NPOI.HSSF.Util;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;

    public class LogStatisticExcel : BaseExcel
    {
        private readonly LogStatisticViewModel _logStatisticReport;

        public LogStatisticExcel(LogStatisticViewModel logStatisticReport, string excelFileName)
            : base(excelFileName)
        {
            _logStatisticReport = logStatisticReport;
        }

        public override void BuildWorkBook()
        {
            int totalDataColumns = _logStatisticReport.GrandTotalOfCommunicationCount.Count + _logStatisticReport.ProductColumns.Count();
            ExportExcelInfo exportExcelInfo = new ExportExcelInfo
            {
                ExcelFile = WorkBook,
                Worksheet = WorkBook.CreateSheet(ExcelFileName),
                StartRow = 0,
                StartColumn = 0,
                CurrentRow = 0,
                MaxColumnIndex = totalDataColumns + 1
            };

            SetColumnsWidth(exportExcelInfo);
            GenerateTitle(exportExcelInfo, _logStatisticReport.Name);

            GenerateHeader(exportExcelInfo);
            GenerateBodyData(exportExcelInfo);
            GenerateFooterData(exportExcelInfo);

            NpoiExcelExtensions.ResponseStreamExcel(exportExcelInfo.ExcelFile, ExcelFileName);
        }

        private void SetColumnsWidth(ExportExcelInfo exportExcelInfo)
        {
            exportExcelInfo.ResetCurrentColumn();
            exportExcelInfo.SetColumnWidthByFontWidth(exportExcelInfo.CurrentColumn++, 280);
            for (int i = 0; i < exportExcelInfo.MaxColumnIndex; i++)
            {
                exportExcelInfo.SetColumnWidthByFontWidth(exportExcelInfo.CurrentColumn++, 100);
            }
        }

        private void GenerateTitle(ExportExcelInfo exportExcelInfo, string title)
        {
            int currentRowIndex = exportExcelInfo.CurrentRow;

            NpoiExcelExtensions.SetHeaderColorIndex(exportExcelInfo);
            ICellStyle titleStyle = NpoiExcelExtensions.HeaderStyle(exportExcelInfo.ExcelFile, new ExcelStyleParam() { Alignment = CRMHorizontalAlignment.Left });
            exportExcelInfo.RenderHeaderTitle(title, titleStyle);
            currentRowIndex += 2;

            NpoiExcelExtensions.RenderEmptyRow(exportExcelInfo, currentRowIndex);
            exportExcelInfo.CurrentRow = currentRowIndex + 1;
        }

        private void GenerateHeader(ExportExcelInfo exportExcelInfo)
        {
            int currentRowIndex = exportExcelInfo.CurrentRow;
            int currentColumnIndex = exportExcelInfo.StartColumn;

            var productColumns = _logStatisticReport.ProductColumns;
            int productColumnsCount = productColumns.Count();

            var communicationColumns = _logStatisticReport.CommunicationColumns;
            int communicationColumnsCount = communicationColumns.Count();

            var headerStyle = NpoiExcelExtensions.HeaderStyle(exportExcelInfo.ExcelFile, new ExcelStyleParam());
            headerStyle.WrapText = true;

            CellRangeAddress categoryCellRangeAddress = new CellRangeAddress(currentRowIndex, currentRowIndex + 1, currentColumnIndex, currentColumnIndex);
            NpoiExcelExtensions.RenderMergedCells(exportExcelInfo, categoryCellRangeAddress, Labels.Categories, headerStyle);
            currentColumnIndex++;

            var row = exportExcelInfo.Worksheet.CreateRow(currentRowIndex + 1);

            int step = communicationColumnsCount + 1;
            for (int i = 0; i < productColumnsCount; i++)
            {
                var cellProductTitleRange = new CellRangeAddress(currentRowIndex, currentRowIndex, currentColumnIndex, currentColumnIndex + step - 1);
                NpoiExcelExtensions.RenderMergedCells(exportExcelInfo, cellProductTitleRange, productColumns.ElementAt(i).Name, headerStyle);

                for (int j = 0; j < communicationColumnsCount; j++)
                {
                    NpoiExcelExtensions.RenderCell(row, currentColumnIndex, communicationColumns.ElementAt(j).Name, headerStyle);
                    currentColumnIndex++;
                }

                NpoiExcelExtensions.RenderCell(row, currentColumnIndex, Labels.GrandTotal, headerStyle);
                currentColumnIndex++;
            }

            CellRangeAddress gandTotalCellRangeAddress = new CellRangeAddress(currentRowIndex, currentRowIndex + 1, currentColumnIndex, currentColumnIndex);
            NpoiExcelExtensions.RenderMergedCells(exportExcelInfo, gandTotalCellRangeAddress, Labels.GrandTotal, headerStyle);

            exportExcelInfo.CurrentRow = currentRowIndex + 2;
        }

        private void GenerateBodyData(ExportExcelInfo exportExcelInfo)
        {
            int currentRowIndex = exportExcelInfo.CurrentRow;
            int currentColumnIndex = exportExcelInfo.StartColumn;

            var mainCategoryTextStyle = NpoiExcelExtensions.TextStyle(exportExcelInfo.ExcelFile, NpoiExcelExtensions.GroupBackgroundColorIndex, false, CRMHorizontalAlignment.Left);
            var mainCategorynumberStyle = NpoiExcelExtensions.TextStyle(exportExcelInfo.ExcelFile, NpoiExcelExtensions.GroupBackgroundColorIndex, false, CRMHorizontalAlignment.Center);            
            var font = exportExcelInfo.ExcelFile.CreateFont();
            SetStyleForMainCategory(font, mainCategoryTextStyle);
            SetStyleForMainCategory(font, mainCategorynumberStyle);

            var subCategoryTextStyle = NpoiExcelExtensions.TextStyle(exportExcelInfo.ExcelFile, NpoiExcelExtensions.BackgroundColorIndex, false, CRMHorizontalAlignment.Left);
            var subCategorynumberStyle = NpoiExcelExtensions.TextStyle(exportExcelInfo.ExcelFile, NpoiExcelExtensions.BackgroundColorIndex, false, CRMHorizontalAlignment.Center);            
            
            var mainCategories = _logStatisticReport.MainCategories;
            foreach (var mainCategory in mainCategories)
            {
                GenerateRowValue(exportExcelInfo, mainCategory, mainCategoryTextStyle, mainCategorynumberStyle);

                var subCategories = mainCategory.SubCategories;
                foreach (var subCategory in subCategories)
                {
                    GenerateRowValue(exportExcelInfo, subCategory, subCategoryTextStyle, subCategorynumberStyle);
                }
            }
        }

        private void GenerateFooterData(ExportExcelInfo exportExcelInfo)
        {
            NpoiExcelExtensions.SetGrandTotalColorIndex(exportExcelInfo);
            var footerTextStyle = NpoiExcelExtensions.TextStyleGrandTotal(exportExcelInfo.ExcelFile);
            var footerValueStyle = NpoiExcelExtensions.IntNumberStyleGrandTotal(exportExcelInfo.ExcelFile);
            footerValueStyle.Alignment = HorizontalAlignment.Center;

            var granTotal = new LogStatisticBaseModel
            {
                Name = Labels.GrandTotal,
                ListOfCommunicationCount = _logStatisticReport.GrandTotalOfCommunicationCount,
                ListOfCommunicationSumByProduct = _logStatisticReport.GrandTotalOfCommunicationSumByProduct              
            };

            GenerateRowValue(exportExcelInfo, granTotal, footerTextStyle, footerValueStyle);
        }

        private void GenerateRowValue(ExportExcelInfo exportExcelInfo, LogStatisticBaseModel category, ICellStyle textStyle, ICellStyle numberStyle)
        {
            exportExcelInfo.ResetCurrentColumn();

            NpoiExcelExtensions.SetGroupBackgroundColorIndex(exportExcelInfo);

            int currentColumnIndex = exportExcelInfo.StartColumn;
            var row = exportExcelInfo.Worksheet.CreateRow(exportExcelInfo.CurrentRow);

            NpoiExcelExtensions.RenderCell(row, currentColumnIndex, category.Name, textStyle);
            currentColumnIndex++;

            var listOfCommunicationSumByProduct = category.ListOfCommunicationSumByProduct;
            var listOfNumberCommunicationCount = category.ListOfCommunicationCount;

            int communicationColumnsCount = _logStatisticReport.CommunicationColumns.Count();
            int currentCommunicationIndex = 0;
            foreach (var communicationSumByProduct in listOfCommunicationSumByProduct)
            {
                for (int i = 0; i < communicationColumnsCount; i++)
                {
                    NpoiExcelExtensions.RenderCell(row, currentColumnIndex, listOfNumberCommunicationCount[currentCommunicationIndex], numberStyle);
                    currentCommunicationIndex++;
                    currentColumnIndex++;
                }

                NpoiExcelExtensions.RenderCell(row, currentColumnIndex, communicationSumByProduct, numberStyle);
                currentColumnIndex++;
            }

            NpoiExcelExtensions.RenderCell(row, currentColumnIndex, category.TotalCommunications, numberStyle);

            exportExcelInfo.CurrentRow++;
        }

        private void SetStyleForMainCategory(IFont font, ICellStyle cellStyle)
        {
            font.FontHeightInPoints = NpoiExcelExtensions.FontSize;
            font.FontName = NpoiExcelExtensions.FontName;
            font.Color = (short)HSSFColor.Blue.Index;
            font.Boldweight = (short)FontBoldWeight.Bold;

            cellStyle.SetFont(font);          
        }
    }
}
