// --------------------------------------------------------------------------
// <copyright file="FeedbackRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.06.03">Created</change>
// </history>
// --------------------------------------------------------------------------

namespace Fanex.CRM.Feedback.Repositories
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Feedback.Criteria;
    using Fanex.CRM.Feedback.Models;
    using Fanex.Data;
    using Fanex.Data.Repository;

    /// <summary>
    /// Feedback Repository.
    /// </summary>
    public class FeedbackRepository : IFeedbackRepository
    {
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        public IEnumerable<FeedbackDetail> GetManagerFeedbacks(ManageFeedbackCriteria criteria)
        {
            using (ObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {                
                var feebbacks = db.Query<FeedbackDetail>(criteria.GetSpParams());
                var outputParams = db.GetOuputParams();
                if (outputParams.ContainsKey("totalrows"))
                {
                    object totalRows = outputParams["totalrows"];
                    criteria.TotalRows = totalRows == null ? 0 : (int)totalRows;
                }

                return criteria.TotalRows > 0 ? feebbacks : new List<FeedbackDetail>();
            }
        }

        public IEnumerable<DeletedFeedback> GetDeletedFeedbacks(ICriteria criteria)
        {
            using (ObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                return db.Query<DeletedFeedback>(criteria.GetSpParams());
            }
        }
    }
}
