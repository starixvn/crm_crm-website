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
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Fanex.CRM.Common.Helpers;
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
        /// <summary>
        /// Gets the manager feedback.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The list of ManagerFeedback object.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        public IList<FeedbackDetail> GetManagerFeedback(ManageFeedbackCriteria criteria)
        {
            using (ObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                var inputParams = new Hashtable();
                inputParams.Add("fromdate", DateTimeUtilize.ChangeClientFromTimeToServerTime(criteria.FromDate));
                inputParams.Add("todate", DateTimeUtilize.ChangeClientToTimeToServerTime(criteria.ToDate));
                if (criteria.FeedbackId.HasValue && criteria.FeedbackId.Value > 0)
                {
                     inputParams.Add("feedbackid", criteria.FeedbackId.Value);
                }

                inputParams.Add("username", criteria.Username);
                if (criteria.IsGuest >= 0)
                {
                    inputParams.Add("isguest", criteria.IsGuest == 1);
                }

                if (criteria.CommunicationId > 0)
                {
                    inputParams.Add("communicationid", criteria.CommunicationId);
                }

                if (criteria.MainCategoryId > 0)
                {
                    inputParams.Add("maincategoryid", criteria.MainCategoryId);
                }

                if (criteria.SubCategoryId > 0)
                {
                    inputParams.Add("subcategoryid", criteria.SubCategoryId);
                }

                if (criteria.StatusId > 0)
                {
                    inputParams.Add("statusid", criteria.StatusId);
                }

                if (criteria.ResponseId > 0)
                {
                    inputParams.Add("responseid", criteria.ResponseId);
                }

                //// 0 : All
                //// 1 : Reviewed
                //// -1: Not reviewed
                if (criteria.IsReviewed == 1)
                {
                    inputParams.Add("isreviewed", true);
                }
                else if (criteria.IsReviewed == -1)
                {
                    inputParams.Add("isreviewed", false);
                }

                inputParams.Add("handledby", string.IsNullOrWhiteSpace(criteria.HandledBy) ? string.Empty : criteria.HandledBy);
                inputParams.Add("pageindex", criteria.PageIndex);
                inputParams.Add("itemsperpage", criteria.ItemsPerPage);
                inputParams.Add("totalrows", 0);

                db.AssignParamValues(inputParams);
                var feedbacks = db.Query<FeedbackDetail>();
                var outputParams = db.GetOuputParams();
                if (outputParams != null && outputParams.ContainsKey("totalrows"))
                {
                    object totalRows = outputParams["totalrows"];
                    criteria.TotalRows = totalRows == null ? 0 : (int)totalRows;
                }

                return criteria.TotalRows > 0 ? feedbacks.ToList() : null;
            }
        }

        /// <summary>
        /// Gets the deleted feedback.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The list of deleted feedbacks.</returns>
        public IEnumerable<DeletedFeedback> GetDeletedFeedback(ICriteria criteria)
        {
            using (ObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                return db.Query<DeletedFeedback>(criteria.GetSpParams());
            }
        }
    }
}
