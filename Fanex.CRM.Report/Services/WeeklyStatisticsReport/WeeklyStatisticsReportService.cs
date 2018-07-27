namespace Fanex.CRM.Report.Services.WeeklyStatisticsReport
{
    using Fanex.CRM.Report.Resources;
    using Fanex.CRM.Report.Services.StatisticByDateReport;

    public class WeeklyStatisticsReportService : StatisticByDateReportService
    {
        public override string GetMainReportTitle()
        {
            return Labels.WeeklyStatisticsReport_Title;
        }

        public override string GetMainReportSheetName()
        {
            return Labels.Weekly_Statistics;
        }

        public override string GetDetailReportTitle()
        {
            return Labels.WeeklyStatisticsReport_DetailTitle;
        }
    }
}
