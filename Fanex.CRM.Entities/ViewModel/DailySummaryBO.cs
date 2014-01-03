// --------------------------------------------------------------------------
// <copyright file="DailySummaryBO.cs" company="Nexcel Solutions Vietnam">
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
    /// Daily Summary BO.
    /// </summary>
    [Serializable]
    [DataContract]
    public class DailySummaryBO
    {
        [DataMember]
        public DateTime ReportDay { get; set; }
        [DataMember]
        public List<DailySummaryGroup> DailySummaries { get; set; }
        [DataMember]
        public DailySummaryDetail GrandTotal { get; set; }
    }
}
