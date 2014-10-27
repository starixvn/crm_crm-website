// --------------------------------------------------------------------------
// <copyright file="IUpdateCustomerFeedbackService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.10.20">Created</change>
// </history>

namespace Fanex.CRM.Feedback.Services
{
    using Fanex.CRM.Feedback.Criteria;
    using Fanex.CRM.Feedback.Models;

    public interface IUpdateCustomerFeedbackService
    {
        dynamic GetAllSettings(bool hasDeleteFeedbackPermission);

        CustomerFeedback GetFeedbackById(GetFeedbackByIdCriteria criteria);

        void UpdateFeedback(UpdateCustomerFeedbackCriteria criteria);
    }
}
