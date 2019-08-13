namespace Fanex.CRM.Report.Criteria.RedemptionStatisticsReport
{
    using System;
    using Fanex.Data.Repository;

    public class GetRedemptionStatisticsSummaryCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "fromdate")]
        public DateTime FromDate { get; set; }

        [SpParam(SqlParamName = "todate")]
        public DateTime ToDate { get; set; }

        [SpParam(SqlParamName = "subject")]
        public string Subject { get; set; }

        [SpParam(SqlParamName = "currencyid")]
        public string CurrencyId { get; set; }

        [SpParam(SqlParamName = "reportby")]
        public virtual int ReportBy { get; set; }

        [SpParam(SqlParamName = "statusid")]
        public string StatusId { get; set; }

        public override string GetSettingKey()
        {
            return "Report_RedemptionStatistics_GetSummaryReport";
        }

        public override bool IsValid() => true;
    }
}