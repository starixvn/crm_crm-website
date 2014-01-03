// --------------------------------------------------------------------------
// <copyright file="FeedbackStatisticsGroup.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Feedback Statistics Group.
    /// </summary>
    [Serializable]
    [DataContract]
    public class FeedbackStatisticsGroup
    {
        [DataMember]
        public List<FeedbackStatistics> FeedbackStatistics { get; set; }
        [DataMember]
        public int TotalMembers { get; set; }
        [DataMember]
        public string Title { get; set; }
    }
}
