// --------------------------------------------------------------------------
// <copyright file="Mails.cs" company="Nexcel Solutions Vietnam">
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
    /// Mails Information.
    /// </summary>
    [Serializable]
    [DataContract]
    public class Mails
    {
        [DataMember]
        public List<MailFormat> Recivers { get; set; }
    }
}
