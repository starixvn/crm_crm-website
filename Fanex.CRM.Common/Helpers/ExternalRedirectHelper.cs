namespace Fanex.CRM.Common.Helpers
{
    using System;
    using System.Web;
    using Fanex.CRM.Configuration.ExternalRedirectConfigs;

    public static class ExternalRedirectHelper
    {
        public static string GetExternalDomain(string roorUrl)
        {
            var request = HttpContext.Current.Request;
            roorUrl = roorUrl.EndsWith("/") ? roorUrl.Substring(0, roorUrl.Length - 1) : roorUrl;
            var config = string.Empty;
            foreach (ExternalRedirectElement element in ExternalRedirectConfigs.GetConfigs())
            {
                var localPath = element.DomainUri.LocalPath.EndsWith("/")
                    ? element.DomainUri.LocalPath.Substring(0, element.DomainUri.LocalPath.Length - 1)
                    : element.DomainUri.LocalPath;
                if (element.DomainUri.Host.Equals(request.Url.Host, StringComparison.OrdinalIgnoreCase)
                    && (!element.IncludePort || request.Url.Port == element.DomainUri.Port)
                    && localPath.Equals(roorUrl, StringComparison.OrdinalIgnoreCase))
                {
                    config = element.ExternalDomain;
                }
            }

            return config;
        }
    }
}