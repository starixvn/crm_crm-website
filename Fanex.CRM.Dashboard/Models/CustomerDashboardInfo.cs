// --------------------------------------------------------------------------
// <copyright file="CustomerDashboardInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Dashboard.Models
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Customer Dashboard Information.
    /// </summary>
    [Serializable]
    public class CustomerDashboardInfo
    {
        public CustomerInfo CustomerInfo { get; set; }

        public List<RedemptionLogInfo> RedemptionLogs { get; set; }

        public List<CommunicationLogInfo> CommunicationLogs { get; set; }

        public List<string> Remarks { get; set; }
    }
}
