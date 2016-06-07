// -----------------------------------------------------------------------
// <copyright file="DeletedFeedbackStatisticsRepository.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.Report.Repositories.DeletedFeedbackStatistics
{
    using System.Collections.Generic;
    using Fanex.CRM.Report.Criteria.DeletedFeedbackStatistics;
    using Fanex.CRM.Report.Models.DeletedFeedbackStatistics;
    using Fanex.Data;

    public class DeletedFeedbackStatisticsRepository : IDeletedFeedbackStatisticsRepository
    {
        public IEnumerable<DeletedFeedback> GetDeletedFeedbacks(DeletedFeedbackStatisticsCriteria criteria)
        {
            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                return objectDb.Query<DeletedFeedback>(criteria.GetSpParams());
            }
        }
    }
}
