// --------------------------------------------------------------------------
// <copyright file="BankInfo.cs" company="Nexcel Solutions Vietnam">
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
    /// Bank Information.
    /// </summary>
    [Serializable]
    [DataContract]
    public class BankInfo
    {
        [DataMember]
        public string BankName { get; set; }

        [DataMember]
        public string BankAddress { get; set; }

        [DataMember]
        public string SwiftCode { get; set; }

        [DataMember]
        public string AccountName { get; set; }

        [DataMember]
        public string AccountHolderName { get; set; }

        [DataMember]
        public string AccountNo { get; set; }
    }
}
