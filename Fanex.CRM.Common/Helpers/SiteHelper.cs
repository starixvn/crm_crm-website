// --------------------------------------------------------------------------
// <copyright file="SiteHelper.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Common.Helpers
{
    using Fanex.CRM.Common.Enumerations;
    using Fanex.CRM.Infrastructure.Resources;

    /// <summary>
    /// Site Helper.
    /// </summary>
    public static class SiteHelper
    {
        /// <summary>
        /// Gets the name of the site type.
        /// </summary>
        /// <param name="siteType">Type of the site.</param>
        /// <returns>The name of the site type.</returns>
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

        public static string GetLogStatisticSiteTypeName(int siteType)
        {
            switch (siteType)
            {
                case (int)LogStatisticSiteType.IBC:
                    return Labels.IBC;

                case (int)LogStatisticSiteType.Licensee:
                    return Labels.Licensee;

                default:
                    return string.Empty;
            }
        }
    }
}
