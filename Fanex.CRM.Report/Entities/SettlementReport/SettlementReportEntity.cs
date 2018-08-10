namespace Fanex.CRM.Report.Entities.SettlementReport
{
    using System;

    [Serializable]
    public class SettlementReportEntity
    {
        public int Month { get; set; }

        public int CountryId { get; set; }

        public string CountryName { get; set; }

        public int NumberOfIssues { get; set; }
    }
}
