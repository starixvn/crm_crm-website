namespace Fanex.CRM.Report.Models.LogStatistics
{
    using System.Collections.Generic;

    public class LogStatisticsMainCategory : LogStatisticsBaseModel
    {
        public LogStatisticsMainCategory()
        {
            SubCategories = new List<LogStatisticsBaseModel>();
        }

        public IEnumerable<LogStatisticsBaseModel> SubCategories { get; set; }
    }
}
