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
                    var feebbacks = multi.Read<FeedbackDetail>();
                    var settings = multi.Read<SettingItem>();

                    feebbacks = feebbacks.Select(f =>
                    {
                        f.Settings = settings.Where(s => s.FeedbackID == f.FeedbackId).ToList();
                        return f;
                    });

                    criteria.TotalRows = feebbacks.ToList().Count;
                    return feebbacks;
                }
            }
        }

        public IEnumerable<DeletedFeedback> GetDeletedFeedbacks(ICriteria criteria)
        {
            using (ObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var multi = db.QueryMultiple(criteria.GetSpParams()))
                {
                    var feebbacks = multi.Read<DeletedFeedback>();
                    var settings = multi.Read<SettingItem>();

                    feebbacks = feebbacks.Select(f =>
                    {
                        f.Settings = settings.Where(s => s.FeedbackID == f.FeedbackId).ToList();
                        return f;
                    });

                    return feebbacks;
                }
            }
        }

        public Result UpdateSiteLicenseeTypes(UpdateSiteLicenseeTypesCriteria criteria)
        {
            return ExecuteNonQuery(criteria);
        }
    }
}
