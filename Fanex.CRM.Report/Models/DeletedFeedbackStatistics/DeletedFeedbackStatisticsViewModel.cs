namespace Fanex.CRM.Report.Models.DeletedFeedbackStatistics
{
    using System.Collections.Generic;

    public class DeletedFeedbackStatisticsViewModel
    {
        public string Name { get; set; }

        public IEnumerable<DeletedFeedbackByHandler> DeletedFeedbackByHandlers { get; set; }

        public int GrandTotal { get; set; }
    }
}