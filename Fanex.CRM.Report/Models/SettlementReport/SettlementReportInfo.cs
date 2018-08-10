namespace Fanex.CRM.Report.Models.SettlementReport
{
    using Fanex.CRM.Infrastructure.Models;
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class SettlementReportInfo
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public PeriodRange PeriodRange { get; set; }

        public int NumberOfIssues { get; set; }

        public List<SettlementReportInfo> ChildReports { get; set; }

        public SettlementReportInfo Total { get; set; }

        public int TotalChildReports { get; set; }

        public SettlementReportInfo ChartInfo { get; set; }
    }
}
