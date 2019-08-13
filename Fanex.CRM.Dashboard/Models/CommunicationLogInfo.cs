namespace Fanex.CRM.Dashboard.Models
{
    using System;
    using Fanex.CRM.Common.Models;

    [Serializable]
    public class CommunicationLogInfo : BaseCommunicationLog
    {
        public new DateTime StartTime { get; set; }

        public new DateTime EndTime { get; set; }
    }
}