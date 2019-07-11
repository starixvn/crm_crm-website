// --------------------------------------------------------------------------
// <copyright file="CustomerInfo.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Customer.Models
{
    using Fanex.CRM.Common.Models;
    using System;

    /// <summary>
    /// Customer Information.
    /// </summary>
    [Serializable]
    public class CustomerInfo : BaseCustomer
    {
        public string BirthDay { get; set; }
    }
}
