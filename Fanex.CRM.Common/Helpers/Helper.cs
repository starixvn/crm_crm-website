// --------------------------------------------------------------------------
// <copyright file="Helper.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Common.Helpers
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Xml;
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Extensions;
    using Fanex.CRM.Infrastructure.Configurations;
    using Fanex.CRM.Infrastructure.Helpers;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Infrastructure.Resources;

    /// <summary>
    /// Helper class.
    /// </summary>
    public class Helper
    {
        public static string GetClientIP()
        {
            return HttpContext.Current.Request.UserHostAddress;
        }

        /// <summary>
        /// Writes the log.
        /// </summary>
        /// <param name="functionName">Name of the function.</param>
        /// <param name="error">The error.</param>
        /// <param name="userName">Name of the user.</param>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public static void WriteLog(string functionName, string error, string userName = "")
        {
            try
            {
                string path = HttpContext.Current.Server.MapPath("~/Logs");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string filePath = string.Format("{0}/{1}_log.xml", path, DateTime.Now.ToString("MM-dd-yyyy"));
                XmlDocument xmlDoc = new XmlDocument();
                if (File.Exists(filePath))
                {
                    xmlDoc.Load(filePath);
                }
                else
                {
                    // if file is not found, create a new xml file
                    XmlTextWriter xmlWriter = new XmlTextWriter(filePath, System.Text.Encoding.UTF8);
                    xmlWriter.Formatting = Formatting.Indented;
                    xmlWriter.WriteProcessingInstruction("xml", "version='1.0' encoding='UTF-8'");
                    xmlWriter.WriteStartElement("Logs");
                    xmlWriter.Close();
                    xmlDoc.Load(filePath);
                }

                // var userInfo = SessionExtension.GetSession(HttpContext.Current.Session, "CRMUserInfo");
                // ------------------
                XmlNode root = xmlDoc.DocumentElement;
                XmlElement log = xmlDoc.CreateElement("Log");
                XmlElement errorTime = xmlDoc.CreateElement("Time");
                XmlElement errorUser = xmlDoc.CreateElement("User");
                XmlElement errorFunction = xmlDoc.CreateElement("Function");
                XmlElement errorDetail = xmlDoc.CreateElement("Error");
                XmlNode nodeTime = xmlDoc.CreateTextNode("Time");
                XmlNode nodeUser = xmlDoc.CreateTextNode("User");
                XmlNode nodeFunction = xmlDoc.CreateTextNode("Function");
                XmlNode nodeDetail = xmlDoc.CreateTextNode("Error");

                // --------------------
                root.AppendChild(log);
                log.AppendChild(errorTime);
                log.AppendChild(errorUser);
                log.AppendChild(errorFunction);
                log.AppendChild(errorDetail);

                // --------------------
                errorTime.AppendChild(nodeTime);
                errorUser.AppendChild(nodeUser);
                errorFunction.AppendChild(nodeFunction);
                errorDetail.AppendChild(nodeDetail);

                // --------------------
                nodeTime.Value = DateTime.Now.ToString();
                nodeUser.Value = userName;
                nodeFunction.Value = functionName;
                nodeDetail.Value = error;

                // --------------------
                xmlDoc.Save(filePath);
            }
            catch
            {
            }
        }

        /// <summary>
        /// Gets the first day of week.
        /// </summary>
        /// <param name="dayInWeek">The day in week.</param>
        /// <returns>The first day of week.</returns>
        public static DateTime GetFirstDayOfWeek(DateTime dayInWeek)
        {
            var firstDay = DayOfWeek.Sunday;
            var firstDayInWeek = dayInWeek.Date;
            while (firstDayInWeek.DayOfWeek != firstDay)
            {
                if (firstDayInWeek.AddDays(-1).Month == firstDayInWeek.Month)
                {
                    firstDayInWeek = firstDayInWeek.AddDays(-1);
                }
                else
                {
                    return firstDayInWeek;
                }
            }

            return firstDayInWeek;
        }

        public static string ReplaceHtmlTag(string value)
        {
            var result = value.Replace("<br/>", "\n").Replace("<br>", "\n");
            result = HttpUtility.HtmlDecode(result);
            result = Regex.Replace(result, "<[^<>]+>", string.Empty);
            return result;
        }

        public static string RemoveSpecialHtmlTag(string value)
        {
            var result = Regex.Replace(value, "<t.+>", string.Empty);
            result = Regex.Replace(result, "</t.+>", string.Empty);
            return result;
        }

        /// <summary>
        /// Sends the mail error notice.
        /// </summary>
        /// <param name="error">The error.</param>
        public static void SendMailErrorNotice(string error)
        {
            var mailSetting = MailSection.Instance[MailSection.Setting.Send]["Notice"].Mails["error"];
            SendMailExtension.AccSendMail(
                MailSection.HostName.Gmail,
                mailSetting.To,
                mailSetting.Cc,
                mailSetting.Bcc,
                "CRM - Error",
                "CRM - Error Notice",
                error,
                new MailAttachment());
        }

        public static string EncodeXml(string xml)
        {
            if (string.IsNullOrWhiteSpace(xml))
            {
                return xml;
            }

            return xml.Replace("\"", "&quot;").Replace("'", "&apos;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("&", "&amp;");
        }

        public static string DecodeXml(string xml)
        {
            if (string.IsNullOrWhiteSpace(xml))
            {
                return xml;
            }

            return xml.Replace("&quot;", "\"").Replace("&apos;", "'").Replace("&lt;", "<").Replace("&gt;", ">").Replace("&amp;", "&");
        }

        public static string ReplaceBreaklineXml(string xml)
        {
            if (string.IsNullOrWhiteSpace(xml))
            {
                return xml;
            }

            return xml.Replace(new string[] { "\n", "\r", "\n\r", "\r\n" }, "&#13;");
        }

        /// <summary>
        /// Gets the name of the site type.
        /// </summary>
        /// <param name="siteType">Type of the site.</param>
        /// <returns>The name of the site type.</returns>
        public static string SiteTypeName(int siteType)
        {
            switch (siteType)
            {
                case (int)Enumerations.ReportTypes.CreditSite:
                    return Labels.IBC;
                case (int)Enumerations.ReportTypes.Licensee:
                    return Labels.Licensee;
                default:
                    return string.Empty;
            }
        }

        public static string ViewChartFromToDateTitle(string fromDate, string toDate)
        {
            return string.Equals(fromDate, toDate, StringComparison.OrdinalIgnoreCase) ?
                fromDate :
                string.Format(Labels.Message_ViewChart_FromToDate, fromDate, toDate);
        }

        public static string ClientIP()
        {
            return HttpContext.Current.Request.UserHostAddress;
        }
    }
}
