namespace Fanex.CRM.Report.Services.SettlementReport
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CRM.Common.Extensions;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Common.Models.Npoi;
    using Fanex.CRM.Common.Services;
    using Fanex.CRM.Infrastructure.Extensions;
    using Fanex.CRM.Report.Criteria.SettlementReport;
    using Fanex.CRM.Report.Entities.SettlementReport;
    using Fanex.CRM.Report.Models.SettlementReport;
    using Fanex.CRM.Report.Resources;
    using Fanex.CRM.Setting.Constants;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;

    public class SettlementReportService : ISettlementReportService
    {
        private readonly IBaseService baseService;

        public SettlementReportService() : this(null)
        {
        }

        public SettlementReportService(IBaseService baseService)
        {
            this.baseService = baseService ?? new BaseService();
        }

        public SettlementReportInfo GetSettlementReport(GetSettlementReportCriteria criteria)
        {
            var rawData = baseService.GetData<SettlementReportEntity>(criteria).ToList();
            var countryList = rawData
               .Select(settlementReportEntity => new Country
               {
                   Id = settlementReportEntity.CountryId,
                   Name = settlementReportEntity.CountryName
               })
               .OrderBy(country => country.Id)
               .ToList();
            var countryDictionary = GetCountryDictionary(countryList);
            var settlementReport = MappingDataSettlementReport(rawData, countryDictionary, criteria);

            settlementReport.TotalChildReports = settlementReport.ChildReports.Count;
            settlementReport.ChartInfo = GetChartInfo(settlementReport);

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

        public void ExportSettlementReport(GetSettlementReportCriteria criteria)
        {
            criteria.IsGetFullReport = true;
            var settlementReport = GetSettlementReport(criteria);

            ExportExcelInfo exportExcelInfo = new ExportExcelInfo
            {
                ExcelFile = new HSSFWorkbook(),
                MaxColumnIndex = 3
            };

            ExportSettlementReportBody(ref exportExcelInfo, criteria, settlementReport);
            string reportName = string.Format(Labels.SettlementReport_ExcelTitle, criteria.Year);
            NpoiExtensions.ResponseStreamExcel(exportExcelInfo.ExcelFile, reportName);
        }

        private List<SettlementReportInfo> GetPagingLogList(
            GetSettlementReportCriteria criteria,
            List<SettlementReportInfo> settlementReportList)
        {
            return settlementReportList.Skip(criteria.PageSize * criteria.PageIndex)
                .Take(criteria.PageSize)
                .ToList();
        }

        private Dictionary<int, int> GetCountryDictionary(List<Country> countryList)
        {
            Dictionary<int, int> countryDictionary = new Dictionary<int, int>();
            var countryLength = countryList.Count;
            var currentIndex = 0;
            for (var i = 0; i < countryLength; i++)
            {
                var country = countryList[i];
                if (countryDictionary.ContainsKey(country.Id))
                {
                    continue;
                }

                countryDictionary.Add(country.Id, currentIndex++);
            }

            return countryDictionary;
        }

        private List<SettlementReportInfo> GetDummyCountryList(Dictionary<int, int> countryDictionary)
        {
            List<SettlementReportInfo> dummyCountries = new List<SettlementReportInfo>();
            foreach (var keyPair in countryDictionary)
            {
                dummyCountries.Add(new SettlementReportInfo
                {
                    Id = keyPair.Key
                });
            }

            return dummyCountries;
        }

        private SettlementReportInfo MappingDataSettlementReport(
            List<SettlementReportEntity> rawData,
            Dictionary<int, int> countryDictionary,
            GetSettlementReportCriteria criteria)
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

            if (rawData == null || rawData.Count == 0)
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
                        Name = monthDetail.CountryId == SettingConstants.NotApply ?
                            Labels.Symbol_NotApply :
                            monthDetail.CountryName,
                        NumberOfIssues = monthDetail.NumberOfIssues
                    };

                    monthReport.ChildReports.Add(countryReport);
                    monthReport.NumberOfIssues += countryReport.NumberOfIssues;

                    var countryIndex = countryDictionary[monthDetail.CountryId];
                    settlementReport.Total.ChildReports[countryIndex].Id = countryReport.Id;
                    if (string.IsNullOrEmpty(settlementReport.Total.ChildReports[countryIndex].Name))
                    {
                        settlementReport.Total.ChildReports[countryIndex].Name = countryReport.Name;
                    }

                    settlementReport.Total.ChildReports[countryIndex].NumberOfIssues += countryReport.NumberOfIssues;
                    settlementReport.Total.NumberOfIssues += countryReport.NumberOfIssues;
                }

                settlementReport.ChildReports.Add(monthReport);
            }

            return settlementReport;
        }

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

        private void ExportSettlementReportBody(
            ref ExportExcelInfo exportExcelInfo,
            GetSettlementReportCriteria criteria,
            SettlementReportInfo settlementReport)
        {
            if (settlementReport == null || settlementReport.ChildReports.Count == 0)
            {
                exportExcelInfo.Worksheet = exportExcelInfo.ExcelFile.CreateSheet(Labels.SettlementReport_SheetName);
                ExportSettlementReportHeader(ref exportExcelInfo, criteria, settlementReport);
                NpoiExtensions.RenderNoInfoMessage(exportExcelInfo, exportExcelInfo.CurrentRow);
                return;
            }

            var monthReportLength = settlementReport.ChildReports.Count;
            NpoiExtensions.SetColorAtIndex(exportExcelInfo, NpoiExtensions.EvenRowColor, NpoiExtensions.EvenRowColorIndex);
            NpoiExtensions.SetGrandTotalColorIndex(exportExcelInfo);

            var oddRowWriter = new OddEvenRowExcelWriter(exportExcelInfo, false);
            var firstGrandTotalRowWriter = new GrandTotalRowExcelWriter(exportExcelInfo, true);
            var grandTotalRowWriter = new GrandTotalRowExcelWriter(exportExcelInfo, false);
            var maxRowPerSheet = NpoiExtensions.MaxRowPerSheet;
            var numberOfSheet = (monthReportLength / maxRowPerSheet) +
                (monthReportLength % maxRowPerSheet != 0 ? 1 : 0);
            int countryReportLength;
            SettlementReportInfo countryReport;
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
                    countryReportLength = monthReport.ChildReports.Count;
                    for (var j = 0; j < countryReportLength; j++)
                    {
                        exportExcelInfo.CurrentColumn = exportExcelInfo.StartColumn;
                        countryReport = monthReport.ChildReports[j];
                        oddRowWriter.WriteRow(exportExcelInfo, countryReport, monthReport, j);
                    }

                    exportExcelInfo.Worksheet.GroupRow(exportExcelInfo.CurrentRow - countryReportLength, exportExcelInfo.CurrentRow - 2);
                }

                var total = settlementReport.Total;
                countryReportLength = total.ChildReports.Count;
                for (var j = 0; j < countryReportLength; j++)
                {
                    countryReport = total.ChildReports[j];
                    var rowWriter = j == 0 ? firstGrandTotalRowWriter : grandTotalRowWriter;
                    rowWriter.WriteRow(exportExcelInfo, countryReport, total);
                }
            }
        }

        private void ExportSettlementReportHeader(
            ref ExportExcelInfo exportExcelInfo,
            GetSettlementReportCriteria criteria,
            SettlementReportInfo settlementReport)
        {
            NpoiExtensions.SetHeaderColorIndex(exportExcelInfo);
            ICellStyle headerStyle = NpoiExtensions.GetHeaderStyle(
                exportExcelInfo.ExcelFile,
                new ExcelStyleParam());

            var currentColumn = exportExcelInfo.StartColumn;
            exportExcelInfo.Worksheet.SetColumnWidth(currentColumn++, 140 * NpoiExtensions.FontWidth);
            exportExcelInfo.Worksheet.SetColumnWidth(currentColumn++, 200 * NpoiExtensions.FontWidth);
            exportExcelInfo.Worksheet.SetColumnWidth(currentColumn++, 180 * NpoiExtensions.FontWidth);
            exportExcelInfo.Worksheet.SetColumnWidth(currentColumn++, 180 * NpoiExtensions.FontWidth);

            string reportTitle = settlementReport != null ?
                settlementReport.Name :
                string.Format(Labels.SettlementReport_Title, criteria.Year);
            exportExcelInfo.RenderHeaderTitle(reportTitle, headerStyle);

            IRow row = exportExcelInfo.Worksheet.CreateRow(exportExcelInfo.CurrentRow);
            currentColumn = exportExcelInfo.StartColumn;
            NpoiExtensions.RenderCell(row, currentColumn, Labels.Month, headerStyle);
            currentColumn++;
            NpoiExtensions.RenderCell(row, currentColumn, Labels.Country, headerStyle);
            currentColumn++;
            NpoiExtensions.RenderCell(row, currentColumn, Labels.Amount, headerStyle);
            currentColumn++;
            NpoiExtensions.RenderCell(row, currentColumn, Labels.Total, headerStyle);
            currentColumn++;
            exportExcelInfo.CurrentRow++;
        }
    }
}