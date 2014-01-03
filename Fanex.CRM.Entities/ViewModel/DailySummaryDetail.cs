// --------------------------------------------------------------------------
// <copyright file="DailySummaryDetail.cs" company="Nexcel Solutions Vietnam">
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
    /// Daily Summary Detail.
    /// </summary>
    [Serializable]
    [DataContract]
    public class DailySummaryDetail
    {
        [DataMember]
        public int MainCategoryId { get; set; }
        [DataMember]
        public string MainCategory { get; set; }
        [DataMember]
        public int SubCaregoryId { get; set; }
        [DataMember]
        public string Subcategory { get; set; }
        [DataMember]
        public List<DailySummaryType> Cells { get; set; }
        [DataMember]
        public int Total { get; set; }
    }
}
