namespace Fanex.CRM.Communication.Models
{
    using Fanex.CRM.Common.Models;
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class CustomerLogInfo
    {
        public int CustomerId { get; set; }

        public IEnumerable<BaseCommunicationLog> Logs { get; set; }
    }
}
