namespace Fanex.CRM.Common.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;
    using Fanex.CRM.Common.Helpers;
    using Fanex.CRM.Common.Resources;

    public static class StringExtension
    {
        private static readonly Regex WebUrlExpression =
            new Regex(
                @"(http|https)://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?",
                RegexOptions.Singleline | RegexOptions.Compiled);

        private static readonly Regex EmailExpression =
            new Regex(
                @"^([0-9a-zA-Z]+[-._+&])*[0-9a-zA-Z]+@([-0-9a-zA-Z]+[.])+[a-zA-Z]{2,6}$",
                RegexOptions.Singleline | RegexOptions.Compiled);

        private static readonly Regex StripHTMLExpression =
            new Regex(
                "<\\S[^><]*>",
                RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.CultureInvariant | RegexOptions.Compiled);

        private static readonly Regex IPExpression =
            new Regex(
                @"(?<First>2[0-4]\d|25[0-5]|[01]?\d\d?)\.(?<Second>2[0-4]\d|25[0-5]|[01]?\d\d?)\.(?<Third>2[0-4]\d|25[0-5]|[01]?\d\d?)\.(?<Fourth>2[0-4]\d|25[0-5]|[01]?\d\d?)",
                RegexOptions.Compiled);

        private static CultureInfo CurrentCulture
        {
            get
            {
                return CultureInfo.CurrentCulture;
            }
        }

        [DebuggerStepThrough]
        public static bool IsValidIP(this string target)
        {
            return !string.IsNullOrEmpty(target) && IPExpression.IsMatch(target);
        }

        [DebuggerStepThrough]
        public static bool IsWebUrl(this string target)
        {
            return !string.IsNullOrEmpty(target) && WebUrlExpression.IsMatch(target);
        }

        [DebuggerStepThrough]
        public static bool IsEmail(this string target)
        {
            return !string.IsNullOrEmpty(target) && EmailExpression.IsMatch(target);
        }

        [DebuggerStepThrough]
        public static string NullSafe(this string target)
        {
            return (target ?? string.Empty).Trim();
        }

        [DebuggerStepThrough]
        public static string FormatWith(this string target, params object[] args)
        {
            ArgumentChecker.IsNotEmpty(target, "target");
            return string.Format(CurrentCulture, target, args);
        }

        [DebuggerStepThrough]
        public static string Hash(this string target)
        {
            ArgumentChecker.IsNotEmpty(target, "target");

            using (MD5 md5 = MD5.Create())
            {
                byte[] data = Encoding.Unicode.GetBytes(target);
                byte[] hash = md5.ComputeHash(data);

                return Convert.ToBase64String(hash);
            }
        }

        [DebuggerStepThrough]
        public static string WrapAt(this string target, int index)
        {
            const int DotCount = 3;

            ArgumentChecker.IsNotEmpty(target, "target");
            ArgumentChecker.IsNotNegativeOrZero(index, "index");

            return (target.Length <= index) ? target : string.Concat(target.Substring(0, index - DotCount), new string('.', DotCount));
        }

        [DebuggerStepThrough]
        public static string StripHtml(this string target)
        {
            return StripHTMLExpression.Replace(target, string.Empty);
        }

        [DebuggerStepThrough]
        public static Guid ToGuid(this string target)
        {
            Guid result = Guid.Empty;

            if ((!string.IsNullOrEmpty(target)) && (target.Trim().Length == 22))
            {
                string encoded = string.Concat(target.Trim().Replace("-", "+").Replace("_", "/"), "==");

                try
                {
                    byte[] base64 = Convert.FromBase64String(encoded);
                    result = new Guid(base64);
                }
                catch (FormatException)
                {
                    // nothing
                }
            }

            return result;
        }

        [DebuggerStepThrough]
        public static T ToEnum<T>(this string target, T defaultValue) where T : IComparable, IFormattable
        {
            T convertedValue = defaultValue;

            if (!string.IsNullOrEmpty(target))
            {
                try
                {
                    convertedValue = (T)Enum.Parse(typeof(T), target.Trim(), true);
                }
                catch (ArgumentException)
                {
                    // nothing
                }
            }

            return convertedValue;
        }

        [DebuggerStepThrough]
        public static string AttributeEncode(this string target)
        {
            return HttpUtility.HtmlAttributeEncode(target);
        }

        [DebuggerStepThrough]
        public static string HtmlEncode(this string target)
        {
            return HttpUtility.HtmlEncode(target);
        }

        [DebuggerStepThrough]
        public static string HtmlDecode(this string target)
        {
            return HttpUtility.HtmlDecode(target);
        }

        public static string Replace(this string target, ICollection<string> oldValues, string newValue)
        {
            foreach (var oldValue in oldValues)
            {
                target = target.Replace(oldValue, newValue);
            }

            return target;
        }

        [DebuggerStepThrough]
        public static string RegexMatch(this string value, string pattern, RegexOptions options, string groupName)
        {
            return string.IsNullOrWhiteSpace(groupName)
                ? Regex.Match(value, pattern, options).Value
                : Regex.Match(value, pattern, options).Groups[groupName].Value;
        }

        [DebuggerStepThrough]
        public static string RegexMatch(this string value, string pattern)
        {
            return RegexMatch(value, pattern, RegexOptions.None, null);
        }

        [DebuggerStepThrough]
        public static string RegexMatch(this string value, string pattern, RegexOptions options)
        {
            return RegexMatch(value, pattern, options, null);
        }

        [DebuggerStepThrough]
        public static string RegexMatch(this string value, string pattern, string groupName)
        {
            return RegexMatch(value, pattern, RegexOptions.None, groupName);
        }

        [DebuggerStepThrough]
        public static bool IsRegexMatch(this string value, string pattern, RegexOptions options)
        {
            return value.RegexMatch(pattern, options).Length > 0;
        }

        [DebuggerStepThrough]
        public static bool IsRegexMatch(this string value, string pattern)
        {
            return value.IsRegexMatch(pattern, RegexOptions.None);
        }

        [DebuggerStepThrough]
        public static string RegexReplace(this string value, string pattern, string replacement, RegexOptions options)
        {
            return Regex.Replace(value, pattern, replacement, options);
        }

        [DebuggerStepThrough]
        public static string RegexReplace(this string value, string pattern, string replacement)
        {
            return Regex.Replace(value, pattern, replacement, RegexOptions.None);
        }

        public static bool IsLike(this string s, string wildcardPattern)
        {
            if (s == null || string.IsNullOrEmpty(wildcardPattern))
            {
                return false;
            }

            // turn into regex pattern, and match the whole string with ^$
            var regexPattern = "^" + Regex.Escape(wildcardPattern) + "$";

            // add support for ?, #, *, [], and [!]
            regexPattern = regexPattern.Replace(@"\[!", "[^")
                                       .Replace(@"\[", "[")
                                       .Replace(@"\]", "]")
                                       .Replace(@"\?", ".")
                                       .Replace(@"\*", ".*")
                                       .Replace(@"\#", @"\d");

            var result = false;
            try
            {
                result = Regex.IsMatch(s, regexPattern);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(string.Format(Labels.Error_InvalidPattern, wildcardPattern), ex);
            }

            return result;
        }

        public static string GetValueNotNull(string value)
        {
            return value ?? string.Empty;
        }
    }
}