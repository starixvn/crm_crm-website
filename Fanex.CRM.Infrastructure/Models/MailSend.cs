// --------------------------------------------------------------------------
// <copyright file="MailSend.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Infrastructure.Models
{
    using System.Net.Mail;
    using Fanex.CRM.Infrastructure.Configurations;

    /// <summary>
    /// Mail Send.
    /// </summary>
    public class MailSend
    {
        public MailSection.HostName HostName { get; set; }

        public string To { get; set; }

        public string Cc { get; set; }

        public string Bcc { get; set; }

        public string Title { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public MailAttachment Attachment { get; set; }

        public AlternateView AltView { get; set; }
    }
}
