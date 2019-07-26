namespace Fanex.CRM.Common.Helpers
{
    using Fanex.CRM.Common.Enumerations;
    using Fanex.CRM.Common.Resources;

    public static class SiteHelper
    {
        public static string GetSiteTypeName(int siteType)
        {
            switch (siteType)
            {
                case (int)ReportType.CreditSite:
                    return Labels.IBC;

                case (int)ReportType.Licensee:
                    return Labels.Licensee;

                default:
                    return string.Empty;
            }
        }
    }
}