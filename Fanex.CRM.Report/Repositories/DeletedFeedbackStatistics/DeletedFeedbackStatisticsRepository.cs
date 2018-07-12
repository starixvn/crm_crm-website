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
                    var feedbacks = multi.Read<DeletedFeedback>();
                    var settings = multi.Read<SettingItem>();

                    var groups = from f in feedbacks
                                 join s in settings on f.FeedbackId equals s.FeedbackID
                                 select new { f, s } into t1
                                 group t1 by t1.f.FeedbackId into g
                                 select new
                                 {
                                     feedback = g.FirstOrDefault().f,
                                     settings = g.Select(r => r.s)
                                 };

                    var result = new List<DeletedFeedback>();
                    foreach (var i in groups)
                    {
                        i.feedback.Settings = i.settings.ToList();
                        result.Add(i.feedback);
                    }

                    return result;
                }
            }
        }
    }
}
