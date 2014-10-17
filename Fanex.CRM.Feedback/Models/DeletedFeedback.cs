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

    /// <summary>
    /// Deleted feedback model.
    /// </summary>
    [Serializable]
    public class DeletedFeedback : ManageFeedback
    {
        public int MainCategoryId { get; set; }

        public int SubCategoryId { get; set; }
    }
}
