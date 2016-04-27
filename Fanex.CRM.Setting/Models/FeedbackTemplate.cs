// --------------------------------------------------------------------------
// <copyright file="FeedbackTemplate.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Setting.Models
{
    using System;

    /// <summary>
    /// Feedback Template.
    /// </summary>
    [Serializable]
    public class FeedbackTemplate
    {
        public int TemplateId { get; set; }

        public string Subject { get; set; }

        public bool IsActive { get; set; }
    }
}
