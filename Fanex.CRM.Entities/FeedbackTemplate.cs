// --------------------------------------------------------------------------
// <copyright file="FeedbackTemplate.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Entities
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Feedback Template.
    /// </summary>
    [Serializable]
    [DataContract]
    public class FeedbackTemplate
    {
        [DataMember]
        public int TemplateId { get; set; }

        [DataMember]
        public string Subject { get; set; }
    }
}
