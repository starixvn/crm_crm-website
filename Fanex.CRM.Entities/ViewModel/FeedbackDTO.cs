// --------------------------------------------------------------------------
// <copyright file="FeedbackDTO.cs" company="Nexcel Solutions Vietnam">
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

    /// <summary>
    /// Feedback DTO.
    /// </summary>
    [Serializable]
    public class FeedbackDTO
    {
        public IEnumerable<Feedback> ListFeedback { get; set; }

        public int TotalPage { get; set; }

        public int PageIndex { get; set; }
    }
}
