namespace Fanex.CRM.Report.Criteria.StatisticByDateReport
{
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.Data.Repository;
    using System;

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
