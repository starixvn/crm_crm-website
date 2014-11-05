// --------------------------------------------------------------------------
// <copyright file="AppSetting.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
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
    using DNAide.Core;

    /// <summary>
    /// Application Setting.
    /// </summary>
    public static class AppSetting
    {
        private static IDictionary<string, bool> actionLogKeyHash = null;
        private static IDictionary<string, string[]> actionLogBoolValueHash = null;

        public static string Version
        {
            get { return CastAide.AsString(ConfigurationManager.AppSettings["version"]); }
        }

        public static bool IsLocalTest
        {
            get { return CastAide.AsString(ConfigurationManager.AppSettings["isLocalTest"]) == "1"; }
        }

        public static int ServerId
        {
            get { return CastAide.AsInt32(ConfigurationManager.AppSettings["serverid"]); }
        }

        public static string LoginUrl
        {
            get { return CastAide.AsString(ConfigurationManager.AppSettings["loginUrl"]); }
        }

        public static int TimeSpan
        {
            get { return CastAide.AsInt32(ConfigurationManager.AppSettings["timeSpan"]); }
        }

        public static DateTime MinReportDate
        {
            get { return CastAide.AsDateTime(ConfigurationManager.AppSettings["minReportDate"]); }
        }

        public static List<string> NotAuthenticateAction
        {
            get
            {
                var exceptionAction = ConfigurationManager.AppSettings["NotAuthenticateAction"].Split(';');
                var notAuthenticateAction = new List<string>();
                if (exceptionAction.Length != 0)
                {
                    foreach (var action in exceptionAction)
                    {
                        if (action != null)
                        {
                            notAuthenticateAction.Add(action.ToLower());
                        }
                    }
                }

                return notAuthenticateAction;
            }
        }

        public static IDictionary<string, bool> ActionLogKey
        {
            get
            {
                if (actionLogKeyHash == null)
                {
                    actionLogKeyHash = new Dictionary<string, bool>();

                    string[] stringArray = ConfigurationManager.AppSettings["CRMActionLogKey"]
                        .Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string item in stringArray)
                    {
                        string lowerKey = item.ToLowerInvariant();
                        if (!actionLogKeyHash.ContainsKey(lowerKey))
                        {
                            actionLogKeyHash.Add(lowerKey, true);
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

                    string[] boolValue = new string[] { "No", "Yes" };
                    string[] stringArray = ConfigurationManager.AppSettings["CRMActionLogBoolValue"]
                        .Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string item in stringArray)
                    {
                        string lowerKey = item.ToLowerInvariant();
                        if (!actionLogBoolValueHash.ContainsKey(lowerKey))
                        {
                            actionLogBoolValueHash.Add(lowerKey, boolValue);
                        }
                    }
                }

                return actionLogBoolValueHash;
            }
        }
    }
}
