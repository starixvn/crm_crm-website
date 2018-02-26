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
    using Fanex.CRM.Common.Models;
    using System;

    /// <summary>
    /// Deleted feedback model.
    /// </summary>
    [Serializable]
    public class DeletedFeedback : FeedbackDetail
    {
        public string Description { get; set; }

        public string AutoField { get; set; }

        public string DescriptionTooltip { get; set; }
    }
}
