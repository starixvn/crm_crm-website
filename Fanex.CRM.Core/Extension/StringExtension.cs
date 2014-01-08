// --------------------------------------------------------------------------
// <copyright file="StringExtension.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Infrastructure.Extension
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;
    using Helper;

    /// <summary>
    /// String Extension.
    /// </summary>
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

        private static readonly char[] IllegalUrlCharacters = new[]
        { 
            ';', 
            '/', 
            '\\', 
            '?', 
            ':',
            '@', 
            '&',
            '=',
            '+', 
            '$', 
            ',', 
            '<', 
            '>', 
            '#', 
            '%', 
            '.',
            '!',
            '*', 
            '\'', 
            '"', 
            '(', 
            ')', 
            '[',
            ']', 
            '{', 
            '}', 
            '|', 
            '^',
            '`', 
            '~', 
            '–', 
            '‘', 
            '’', 
            '“', 
            '”',
            '»', 
            '«'
        };

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
            Check.Argument.IsNotEmpty(target, "target");
            return string.Format(Constants.CurrentCulture, target, args);
        }

        /// <summary>
        /// Hashes the specified target.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns>Hash result.</returns>
        [DebuggerStepThrough]
        public static string Hash(this string target)
        {
            Check.Argument.IsNotEmpty(target, "target");

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

            Check.Argument.IsNotEmpty(target, "target");
            Check.Argument.IsNotNegativeOrZero(index, "index");

            return (target.Length <= index) ? target : string.Concat(target.Substring(0, index - DotCount), new string('.', DotCount));
        }

        [DebuggerStepThrough]
        public static string StripHtml(this string target)
        {
            return StripHTMLExpression.Replace(target, string.Empty);
        }

        /// <summary>
        /// To the unique identifier.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns>Convert result.</returns>
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
                }
            }

            return result;
        }

        /// <summary>
        /// To the enumeration.
        /// </summary>
        /// <typeparam name="T">Type value.</typeparam>
        /// <param name="target">The target.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>
        /// Convert result.
        /// </returns>
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
                }
            }

            return convertedValue;
        }

        /// <summary>
        /// To the legal URL.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns>Convert result.</returns>
        [DebuggerStepThrough]
        public static string ToLegalUrl(this string target)
        {
            if (string.IsNullOrEmpty(target))
            {
                return target;
            }

            target = target.Trim();

            if (target.IndexOfAny(IllegalUrlCharacters) > -1)
            {
                foreach (char character in IllegalUrlCharacters)
                {
                    target = target.Replace(character.ToString(Constants.CurrentCulture), string.Empty);
                }
            }

            target = target.Replace(" ", "-");
            while (target.Contains("--"))
            {
                target = target.Replace("--", "-");
            }

            return target;
        }

        [DebuggerStepThrough]
        public static string UrlEncode(this string target)
        {
            return HttpUtility.UrlEncode(target);
        }

        [DebuggerStepThrough]
        public static string UrlDecode(this string target)
        {
            return HttpUtility.UrlDecode(target);
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
            oldValues.ForEach(oldValue => target = target.Replace(oldValue, newValue));
            return target;
        }

        /// <summary>
        /// Same as String.IsNullOrEmpty except that
        /// it captures the Empty state for whitespace
        /// strings by Trimming first.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>Check result.</returns>
        [DebuggerStepThrough]
        public static bool IsNullOrTrimEmpty(this string value)
        {
            if (value == null)
            {
                return true;
            }
            else
            {
                return string.Empty == value.Trim();
            }
        }

        [DebuggerStepThrough]
        public static int TrimLength(this string value)
        {
            return value.Trim().Length;
        }

        /// <summary>
        /// Regexes the match.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="pattern">The pattern.</param>
        /// <param name="options">The options.</param>
        /// <param name="groupName">Name of the group.</param>
        /// <returns>Match result.</returns>
        [DebuggerStepThrough]
        public static string RegexMatch(this string value, string pattern, RegexOptions options, string groupName)
        {
            if (groupName.IsNullOrTrimEmpty())
            {
                return Regex.Match(value, pattern, options).Value;
            }
            else
            {
                return Regex.Match(value, pattern, options).Groups[groupName].Value;
            }
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

        /// <summary>
        /// Returns true if there is a match from the regex pattern.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="pattern">The pattern.</param>
        /// <param name="options">The options.</param>
        /// <returns>Checking result.</returns>
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

        /// <summary>
        /// Returns a string where matching patterns are replaced by the replacement string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="pattern">The regex pattern for matching the items to be replaced.</param>
        /// <param name="replacement">The string to replace matching items.</param>
        /// <param name="options">The options.</param>
        /// <returns>Replace result.</returns>
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

        /// <summary>
        /// Replace function string. Format.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>Format result.</returns>
        [DebuggerStepThrough]
        public static string Format(this string value, params object[] args)
        {
            return string.Format(value, args);
        }

        /// <summary>
        /// Replace for substring, but shorter format.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="length">The length.</param>
        /// <returns>Replace result.</returns>
        [DebuggerStepThrough]
        public static string Limit(this string value, int length)
        {
            if (length > 0)
            {
                int stringLength = value.Length;
                return (stringLength > length) ? value.Substring(0, length) : value;
            }

            return value;
        }

        /// <summary>
        /// Indicates whether the current string matches the supplied wildcard pattern.  Behaves the same
        /// as VB's "Like" Operator.
        /// </summary>
        /// <param name="s">The string instance where the extension method is called.</param>
        /// <param name="wildcardPattern">The wildcard pattern to match. Syntax matches VB's Like operator.</param>
        /// <returns>
        /// True if the string matches the supplied pattern, false otherwise.
        /// </returns>
        /// <exception cref="System.ArgumentException">Argument Exception.</exception>
        /// <remarks>
        /// See http://MSDN.microsoft.com/en-us/library/SWF8KAXW(v=VS.100).ASPX.
        /// </remarks>
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
                throw new ArgumentException(string.Format("Invalid pattern: {0}", wildcardPattern), ex);
            }

            return result;
        }
    }
}