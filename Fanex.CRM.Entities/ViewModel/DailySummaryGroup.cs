// --------------------------------------------------------------------------
// <copyright file="DailySummaryGroup.cs" company="Nexcel Solutions Vietnam">
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
    /// Daily Summary Group.
    /// </summary>
    [Serializable]
    [DataContract]
    public class DailySummaryGroup
    {
        [DataMember]
        public List<DailySummaryDetail> DailySummaries { get; set; }
        [DataMember]
        public DailySummaryDetail Total { get; set; }
    }
}
