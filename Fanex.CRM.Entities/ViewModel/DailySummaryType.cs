// --------------------------------------------------------------------------
// <copyright file="DailySummaryType.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Entities.ViewModel
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Daily Summary Type.
    /// </summary>
    [Serializable]
    [DataContract]
    public class DailySummaryType
    {
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public int Value { get; set; }
    }
}
