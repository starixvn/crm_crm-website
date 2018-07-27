namespace Fanex.CRM.Report.Criteria.CasinoReport
{
    using Fanex.Data.Repository;
    using System;

    public class GetCasinoSummaryCriteria : CriteriaBase
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        [SpParam(SqlParamName = "type")]
        public int ReportType { get; set; }

        public override string GetSettingKey()
        {
            return "casinodailysummary";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
