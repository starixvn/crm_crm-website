// --------------------------------------------------------------------------
// <copyright file="AppSetting.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Common.Services
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;

    /// <summary>
    /// Application Setting.
    /// </summary>
    public static class AppSetting
    {
        private static IDictionary<string, bool> actionLogKeyHash = null;
        private static IDictionary<string, string[]> actionLogBoolValueHash = null;

        /// <summary>
        /// Used in razor helper
        /// </summary>
        public static string ResourceVersion
        {
            get { return ConfigurationManager.AppSettings["resourceVersion"]; }
        }

        public static bool IsLocalTest
        {
            get { return ConfigurationManager.AppSettings["isLocalTest"] == "1"; }
        }

        /// <summary>
        /// Used in view
        /// </summary>
        public static string LoginLink
        {
            get { return ConfigurationManager.AppSettings["loginUrl"]; }
        }

        public static int TimeSpan
        {
            get { return Convert.ToInt32(ConfigurationManager.AppSettings["timeSpan"]); }
        }

        public static DateTime MinReportDate
        {
            get { return Convert.ToDateTime(ConfigurationManager.AppSettings["minReportDate"]); }
        }

        public static IDictionary<string, bool> ActionLogKey
        {
            get
            {
                if (actionLogKeyHash == null)
                {
                    actionLogKeyHash = new Dictionary<string, bool>();

                    var stringArray = ConfigurationManager.AppSettings["CRMActionLogKey"]
                        .Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
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

                    var boolValue = new string[] { "No", "Yes" };
                    var stringArray = ConfigurationManager.AppSettings["CRMActionLogBoolValue"]
                        .Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
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
        public static string SiteId { get { return ConfigurationManager.AppSettings["SiteId"]; } }
        public static string CN88SiteId { get { return ConfigurationManager.AppSettings["CN88SiteId"]; } }

        public static bool EnableManualLogging
        {
            get { return ConfigurationManager.AppSettings["EnableManualLogging"] == "true"; }
        }
    }
}
