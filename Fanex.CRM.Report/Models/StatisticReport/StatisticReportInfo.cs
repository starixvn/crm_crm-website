namespace Fanex.CRM.Report.Models.StatisticReport
{
    using System;
    using System.Collections.Generic;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Infrastructure.Models;

    [Serializable]
    public class StatisticReportInfo
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string ShortName { get; set; }

        public int ReportTypeId { get; set; }

        public PeriodRange PeriodRange { get; set; }

        public bool HasData { get; set; }

        public Country CountryInfo { get; set; }

        public int NumberOfIssues { get; set; }

        public IList<StatisticReportInfo> ChildReports { get; set; }

        public StatisticReportInfo Total { get; set; }

        public int TotalChildReports { get; set; }

        public StatisticReportInfo ChartInfo { get; set; }
    }
}