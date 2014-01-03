// --------------------------------------------------------------------------
// <copyright file="IBankInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Entities
{
    public interface IBankInfo
    {
        string BankName { get; set; }

        string BankAddress { get; set; }

        string SwiftCode { get; set; }

        string AccountName { get; set; }

        string AccountNo { get; set; }
    }
}
