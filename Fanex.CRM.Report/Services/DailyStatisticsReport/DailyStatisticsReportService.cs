namespace Fanex.CRM.Report.Services.DailyStatisticsReport
{
    using Fanex.CRM.Report.Resources;
    using Fanex.CRM.Report.Services.StatisticByDateReport;

    public class DailyStatisticsReportService : StatisticByDateReportService
    {
        public override string GetMainReportTitle()
        {
            return Labels.DailyStatisticsReport_Title;
        }

        public override string GetMainReportSheetName()
        {
            return Labels.Daily_Statistics;
        }

        public override string GetDetailReportTitle()
        {
            return Labels.DailyStatisticsReport_DetailTitle;
        }
    }
}
