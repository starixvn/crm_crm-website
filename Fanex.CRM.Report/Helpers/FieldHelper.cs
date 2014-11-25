// --------------------------------------------------------------------------
// <copyright file="FieldHelper.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ken.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Helpers
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web;
    using Fanex.CRM.Report.Resources;

    /// <summary>
    /// Site Helper.
    /// </summary>
    public static class FieldHelper
    {
        /// <summary>
        /// Generates the contact information.
        /// </summary>
        /// <param name="bbm">The BBM information.</param>
        /// <param name="email">The email information.</param>
        /// <param name="line">The line information.</param>
        /// <param name="msn">The MSN information.</param>
        /// <param name="mobilePhone">The mobile phone information.</param>
        /// <param name="phone">The phone information.</param>
        /// <param name="qq">The QQ information.</param>
        /// <param name="skype">The skype information.</param>
        /// <param name="wechat">The we chat information.</param>
        /// <param name="whatsApp">The Communicate information.</param>
        /// <param name="yahoo">The yahoo information.</param>
        /// <returns>The contact information.</returns>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1306:FieldNamesMustBeginWithLowerCaseLetter", Justification = "Need Review.")]
        public static string GenerateContactInfo(
                                                string bbm,
                                                string email,
                                                string line,
                                                string msn,
                                                string mobilePhone,
                                                string phone,
                                                string qq,
                                                string skype,
                                                string wechat,
                                                string whatsApp,
                                                string yahoo)
        {
            return string.Format(
                    "{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}",
                    string.IsNullOrEmpty(bbm) ? string.Empty : string.Format("{0}: {1}{2}", Labels.BBM, HttpContext.Current.Server.HtmlEncode(bbm), "<br/>"),
                    string.IsNullOrEmpty(email) ? string.Empty : string.Format("{0}: {1}{2}", Labels.Email, HttpContext.Current.Server.HtmlEncode(email), "<br/>"),
                    string.IsNullOrEmpty(line) ? string.Empty : string.Format("{0}: {1}{2}", Labels.Line, HttpContext.Current.Server.HtmlEncode(line), "<br/>"),
                    string.IsNullOrEmpty(msn) ? string.Empty : string.Format("{0}: {1}{2}", Labels.MSN, HttpContext.Current.Server.HtmlEncode(msn), "<br/>"),
                    string.IsNullOrEmpty(mobilePhone) ? string.Empty : string.Format("{0}: {1}{2}", Labels.Mobile, HttpContext.Current.Server.HtmlEncode(mobilePhone), "<br/>"),
                    string.IsNullOrEmpty(phone) ? string.Empty : string.Format("{0}: {1}{2}", Labels.Phone, HttpContext.Current.Server.HtmlEncode(phone), "<br/>"),
                    string.IsNullOrEmpty(qq) ? string.Empty : string.Format("{0}: {1}{2}", Labels.QQ, HttpContext.Current.Server.HtmlEncode(qq), "<br/>"),
                    string.IsNullOrEmpty(skype) ? string.Empty : string.Format("{0}: {1}{2}", Labels.Skype, HttpContext.Current.Server.HtmlEncode(skype), "<br/>"),
                    string.IsNullOrEmpty(wechat) ? string.Empty : string.Format("{0}: {1}{2}", Labels.WeChat, HttpContext.Current.Server.HtmlEncode(wechat), "<br/>"),
                    string.IsNullOrEmpty(whatsApp) ? string.Empty : string.Format("{0}: {1}{2}", Labels.WhatsApp, HttpContext.Current.Server.HtmlEncode(whatsApp), "<br/>"),
                    string.IsNullOrEmpty(yahoo) ? string.Empty : string.Format("{0}: {1}{2}", Labels.Yahoo, HttpContext.Current.Server.HtmlEncode(yahoo), string.Empty));
        }
    }
}
