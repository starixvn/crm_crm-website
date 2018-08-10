namespace Fanex.CRM.Report.Models.RedemptionStatistics
{
    using Fanex.CRM.Report.Entities.RedemptionStatistics;
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class RedemptionStatisticsInfo
    {
        public string Title { get; set; }

        public List<RedemptionStatisticsEntity> ReportData { get; set; }

        public List<RedemptionStatisticsEntity> TotalData { get; set; }

        public int TotalItems { get; set; }

        public int CurrentPage { get; set; }

        public int ItemsPerPage { get; set; }
    }
}
