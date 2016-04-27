// -----------------------------------------------------------------------
// <copyright file="IDeletedFeedbackStatisticsRepository.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.Report.Repositories.DeletedFeedbackStatistics
{
    using System.Collections.Generic;
    using Fanex.CRM.Report.Criteria.DeletedFeedbackStatistics;
    using Fanex.CRM.Report.Models.DeletedFeedbackStatistics;

    public interface IDeletedFeedbackStatisticsRepository
    {
        IEnumerable<DeletedFeedback> GetDeletedFeedbacks(DeletedFeedbackStatisticsCriteria criteria);
    }
}
