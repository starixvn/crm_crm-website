// --------------------------------------------------------------------------
// <copyright file="GetSettlementReportCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Criteria.SettlementReport
{
    using System;
    using System.Collections;
    using Fanex.CRM.Common.Helpers;
    using Fanex.Data.Repository;

    /// <summary>
    /// Get Settlement Report Criteria.
    /// </summary>
    public class GetSettlementReportCriteria : CriteriaBase
    {
        [SpParam(Ignored = true)]
        public int Year { get; set; }

        [SpParam(Ignored = true)]
        public int PageIndex { get; set; }

        [SpParam(Ignored = true)]
        public int PageSize { get; set; }

        [SpParam(Ignored = true)]
        public bool IsGetFromCached { get; set; }

        [SpParam(Ignored = true)]
        public bool IsGetFullReport { get; set; }

        public override string GetSettingKey()
        {
            return "Report_SettlementReport_GetReport";
        }

        public override bool IsValid()
        {
            return true;
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
                { "ToDate", DateTimeUtilize.ChangeClientToTimeToServerTime(toDate) }
            };

            return inputParams;
        }
    }
}
