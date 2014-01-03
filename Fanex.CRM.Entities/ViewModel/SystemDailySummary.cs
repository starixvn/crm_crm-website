// --------------------------------------------------------------------------
// <copyright file="SystemDailySummary.cs" company="Nexcel Solutions Vietnam">
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
    /// System Daily Summary.
    /// </summary>
    [Serializable]
    [DataContract]
    public class SystemDailySummary
    {
        [DataMember]
        public int SystemId { get; set; }
        [DataMember]
        public string Sytem { get; set; }
        [DataMember]
        public List<DailySummaryType> Cells { get; set; }
    }
}
