// --------------------------------------------------------------------------
// <copyright file="GetMainReportCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Criteria.StatisticByDateReport
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Get Main Report Criteria.
    /// </summary>
    public class GetMainReportCriteria : CriteriaBase
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public int SiteType { get; set; }

        public override string GetSettingKey()
        {
            return "dailysummaryreport";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
