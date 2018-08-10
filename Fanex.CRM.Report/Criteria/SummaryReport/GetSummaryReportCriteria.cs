namespace Fanex.CRM.Report.Criteria.SummaryReport
{
    using Fanex.CRM.Common.Services;
    using Fanex.Data.Repository;

    public class GetSummaryReportCriteria : CriteriaBase
    {
        public int Year { get; set; }

        public int SiteType { get; set; }

        public int TimeSpan
        {
            get
            {
                return AppSetting.TimeSpan * -1;
            }
        }

        [SpParam(Ignored = true)]
        public string Categories { get; set; }

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
