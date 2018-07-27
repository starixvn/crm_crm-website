namespace Fanex.CRM.Report.Entities.RedemptionStatistics
{
    using System;

    [Serializable]
    public class RedemptionStatisticsEntity
    {
        public DateTime ClaimTime { get; set; }

        public string Subject { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }

        public string Currency { get; set; }

        public int CurrencyId { get; set; }

        public decimal Amount { get; set; }
    }
}
