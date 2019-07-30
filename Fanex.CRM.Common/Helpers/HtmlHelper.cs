namespace Fanex.CRM.Common.Helpers
{
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;
    using Fanex.CRM.Common.Extensions;

    public static class HtmlHelper
    {
        public static string RemoveHtmlTags(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            var result = HttpUtility.HtmlDecode(value);
            var strBuilder = new StringBuilder(result);
            var toReplace = new[] {"<div><br></div>", "<br>", "<div>", "<p>", "<h1>", "<h2>", "<h3>", "<h4>", "<h5>",
                                            "<h6>", "<li>", "<br/>", "</p>", "</h1>", "</h2>", "</h3>", "</h4>", "</h5>", "</h6>"};
            foreach (var str in toReplace)
            {
                strBuilder.Replace(str, "\n");
            }

            result = Regex.Replace(strBuilder.ToString(), "<[^<>]+>", string.Empty);
            return result;
        }

        public static string RemoveSpecialHtmlTag(string value)
        {
            var result = Regex.Replace(value, "<t.+>", string.Empty);
            result = Regex.Replace(result, "</t.+>", string.Empty);
            return result;
        }

        public static string EncodeXml(string xml)
        {
            if (string.IsNullOrWhiteSpace(xml))
            {
                return xml;
            }

            return xml.Replace("&", "&amp;").Replace("\"", "&quot;").Replace("'", "&apos;").Replace("<", "&lt;").Replace(">", "&gt;");
        }

        public static string DecodeXml(string xml)
        {
            if (string.IsNullOrWhiteSpace(xml))
            {
                return xml;
            }

            return xml.Replace("&amp;", "&").Replace("&quot;", "\"").Replace("&apos;", "'").Replace("&lt;", "<").Replace("&gt;", ">");
        }

        public static string ReplaceBreaklineXml(string xml)
        {
            if (string.IsNullOrWhiteSpace(xml))
            {
                return xml;
            }

            return xml.Replace(new[] { "\n", "\r", "\n\r", "\r\n" }, "&#13;");
        }
    }
}