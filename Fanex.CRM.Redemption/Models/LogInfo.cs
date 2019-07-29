namespace Fanex.CRM.Redemption.Models
{
    using System;
    using Fanex.CRM.Common.Models;

    [Serializable]
    public class LogInfo
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public string Subject { get; set; }

        public string ClaimedTime { get; set; }

        public Setting Status { get; set; }

        public string Remark { get; set; }

        public BaseModel Currency { get; set; }

        public string DoneBy { get; set; }

        public decimal Amount { get; set; }
    }
}