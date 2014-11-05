// --------------------------------------------------------------------------
// <copyright file="MailAttachment.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Infrastructure.Models
{
    using System.IO;

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
