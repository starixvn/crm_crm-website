// --------------------------------------------------------------------------
// <copyright file="CasinoDailySummaryDetail.cs" company="Nexcel Solutions Vietnam">
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
    /// Casino Daily Summary Detail.
    /// </summary>
    [Serializable]
    [DataContract]
    public class CasinoDailySummaryDetail
    {
        [DataMember]
        public int MainCategoryId { get; set; }
        [DataMember]
        public string MainCategory { get; set; }
        [DataMember]
        public List<SystemDailySummary> SystemSummary { get; set; }
    }
}
