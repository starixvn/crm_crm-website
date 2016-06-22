// --------------------------------------------------------------------------
// <copyright file="CustomerReportInfo.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Models.CustomerReport
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Customer Report Information.
    /// </summary>
    [Serializable]
    public class CustomerReportInfo
    {
        public List<CustomerInfo> CustomerList { get; set; }

        public int TotalCustomer { get; set; }
    }
}
