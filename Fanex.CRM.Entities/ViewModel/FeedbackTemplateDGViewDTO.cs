// --------------------------------------------------------------------------
// <copyright file="FeedbackTemplateDGViewDTO.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Entities.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Feedback Template DG View DTO.
    /// </summary>
    [Serializable]
    [DataContract]
    public class FeedbackTemplateDGViewDTO
    {
        [DataMember]
        public int PageIndex { get; set; }

        [DataMember]
        public int TotalItems { get; set; }

        [DataMember]
        public List<FeedbackTemplate> Rows { get; set; }
    }
}
