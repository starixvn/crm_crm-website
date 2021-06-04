namespace Fanex.CRM.Common.Services
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;

    public static class AppSetting
    {
        private static IDictionary<string, bool> actionLogKeyHash = null;
        private static IDictionary<string, string[]> actionLogBoolValueHash = null;

        public static string ResourceVersion => ConfigurationManager.AppSettings["resourceVersion"];
        public static bool IsLocalTest => ConfigurationManager.AppSettings["isLocalTest"] == "1";
        public static string LoginLink => ConfigurationManager.AppSettings["loginUrl"];
        public static int TimeSpan => Convert.ToInt32(ConfigurationManager.AppSettings["timeSpan"]);
        public static DateTime MinReportDate => Convert.ToDateTime(ConfigurationManager.AppSettings["minReportDate"]);

        public static IDictionary<string, bool> ActionLogKey
        {
            get
            {
                if (actionLogKeyHash == null)
                {
                    actionLogKeyHash = new Dictionary<string, bool>();

                    var stringArray = ConfigurationManager.AppSettings["CRMActionLogKey"]
                        .Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string item in stringArray)
                    {
                        var upperKey = item.ToUpperInvariant();
                        if (!actionLogKeyHash.ContainsKey(upperKey))
                        {
                            actionLogKeyHash.Add(upperKey, true);
                        }
                    }
                }

                return actionLogKeyHash;
            }
        }

        public static IDictionary<string, string[]> ActionLogBoolValue
        {
            get
            {
                if (actionLogBoolValueHash == null)
                {
                    actionLogBoolValueHash = new Dictionary<string, string[]>();

                    var boolValue = new[] { "No", "Yes" };
                    var stringArray = ConfigurationManager.AppSettings["CRMActionLogBoolValue"]
                        .Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string item in stringArray)
                    {
                        var upperKey = item.ToUpperInvariant();
                        if (!actionLogBoolValueHash.ContainsKey(upperKey))
                        {
                            actionLogBoolValueHash.Add(upperKey, boolValue);
                        }
                    }
                }

                return actionLogBoolValueHash;
            }
        }

        public static string SiteId => ConfigurationManager.AppSettings["SiteId"];
        public static string CN88SiteId => ConfigurationManager.AppSettings["CN88SiteId"];
        public static bool EnableManualLogging => ConfigurationManager.AppSettings["EnableManualLogging"] == "true";

        public static string SentryCategory => ConfigurationManager.AppSettings["SentryCategory"];
        public static string SentryDNS => ConfigurationManager.AppSettings["SentryDNS"];
    }
}