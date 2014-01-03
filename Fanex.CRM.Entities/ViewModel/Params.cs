// --------------------------------------------------------------------------
// <copyright file="Params.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Entities.ViewModel
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Parameter information.
    /// </summary>
    [DataContract]
    public class Params
    {
        [DataMember]
        public int Type { get; set; }
        [DataMember]
        public int ValueId { get; set; }
        [DataMember]
        public int MainId { get; set; }
        [DataMember]
        public int SubId { get; set; }
        [DataMember]
        public string FromDate { get; set; }
        [DataMember]
        public string ToDate { get; set; }
        [DataMember]
        public bool FromCache { get; set; }
        [DataMember]
        public int ItemPerPage { get; set; }
        [DataMember]
        public int CurrentPage { get; set; }
        [DataMember]
        public SolvingDuration FromDuration { get; set; }
        [DataMember]
        public SolvingDuration ToDuration { get; set; }
    }
}
