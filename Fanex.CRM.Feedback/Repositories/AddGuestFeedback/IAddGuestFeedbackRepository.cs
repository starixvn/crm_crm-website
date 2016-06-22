// --------------------------------------------------------------------------
// <copyright file="IAddGuestFeedbackRepository.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Feedback.Repositories.AddCustomerFeedback
{
    using Fanex.CRM.Feedback.Criteria;
    using Fanex.CRM.Infrastructure.Models;

    public interface IAddGuestFeedbackRepository
    {
        Result AddGuestFeedback(AddGuestFeedbackCriteria criteria);
    }
}
