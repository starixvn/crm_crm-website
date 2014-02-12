// --------------------------------------------------------------------------
// <copyright file="CustomerInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Dashboard.Models
{
    using System;
    using Fanex.CRM.Infrastructure.Models;

    /// <summary>
    /// Customer Information.
    /// </summary>
    [Serializable]
    public class CustomerInfo : BaseModel
    {
        public string Email { get; set; }

        public string Passport { get; set; }

        public string SecretCode { get; set; }

        public decimal BetCredit { get; set; }

        public bool IsVerifyEmail { get; set; }

        public bool IsVerifyPassport { get; set; }

        public bool IsVerifyBirthday { get; set; }

        public bool IsVerifySecretCode { get; set; }

        public bool IsVerifyBetCredit { get; set; }
    }
}
