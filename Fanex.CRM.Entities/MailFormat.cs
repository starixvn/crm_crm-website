// --------------------------------------------------------------------------
// <copyright file="MailFormat.cs" company="Nexcel Solutions Vietnam">
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
    /// Mail Format.
    /// </summary>
    [Serializable]
    [DataContract]
    public class MailFormat
    {
        [DataMember]
        public string Reciver { get; set; }
        [DataMember]
        public string To { get; set; }
        [DataMember]
        public string Cc { get; set; }
        [DataMember]
        public string Bcc { get; set; }
    }
}
