// --------------------------------------------------------------------------
// <copyright file="GetDetailReportCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Criteria.WeeklyStatisticsReport
{
    using System;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get Detail Report Criteria.
    /// </summary>
    public class GetDetailReportCriteria : PagingCriteria
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public int SiteType { get; set; }

        [SpParam(Ignored = true)]
        public bool IsGetFullReport { get; set; }

        public override string GetSettingKey()
        {
            return "dailydetailreport";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
