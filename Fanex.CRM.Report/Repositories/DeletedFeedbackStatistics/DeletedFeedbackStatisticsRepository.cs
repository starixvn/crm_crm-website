// -----------------------------------------------------------------------
// <copyright file="DeletedFeedbackStatisticsRepository.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.Report.Repositories.DeletedFeedbackStatistics
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CRM.Report.Criteria.DeletedFeedbackStatistics;
    using Fanex.CRM.Report.Models.DeletedFeedbackStatistics;
    using Fanex.Data;

    public class DeletedFeedbackStatisticsRepository : IDeletedFeedbackStatisticsRepository
    {
        public IEnumerable<DeletedFeedback> GetDeletedFeedbacks(DeletedFeedbackStatisticsCriteria criteria)
        {
            var deletedFeedbacks = new List<DeletedFeedback>();

            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                deletedFeedbacks = objectDb.Query<DeletedFeedback>(criteria.GetSpParams()).ToList();
            }

            return deletedFeedbacks;
        }
    }
}
