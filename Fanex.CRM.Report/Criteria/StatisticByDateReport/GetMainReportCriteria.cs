namespace Fanex.CRM.Report.Criteria.StatisticByDateReport
{
    using Fanex.Data.Repository;
    using System;

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
