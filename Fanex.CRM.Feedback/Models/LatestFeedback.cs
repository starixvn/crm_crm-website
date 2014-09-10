// --------------------------------------------------------------------------
// <copyright file="LatestFeedback.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.09.05">Created</change>
// </history>
// --------------------------------------------------------------------------

namespace Fanex.CRM.Feedback.Models
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Lasted Feedback.
    /// </summary>
    [Serializable]
    public class LatestFeedback : ManageFeedback
    {
        public string Description { get; set; }
    }
}
