// --------------------------------------------------------------------------
// <copyright file="GetRedemptionStatisticsDetailCriteria.cs" company="Nexcel Solutions Vietnam">
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
    /// Get Redemption Statistics Detail Report.
    /// </summary>
    public class GetRedemptionStatisticsDetailCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "month")]
        public int Month { get; set; }

        [SpParam(SqlParamName = "year")]
        public int Year { get; set; }

        [SpParam(SqlParamName = "subject")]
        public string Subject { get; set; }

        [SpParam(Ignored = true)]
        public int PageIndex { get; set; }

        [SpParam(Ignored = true)]
        public int PageSize { get; set; }

        [SpParam(SqlParamName = "currencyid")]
        public int CurrencyId { get; set; }

        [SpParam(SqlParamName = "reportby")]
        public int ReportBy { get; set; }

        [SpParam(SqlParamName = "statusid")]
        public string StatusId { get; set; }


        public override string GetSettingKey()
        {
            return "Report_RedemptionStatistics_GetDetailReport";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
