// --------------------------------------------------------------------------
// <copyright file="MailAttachment.cs" company="Nexcel Solutions Vietnam">
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
    using System.IO;
    using System.Net.Mail;
    using System.Threading.Tasks;
    using Fanex.CRM.Infrastructure.Configuration;
    using Fanex.CRM.Infrastructure.Logging;
    using Fanex.MailDeliver.MailClient.API;
    using Fanex.MailDeliver.Model;

    /// <summary>
    /// Mail Attachment.
    /// </summary>
    public class MailAttachment
    {
        public string FileName { get; set; }

        public MemoryStream Content { get; set; }

        public string ContentType { get; set; }
    }
}
