// --------------------------------------------------------------------------
// <copyright file="SendMailToReciverResult.cs" company="Nexcel Solutions Vietnam">
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
    /// Send Mail To Receiver Result.
    /// </summary>
    [Serializable]
    [DataContract]
    public class SendMailToReciverResult
    {
        [DataMember]
        public string Reciver { get; set; }
        [DataMember]
        public bool IsSuccess { get; set; }
        [DataMember]
        public string ErrorMessage { get; set; }
    }
}
