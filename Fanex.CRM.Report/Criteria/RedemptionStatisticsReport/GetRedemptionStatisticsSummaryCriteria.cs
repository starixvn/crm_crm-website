// --------------------------------------------------------------------------
// <copyright file="GetRedemptionStatisticsSummaryCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ken.Nguyen" date="2014.11.26">Created</change>
// </history>

namespace Fanex.CRM.Report.Criteria.RedemptionStatisticsReport
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get Redemption Statistics Summary Report.
    /// </summary>
    public class GetRedemptionStatisticsSummaryCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "fromdate")]
        public DateTime FromDate { get; set; }

        [SpParam(SqlParamName = "todate")]
        public DateTime ToDate { get; set; }

        [SpParam(SqlParamName = "subject")]
        public string Subject { get; set; }

        [SpParam(SqlParamName = "currencyid")]
        public int CurrencyId { get; set; }

        [SpParam(SqlParamName = "reportby")]
        public int ReportBy { get; set; }

        [SpParam(SqlParamName = "statusid")]
        public string StatusId { get; set; }

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
            return "Report_RedemptionStatistics_GetSummaryReport";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
