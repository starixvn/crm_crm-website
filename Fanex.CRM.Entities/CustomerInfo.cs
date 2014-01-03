// --------------------------------------------------------------------------
// <copyright file="CustomerInfo.cs" company="Nexcel Solutions Vietnam">
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
    /// Customer Information.
    /// </summary>
    [Serializable]
    [DataContract]
    public class CustomerInfo
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public Setting Title { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string Fax { get; set; }
        [DataMember]
        public string Phone { get; set; }
        [DataMember]
        public string MobilePhone { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string Skype { get; set; }
        [DataMember]
        public string MSN { get; set; }
        [DataMember]
        public DateTime? BirthDay { get; set; }       
    }
}
