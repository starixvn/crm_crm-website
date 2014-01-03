// --------------------------------------------------------------------------
// <copyright file="FeedbackStatistics.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Entities
{
    using System;
    using System.Collections;
    using System.Runtime.Serialization;

    /// <summary>
    /// Feedback Statistics.
    /// </summary>
    [Serializable]
    [DataContract]
    public class FeedbackStatistics
    {
        [DataMember]
        public ArrayList Cells { get; set; }
        [DataMember]
        public string MainCategory { get; set; }
        [DataMember]
        public string SubCategory { get; set; }
        [DataMember]
        public int MainCategoryId { get; set; }
        [DataMember]
        public int SubCategoryId { get; set; }
        [DataMember]
        public bool IsTotal { get; set; }
    }
}
