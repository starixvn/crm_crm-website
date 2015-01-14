// --------------------------------------------------------------------------
// <copyright file="GetWagerReportCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.03.28">Created</change>
// </history>

namespace Fanex.CRM.Report.Criteria.WagerReport
{
    using System;
    using System.Collections;
    using Fanex.CRM.Common.Helpers;
    using Fanex.Data.Repository;

    /// <summary>
    /// GetWagerReportCriteria criteria.
    /// </summary>
    public class GetWagerReportCriteria : CriteriaBase
    {
        [SpParam(Ignored = true)]
        public int Year { get; set; }

        [SpParam(Ignored = true)]
        public bool IsCached { get; set; }

        public override string GetSettingKey()
        {
            return "Report_WagerReport_GetReport";
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
                { "ToDate", DateTimeUtilize.ChangeClientToTimeToServerTime(toDate) }
            };

            return inputParams;
        }
    }
}
