namespace Fanex.CRM.Dashboard.Models
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class CustomerDashboardInfo
    {
        public CustomerInfo CustomerInfo { get; set; }

        public List<RedemptionLogInfo> RedemptionLogs { get; set; }

        public List<CommunicationLogInfo> CommunicationLogs { get; set; }

        public List<string> Remarks { get; set; }
    }
}