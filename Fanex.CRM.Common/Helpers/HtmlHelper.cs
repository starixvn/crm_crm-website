// --------------------------------------------------------------------------
// <copyright file="HtmlHelper.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Common.Helpers
{
    using System.Text.RegularExpressions;
    using System.Web;
    using Fanex.CRM.Common.Extensions;

    /// <summary>
    /// HTML Helper.
    /// </summary>
    public static class HtmlHelper
    {
        /// <summary>
        /// Replaces the HTML tag.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The string without HTML tag.</returns>
        public static string RemoveHtmlTags(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

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

            return xml.Replace(new string[] { "\n", "\r", "\n\r", "\r\n" }, "&#13;");
        }
    }
}
