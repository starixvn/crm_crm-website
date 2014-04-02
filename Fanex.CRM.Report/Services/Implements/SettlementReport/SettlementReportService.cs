// --------------------------------------------------------------------------
// <copyright file="SettlementReportService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Services.Implements.SettlementReport
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web;
    using Fanex.CRM.Infrastructure.Common;
    using Fanex.CRM.Infrastructure.Extension;
    using Fanex.CRM.Infrastructure.Logging;
    using Fanex.CRM.Infrastructure.Service.Implements;
    using Fanex.CRM.Report.Criteria.SettlementReport;
    using Fanex.CRM.Report.Entities.SettlementReport;
    using Fanex.CRM.Report.Models.SettlementReport;
    using Fanex.CRM.Report.Resources;
    using Fanex.CRM.Report.Services.Interfaces;
    using Fanex.CRM.Setting.Models;
    using Fanex.CRM.Infrastructure.Models;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;

    /// <summary>
    /// Settlement Report Service.
    /// </summary>
    public class SettlementReportService : ServiceBase, ISettlementReportService
    {
        public readonly string SettlementReportSessionKey = "Fanex.CRM.Report.SettlementReport.SettlementReportSessionKey_" +
            (AppSession.CurrentUser != null ? AppSession.CurrentUser.Id : 0);

        /// <summary>
        /// Gets the settlement report.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The settlement report.</returns>      
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public SettlementReportInfo GetSettlementReport(GeSettlementReportCriteria criteria)
        {
            try
            {
                SettlementReportInfo settlementReport = null;
                if (criteria.IsGetFromCached)
                {
                    settlementReport = (SettlementReportInfo)SessionExtension.GetSession(HttpContext.Current.Session, SettlementReportSessionKey);
                }

                if (settlementReport == null)
                {
                    // var rawData = GetDynamicData<SettlementReportEntity>(criteria);
                    var rawData = new DummyData().Get();
                    var countryList = rawData
                        .Select(settlementReportEntity => new CountryInfo
                        {
                            Id = settlementReportEntity.CountryId,
                            Name = settlementReportEntity.CountryName
                        })
                        .ToList();
                    var countryDictionary = GetCountryDictionary(countryList);
                    settlementReport = MappingDataSettlementReport(rawData, countryList, countryDictionary, criteria);

                    settlementReport.TotalChildReports = settlementReport.ChildReports.Count;
                    settlementReport.ChartInfo = GetChartInfo(settlementReport);
                    SessionExtension.AddSession(HttpContext.Current.Session, SettlementReportSessionKey, settlementReport);
                }

                return new SettlementReportInfo
                {
                    Name = settlementReport.Name,
                    ChildReports = criteria.IsGetFullReport ?
                        settlementReport.ChildReports :
                        GetPagingLogList(criteria, settlementReport.ChildReports),
                    Total = settlementReport.Total,
                    TotalChildReports = settlementReport.TotalChildReports,
                    ChartInfo = settlementReport.ChartInfo
                };
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        /// <summary>
        /// Exports the settlement report.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        public void ExportSettlementReport(GeSettlementReportCriteria criteria)
        {
            try
            {
                SettlementReportInfo settlementReport = (SettlementReportInfo)SessionExtension.GetSession(HttpContext.Current.Session, SettlementReportSessionKey);
                criteria.IsGetFullReport = true;
                if (settlementReport == null)
                {
                    settlementReport = GetSettlementReport(criteria);
                }

                ExportExcelInfoNPOI exportExcelInfo = new ExportExcelInfoNPOI
                {
                    ExcelFile = new HSSFWorkbook(),
                    StartRow = 0,
                    StartColumn = 0,
                    CurrentRow = 0,
                    MaxColumnIndex = 3
                };

                ExportSettlementReportBody(ref exportExcelInfo, criteria, settlementReport);
                string reportName = string.Format(Labels.SettlementReport_ExcelTitle, criteria.Year);
                NpoiExcelExtensions.ResponseStreamExcel(exportExcelInfo.ExcelFile, reportName);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
        }

        /// <summary>
        /// Gets the paging log list.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="settlementReportList">The settlement report list.</param>
        /// <returns>
        /// The paging log list.
        /// </returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private List<SettlementReportInfo> GetPagingLogList(
            GeSettlementReportCriteria criteria,
            List<SettlementReportInfo> settlementReportList)
        {
            return settlementReportList.Skip(criteria.PageSize * criteria.PageIndex)
                .Take(criteria.PageSize)
                .ToList();
        }

        /// <summary>
        /// Gets the country dictionary.
        /// </summary>
        /// <param name="countryList">The country list.</param>
        /// <returns>The country dictionary.</returns>
        private Dictionary<string, int> GetCountryDictionary(List<CountryInfo> countryList)
        {
            Dictionary<string, int> countryDictionary = new Dictionary<string, int>();
            var countryLength = countryList.Count;
            var currentIndex = 0;
            for (var i = 0; i < countryLength; i++)
            {
                var country = countryList[i];
                if (countryDictionary.ContainsKey(country.Name))
                {
                    continue;
                }

                countryDictionary.Add(country.Name, currentIndex++);
            }

            return countryDictionary;
        }

        /// <summary>
        /// Gets the dummy country list.
        /// </summary>
        /// <param name="countryLength">Length of the country.</param>
        /// <param name="countryList">The country list.</param>
        /// <returns>The dummy country list.</returns>
        private List<SettlementReportInfo> GetDummyCountryList(Dictionary<string, int> countryDictionary)
        {
            var countryLength = countryDictionary.Count;
            List<SettlementReportInfo> dummyCountries = new List<SettlementReportInfo>();
            foreach (var keyPair in countryDictionary)
            {
                dummyCountries.Add(new SettlementReportInfo
                {
                    Name = keyPair.Key
                });
            }

            return dummyCountries;
        }

        /// <summary>
        /// Gets the statistic report by daily.
        /// </summary>
        /// <param name="rawData">The raw data.</param>
        /// <param name="countryList">The country list.</param>
        /// <param name="countryDictionary">The country dictionary.</param>
        /// <param name="criteria">The criteria.</param>
        /// <returns>
        /// The statistic report by daily.
        /// </returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private SettlementReportInfo MappingDataSettlementReport(
            List<SettlementReportEntity> rawData,
            List<CountryInfo> countryList,
            Dictionary<string, int> countryDictionary,
            GeSettlementReportCriteria criteria)
        {
            var settlementReport = new SettlementReportInfo
            {
                ChildReports = new List<SettlementReportInfo>(),
                Name = string.Format(
                    Labels.SettlementReport_Title,
                    criteria.Year),
                Total = new SettlementReportInfo
                {
                    Name = Labels.Grand_Total,
                    ChildReports = GetDummyCountryList(countryDictionary)
                }
            };

            if (rawData == null ||
                rawData.Count == 0)
            {
                return settlementReport;
            }

            var monthGroups = rawData.GroupBy(data => data.Month);
            foreach (var monthGroup in monthGroups)
            {
                var monthReport = new SettlementReportInfo
                {
                    Name = new DateTime(criteria.Year, monthGroup.Key, 1).ToMonth(),
                    ChildReports = new List<SettlementReportInfo>()
                };

                foreach (var monthDetail in monthGroup)
                {
                    var countryReport = new SettlementReportInfo
                    {
                        Id = monthDetail.CountryId,
                        Name = monthDetail.CountryName,
                        NumberOfIssues = monthDetail.NumberOfIssues
                    };

                    monthReport.ChildReports.Add(countryReport);
                    monthReport.NumberOfIssues += countryReport.NumberOfIssues;

                    var countryIndex = countryDictionary[monthDetail.CountryName];
                    settlementReport.Total.ChildReports[countryIndex].Id = countryReport.Id;
                    settlementReport.Total.ChildReports[countryIndex].NumberOfIssues += countryReport.NumberOfIssues;
                    settlementReport.Total.NumberOfIssues += countryReport.NumberOfIssues;
                }

                settlementReport.ChildReports.Add(monthReport);
            }

            return settlementReport;
        }

        /// <summary>
        /// Gets the chart information.
        /// </summary>
        /// <param name="statisticReport">The statistic report.</param>
        /// <param name="countryList">The country list.</param>
        /// <param name="countryDictionary">The country dictionary.</param>
        /// <returns>The chart information.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private SettlementReportInfo GetChartInfo(SettlementReportInfo settlementReport)
        {
            var chartInfo = new SettlementReportInfo
            {
                Name = settlementReport.Name,
                NumberOfIssues = settlementReport.Total.NumberOfIssues,
                ChildReports = new List<SettlementReportInfo>()
            };

            var monthReportLength = settlementReport.ChildReports.Count;
            for (var monthIndex = 0; monthIndex < monthReportLength; monthIndex++)
            {
                var monthReport = settlementReport.ChildReports[monthIndex];
                var monthChart = new SettlementReportInfo
                {
                    Name = monthReport.Name,
                    NumberOfIssues = monthReport.NumberOfIssues,
                    ChildReports = new List<SettlementReportInfo>()
                };

                var countryReportLength = monthReport.ChildReports.Count;
                for (var countryIndex = 0; countryIndex < countryReportLength; countryIndex++)
                {
                    var countryReport = monthReport.ChildReports[countryIndex];
                    var timeChart = new SettlementReportInfo
                    {
                        Name = countryReport.Name,
                        NumberOfIssues = countryReport.NumberOfIssues,
                        ChildReports = new List<SettlementReportInfo>()
                    };

                    monthChart.ChildReports.Add(timeChart);
                }

                chartInfo.ChildReports.Add(monthChart);
            }

            return chartInfo;
        }

        /// <summary>
        /// Exports the settlement report body.
        /// </summary>
        /// <param name="exportExcelInfo">The export excel information.</param>
        /// <param name="criteria">The criteria.</param>
        /// <param name="statisticReport">The settlement report.</param>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
          "SP2101:MethodMustNotContainMoreLinesThan",
          Justification = "Reviewed. Suppression is OK here.")]
        private void ExportSettlementReportBody(
            ref ExportExcelInfoNPOI exportExcelInfo,
            GeSettlementReportCriteria criteria,
            SettlementReportInfo settlementReport)
        {
            if (settlementReport == null || settlementReport.ChildReports.Count == 0)
            {
                exportExcelInfo.Worksheet = exportExcelInfo.ExcelFile.CreateSheet(Labels.SettlementReport_SheetName);
                ExportSettlementReportHeader(ref exportExcelInfo, criteria, settlementReport);
                NpoiExcelExtensions.RenderNoInfoMessage(ref exportExcelInfo, exportExcelInfo.CurrentRow);
                return;
            }

            var monthReportLength = settlementReport.ChildReports.Count;
            var normalRowWriter = new NormalRowExcelWriter(exportExcelInfo);
            //var firstGrandTotalRowWriter = new FirstGrandTotalRowExcelWriter(exportExcelInfo);
            //var grandTotalRowWriter = new FirstGrandTotalRowExcelWriter(exportExcelInfo);
            var maxRowPerSheet = NpoiExcelExtensions.MaxRowPerSheet;
            var numberOfSheet = (monthReportLength / maxRowPerSheet) +
                (monthReportLength % maxRowPerSheet != 0 ? 1 : 0);
            for (var sheetIndex = 0; sheetIndex < numberOfSheet; sheetIndex++)
            {
                var sheetName = numberOfSheet > 1 ?
                       string.Format("{0} - {1}", Labels.SettlementReport_SheetName, sheetIndex + 1) :
                       Labels.SettlementReport_SheetName;
                exportExcelInfo.Worksheet = exportExcelInfo.ExcelFile.CreateSheet(sheetName);
                exportExcelInfo.CurrentRow = 0;
                ExportSettlementReportHeader(ref exportExcelInfo, criteria, settlementReport);
                var startIndex = sheetIndex * maxRowPerSheet;
                var endIndex = startIndex + maxRowPerSheet;
                for (var i = startIndex; i < endIndex && i < monthReportLength; i++)
                {
                    var monthReport = settlementReport.ChildReports[i];
                    var countryReportLength = monthReport.ChildReports.Count;
                    for (var j = 0; j < countryReportLength; j++)
                    {
                        exportExcelInfo.CurrentColumn = exportExcelInfo.StartColumn;
                        var countryReport = monthReport.ChildReports[j];
                        normalRowWriter.WriteRow(ref exportExcelInfo, countryReport, monthReport, j);
                    }
                }
            }
        }

        /// <summary>
        /// Exports the settlement report header.
        /// </summary>
        /// <param name="exportExcelInfo">The export excel information.</param>
        /// <param name="criteria">The criteria.</param>
        /// <param name="statisticReport">The settlement report.</param>
        private void ExportSettlementReportHeader(
            ref ExportExcelInfoNPOI exportExcelInfo,
            GeSettlementReportCriteria criteria,
            SettlementReportInfo settlementReport)
        {
            NpoiExcelExtensions.SetHeaderColorIndex(ref exportExcelInfo);
            ICellStyle headerStyle = NpoiExcelExtensions.HeaderStyle(
                exportExcelInfo.ExcelFile,
                new ExcelStyleParamNPOI());

            var currentColumn = exportExcelInfo.StartColumn;
            exportExcelInfo.Worksheet.SetColumnWidth(currentColumn++, 140 * NpoiExcelExtensions.FontWidth);
            exportExcelInfo.Worksheet.SetColumnWidth(currentColumn++, 200 * NpoiExcelExtensions.FontWidth);
            exportExcelInfo.Worksheet.SetColumnWidth(currentColumn++, 180 * NpoiExcelExtensions.FontWidth);
            exportExcelInfo.Worksheet.SetColumnWidth(currentColumn++, 180 * NpoiExcelExtensions.FontWidth);

            string reportTitle = settlementReport != null ?
                settlementReport.Name :
                string.Format(Labels.SettlementReport_Title, criteria.Year);
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
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, Labels.Month, headerStyle);
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, Labels.Country, headerStyle);
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, Labels.Amount, headerStyle);
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, Labels.Total, headerStyle);
            exportExcelInfo.CurrentRow++;
        }
    }
}
