// --------------------------------------------------------------------------
// <copyright file="SettingRelation.cs" company="Nexcel Solutions Vietnam">
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
    /// Setting Relation.
    /// </summary>
    [Serializable]
    [DataContract]
    public class SettingRelation
    {
        [DataMember]
        public int SettingId { get; set; }
        [DataMember]
        public int ParentId { get; set; }
        [DataMember]
        public int Order { get; set; }
    }
}
