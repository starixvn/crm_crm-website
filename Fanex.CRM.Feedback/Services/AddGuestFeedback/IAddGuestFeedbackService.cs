// --------------------------------------------------------------------------
// <copyright file="IAddGuestFeedbackService.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.06.03">Created</change>
// </history>
// --------------------------------------------------------------------------

namespace Fanex.CRM.Feedback.Services.AddCustomerFeedback
{
    using Fanex.CRM.Feedback.Criteria;
    using Fanex.CRM.Infrastructure.Models;

    public interface IAddGuestFeedbackService
    {
        Result AddGuestFeedback(AddGuestFeedbackCriteria criteria);
    }
}
