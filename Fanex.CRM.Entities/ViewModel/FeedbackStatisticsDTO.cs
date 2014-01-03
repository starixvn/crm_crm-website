// --------------------------------------------------------------------------
// <copyright file="FeedbackStatisticsDTO.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Entities.ViewModel
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Feedback Statistics DTO.
    /// </summary>
    [Serializable]
    [DataContract]
    public class FeedbackStatisticsDTO
    {
        [DataMember]
        public ArrayList ColumnNames { get; set; }
        [DataMember]
        public ArrayList ColumnIDs { get; set; }
        [DataMember]
        public int Total { get; set; }
        [DataMember]
        public int PageIndex { get; set; }
        [DataMember]
        public IEnumerable<FeedbackStatisticsGroup> Rows { get; set; }
        [DataMember]
        public FeedbackStatistics GrandTotal { get; set; }
    }
}
