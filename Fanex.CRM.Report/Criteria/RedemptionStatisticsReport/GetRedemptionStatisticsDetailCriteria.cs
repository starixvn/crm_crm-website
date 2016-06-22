// --------------------------------------------------------------------------
// <copyright file="GetRedemptionStatisticsDetailCriteria.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ken.Nguyen" date="2014.11.26">Created</change>
// </history>

namespace Fanex.CRM.Report.Criteria.RedemptionStatisticsReport
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get Redemption Statistics Detail Report.
    /// </summary>
    public class GetRedemptionStatisticsDetailCriteria : GetRedemptionStatisticsSummaryCriteria
    {
        [SpParam(Ignored = true)]
        public override int ReportBy { get; set; }

        [SpParam(SqlParamName = "pageindex")]
        public int PageIndex { get; set; }

        [SpParam(SqlParamName = "itemsperpage")]
        public int ItemsPerPage { get; set; }

        [SpParam(SqlParamName = "totalrows")]
        public int TotalRows { get; set; }

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
