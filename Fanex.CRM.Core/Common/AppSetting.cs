// --------------------------------------------------------------------------
// <copyright file="AppSetting.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Infrastructure.Common
{
    using System.Configuration;
    using DNAide.Core;

    /// <summary>
    /// Application Setting.
    /// </summary>
    public static class AppSetting
    {
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

        public static string TestAccount
        {
            get { return CastAide.AsString(ConfigurationManager.AppSettings["testaccount"]); }
        }

        public static int TimeSpan
        {
            get { return CastAide.AsInt32(ConfigurationManager.AppSettings["timeSpan"]); }
        }
    }
}
