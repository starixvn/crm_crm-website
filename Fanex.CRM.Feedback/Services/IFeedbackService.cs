// --------------------------------------------------------------------------
// <copyright file="IFeedbackService.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.06.03">Created</change>
// </history>
// --------------------------------------------------------------------------

namespace Fanex.CRM.Feedback.Services
{
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Feedback.Criteria;
    using Fanex.CRM.Feedback.Models;
    using Fanex.CRM.Infrastructure.Models;
    using System.Collections.Generic;

    /// <summary>
    /// IFeedback Service.
    /// </summary>
    public interface IFeedbackService
    {
        IEnumerable<BaseModel> GetTopItems { get; }

        dynamic GetAllSettings();

        IEnumerable<FeedbackDetail> GetManageFeedbacks(ManageFeedbackCriteria criteria);

        IEnumerable<DeletedFeedback> GetLatestFeedbackReport(LatestFeedbackCriteria criteria);

        IEnumerable<DeletedFeedback> GetDeletedFeedbackReport(DeletedFeedbackCriteria criteria);

        Result UpdateSiteLicenseeTypes(UpdateSiteLicenseeTypesCriteria criteria);
    }
}
