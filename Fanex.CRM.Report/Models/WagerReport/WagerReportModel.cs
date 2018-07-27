namespace Fanex.CRM.Report.Models.WagerReport
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class WagerReportModel
    {
        public string ReportTitle { get; set; }

        public IList<WagerByMonth> WagerByMonths { get; set; }

        public WagerByMonth GrandTotal { get; set; }
    }
}
