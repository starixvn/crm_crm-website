// --------------------------------------------------------------------------
// <copyright file="WebConfigApplicationSettings.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Infrastructure.Configuration
{
    using System.Collections.Generic;
    using System.Configuration;
    using DNAide.Core;

    /// <summary>
    /// Web Configuration Application Settings.
    /// </summary>
    public class WebConfigApplicationSettings : IApplicationSettings
    {
        public static List<string> NotAuthenticateAction
        {
            get
            {
                var exceptionAction = ConfigurationManager.AppSettings["NotAuthenticateAction"].Split(';');
                var notAuthenticateAction = new List<string>();
                if (exceptionAction.Length != 0)
                {
                    foreach (var acction in exceptionAction)
                    {
                        if (acction != null)
                        {
                            notAuthenticateAction.Add(acction.ToLower());
                        }
                    }
                }

                return notAuthenticateAction;
            }
        }

        public static string MongoDbServer
        {
            get
            {
                return ConfigurationManager.AppSettings["CRMMongoDbServer"];
            }
        }

        public static string MongoDbName
        {
            get
            {
                return ConfigurationManager.AppSettings["CRMMongoDbName"];
            }
        }

        public static string MongoCustomerTable
        {
            get
            {
                return ConfigurationManager.AppSettings["CRMMongoCustomerTable"];
            }
        }

        public static string MongoUserNameHintValue
        {
            get
            {
                return ConfigurationManager.AppSettings["CRMMongoUserNameHint"];
            }
        }

        public static string ReportFolderPath
        {
            get
            {
                var reportFolderPath = ConfigurationManager.AppSettings["ReportFolderPath"];
                var reportFullPath = string.Format(@"{0}{1}", System.Web.HttpRuntime.AppDomainAppPath, reportFolderPath);
                return reportFullPath;
            }
        }

        public static string ReportDownloadUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["ReportDownLoadUrl"];
            }
        }

        public static int ReportDeleteFileTime
        {
            get
            {
                return CastAide.AsInt32(ConfigurationManager.AppSettings["ReportDeleteFileTime"]);
            }
        }

        public int NumberOfRecordsPerPage
        {
            get { return int.Parse(ConfigurationManager.AppSettings["NumberOfRecordsPerPage"]); }
        }
    }
}
