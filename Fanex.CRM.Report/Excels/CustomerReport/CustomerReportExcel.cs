namespace Fanex.CRM.Report.Excels.CustomerReport
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.CRM.Common.Excel;
    using Fanex.CRM.Common.Extensions;
    using Fanex.CRM.Common.Models.Npoi;
    using Fanex.CRM.Infrastructure.Extensions;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Report.Resources;
    using Models.CustomerReport;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;

    public class CustomerReportExcel : BaseExcel
    {
        private readonly CustomerReportInfo _customerReportInfo;

        private readonly int _totalRows;

        public CustomerReportExcel(CustomerReportInfo customerReportInfo, string excelFileName, int totalRows)
            : base(excelFileName)
        {
            _customerReportInfo = customerReportInfo;
            _totalRows = totalRows;
        }

        public override void BuildWorkBook()
        {
            var exportExcelInfo = new ExportExcelInfo
            {
                ExcelFile = new HSSFWorkbook(),
                MaxColumnIndex = 26
            };

            ExportCustomerReportBody(exportExcelInfo);
            string reportName = Labels.CustomerReport_Title;
            NpoiExtensions.ResponseStreamExcel(exportExcelInfo.ExcelFile, NpoiExtensions.StandardizeExcelReportName(ExcelFileName));
        }

        private static void ExportCustomerReportHeader(ref ExportExcelInfo exportExcelInfo)
        {
            NpoiExtensions.SetHeaderColorIndex(exportExcelInfo);
            ICellStyle headerStyle = NpoiExtensions.GetHeaderStyle(
                exportExcelInfo.ExcelFile,
                new ExcelStyleParam());
            var currentColumn = exportExcelInfo.StartColumn;
            var columnWidths = new int[]
            {
                160, 160, 160, 160, 200, 200, 200, 200, 200, 200, 150, 150, 150, 150,
                150, 150, 150, 150, 150, 80, 200, 200, 200, 200, 140, 160, 160
            };

            foreach (var item in columnWidths)
            {
                exportExcelInfo.Worksheet.SetColumnWidth(currentColumn++, item * NpoiExtensions.FontWidth);
            }

            string reportTitle = Labels.CustomerReport_Title;
            exportExcelInfo.RenderHeaderTitle(reportTitle, headerStyle);

            IRow row = exportExcelInfo.Worksheet.CreateRow(exportExcelInfo.CurrentRow);
            currentColumn = exportExcelInfo.StartColumn;
            var columnHeaders = new string[]
            {
                Labels.Headline, Labels.Username, Labels.First_Name, Labels.Last_Name,
                Labels.Phone + "1", Labels.Phone + "2", Labels.Phone + "3", Labels.Mobile, Labels.Fax,
                Labels.Email, Labels.Skype, Labels.MSN, Labels.WhatsApp, Labels.Line,
                Labels.WeChat, Labels.Yahoo, Labels.BBM, Labels.QQ, Labels.Language,
                Labels.VIP, Labels.PIC1, Labels.PIC2, Labels.PIC3, Labels.PIC4,
                Labels.Birthday, Labels.Passport, Labels.SiteName
            };

            foreach (var item in columnHeaders)
            {
                NpoiExtensions.RenderCell(row, currentColumn++, item, headerStyle);
            }

            exportExcelInfo.CurrentRow++;
        }

        private static void ExportCustomerReportRow(
            ExportExcelInfo exportExcelInfo,
            IRow row,
            int currentColumn,
            CustomerInfo customer,
            ICellStyle centerTextStyle)
        {
            NpoiExtensions.RenderCell(row, currentColumn++, customer.Headline, exportExcelInfo.TextStyle);
            NpoiExtensions.RenderCell(row, currentColumn++, customer.Username, exportExcelInfo.TextStyle);
            NpoiExtensions.RenderCell(row, currentColumn++, customer.FirstName, exportExcelInfo.TextStyle);
            NpoiExtensions.RenderCell(row, currentColumn++, customer.LastName, exportExcelInfo.TextStyle);
            NpoiExtensions.RenderCell(row, currentColumn++, customer.Phone1, exportExcelInfo.TextStyle);
            NpoiExtensions.RenderCell(row, currentColumn++, customer.Phone2, exportExcelInfo.TextStyle);
            NpoiExtensions.RenderCell(row, currentColumn++, customer.Phone3, exportExcelInfo.TextStyle);
            NpoiExtensions.RenderCell(row, currentColumn++, customer.Mobile, exportExcelInfo.TextStyle);
            NpoiExtensions.RenderCell(row, currentColumn++, customer.Fax, exportExcelInfo.TextStyle);
            NpoiExtensions.RenderCell(row, currentColumn++, customer.Email, exportExcelInfo.TextStyle);
            NpoiExtensions.RenderCell(row, currentColumn++, customer.Skype, exportExcelInfo.TextStyle);
            NpoiExtensions.RenderCell(row, currentColumn++, customer.MSN, exportExcelInfo.TextStyle);
            NpoiExtensions.RenderCell(row, currentColumn++, customer.WhatsApp, exportExcelInfo.TextStyle);
            NpoiExtensions.RenderCell(row, currentColumn++, customer.Line, exportExcelInfo.TextStyle);
            NpoiExtensions.RenderCell(row, currentColumn++, customer.WeChat, exportExcelInfo.TextStyle);
            NpoiExtensions.RenderCell(row, currentColumn++, customer.Yahoo, exportExcelInfo.TextStyle);
            NpoiExtensions.RenderCell(row, currentColumn++, customer.BBM, exportExcelInfo.TextStyle);
            NpoiExtensions.RenderCell(row, currentColumn++, customer.QQ, exportExcelInfo.TextStyle);
            NpoiExtensions.RenderCell(row, currentColumn++, customer.Language, exportExcelInfo.TextStyle);
            NpoiExtensions.RenderCell(row, currentColumn++, customer.IsVIP ? Labels.Symbol_IsVIP : string.Empty, centerTextStyle);
            NpoiExtensions.RenderCell(row, currentColumn++, customer.Pic1, exportExcelInfo.TextStyle);
            NpoiExtensions.RenderCell(row, currentColumn++, customer.Pic2, exportExcelInfo.TextStyle);
            NpoiExtensions.RenderCell(row, currentColumn++, customer.Pic3, exportExcelInfo.TextStyle);
            NpoiExtensions.RenderCell(row, currentColumn++, customer.Pic4, exportExcelInfo.TextStyle);
            NpoiExtensions.RenderCell(row, currentColumn++, customer.BirthDay.HasValue ? customer.BirthDay.Value.ToShortFormatedDate() : string.Empty, centerTextStyle);
            NpoiExtensions.RenderCell(row, currentColumn++, customer.Passport, exportExcelInfo.TextStyle);
            NpoiExtensions.RenderCell(row, currentColumn++, customer.SiteName, exportExcelInfo.TextStyle);
            exportExcelInfo.CurrentRow++;
        }

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private void ExportCustomerReportBody(ExportExcelInfo exportExcelInfo)
        {
            if (_customerReportInfo.CustomerList.Count == 0)
            {
                exportExcelInfo.Worksheet.VerticallyCenter = true;
                exportExcelInfo.Worksheet = exportExcelInfo.ExcelFile.CreateSheet(Labels.CustomerReport_SheetName);
                ExportCustomerReportHeader(ref exportExcelInfo);
                NpoiExtensions.RenderNoInfoMessage(exportExcelInfo, exportExcelInfo.CurrentRow);
                return;
            }

            NpoiExtensions.SetColorAtIndex(exportExcelInfo, NpoiExtensions.EvenRowColor, NpoiExtensions.EvenRowColorIndex);

            var textStyle = NpoiExtensions.GetTextStyle(
                exportExcelInfo.ExcelFile,
                NpoiExtensions.BackgroundColorIndex,
                false,
                CRMHorizontalAlignment.Left);
            var centerTextStyle = NpoiExtensions.GetTextStyle(
                exportExcelInfo.ExcelFile,
                NpoiExtensions.BackgroundColorIndex,
                false,
                CRMHorizontalAlignment.Center);

            var evenRowTextStyle = NpoiExtensions.GetTextStyle(
                exportExcelInfo.ExcelFile,
                NpoiExtensions.EvenRowColorIndex,
                false,
                CRMHorizontalAlignment.Left);
            var evenRowCenterTextStyle = NpoiExtensions.GetTextStyle(
                exportExcelInfo.ExcelFile,
                NpoiExtensions.EvenRowColorIndex,
                false,
                CRMHorizontalAlignment.Center);

            var numberOfSheet = (_totalRows / NpoiExtensions.MaxRowPerSheet) +
                (_totalRows % NpoiExtensions.MaxRowPerSheet != 0 ? 1 : 0);
            for (var sheetIndex = 0; sheetIndex < numberOfSheet; sheetIndex++)
            {
                var sheetName = numberOfSheet > 1 ?
                    string.Format("{0} - {1}", Labels.CustomerReport_SheetName, sheetIndex + 1) :
                    Labels.CustomerReport_SheetName;
                exportExcelInfo.Worksheet = exportExcelInfo.ExcelFile.CreateSheet(sheetName);
                exportExcelInfo.Worksheet.VerticallyCenter = true;
                exportExcelInfo.CurrentRow = 0;
                ExportCustomerReportHeader(ref exportExcelInfo);
                var startIndex = sheetIndex * NpoiExtensions.MaxRowPerSheet;
                var endIndex = startIndex + NpoiExtensions.MaxRowPerSheet;
                for (var i = startIndex; i < endIndex && i < _totalRows; i++)
                {
                    var row = exportExcelInfo.Worksheet.CreateRow(exportExcelInfo.CurrentRow);
                    var currentColumn = exportExcelInfo.StartColumn;
                    var customer = _customerReportInfo.CustomerList[i];
                    var isEvenRow = (i + 1) % 2 == 0;
                    if (isEvenRow)
                    {
                        exportExcelInfo.TextStyle = evenRowTextStyle;
                    }
                    else
                    {
                        exportExcelInfo.TextStyle = textStyle;
                    }

                    ExportCustomerReportRow(
                        exportExcelInfo,
                        row,
                        currentColumn,
                        customer,
                        isEvenRow ? evenRowCenterTextStyle : centerTextStyle);
                }
            }
        }
    }
}