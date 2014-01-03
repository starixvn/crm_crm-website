// --------------------------------------------------------------------------
// <copyright file="Site.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Entities
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Site class.
    /// </summary>
    [Serializable]
    [DataContract]
    public class Site
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string SiteId { get; set; }
        [DataMember]
        public string SiteName { get; set; }
        [DataMember]

        // 0: credit; 1: licensee; 2: other
        public int SiteType { get; set; }
        [DataMember]
        public int Order { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
    }
}
