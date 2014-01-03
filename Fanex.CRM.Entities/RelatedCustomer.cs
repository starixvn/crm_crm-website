// --------------------------------------------------------------------------
// <copyright file="RelatedCustomer.cs" company="Nexcel Solutions Vietnam">
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
    /// Related Customer.
    /// </summary>
    [Serializable]
    [DataContract]
    public class RelatedCustomer
    {
        [DataMember]
        public int CustId { get; set; }
        [DataMember]
        public string UserName { get; set; }
    }
}
