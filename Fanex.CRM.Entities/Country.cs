// --------------------------------------------------------------------------
// <copyright file="Country.cs" company="Nexcel Solutions Vietnam">
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
    /// Country information.
    /// </summary>
    [Serializable]
    [DataContract]
    public class Country
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int CountryId { get; set; }

        [DataMember]
        public string CountryName { get; set; }

        [DataMember]
        public int CountryCode { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public int DisplayOrder { get; set; }
    }
}
