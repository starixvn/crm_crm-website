// --------------------------------------------------------------------------
// <copyright file="CustomerReportService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Repositories.Implements
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Fanex.CRM.Infrastructure.Common;
    using Fanex.CRM.Infrastructure.Core;
    using Fanex.CRM.Infrastructure.Extension;
    using Fanex.CRM.Infrastructure.Logging;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Infrastructure.Service.Implements;
    using Fanex.CRM.Report.Criteria.CustomerReport;
    using Fanex.CRM.Report.Models.CustomerReport;
    using Fanex.CRM.Report.Repositories.Interfaces;
    using Fanex.CRM.Report.Resources;
    using Fanex.CRM.Setting.Constants;
    using Fanex.CRM.Setting.Models;
    using Fanex.CRM.Setting.Services.Implements;
    using Fanex.CRM.Setting.Services.Interfaces;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;

    /// <summary>
    /// Customer Report Service.
    /// </summary>
    public class CustomerReportService : ICustomerReportService
    {
        public readonly string CustomerReportSessionKey = "Fanex.CRM.Report.CustomerReport.CustomerReportSessionKey_" +
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

        /// <summary>
        /// Gets the customer report.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The customer report.</returns>
        public CustomerReportInfo GetCustomerReport(GetCustomerReportCriteria criteria)
        {
            try
            {
                CustomerReportInfo customerReport = null;
                if (criteria.IsGetFromCached)
                {
                    customerReport = CacheService.Get<CustomerReportInfo>(CustomerReportSessionKey);
                }

                if (customerReport == null)
                {
                    customerReport = new CustomerReportInfo();
                    customerReport.CustomerList = _customerReportRepository.GetCustomerReport(criteria);
                    customerReport.TotalCustomer = customerReport.CustomerList.Count;
                    if (!string.IsNullOrEmpty(criteria.SortField) &&
                        !string.IsNullOrEmpty(criteria.SortDirection))
                    {
                        customerReport.CustomerList = SortCustomerList(customerReport.CustomerList, criteria);
                    }
                }

                if (customerReport != null)
                {
                    CacheService.Set(CustomerReportSessionKey, customerReport);
                    if (!criteria.IsGetFullReport)
                    {
                        customerReport.CustomerList = customerReport.CustomerList.Skip(criteria.PageSize * criteria.PageIndex)
                           .Take(criteria.PageSize)
                           .ToList();
                    }
                }

                return customerReport;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return null;
        }

        /// <summary>
        /// Exports the customer report.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        public void ExportCustomerReport(GetCustomerReportCriteria criteria)
        {
            try
            {
                criteria.IsGetFullReport = true;
                CustomerReportInfo customerReport = CacheService.Get<CustomerReportInfo>(CustomerReportSessionKey);
                if (customerReport == null)
                {
                    customerReport = GetCustomerReport(criteria);
                }

                ExportExcelInfo exportExcelInfo = new ExportExcelInfo
                {
                    ExcelFile = new HSSFWorkbook(),
                    StartRow = 0,
                    StartColumn = 0,
                    CurrentRow = 0,
                    MaxColumnIndex = 7
                };

                ExportCustomerReportBody(ref exportExcelInfo, criteria, customerReport);
                string reportName = Labels.CustomerReport_Title;
                NpoiExcelExtensions.ResponseStreamExcel(exportExcelInfo.ExcelFile, NpoiExcelExtensions.StandardizeExcelReportName(reportName));
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
        }

        /// <summary>
        /// Gets all site.
        /// </summary>
        /// <returns>All site value.</returns>
        public List<SiteInfo> GetAllSite()
        {
            try
            {
                var allSite = _serviceSetting.GetAllSite();
                return allSite;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return new List<SiteInfo>();
        }

        /// <summary>
        /// Gets all title.
        /// </summary>
        /// <returns>All title.</returns>
        public List<SettingInfo> GetAllTitle()
        {
            try
            {
                var allTitle = _serviceSetting.GetAllTitle();
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
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return new List<SettingInfo>();
        }

        /// <summary>
        /// Gets all country.
        /// </summary>
        /// <returns>All country.</returns>
        public List<CountryInfo> GetAllCountry()
        {
            try
            {
                var allCountry = _serviceSetting.GetAllCountry();
                return allCountry;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return new List<CountryInfo>();
        }

        /// <summary>
        /// Gets all language.
        /// </summary>
        /// <returns>All language.</returns>
        public List<SettingInfo> GetAllLanguage()
        {
            try
            {
                var allLanguage = _serviceSetting.GetAllLanguage();
                return allLanguage;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return new List<SettingInfo>();
        }

        /// <summary>
        /// Gets all customer type.
        /// </summary>
        /// <returns>All customer type.</returns>
        public List<SettingInfo> GetAllCustomerType()
        {
            try
            {
                var allCustomerType = _serviceSetting.GetAllCustomerType();
                return allCustomerType;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return new List<SettingInfo>();
        }

        /// <summary>
        /// Gets all VIP type.
        /// </summary>
        /// <returns>All VIP type.</returns>
        public List<SettingInfo> GetAllVIPType()
        {
            try
            {
                var allVIPType = _serviceSetting.GetAllVIPType();
                return allVIPType;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return new List<SettingInfo>();
        }

        /// <summary>
        /// Exports the customer report header.
        /// </summary>
        /// <param name="exportExcelInfo">The export excel information.</param>
        private void ExportCustomerReportHeader(ref ExportExcelInfo exportExcelInfo)
        {
            ICellStyle headerStyle = NpoiExcelExtensions.HeaderStyle(exportExcelInfo.ExcelFile);
            var currentColumn = exportExcelInfo.StartColumn;
            exportExcelInfo.Worksheet.SetColumnWidth(currentColumn++, 80 * NpoiExcelExtensions.FontWidth);
            exportExcelInfo.Worksheet.SetColumnWidth(currentColumn++, 160 * NpoiExcelExtensions.FontWidth);
            exportExcelInfo.Worksheet.SetColumnWidth(currentColumn++, 160 * NpoiExcelExtensions.FontWidth);
            exportExcelInfo.Worksheet.SetColumnWidth(currentColumn++, 80 * NpoiExcelExtensions.FontWidth);
            exportExcelInfo.Worksheet.SetColumnWidth(currentColumn++, 160 * NpoiExcelExtensions.FontWidth);
            exportExcelInfo.Worksheet.SetColumnWidth(currentColumn++, 400 * NpoiExcelExtensions.FontWidth);
            exportExcelInfo.Worksheet.SetColumnWidth(currentColumn++, 160 * NpoiExcelExtensions.FontWidth);
            exportExcelInfo.Worksheet.SetColumnWidth(currentColumn++, 140 * NpoiExcelExtensions.FontWidth);

            string reportTitle = Labels.CustomerReport_Title;
            CellRangeAddress cellRangeAddress;
            cellRangeAddress = new CellRangeAddress(
                exportExcelInfo.CurrentRow,
                exportExcelInfo.CurrentRow + 1,
                exportExcelInfo.StartColumn,
                exportExcelInfo.MaxColumnIndex);
            NpoiExcelExtensions.RenderMergedCells(ref exportExcelInfo, cellRangeAddress, reportTitle, headerStyle);
            exportExcelInfo.CurrentRow += 2;

            NpoiExcelExtensions.RenderEmptyRow(ref exportExcelInfo, exportExcelInfo.CurrentRow);
            exportExcelInfo.CurrentRow++;

            IRow row = exportExcelInfo.Worksheet.CreateRow(exportExcelInfo.CurrentRow);
            currentColumn = exportExcelInfo.StartColumn;
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, Labels.Title, headerStyle);
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, Labels.First_Name, headerStyle);
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, Labels.Last_Name, headerStyle);
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, Labels.VIP, headerStyle);
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, Labels.Country, headerStyle);
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, Labels.Language, headerStyle);
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, Labels.Site, headerStyle);
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, Labels.Birthday, headerStyle);
            exportExcelInfo.CurrentRow++;
        }

        /// <summary>
        /// Exports the dynamic report body.
        /// </summary>
        /// <param name="exportExcelInfo">The export excel information.</param>
        /// <param name="criteria">The criteria.</param>
        /// <param name="customerReport">The customer report.</param>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private void ExportCustomerReportBody(
            ref ExportExcelInfo exportExcelInfo,
            GetCustomerReportCriteria criteria,
            CustomerReportInfo customerReport)
        {
            if (customerReport == null || customerReport.CustomerList.Count == 0)
            {
                exportExcelInfo.Worksheet = exportExcelInfo.ExcelFile.CreateSheet(Labels.CustomerReport_SheetName);
                ExportCustomerReportHeader(ref exportExcelInfo);
                NpoiExcelExtensions.RenderNoInfoMessage(ref exportExcelInfo, exportExcelInfo.CurrentRow);
                return;
            }

            IRow row;
            int currentColumn;
            var reportLength = customerReport.CustomerList.Count;
            exportExcelInfo.TextStyle = NpoiExcelExtensions.TextStyle(
                exportExcelInfo.ExcelFile,
                NpoiExcelExtensions.BackgroundColorIndex,
                false,
                NpoiExcelExtensions.Alignment.Left);
            var centerTextStyle = NpoiExcelExtensions.TextStyle(
                exportExcelInfo.ExcelFile,
                NpoiExcelExtensions.BackgroundColorIndex,
                false,
                NpoiExcelExtensions.Alignment.Center);

            var numberOfSheet = (reportLength / NpoiExcelExtensions.MaxRowPerSheet) +
                (reportLength % NpoiExcelExtensions.MaxRowPerSheet != 0 ? 1 : 0);
            for (var sheetIndex = 0; sheetIndex < numberOfSheet; sheetIndex++)
            {
                var sheetName = numberOfSheet > 1 ?
                    string.Format("{0} - {1}", Labels.CustomerReport_SheetName, sheetIndex + 1) :
                    Labels.CustomerReport_SheetName;
                exportExcelInfo.Worksheet = exportExcelInfo.ExcelFile.CreateSheet(sheetName);
                exportExcelInfo.CurrentRow = 0;
                ExportCustomerReportHeader(ref exportExcelInfo);
                var startIndex = sheetIndex * NpoiExcelExtensions.MaxRowPerSheet;
                var endIndex = startIndex + NpoiExcelExtensions.MaxRowPerSheet;
                for (var i = startIndex; i < endIndex && i < reportLength; i++)
                {
                    row = exportExcelInfo.Worksheet.CreateRow(exportExcelInfo.CurrentRow);
                    currentColumn = exportExcelInfo.StartColumn;
                    var customer = customerReport.CustomerList[i];
                    ExportCustomerReportRow(ref exportExcelInfo, ref row, ref currentColumn, customer, centerTextStyle);
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
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, customer.Title.Name, centerTextStyle);
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, customer.FirstName, exportExcelInfo.TextStyle);
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, customer.LastName, exportExcelInfo.TextStyle);
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, customer.IsVIP ? Labels.Symbol_IsVIP : string.Empty, centerTextStyle);
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, customer.Country.Name, exportExcelInfo.TextStyle);

            var customerLanguages = string.Join(", ", customer.Languages.Select(language => language.Name));
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, customerLanguages, exportExcelInfo.TextStyle);
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, customer.SiteSetting.Name, exportExcelInfo.TextStyle);
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, customer.BirthDayString, centerTextStyle);
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
            if (string.Equals(criteria.SortDirection, SortDirectionConstants.ASC, StringComparison.OrdinalIgnoreCase))
            {
                return SortCustomerListByAscending(customerList, criteria);
            }

            if (string.Equals(criteria.SortDirection, SortDirectionConstants.DESC, StringComparison.OrdinalIgnoreCase))
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
                case "TitleName":
                    sortedCustomerList.Sort((customerA, customerB) => string.Compare(customerA.Title.Name, customerB.Title.Name, true));
                    break;
                case "FirstName":
                    sortedCustomerList.Sort((customerA, customerB) => string.Compare(customerA.FirstName, customerB.FirstName, true));
                    break;
                case "LastName":
                    sortedCustomerList.Sort((customerA, customerB) => string.Compare(customerA.LastName, customerB.LastName, true));
                    break;
                case "CountryName":
                    sortedCustomerList.Sort((customerA, customerB) => string.Compare(customerA.Country.Name, customerB.Country.Name, true));
                    break;
                case "Languages":
                    sortedCustomerList.Sort((customerA, customerB) => string.Compare(GetCustomerLanguages(customerA), GetCustomerLanguages(customerB), true));
                    break;
                case "SiteSettingName":
                    sortedCustomerList.Sort((customerA, customerB) => string.Compare(customerA.SiteSetting.Name, customerB.SiteSetting.Name, true));
                    break;
                case "BirthDay":
                    sortedCustomerList = sortedCustomerList
                       .OrderBy(customerInfo => customerInfo.BirthDay)
                       .ToList();
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
                case "TitleName":
                    sortedCustomerList.Sort((customerA, customerB) => string.Compare(customerB.Title.Name, customerA.Title.Name, true));
                    break;
                case "FirstName":
                    sortedCustomerList.Sort((customerA, customerB) => string.Compare(customerB.FirstName, customerA.FirstName, true));
                    break;
                case "LastName":
                    sortedCustomerList.Sort((customerA, customerB) => string.Compare(customerB.LastName, customerA.LastName, true));
                    break;
                case "CountryName":
                    sortedCustomerList.Sort((customerA, customerB) => string.Compare(customerB.Country.Name, customerA.Country.Name, true));
                    break;
                case "Languages":
                    sortedCustomerList.Sort((customerA, customerB) => string.Compare(GetCustomerLanguages(customerB), GetCustomerLanguages(customerA), true));
                    break;
                case "SiteSettingName":
                    sortedCustomerList.Sort((customerA, customerB) => string.Compare(customerB.SiteSetting.Name, customerA.SiteSetting.Name, true));
                    break;
                case "BirthDay":
                    sortedCustomerList = sortedCustomerList
                       .OrderByDescending(customerInfo => customerInfo.BirthDay)
                       .ToList();
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
