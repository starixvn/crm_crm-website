// --------------------------------------------------------------------------
// <copyright file="IFeedbackService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.06.03">Created</change>
// </history>
// --------------------------------------------------------------------------

namespace Fanex.CRM.Feedback.Services
{
    using System.Collections.Generic;
    using Fanex.CRM.Feedback.Criteria;
    using Fanex.CRM.Feedback.Models;
    using Fanex.CRM.Infrastructure.Models;

    /// <summary>
    /// IFeedback Service.
    /// </summary>
    public interface IFeedbackService
    {
        IList<BaseModel> GetTop { get; }

        dynamic GetAllSetting();

        IList<ManageFeedback> GetManageFeedback(ManageFeedbackCriteria criteria);

        IList<LatestFeedback> GetLatestFeedback(LatestFeedbackCriteria criteria);

        IList<DeletedFeedback> GetDeletedFeedback(DeletedFeedbackCriteria criteria);
    }
}
