// --------------------------------------------------------------------------
// <copyright file="DailyComplaintsDTO.cs" company="Nexcel Solutions Vietnam">
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
    /// Daily Complaints DTO.
    /// </summary>
    [Serializable]
    [DataContract]
    public class DailyComplaintsDTO
    {
        [DataMember]
        public List<DailyComplaintsDetail> DailyComplaints { get; set; }

        [DataMember]
        public int Pageindex { get; set; }

        [DataMember]
        public int TotalItems { get; set; }
    }
}
