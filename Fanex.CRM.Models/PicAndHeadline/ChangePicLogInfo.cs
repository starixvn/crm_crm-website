namespace Fanex.CRM.Models.PicAndHeadline
{
    using System;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Xml.Serialization;

    public class ChangePicLogInfo
    {
        public int LogId { get; set; }

        public string LogTime => $"{LogDate.ToString("MMM dd, yyyy")}\n({LogDate.ToString("hh:mm:ss tt")})";

        public string PIC { get; set; }

        public int ActionId { get; set; }

        public string Action { get; set; }

        public string OldValues { get; set; }

        public string NewValues { get; set; }

        public string EditBy { get; set; }

        public DateTime LogDate { get; set; }

        public PicInfoLog OldPicInfo => ConvertXmlToPicInfo(OldValues);

        public PicInfoLog NewPicInfo => ConvertXmlToPicInfo(NewValues);

        private PicInfoLog ConvertXmlToPicInfo(string xml)
        {
            if (!string.IsNullOrEmpty(xml))
            {
                var regexStart = new Regex(@"<\s*[A-Za-z]");
                var matchStart = regexStart.Match(xml);
                var regexEnd = new Regex(@"[_A-Za-z0-9][^_A-Za-z0-9]");
                var matchEnd = regexEnd.Match(xml);
                var root = xml.Substring(matchStart.Index + matchStart.Length - 1, matchEnd.Index);
                XmlSerializer serializer = new XmlSerializer(typeof(PicInfoLog), new XmlRootAttribute(root));

                using (var stringReader = new StringReader(xml))
                {
                    return (PicInfoLog)serializer.Deserialize(stringReader);
                }
            }

            return new PicInfoLog();
        }
    }
}