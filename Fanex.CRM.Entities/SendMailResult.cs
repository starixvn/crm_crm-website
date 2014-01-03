// --------------------------------------------------------------------------
// <copyright file="SendMailResult.cs" company="Nexcel Solutions Vietnam">
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
    /// Send Mail Result.
    /// </summary>
    [Serializable]
    [DataContract]
    public class SendMailResult
    {
        [DataMember]
        public bool IsFail { get; set; }
        [DataMember]
        public List<SendMailToReciverResult> Recivers { get; set; }
    }
}
