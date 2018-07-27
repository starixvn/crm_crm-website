namespace Fanex.CRM.Report.Repositories.DeletedFeedbackStatistics
{
    using Fanex.CRM.Report.Criteria.DeletedFeedbackStatistics;
    using Fanex.CRM.Report.Models.DeletedFeedbackStatistics;
    using System.Collections.Generic;

    public interface IDeletedFeedbackStatisticsRepository
    {
        IEnumerable<DeletedFeedback> GetDeletedFeedbacks(DeletedFeedbackStatisticsCriteria criteria);
    }
}
