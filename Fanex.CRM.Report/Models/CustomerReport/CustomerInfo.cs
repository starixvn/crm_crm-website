// --------------------------------------------------------------------------
// <copyright file="CustomerInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Models.CustomerReport
{
    using System;
    using Fanex.CRM.Common.Models;

    /// <summary>
    /// Customer Information.
    /// </summary>
    [Serializable]
    public class CustomerInfo : BaseCustomerInfo
    {
        public DateTime BirthDay { get; set; }

        public string Username { get; set; }

        public string PIC1 { get; set; }

        public string PIC2 { get; set; }

        public string PIC3 { get; set; }

        public string PIC4 { get; set; }
    }
}
