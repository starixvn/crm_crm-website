// --------------------------------------------------------------------------
// <copyright file="IAddGuestFeedbackService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.06.03">Created</change>
// </history>
// --------------------------------------------------------------------------

namespace Fanex.CRM.Feedback.Services.AddCustomerFeedback
{
    using System.Collections.Generic;
    using Fanex.CRM.Feedback.Criteria;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Setting.Models;

    public interface IAddGuestFeedbackService
    {
        Result AddGuestFeedback(AddGuestFeedbackCriteria criteria);
    }
}
