// --------------------------------------------------------------------------
// <copyright file="CustomerReportService.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Repositories.CustomerReport
{
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Fanex.CRM.Common.Extensions;
    using Fanex.CRM.Common.Models.Npoi;
    using Fanex.CRM.Infrastructure.Extensions;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Report.Criteria.CustomerReport;
    using Fanex.CRM.Report.Models.CustomerReport;
    using Fanex.CRM.Report.Resources;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;

    /// <summary>
    /// Customer Report Service.
    /// </summary>
    public class CustomerReportService : ICustomerReportService
    {
        private readonly ICustomerReportRepository _customerReportRepository;

        public CustomerReportService()
            : this(null)
        {
        }

        public CustomerReportService(
            ICustomerReportRepository customerReportRepository)
        {
            _customerReportRepository = customerReportRepository ?? new CustomerReportRepository();
        }

        public CustomerReportInfo GetCustomerReport(GetCustomerReportCriteria criteria)
        {
            var customerReport = new CustomerReportInfo();
            customerReport.CustomerList = _customerReportRepository.GetCustomerReport(criteria).ToList();
            customerReport.TotalCustomer = customerReport.CustomerList.Count;

            return new CustomerReportInfo
            {
                CustomerList = criteria.IsGetFullReport ?
                    customerReport.CustomerList :
                    customerReport.CustomerList.Skip(criteria.PageSize * criteria.PageIndex)
                       .Take(criteria.PageSize)
                       .ToList(),
                TotalCustomer = customerReport.TotalCustomer
            };
        }

        public void ExportCustomerReport(GetCustomerReportCriteria criteria)
        {
            criteria.IsGetFullReport = true;
            var customerReport = GetCustomerReport(criteria);

            var exportExcelInfo = new ExportExcelInfo
            {
                ExcelFile = new HSSFWorkbook(),
                MaxColumnIndex = 11
            };

            ExportCustomerReportBody(ref exportExcelInfo, customerReport);
            string reportName = Labels.CustomerReport_Title;
            NpoiExtensions.ResponseStreamExcel(exportExcelInfo.ExcelFile, NpoiExtensions.StandardizeExcelReportName(reportName));
        }

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private static void ExportCustomerReportHeader(ref ExportExcelInfo exportExcelInfo)
        {
            NpoiExtensions.SetHeaderColorIndex(exportExcelInfo);
            ICellStyle headerStyle = NpoiExtensions.GetHeaderStyle(
                exportExcelInfo.ExcelFile,
                new ExcelStyleParam());
            var currentColumn = exportExcelInfo.StartColumn;
            exportExcelInfo.Worksheet.SetColumnWidth(currentColumn++, 50 * NpoiExtensions.FontWidth);
            exportExcelInfo.Worksheet.SetColumnWidth(currentColumn++, 160 * NpoiExtensions.FontWidth);
            exportExcelInfo.Worksheet.SetColumnWidth(currentColumn++, 40 * NpoiExtensions.FontWidth);
            exportExcelInfo.Worksheet.SetColumnWidth(currentColumn++, 100 * NpoiExtensions.FontWidth);

            exportExcelInfo.Worksheet.SetColumnWidth(currentColumn++, 200 * NpoiExtensions.FontWidth);
            exportExcelInfo.Worksheet.SetColumnWidth(currentColumn++, 200 * NpoiExtensions.FontWidth);
            exportExcelInfo.Worksheet.SetColumnWidth(currentColumn++, 200 * NpoiExtensions.FontWidth);
            exportExcelInfo.Worksheet.SetColumnWidth(currentColumn++, 200 * NpoiExtensions.FontWidth);
            exportExcelInfo.Worksheet.SetColumnWidth(currentColumn++, 200 * NpoiExtensions.FontWidth);
            exportExcelInfo.Worksheet.SetColumnWidth(currentColumn++, 200 * NpoiExtensions.FontWidth);

            exportExcelInfo.Worksheet.SetColumnWidth(currentColumn++, 400 * NpoiExtensions.FontWidth);
            exportExcelInfo.Worksheet.SetColumnWidth(currentColumn++, 160 * NpoiExtensions.FontWidth);

            string reportTitle = Labels.CustomerReport_Title;
            exportExcelInfo.RenderHeaderTitle(reportTitle, headerStyle);

            IRow row = exportExcelInfo.Worksheet.CreateRow(exportExcelInfo.CurrentRow);
            currentColumn = exportExcelInfo.StartColumn;
            NpoiExtensions.RenderCell(row, currentColumn, Labels.Title, headerStyle);
            currentColumn++;
            NpoiExtensions.RenderCell(row, currentColumn, Labels.Username, headerStyle);
            currentColumn++;
            NpoiExtensions.RenderCell(row, currentColumn, Labels.VIP, headerStyle);
            currentColumn++;
            NpoiExtensions.RenderCell(row, currentColumn, Labels.Birthday, headerStyle);
            currentColumn++;

            NpoiExtensions.RenderCell(row, currentColumn, Labels.PIC1, headerStyle);
            currentColumn++;
            NpoiExtensions.RenderCell(row, currentColumn, Labels.PIC2, headerStyle);
            currentColumn++;
            NpoiExtensions.RenderCell(row, currentColumn, Labels.PIC3, headerStyle);
            currentColumn++;
            NpoiExtensions.RenderCell(row, currentColumn, Labels.PIC4, headerStyle);
            currentColumn++;
            NpoiExtensions.RenderCell(row, currentColumn, Labels.Skype, headerStyle);
            currentColumn++;
            NpoiExtensions.RenderCell(row, currentColumn, Labels.Email, headerStyle);
            currentColumn++;

            NpoiExtensions.RenderCell(row, currentColumn, Labels.Language, headerStyle);
            currentColumn++;
            NpoiExtensions.RenderCell(row, currentColumn, Labels.Site, headerStyle);
            currentColumn++;
            exportExcelInfo.CurrentRow++;
        }

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private static void ExportCustomerReportBody(ref ExportExcelInfo exportExcelInfo, CustomerReportInfo customerReport)
        {
            if (customerReport.CustomerList.Count == 0)
            {
                exportExcelInfo.Worksheet.VerticallyCenter = true;
                exportExcelInfo.Worksheet = exportExcelInfo.ExcelFile.CreateSheet(Labels.CustomerReport_SheetName);
                ExportCustomerReportHeader(ref exportExcelInfo);
                NpoiExtensions.RenderNoInfoMessage(exportExcelInfo, exportExcelInfo.CurrentRow);
                return;
            }

            NpoiExtensions.SetColorAtIndex(exportExcelInfo, NpoiExtensions.EvenRowColor, NpoiExtensions.EvenRowColorIndex);

            IRow row;
            int currentColumn;
            var reportLength = customerReport.CustomerList.Count;
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

            var numberOfSheet = (reportLength / NpoiExtensions.MaxRowPerSheet) +
                (reportLength % NpoiExtensions.MaxRowPerSheet != 0 ? 1 : 0);
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
                for (var i = startIndex; i < endIndex && i < reportLength; i++)
                {
                    row = exportExcelInfo.Worksheet.CreateRow(exportExcelInfo.CurrentRow);
                    currentColumn = exportExcelInfo.StartColumn;
                    var customer = customerReport.CustomerList[i];
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
                        ref exportExcelInfo,
                        ref row,
                        ref currentColumn,
                        customer,
                        isEvenRow ? evenRowCenterTextStyle : centerTextStyle);
                }
            }
        }

        /// <summary>
        /// Exports the dynamic report row.
        /// </summary>
        /// <param name="exportExcelInfo">The export excel information.</param>
        /// <param name="row">The current row.</param>
        /// <param name="currentColumn">The current column.</param>
        /// <param name="customer">The customer.</param>
        /// <param name="centerTextStyle">The center text style.</param>
        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private static void ExportCustomerReportRow(
            ref ExportExcelInfo exportExcelInfo,
            ref IRow row,
            ref int currentColumn,
            CustomerInfo customer,
            ICellStyle centerTextStyle)
        {
            NpoiExtensions.RenderCell(row, currentColumn, customer.Headline, centerTextStyle);
            currentColumn++;
            NpoiExtensions.RenderCell(row, currentColumn, customer.Username, exportExcelInfo.TextStyle);
            currentColumn++;
            NpoiExtensions.RenderCell(row, currentColumn, customer.IsVIP ? Labels.Symbol_IsVIP : string.Empty, centerTextStyle);
            currentColumn++;
            NpoiExtensions.RenderCell(row, currentColumn, customer.BirthDay.HasValue ? customer.BirthDay.Value.ToShortFormatedDate() : string.Empty, centerTextStyle);
            currentColumn++;

            NpoiExtensions.RenderCell(row, currentColumn, customer.Pic1, exportExcelInfo.TextStyle);
            currentColumn++;
            NpoiExtensions.RenderCell(row, currentColumn, customer.Pic2, exportExcelInfo.TextStyle);
            currentColumn++;
            NpoiExtensions.RenderCell(row, currentColumn, customer.Pic3, exportExcelInfo.TextStyle);
            currentColumn++;
            NpoiExtensions.RenderCell(row, currentColumn, customer.Pic4, exportExcelInfo.TextStyle);
            currentColumn++;
            NpoiExtensions.RenderCell(row, currentColumn, customer.Skype, exportExcelInfo.TextStyle);
            currentColumn++;
            NpoiExtensions.RenderCell(row, currentColumn, customer.Email, exportExcelInfo.TextStyle);
            currentColumn++;

            NpoiExtensions.RenderCell(row, currentColumn, customer.Language, exportExcelInfo.TextStyle);
            currentColumn++;
            NpoiExtensions.RenderCell(row, currentColumn, customer.SiteName, exportExcelInfo.TextStyle);
            currentColumn++;
            exportExcelInfo.CurrentRow++;
        }
    }
}