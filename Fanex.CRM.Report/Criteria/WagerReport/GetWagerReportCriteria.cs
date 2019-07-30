namespace Fanex.CRM.Report.Criteria.WagerReport
{
    using Fanex.CRM.Common.Services;
    using Fanex.Data.Repository;

    public class GetWagerReportCriteria : CriteriaBase
    {
        public int Year { get; set; }

        public int TimeSpan
        {
            get
            {
                return AppSetting.TimeSpan * -1;
            }
        }

        public override string GetSettingKey()
        {
            return "Report_WagerReport_GetReport";
        }

        public override bool IsValid()
        {
            return Year > 0;
        }
    }
}