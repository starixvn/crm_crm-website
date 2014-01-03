// --------------------------------------------------------------------------
// <copyright file="SearchCustomerInfo.cs" company="Nexcel Solutions Vietnam">
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
    /// Search Customer Information.
    /// </summary>
    [Serializable]
    [DataContract]
    public class SearchCustomerInfo
    {
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public bool IsGuest { get; set; }
    }
}
