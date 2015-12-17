// --------------------------------------------------------------------------
// <copyright file="CustomerInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Feedback.Models
{
    using System;
    using Fanex.CRM.Common.Models;

    /// <summary>
    /// Customer Information.
    /// </summary>
    [Serializable]
    public class CustomerInfo : BaseCustomer
    {
        public string BirthDay { get; set; }
    }
}
