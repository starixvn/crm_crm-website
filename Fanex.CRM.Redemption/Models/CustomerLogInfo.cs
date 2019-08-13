namespace Fanex.CRM.Redemption.Models
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class CustomerLogInfo
    {
        public int CustomerId { get; set; }

        public IEnumerable<LogInfo> Logs { get; set; }
    }
}