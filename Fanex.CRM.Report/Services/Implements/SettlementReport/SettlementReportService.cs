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
    using Fanex.CRM.Report.Models.SettlementReport;
    using Fanex.CRM.Report.Resources;
    using Fanex.CRM.Report.Services.Interfaces;
    using Fanex.CRM.Setting.Models;
    using Fanex.CRM.Report.Entities.SettlementReport;

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
                    //var rawData = GetDynamicData<SettlementReportEntity>(criteria);
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
                    //settlementReport.ChartInfo = GetChartInfo(settlementReport, countryList, countryDictionary);
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
    }
}
