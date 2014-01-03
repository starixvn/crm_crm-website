// --------------------------------------------------------------------------
// <copyright file="CasinoDailySummary.cs" company="Nexcel Solutions Vietnam">
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
    /// Casino Daily Summary.
    /// </summary>
    [Serializable]
    [DataContract]
    public class CasinoDailySummary
    {
        [DataMember]
        public DateTime ReportDate { get; set; }
        [DataMember]
        public List<CasinoDailySummaryDetail> DailySummary { get; set; }
        [DataMember]
        public CasinoDailySummaryDetail GrandTotal { get; set; }
        [DataMember]
        public List<int> SumAllFeedbacks { get; set; }
    }
}
