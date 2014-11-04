// --------------------------------------------------------------------------
// <copyright file="BankInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Common.Models
{
    using System;
    using Fanex.CRM.Infrastructure.Models;

    /// <summary>
    /// Bank Information.
    /// </summary>
    [Serializable]
    public class BankInfo : BaseModel
    {
        public string Address { get; set; }

        public string SwiftCode { get; set; }

        public string AccountName { get; set; }

        public string AccountNumber { get; set; }

        public string HolderName { get; set; }

        public AddressInfo AddressInfo { get; set; }
    }
}
