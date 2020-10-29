namespace Fanex.CRM.Feedback.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Common;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Feedback.Criteria;
    using Fanex.CRM.Feedback.Models;
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class FeedbackRepository : IFeedbackRepository
    {
        public IEnumerable<DeletedFeedback> GetManagerFeedbacks(ManageFeedbackCriteria criteria)
        {
            using (ObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var multi = db.QueryMultiple(criteria.GetSpParams()))
                {
                    var feedbacks = multi.Read<DeletedFeedback>();
                    var settings = multi.Read<SettingItem>();

                    var outputParams = db.GetOuputParams();
                    if (outputParams.Count != 0)
                    {
                        criteria.TotalRows = Converter.ToInt(outputParams["totalrows"]);
                    }

                    return GenerateDeletedFeedbackWithSetting(feedbacks, settings);
                }
            }
        }

        public IEnumerable<DeletedFeedback> GetDeletedFeedbacks(ICriteria criteria)
        {
            using (ObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var multi = db.QueryMultiple(criteria.GetSpParams()))
                {
                    var feedbacks = multi.Read<DeletedFeedback>();
                    var settings = multi.Read<SettingItem>();

                    return GenerateDeletedFeedbackWithSetting(feedbacks, settings);
                }
            }
        }

        public int CheckFeedbackDuplication(CheckFeedbackDuplicationCriteria criteria)
        {
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                objectDB.ExecuteNonQuery(criteria.GetSpParams());
                var outputParams = objectDB.GetOuputParams();
                var feedbackId = Converter.ToInt(outputParams["feedbackid"]);
                return feedbackId;
            }
        }

        private static IEnumerable<DeletedFeedback> GenerateDeletedFeedbackWithSetting(
            IEnumerable<DeletedFeedback> feedbacks,
            IEnumerable<SettingItem> settings)
        {
            Func<DeletedFeedback, List<SettingItem>, DeletedFeedback> regenerateDeletedFeedback = delegate (DeletedFeedback f, List<SettingItem> s)
            {
                f.Settings = s;
                return f;
            };
            var result = from f in feedbacks
                         join s in settings on f.FeedbackId equals s.FeedbackID
                         select new { f, s } into t1
                         group t1 by t1.f.FeedbackId into g
                         select regenerateDeletedFeedback(g.FirstOrDefault().f, g.Select(i => i.s).ToList());

            return result.ToList();
        }
    }
}