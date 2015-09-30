// --------------------------------------------------------------------------
// <copyright file="CustomerReportService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Repositories.CustomerReport
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web;
    using Fanex.CRM.Common.Extensions;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Common.Models.Npoi;
    using Fanex.CRM.Common.Services;
    using Fanex.CRM.Infrastructure.Constants;
    using Fanex.CRM.Infrastructure.Extensions;
    using Fanex.CRM.Infrastructure.Helpers;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Report.Criteria.CustomerReport;
    using Fanex.CRM.Report.Models.CustomerReport;
    using Fanex.CRM.Report.Resources;
    using Fanex.CRM.Setting.Constants;
    using Fanex.CRM.Setting.Models;
    using Fanex.CRM.Setting.Services;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;

    /// <summary>
    /// Customer Report Service.
    /// </summary>
    public class CustomerReportService : ICustomerReportService
    {
        private readonly string _customerReportSessionKey = "Fanex.CRM.Report.CustomerReport.CustomerReportSessionKey_" +
            (AppSession.CurrentUser != null ? AppSession.CurrentUser.Id : 0);

        private ICustomerReportRepository _customerReportRepository;

        private ISettingService _serviceSetting;

        public CustomerReportService()
            : this(null, null)
        {
        }

        public CustomerReportService(
            ICustomerReportRepository customerReportRepository,
            ISettingService serviceSetting)
        {
            _customerReportRepository = customerReportRepository ?? new CustomerReportRepository();
            _serviceSetting = serviceSetting ?? new SettingService();
        }

        public CustomerReportInfo CustomerReport(GetCustomerReportCriteria criteria)
        {
            CustomerReportInfo customerReport = null;
            if (criteria.IsGetFromCached)
            {
                customerReport = (CustomerReportInfo)SessionExtension.GetSession(HttpContext.Current.Session, _customerReportSessionKey);
            }

            if (customerReport == null)
            {
                customerReport = new CustomerReportInfo();
                customerReport.CustomerList = _customerReportRepository.GetCustomerReport(criteria);
                customerReport.TotalCustomer = customerReport.CustomerList.Count;
            }

            if (criteria.IsSortReport &&
                !string.IsNullOrEmpty(criteria.SortField) &&
                !string.IsNullOrEmpty(criteria.SortDirection))
            {
                customerReport.CustomerList = SortCustomerList(customerReport.CustomerList, criteria);
            }

            if (customerReport != null)
            {
                SessionExtension.AddSession(HttpContext.Current.Session, _customerReportSessionKey, customerReport);
            }

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
            CustomerReportInfo customerReport = CacheService.Get<CustomerReportInfo>(_customerReportSessionKey);
            if (customerReport == null)
            {
                customerReport = CustomerReport(criteria);
            }

            ExportExcelInfo exportExcelInfo = new ExportExcelInfo
            {
                ExcelFile = new HSSFWorkbook(),
                MaxColumnIndex = 11
            };

            ExportCustomerReportBody(ref exportExcelInfo, customerReport);
            string reportName = Labels.CustomerReport_Title;
            NpoiExcelExtensions.ResponseStreamExcel(exportExcelInfo.ExcelFile, NpoiExcelExtensions.StandardizeExcelReportName(reportName));
        }

        public IEnumerable<SiteInfo> GetAllSites()
        {
            return _serviceSetting.GetAllSites();
        }

        public IEnumerable<SettingInfo> GetAllTitles()
        {
            var allTitle = _serviceSetting.GetAllTitles().ToList();
            allTitle.Insert(
                0,
                new SettingInfo
                {
                    Value = Labels.All,
                    Id = TitleConstants.All,
                    DisplayOrder = 0
                });

            return allTitle;
        }

        public IEnumerable<CountryInfo> GetAllCountries()
        {
            return _serviceSetting.GetAllCountries();
        }

        public IEnumerable<SettingInfo> GetAllLanguages()
        {
            return _serviceSetting.GetAllLanguages();
        }

        public IEnumerable<SettingInfo> GetAllCustomerTypes()
        {
            return _serviceSetting.GetAllCustomerTypes();
        }

        public IEnumerable<SettingInfo> GetAllVIPTypes()
        {
            return _serviceSetting.GetAllVIPTypes();
        }

        /// <summary>
        /// Exports the customer report header.
        /// </summary>
        /// <param name="exportExcelInfo">The export excel information.</param>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private void ExportCustomerReportHeader(ref ExportExcelInfo exportExcelInfo)
        {
            NpoiExcelExtensions.SetHeaderColorIndex(exportExcelInfo);
            ICellStyle headerStyle = NpoiExcelExtensions.HeaderStyle(
                exportExcelInfo.ExcelFile,
                new ExcelStyleParam());
            var currentColumn = exportExcelInfo.StartColumn;
            exportExcelInfo.Worksheet.SetColumnWidth(currentColumn++, 50 * NpoiExcelExtensions.FontWidth);
            exportExcelInfo.Worksheet.SetColumnWidth(currentColumn++, 160 * NpoiExcelExtensions.FontWidth);
            exportExcelInfo.Worksheet.SetColumnWidth(currentColumn++, 40 * NpoiExcelExtensions.FontWidth);
            exportExcelInfo.Worksheet.SetColumnWidth(currentColumn++, 100 * NpoiExcelExtensions.FontWidth);

            exportExcelInfo.Worksheet.SetColumnWidth(currentColumn++, 200 * NpoiExcelExtensions.FontWidth);
            exportExcelInfo.Worksheet.SetColumnWidth(currentColumn++, 200 * NpoiExcelExtensions.FontWidth);
            exportExcelInfo.Worksheet.SetColumnWidth(currentColumn++, 200 * NpoiExcelExtensions.FontWidth);
            exportExcelInfo.Worksheet.SetColumnWidth(currentColumn++, 200 * NpoiExcelExtensions.FontWidth);
            exportExcelInfo.Worksheet.SetColumnWidth(currentColumn++, 200 * NpoiExcelExtensions.FontWidth);
            exportExcelInfo.Worksheet.SetColumnWidth(currentColumn++, 200 * NpoiExcelExtensions.FontWidth);

            exportExcelInfo.Worksheet.SetColumnWidth(currentColumn++, 400 * NpoiExcelExtensions.FontWidth);
            exportExcelInfo.Worksheet.SetColumnWidth(currentColumn++, 160 * NpoiExcelExtensions.FontWidth);

            string reportTitle = Labels.CustomerReport_Title;
            exportExcelInfo.RenderHeaderTitle(reportTitle, headerStyle);

            IRow row = exportExcelInfo.Worksheet.CreateRow(exportExcelInfo.CurrentRow);
            currentColumn = exportExcelInfo.StartColumn;
            NpoiExcelExtensions.RenderCell(row, currentColumn, Labels.Title, headerStyle);
            currentColumn++;
            NpoiExcelExtensions.RenderCell(row, currentColumn, Labels.Username, headerStyle);
            currentColumn++;
            NpoiExcelExtensions.RenderCell(row, currentColumn, Labels.VIP, headerStyle);
            currentColumn++;
            NpoiExcelExtensions.RenderCell(row, currentColumn, Labels.Birthday, headerStyle);
            currentColumn++;

            NpoiExcelExtensions.RenderCell(row, currentColumn, Labels.PIC1, headerStyle);
            currentColumn++;
            NpoiExcelExtensions.RenderCell(row, currentColumn, Labels.PIC2, headerStyle);
            currentColumn++;
            NpoiExcelExtensions.RenderCell(row, currentColumn, Labels.PIC3, headerStyle);
            currentColumn++;
            NpoiExcelExtensions.RenderCell(row, currentColumn, Labels.PIC4, headerStyle);
            currentColumn++;
            NpoiExcelExtensions.RenderCell(row, currentColumn, Labels.Skype, headerStyle);
            currentColumn++;
            NpoiExcelExtensions.RenderCell(row, currentColumn, Labels.Email, headerStyle);
            currentColumn++;

            NpoiExcelExtensions.RenderCell(row, currentColumn, Labels.Language, headerStyle);
            currentColumn++;
            NpoiExcelExtensions.RenderCell(row, currentColumn, Labels.Site, headerStyle);
            currentColumn++;
            exportExcelInfo.CurrentRow++;
        }

        /// <summary>
        /// Exports the dynamic report body.
        /// </summary>
        /// <param name="exportExcelInfo">The export excel information.</param>
        /// <param name="customerReport">The customer report.</param>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private void ExportCustomerReportBody(ref ExportExcelInfo exportExcelInfo, CustomerReportInfo customerReport)
        {
            if (customerReport == null || customerReport.CustomerList.Count == 0)
            {
                exportExcelInfo.Worksheet.VerticallyCenter = true;
                exportExcelInfo.Worksheet = exportExcelInfo.ExcelFile.CreateSheet(Labels.CustomerReport_SheetName);
                ExportCustomerReportHeader(ref exportExcelInfo);
                NpoiExcelExtensions.RenderNoInfoMessage(exportExcelInfo, exportExcelInfo.CurrentRow);
                return;
            }

            NpoiExcelExtensions.SetColorAtIndex(exportExcelInfo, NpoiExcelExtensions.EvenRowColor, NpoiExcelExtensions.EvenRowColorIndex);

            IRow row;
            int currentColumn;
            var reportLength = customerReport.CustomerList.Count;
            var textStyle = NpoiExcelExtensions.TextStyle(
                exportExcelInfo.ExcelFile,
                NpoiExcelExtensions.BackgroundColorIndex,
                false,
                CRMHorizontalAlignment.Left);
            var centerTextStyle = NpoiExcelExtensions.TextStyle(
                exportExcelInfo.ExcelFile,
                NpoiExcelExtensions.BackgroundColorIndex,
                false,
                CRMHorizontalAlignment.Center);

            var evenRowTextStyle = NpoiExcelExtensions.TextStyle(
                exportExcelInfo.ExcelFile,
                NpoiExcelExtensions.EvenRowColorIndex,
                false,
                CRMHorizontalAlignment.Left);
            var evenRowCenterTextStyle = NpoiExcelExtensions.TextStyle(
                exportExcelInfo.ExcelFile,
                NpoiExcelExtensions.EvenRowColorIndex,
                false,
                CRMHorizontalAlignment.Center);

            var numberOfSheet = (reportLength / NpoiExcelExtensions.MaxRowPerSheet) +
                (reportLength % NpoiExcelExtensions.MaxRowPerSheet != 0 ? 1 : 0);
            for (var sheetIndex = 0; sheetIndex < numberOfSheet; sheetIndex++)
            {
                var sheetName = numberOfSheet > 1 ?
                    string.Format("{0} - {1}", Labels.CustomerReport_SheetName, sheetIndex + 1) :
                    Labels.CustomerReport_SheetName;
                exportExcelInfo.Worksheet = exportExcelInfo.ExcelFile.CreateSheet(sheetName);
                exportExcelInfo.Worksheet.VerticallyCenter = true;
                exportExcelInfo.CurrentRow = 0;
                ExportCustomerReportHeader(ref exportExcelInfo);
                var startIndex = sheetIndex * NpoiExcelExtensions.MaxRowPerSheet;
                var endIndex = startIndex + NpoiExcelExtensions.MaxRowPerSheet;
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
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private void ExportCustomerReportRow(
            ref ExportExcelInfo exportExcelInfo,
            ref IRow row,
            ref int currentColumn,
            CustomerInfo customer,
            ICellStyle centerTextStyle)
        {
            NpoiExcelExtensions.RenderCell(row, currentColumn, customer.Title.Name, centerTextStyle);
            currentColumn++;
            NpoiExcelExtensions.RenderCell(row, currentColumn, customer.Username, exportExcelInfo.TextStyle);
            currentColumn++;
            NpoiExcelExtensions.RenderCell(row, currentColumn, customer.IsVIP ? Labels.Symbol_IsVIP : string.Empty, centerTextStyle);
            currentColumn++;
            NpoiExcelExtensions.RenderCell(row, currentColumn, customer.BirthDayString, centerTextStyle);
            currentColumn++;

            NpoiExcelExtensions.RenderCell(row, currentColumn, customer.PIC1, exportExcelInfo.TextStyle);
            currentColumn++;
            NpoiExcelExtensions.RenderCell(row, currentColumn, customer.PIC2, exportExcelInfo.TextStyle);
            currentColumn++;
            NpoiExcelExtensions.RenderCell(row, currentColumn, customer.PIC3, exportExcelInfo.TextStyle);
            currentColumn++;
            NpoiExcelExtensions.RenderCell(row, currentColumn, customer.PIC4, exportExcelInfo.TextStyle);
            currentColumn++;
            NpoiExcelExtensions.RenderCell(row, currentColumn, customer.Skype, exportExcelInfo.TextStyle);
            currentColumn++;
            NpoiExcelExtensions.RenderCell(row, currentColumn, customer.Email, exportExcelInfo.TextStyle);
            currentColumn++;

            var customerLanguages = string.Join(", ", customer.Languages.Select(language => language.Name));
            NpoiExcelExtensions.RenderCell(row, currentColumn, customerLanguages, exportExcelInfo.TextStyle);
            currentColumn++;
            NpoiExcelExtensions.RenderCell(row, currentColumn, customer.SiteSetting.Name, exportExcelInfo.TextStyle);
            currentColumn++;
            exportExcelInfo.CurrentRow++;
        }

        /// <summary>
        /// Sorts the customer list.
        /// </summary>
        /// <param name="customerList">The customer list.</param>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Sorted Customer List.</returns>
        private List<CustomerInfo> SortCustomerList(
            List<CustomerInfo> customerList,
            GetCustomerReportCriteria criteria)
        {
            if (string.Equals(criteria.SortDirection, SortDirectionConstants.Asc, StringComparison.OrdinalIgnoreCase))
            {
                return SortCustomerListByAscending(customerList, criteria);
            }

            if (string.Equals(criteria.SortDirection, SortDirectionConstants.Desc, StringComparison.OrdinalIgnoreCase))
            {
                return SortCustomerListByDescending(customerList, criteria);
            }

            return customerList;
        }

        /// <summary>
        /// Sorts the customer list by ascending.
        /// </summary>
        /// <param name="customerList">The customer list.</param>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Sorted Customer List.</returns>
        [SuppressMessage("Microsoft.Maintainability",
            "CA1502:AvoidExcessiveComplexity",
            Justification = "Reviewed. Suppression is OK here.")]
        private List<CustomerInfo> SortCustomerListByAscending(
            List<CustomerInfo> customerList,
            GetCustomerReportCriteria criteria)
        {
            var sortedCustomerList = customerList;
            switch (criteria.SortField)
            {
                case "IsVIP":
                    sortedCustomerList = sortedCustomerList
                        .OrderBy(customerInfo => customerInfo.IsVIP ? 1 : 0)
                        .ToList();
                    break;
                case "IsHeadline":
                    sortedCustomerList = sortedCustomerList
                        .OrderBy(customerInfo => customerInfo.IsHeadline ? 1 : 0)
                        .ToList();
                    break;
                case "TitleName":
                    sortedCustomerList.Sort((customerA, customerB) => string.Compare(customerA.Title.Name, customerB.Title.Name, StringComparison.OrdinalIgnoreCase));
                    break;
                case "Username":
                    sortedCustomerList.Sort((customerA, customerB) => string.Compare(customerA.Username, customerB.Username, StringComparison.OrdinalIgnoreCase));
                    break;
                case "BirthDay":
                    sortedCustomerList = sortedCustomerList
                       .OrderBy(customerInfo => customerInfo.BirthDay)
                       .ToList();
                    break;
                case "Languages":
                    sortedCustomerList.Sort((customerA, customerB) =>
                        string.Compare(GetCustomerLanguages(customerA), GetCustomerLanguages(customerB), StringComparison.OrdinalIgnoreCase));
                    break;
                case "SiteSettingName":
                    sortedCustomerList.Sort((customerA, customerB) =>
                        string.Compare(customerA.SiteSetting.Name, customerB.SiteSetting.Name, StringComparison.OrdinalIgnoreCase));
                    break;
                default:
                    sortedCustomerList = customerList;
                    break;
            }

            return sortedCustomerList;
        }

        /// <summary>
        /// Sorts the customer list by descending.
        /// </summary>
        /// <param name="customerList">The customer list.</param>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Sorted Customer List.</returns>
        [SuppressMessage("Microsoft.Maintainability",
            "CA1502:AvoidExcessiveComplexity",
            Justification = "Reviewed. Suppression is OK here.")]
        private List<CustomerInfo> SortCustomerListByDescending(
            List<CustomerInfo> customerList,
            GetCustomerReportCriteria criteria)
        {
            var sortedCustomerList = customerList;
            switch (criteria.SortField)
            {
                case "IsVIP":
                    sortedCustomerList = sortedCustomerList
                        .OrderByDescending(customerInfo => customerInfo.IsVIP ? 1 : 0)
                        .ToList();
                    break;
                case "IsHeadline":
                    sortedCustomerList = sortedCustomerList
                        .OrderByDescending(customerInfo => customerInfo.IsHeadline ? 1 : 0)
                        .ToList();
                    break;
                case "TitleName":
                    sortedCustomerList.Sort((customerA, customerB) => string.Compare(customerB.Title.Name, customerA.Title.Name, StringComparison.OrdinalIgnoreCase));
                    break;
                case "Username":
                    sortedCustomerList.Sort((customerA, customerB) => string.Compare(customerB.Username, customerA.Username, StringComparison.OrdinalIgnoreCase));
                    break;
                case "BirthDay":
                    sortedCustomerList = sortedCustomerList
                       .OrderByDescending(customerInfo => customerInfo.BirthDay)
                       .ToList();
                    break;
                case "Languages":
                    sortedCustomerList.Sort((customerA, customerB) =>
                        string.Compare(GetCustomerLanguages(customerB), GetCustomerLanguages(customerA), StringComparison.OrdinalIgnoreCase));
                    break;
                case "SiteSettingName":
                    sortedCustomerList.Sort((customerA, customerB) => string.Compare(customerB.SiteSetting.Name, customerA.SiteSetting.Name, StringComparison.OrdinalIgnoreCase));
                    break;
                default:
                    sortedCustomerList = customerList;
                    break;
            }

            return sortedCustomerList;
        }

        private string GetCustomerLanguages(CustomerInfo customerInfo)
        {
            return string.Join(", ", customerInfo.Languages.Select(language => language.Name));
        }
    }
}
