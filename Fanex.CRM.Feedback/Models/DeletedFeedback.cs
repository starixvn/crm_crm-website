// --------------------------------------------------------------------------
// <copyright file="DeletedFeedback.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.10.07">Created</change>
// </history>
// --------------------------------------------------------------------------

namespace Fanex.CRM.Feedback.Models
{
    using System;
    using Fanex.CRM.Common.Models;

    /// <summary>
    /// Deleted feedback model.
    /// </summary>
    [Serializable]
    public class DeletedFeedback : FeedbackDetail
    {
        public int MainCategoryId { get; set; }

        public int SubCategoryId { get; set; }

        public int StatusId { get; set; }

        public string Description { get; set; }
    }
}
