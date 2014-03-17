// --------------------------------------------------------------------------
// <copyright file="AllSetting.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// All Setting Information.
    /// </summary>
    [Serializable]
    [DataContract]
    public class AllSetting
    {
        [DataMember]
        public List<Setting> Title { get; set; }

        [DataMember]
        public List<Setting> Extension { get; set; }

        [DataMember]
        public List<Setting> Communication { get; set; }

        [DataMember]
        public List<Setting> Language { get; set; }

        [DataMember]
        public List<Setting> Category { get; set; }

        [DataMember]
        public List<Setting> SubCategory { get; set; }

        [DataMember]
        public List<Setting> Product { get; set; }

        [DataMember]
        public List<Setting> BetType { get; set; }

        [DataMember]
        public List<Country> Country { get; set; }

        [DataMember]
        public List<Site> Site { get; set; }

        [DataMember]
        public IList<Setting> Status { get; set; }

        [DataMember]
        public List<Setting> Response { get; set; }

        [DataMember]
        public List<Setting> Role { get; set; }
    }
}
