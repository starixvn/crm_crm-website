// -----------------------------------------------------------------------
// <copyright file="SiteHelper.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

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
                case (int)LogStatisticReportType.IBC:
                    return Labels.IBC;

                case (int)LogStatisticReportType.Licensee:
                    return Labels.Licensee;

                default:
                    return string.Empty;
            }
        }
    }
}
