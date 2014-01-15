// --------------------------------------------------------------------------
// <copyright file="User.cs" company="Nexcel Solutions Vietnam">
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
    /// User information.
    /// </summary>
    [Serializable]
    [DataContract]
    public class User
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Username { get; set; }
        [DataMember]
        public int RoleId { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public bool IsAllowUpdateReview { get; set; }

        private IEnumerable<Menu> ListMenu { get; set; }
    }
}
