// --------------------------------------------------------------------------
// <copyright file="FaxInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.24.07">Created</change>
// </history>

namespace Fanex.CRM.Entities
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [Serializable]
    [DataContract]
    public class FaxInfo
    {
        [DataMember]
        public string CountryCode { get; set; }
        [DataMember]
        public string AreaCode { get; set; }
        [DataMember]
        public string FaxNumber { get; set; }
    }
}
