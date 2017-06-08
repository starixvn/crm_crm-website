// --------------------------------------------------------------------------
// <copyright file="IFeedbackRepository.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.06.03">Created</change>
// </history>
// --------------------------------------------------------------------------

namespace Fanex.CRM.Feedback.Repositories
{
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Feedback.Criteria;
    using Fanex.CRM.Feedback.Models;
    using Fanex.Data.Repository;
    using Infrastructure.Models;
    using System.Collections.Generic;

    /// <summary>
    /// IFeedback Repository.
    /// </summary>
    public interface IFeedbackRepository
    {
        IEnumerable<FeedbackDetail> GetManagerFeedbacks(ManageFeedbackCriteria criteria);

        IEnumerable<DeletedFeedback> GetDeletedFeedbacks(ICriteria criteria);

        Result UpdateSiteLicenseeTypes(UpdateSiteLicenseeTypesCriteria criteria);
    }
}
