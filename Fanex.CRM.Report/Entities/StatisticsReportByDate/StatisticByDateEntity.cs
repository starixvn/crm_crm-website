namespace Fanex.CRM.Report.Entities.StatisticsReportByDate
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class StatisticByDateEntity
    {
        public int MainCategoryId { get; set; }

        public string MainCategoryName { get; set; }

        public int SubCategoryId { get; set; }

        public string SubCategoryName { get; set; }

        public List<int> NumberOfIssues { get; set; }
    }
}
