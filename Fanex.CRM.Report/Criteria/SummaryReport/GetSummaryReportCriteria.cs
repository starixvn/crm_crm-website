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
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// GetSummaryReportCriteria criteria.
    /// </summary>
    public class GetSummaryReportCriteria : CriteriaBase
    {
        public int Year { get; set; }

        [SpParam(Ignored = true)]
        public int ChartType { get; set; }

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
    }
}
