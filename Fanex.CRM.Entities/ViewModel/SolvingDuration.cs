// --------------------------------------------------------------------------
// <copyright file="SolvingDuration.cs" company="Nexcel Solutions Vietnam">
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
    /// Solving Duration.
    /// </summary>
    [DataContract]
    public class SolvingDuration
    {
        [DataMember]
        public string Day { get; set; }
        [DataMember]
        public string Hour { get; set; }
        [DataMember]
        public string Minute { get; set; }
        [DataMember]
        public string Second { get; set; }
    }
}
