

namespace Fanex.CRM.Infrastructure.Helper
{
    using System;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Xml;
    using Fanex.CRM.Infrastructure.Configuration;
    using Fanex.CRM.Infrastructure.Extension;

    public class Helper
    {
        public static string getClientIP()
        {
            return HttpContext.Current.Request.UserHostAddress;
            
        }

        public static void WriteLog(string functionName, string error, string userName = "")
        {
            try
            {
                string path = HttpContext.Current.Server.MapPath("~/Logs");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                string filePath = string.Format("{0}/{1}_log.xml", path, DateTime.Now.ToString("MM-dd-yyyy"));
                XmlDocument xmlDoc = new XmlDocument();
                if (File.Exists(filePath))
                    xmlDoc.Load(filePath);
                else
                {
                    //if file is not found, create a new xml file
                    XmlTextWriter xmlWriter = new XmlTextWriter(filePath, System.Text.Encoding.UTF8);
                    xmlWriter.Formatting = Formatting.Indented;
                    xmlWriter.WriteProcessingInstruction("xml", "version='1.0' encoding='UTF-8'");
                    xmlWriter.WriteStartElement("Logs");
                    xmlWriter.Close();
                    xmlDoc.Load(filePath);
                }
                //var userInfo = SessionExtension.GetSession(HttpContext.Current.Session, "CRMUserInfo");
                //------------------
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
                //--------------------
                root.AppendChild(log);
                log.AppendChild(errorTime);
                log.AppendChild(errorUser);
                log.AppendChild(errorFunction);
                log.AppendChild(errorDetail);
                //--------------------
                errorTime.AppendChild(nodeTime);
                errorUser.AppendChild(nodeUser);
                errorFunction.AppendChild(nodeFunction);
                errorDetail.AppendChild(nodeDetail);
                //--------------------
                nodeTime.Value = DateTime.Now.ToString();
                nodeUser.Value = userName;
                nodeFunction.Value = functionName;
                nodeDetail.Value = error;
                //--------------------
                xmlDoc.Save(filePath);
            }
            catch
            {
             
            }
            
        }

        public static DateTime GetFirstDayOfWeek(DateTime dayInWeek)
        {
            var firstDay = DayOfWeek.Sunday;
            var firstDayInWeek = dayInWeek.Date;
            while (firstDayInWeek.DayOfWeek != firstDay)
            {
                if (firstDayInWeek.AddDays(-1).Month == firstDayInWeek.Month)
                    firstDayInWeek = firstDayInWeek.AddDays(-1);
                else
                    return firstDayInWeek;
            }

            return firstDayInWeek;
        }

        public static string ReplaceHtmlTag(string value)
        {
            var result = value.Replace("<br/>", "\n").Replace("<br>", "\n");
            result = HttpUtility.HtmlDecode(result);
            result = Regex.Replace(result, "<[^<>]+>", "");
            return result;
        }

        public static string RemoveSpecialHtmlTag(string value)
        {
            var result = Regex.Replace(value, "<t.+>", "");
            result = Regex.Replace(result, "</t.+>", "");
            return result;
        }
        
        public static void SendMailErrorNotice(string errror)
        {
            var mailSetting = MailSection.Instance[MailSection.Setting.Send]["Notice"].Mails["error"];

            SendMailExtension.AccSendMail(MailSection.HostName.Gmail, mailSetting.To, mailSetting.Cc, mailSetting.Bcc,
                                          "CRM - Error", "CRM - Error Notice", errror, new MailAttachment());
        }
    }
}
