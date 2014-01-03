// --------------------------------------------------------------------------
// <copyright file="CommunicationTool.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Entities.ViewModel
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Communication Tool.
    /// </summary>
    [Serializable]
    [DataContract]
    public class CommunicationTool
    {
        [DataMember]
        public string ContactBy { get; set; }

        [DataMember]
        public string Address { get; set; }
    }
}
