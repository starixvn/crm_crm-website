// --------------------------------------------------------------------------
// <copyright file="FeedbackRepository.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.06.03">Created</change>
// </history>
// --------------------------------------------------------------------------

namespace Fanex.CRM.Feedback.Repositories
{
    using Common.Repositories;
    using Fanex.Common;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Feedback.Criteria;
    using Fanex.CRM.Feedback.Models;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Infrastructure.Models;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    /// <summary>
    /// Feedback Repository.
    /// </summary>
    public class FeedbackRepository : BaseRepository, IFeedbackRepository
    {
        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        public IEnumerable<FeedbackDetail> GetManagerFeedbacks(ManageFeedbackCriteria criteria)
        {
            using (ObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var multi = db.QueryMultiple(criteria.GetSpParams()))
                {
                    var feedbacks = multi.Read<FeedbackDetail>();
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

                    var result = new List<FeedbackDetail>();
                    foreach (var i in groups)
                    {
                        i.feedback.Settings = i.settings.ToList();
                        result.Add(i.feedback);
                    }

                    var outputParams = db.GetOuputParams();
                    if (outputParams.Count != 0)
                    {
                        criteria.TotalRows = Converter.ToInt(outputParams["totalrows"]);
                    }

                    return result;
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

        public Result UpdateSiteLicenseeTypes(UpdateSiteLicenseeTypesCriteria criteria)
        {
            return ExecuteNonQuery(criteria);
        }
    }
}
