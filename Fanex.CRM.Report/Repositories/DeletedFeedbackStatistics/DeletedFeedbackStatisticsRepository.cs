namespace Fanex.CRM.Report.Repositories.DeletedFeedbackStatistics
{
    using Common.Models;
    using Fanex.CRM.Report.Criteria.DeletedFeedbackStatistics;
    using Fanex.CRM.Report.Models.DeletedFeedbackStatistics;
    using Fanex.Data;
    using System;
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

                    return GenerateFeedBackDetailWithSetting(feedbacks, settings);
                }
            }
        }

        private IEnumerable<DeletedFeedback> GenerateFeedBackDetailWithSetting(IEnumerable<DeletedFeedback> feedbacks, IEnumerable<SettingItem> settings)
        {
            Func<DeletedFeedback, List<SettingItem>, DeletedFeedback> regenerateFeedBackDetail = delegate (DeletedFeedback f, List<SettingItem> s)
            {
                f.Settings = s;
                return f;
            };
            var result = from f in feedbacks
                         join s in settings on f.FeedbackId equals s.FeedbackID
                         select new { f, s } into t1
                         group t1 by t1.f.FeedbackId into g
                         select regenerateFeedBackDetail(g.FirstOrDefault().f, g.Select(i => i.s).ToList());

            return result.ToList();
        }
    }
}
