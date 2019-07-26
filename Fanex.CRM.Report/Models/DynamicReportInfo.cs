namespace Fanex.CRM.Report.Models
{
    using System;
    using System.Collections.Generic;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Infrastructure.Models;

    [Serializable]
    public class DynamicReportInfo : BaseCommunicationLog
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string ShortName { get; set; }

        public int ReportTypeId { get; set; }

        public PeriodRange PeriodRange { get; set; }

        public string Username { get; set; }

        public List<DynamicReportInfo> ChildReports { get; set; }

        public int TotalChildReports { get; set; }

        public int StartChildIndex { get; set; }

        public DynamicReportInfo ChartInfo { get; set; }
    }
}