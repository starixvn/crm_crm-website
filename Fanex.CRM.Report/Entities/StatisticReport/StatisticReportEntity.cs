namespace Fanex.CRM.Report.Entities.StatisticReport
{
    using System;

    [Serializable]
    public class StatisticReportEntity
    {
        public DateTime DateTime { get; set; }

        public int CountryId { get; set; }

        public string CountryName { get; set; }

        public int NumberOfIssues { get; set; }
    }
}
