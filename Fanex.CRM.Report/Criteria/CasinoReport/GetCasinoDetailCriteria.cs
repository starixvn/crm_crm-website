namespace Fanex.CRM.Report.Criteria.CasinoReport
{
    using Fanex.Data.Repository;
    using System;

    public class GetCasinoDetailCriteria : CriteriaBase
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        [SpParam(Ignored = true)]
        public int PageIndex { get; set; }

        [SpParam(Ignored = true)]
        public int ItemsPerPage { get; set; }

        [SpParam(Ignored = true)]
        public bool FullReport { get; set; }

        [SpParam(Ignored = true)]
        public int ReportType { get; set; }

        public override string GetSettingKey()
        {
            return "casinodailydetail";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
