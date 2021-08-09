namespace Fanex.CRM.Report.Excels.PICManagement
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CRM.Common.Enumerations;
    using Fanex.CRM.Common.Excel;
    using Fanex.CRM.Common.Extensions;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Common.Models.Npoi;
    using Fanex.CRM.Models.PicAndHeadline;
    using Fanex.CRM.Report.Resources;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;

    public class PICManagementExcel : BaseExcel
    {
        private readonly PicReport _picReport;
        private readonly IEnumerable<BaseModel> _communicationTools;
        private const int _baseColumnIndex = 9;
        private const int _startCommToolsIndex = 7;

        public PICManagementExcel(PicReport picReport, IEnumerable<BaseModel> communicationTools, string excelFileName)
            : base(excelFileName)
        {
            _picReport = picReport;
            _communicationTools = communicationTools;
        }

        public override void BuildWorkBook()
        {
            var exportExcelInfo = new ExportExcelInfo
            {
                ExcelFile = new HSSFWorkbook(),
                MaxColumnIndex = _communicationTools.Count() + _baseColumnIndex - 1
            };

            RenderBody(exportExcelInfo);
            NpoiExtensions.ResponseStreamExcel(exportExcelInfo.ExcelFile, NpoiExtensions.StandardizeExcelReportName(ExcelFileName));
        }

        private void RenderHeader(ExportExcelInfo exportExcelInfo)
        {
            const int headerRowNumber = 2;
            const int largeWidth = 120;
            const int mediumWidth = 80;
            const int smallWidth = 50;
            const int largerWidth = 200;

            NpoiExtensions.SetHeaderColorIndex(exportExcelInfo);
            var headerStyle = NpoiExtensions.GetHeaderStyle(exportExcelInfo.ExcelFile, new ExcelStyleParam());
            headerStyle.WrapText = true;
            var currentColumn = exportExcelInfo.StartColumn;
            var columnWidths = new List<int>
            {
                smallWidth, largeWidth, mediumWidth, largeWidth, largeWidth, largerWidth
            };

            for (int index = 0; index < _communicationTools.Count(); index++)
            {
                columnWidths.Add(largeWidth);
            }

            columnWidths.AddRange(new List<int> { largerWidth, smallWidth });

            foreach (var item in columnWidths)
            {
                exportExcelInfo.Worksheet.SetColumnWidth(currentColumn++, item * NpoiExtensions.FontWidth);
            }

            string reportTitle = Labels.PICManagement;
            exportExcelInfo.RenderHeaderTitle(reportTitle, headerStyle);
            
            var columnHeaders = new List<string>
            {
                Labels.NumberColumn, Labels.PicType ,Labels.Name, Labels.Role, Labels.Language, Labels.Country, Labels.Group_Headline
            };

            columnHeaders.AddRange(_communicationTools.Select(tool => tool.Name));
            columnHeaders.AddRange(new List<string> { Labels.Remark, Labels.Active });

            var firstHeaderRowIndex = exportExcelInfo.CurrentRow;
            IRow secondHeaderRow = exportExcelInfo.Worksheet.CreateRow(firstHeaderRowIndex + 1);
            for (var index = 0; index < _baseColumnIndex + _communicationTools.Count(); index++)
            {
                if (index == _startCommToolsIndex)
                {
                    var commToolsCellRangeAddress = new CellRangeAddress(firstHeaderRowIndex, firstHeaderRowIndex, index, index + _communicationTools.Count() - 1);
                    NpoiExtensions.RenderMergedCells(exportExcelInfo, commToolsCellRangeAddress, Labels.Communication_Tool, headerStyle);
                }

                if (index < _startCommToolsIndex || index >= _startCommToolsIndex + _communicationTools.Count())
                {
                    var cellRangeAddress = new CellRangeAddress(firstHeaderRowIndex, firstHeaderRowIndex + 1, index, index);
                    NpoiExtensions.RenderMergedCells(exportExcelInfo, cellRangeAddress, columnHeaders[index], headerStyle);
                }
                else
                {
                    NpoiExtensions.RenderCell(secondHeaderRow, index, columnHeaders[index], headerStyle);
                }
            }

            exportExcelInfo.CurrentRow = firstHeaderRowIndex + headerRowNumber;
        }

        private void RenderRow(
            ExportExcelInfo exportExcelInfo,
            IRow row,
            int indexNumber,
            int currentColumn,
            PicInfo picInfo,
            ICellStyle centerTextStyle)
        {
            NpoiExtensions.RenderCell(row, currentColumn++, indexNumber, centerTextStyle);
            NpoiExtensions.RenderCell(row, currentColumn++, picInfo.PicType, centerTextStyle);
            NpoiExtensions.RenderCell(row, currentColumn++, picInfo.Username, exportExcelInfo.TextStyle);
            NpoiExtensions.RenderCell(row, currentColumn++, picInfo.RoleName, exportExcelInfo.TextStyle);

            var languages = string.Join(", ", picInfo.Languages.Select(lang => lang.Value));
            NpoiExtensions.RenderCell(row, currentColumn++, languages, exportExcelInfo.TextStyle);
            NpoiExtensions.RenderCell(row, currentColumn++, picInfo.CountryName, exportExcelInfo.TextStyle);
            NpoiExtensions.RenderCell(row, currentColumn++, picInfo.GroupHeadline, exportExcelInfo.TextStyle);

            var tools = _communicationTools.ToList();
            for (var index = 0; index < tools.Count; index++)
            {
                var item = picInfo.CommunicationTools.Find(t => t.SettingId == tools[index].Id);
                NpoiExtensions.RenderCell(row, currentColumn++, item == null ? string.Empty : item.Value, exportExcelInfo.TextStyle);
            }
            
            NpoiExtensions.RenderCell(row, currentColumn++, picInfo.Remark, exportExcelInfo.TextStyle);
            NpoiExtensions.RenderCell(row, currentColumn, picInfo.IsActive ? Labels.Yes : Labels.No, centerTextStyle);

            exportExcelInfo.CurrentRow++;
        }

        private void RenderBody(ExportExcelInfo exportExcelInfo)
        {
            if (_picReport.DataList.Count == 0)
            {
                exportExcelInfo.Worksheet = exportExcelInfo.ExcelFile.CreateSheet(Labels.PICManagement);
                exportExcelInfo.Worksheet.VerticallyCenter = true;
                RenderHeader(exportExcelInfo);
                NpoiExtensions.RenderNoInfoMessage(exportExcelInfo, exportExcelInfo.CurrentRow);
                return;
            }

            NpoiExtensions.SetColorAtIndex(exportExcelInfo, NpoiExtensions.EvenRowColor, NpoiExtensions.EvenRowColorIndex);

            var textStyle = NpoiExtensions.GetTextStyle(
                exportExcelInfo.ExcelFile,
                NpoiExtensions.BackgroundColorIndex,
                false,
                ExcelHorizontalAlignment.Left);
            textStyle.WrapText = true;

            var centerTextStyle = NpoiExtensions.GetTextStyle(
                exportExcelInfo.ExcelFile,
                NpoiExtensions.BackgroundColorIndex,
                false,
                ExcelHorizontalAlignment.Center);
            centerTextStyle.WrapText = true;

            var evenRowTextStyle = NpoiExtensions.GetTextStyle(
                exportExcelInfo.ExcelFile,
                NpoiExtensions.EvenRowColorIndex,
                false,
                ExcelHorizontalAlignment.Left);
            evenRowTextStyle.WrapText = true;

            var evenRowCenterTextStyle = NpoiExtensions.GetTextStyle(
                exportExcelInfo.ExcelFile,
                NpoiExtensions.EvenRowColorIndex,
                false,
                ExcelHorizontalAlignment.Center);
            evenRowCenterTextStyle.WrapText = true;

            var totalRows = _picReport.DataList.Count;
            var numberOfSheet = (totalRows / NpoiExtensions.MaxRowPerSheet) +
                (totalRows % NpoiExtensions.MaxRowPerSheet != 0 ? 1 : 0);
            for (var sheetIndex = 0; sheetIndex < numberOfSheet; sheetIndex++)
            {
                var sheetName = numberOfSheet > 1 ?
                    string.Format("{0} - {1}", Labels.PICManagement, sheetIndex + 1) :
                    Labels.PICManagement;
                exportExcelInfo.Worksheet = exportExcelInfo.ExcelFile.CreateSheet(sheetName);
                exportExcelInfo.Worksheet.VerticallyCenter = true;
                exportExcelInfo.CurrentRow = 0;
                RenderHeader(exportExcelInfo);
                var startIndex = sheetIndex * NpoiExtensions.MaxRowPerSheet;
                var endIndex = startIndex + NpoiExtensions.MaxRowPerSheet;
                int i;
                for (i = startIndex; i < endIndex && i < totalRows; i++)
                {
                    var row = exportExcelInfo.Worksheet.CreateRow(exportExcelInfo.CurrentRow);
                    var currentColumn = exportExcelInfo.StartColumn;
                    var pic = _picReport.DataList[i];

                    var isEvenRow = (i + 1) % 2 == 0;
                    exportExcelInfo.TextStyle = isEvenRow ? evenRowTextStyle : textStyle;

                    RenderRow(
                        exportExcelInfo,
                        row,
                        i + 1,
                        currentColumn,
                        pic,
                        isEvenRow ? evenRowCenterTextStyle : centerTextStyle);
                }
            }
        }
    }
}