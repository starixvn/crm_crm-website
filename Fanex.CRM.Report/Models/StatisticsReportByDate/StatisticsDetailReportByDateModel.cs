namespace Fanex.CRM.Report.Models.StatisticsReportByDate
{
    using Fanex.CRM.Infrastructure.Models;
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class StatisticsDetailReportByDateModel : BaseModel
    {
        public List<StatisticsDetailbyDateModel> ChildReports { get; set; }

        public int TotalChildReports { get; set; }

        public int StartChildIndex { get; set; }
    }
}
