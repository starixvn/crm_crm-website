namespace Fanex.CRM.Report.Helpers
{
    using Fanex.CRM.Infrastructure.Resources;
    using Fanex.CRM.Report.Enums;

    public static class ReportHelper
    {
        public static string GetLogStatisticSiteTypeName(int siteType)
        {
            switch (siteType)
            {
                case (int)LogStatisticsReportType.IBC:
                    return Labels.MBC;

                case (int)LogStatisticsReportType.Licensee:
                    return Labels.Licensee;

                default:
                    return string.Empty;
            }
        }
    }
}
