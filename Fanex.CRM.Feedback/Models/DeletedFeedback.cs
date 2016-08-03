// --------------------------------------------------------------------------
// <copyright file="DeletedFeedback.cs" company="">
// Copyright (c) . All rights reserved.
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
        public string Description { get; set; }
    }
}
