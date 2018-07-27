namespace Fanex.CRM.Report.Models.SummaryReport
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class SummaryReportModel
    {
        public string ReportTitle { get; set; }

        public IList<CategorySummary> CategorySummaries { get; set; }

        public IList<Summary> GrandTotals { get; set; }

        public IList<string> Months { get; set; }
    }
}
