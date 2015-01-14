// --------------------------------------------------------------------------
// <copyright file="GetSummaryReportCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.04.08">Created</change>
// </history>

namespace Fanex.CRM.Report.Criteria.SummaryReport
{
    using System;
    using System.Collections;
    using Fanex.CRM.Common.Helpers;
    using Fanex.Data.Repository;

    /// <summary>
    /// GetSummaryReportCriteria criteria.
    /// </summary>
    public class GetSummaryReportCriteria : CriteriaBase
    {
        [SpParam(Ignored = true)]
        public int Year { get; set; }

        public int SiteType { get; set; }

        [SpParam(Ignored = true)]
        public string Categories { get; set; }

        [SpParam(Ignored = true)]
        public bool IsCached { get; set; }

        public override string GetSettingKey()
        {
            return "Report_SummaryReport_GetReport";
        }

        public override bool IsValid()
        {
            return Year > 0;
        }

        /// <summary>
        /// Gets the stored procedure parameter.
        /// </summary>
        /// <returns>The hash table.</returns>
        public override Hashtable GetSpParams()
        {
            var fromDate = new DateTime(Year, 1, 1);
            var toDate = new DateTime(Year, 12, 31, 23, 59, 59);

            Hashtable inputParams = new Hashtable 
            {
                { "FromDate", DateTimeUtilize.ChangeClientFromTimeToServerTime(fromDate) },
                { "ToDate", DateTimeUtilize.ChangeClientToTimeToServerTime(toDate) },
                { "SiteType", SiteType }
            };

            return inputParams;
        }
    }
}
