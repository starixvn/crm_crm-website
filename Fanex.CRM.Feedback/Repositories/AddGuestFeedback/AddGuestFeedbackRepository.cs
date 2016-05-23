// --------------------------------------------------------------------------
// <copyright file="AddGuestFeedbackRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Feedback.Repositories.AddCustomerFeedback
{
    using Fanex.CRM.Common.Repositories;
    using Fanex.CRM.Feedback.Criteria;
    using Fanex.CRM.Infrastructure.Models;

    /// <summary>
    /// Add Guest Feedback Repository.
    /// </summary>
    public class AddGuestFeedbackRepository : BaseRepository, IAddGuestFeedbackRepository
    {
        public Result AddGuestFeedback(AddGuestFeedbackCriteria criteria)
        {
            return ExecuteNonQuery(criteria);
        }
    }
}
