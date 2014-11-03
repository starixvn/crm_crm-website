// --------------------------------------------------------------------------
// <copyright file="IFeedbackRepository.cs" company="Nexcel Solutions Vietnam">
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
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Feedback.Criteria;
    using Fanex.CRM.Feedback.Models;
    using Fanex.Data.Repository;

    /// <summary>
    /// IFeedback Repository.
    /// </summary>
    public interface IFeedbackRepository
    {
        IList<FeedbackDetail> GetManagerFeedback(ManageFeedbackCriteria criteria);

        IEnumerable<DeletedFeedback> GetDeletedFeedback(ICriteria criteria);
    }
}
