// -----------------------------------------------------------------------
// <copyright file="DeletedFeedbackStatisticsRepository.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.Report.Repositories.DeletedFeedbackStatistics
{
    using Common.Models;
    using Fanex.CRM.Report.Criteria.DeletedFeedbackStatistics;
    using Fanex.CRM.Report.Models.DeletedFeedbackStatistics;
    using Fanex.Data;
    using System.Collections.Generic;
    using System.Linq;

    public class DeletedFeedbackStatisticsRepository : IDeletedFeedbackStatisticsRepository
    {
        public IEnumerable<DeletedFeedback> GetDeletedFeedbacks(DeletedFeedbackStatisticsCriteria criteria)
        {
            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var multi = objectDb.QueryMultiple(criteria.GetSpParams()))
                {
                    var feebbacks = multi.Read<DeletedFeedback>();
                    var settings = multi.Read<SettingItem>();

                    feebbacks = feebbacks.Select(f =>
                    {
                        f.Settings = settings.Where(s => s.FeedbackID == f.FeedbackId).ToList();
                        return f;
                    });

                    return feebbacks.ToList();
                }
            }
        }
    }
}
