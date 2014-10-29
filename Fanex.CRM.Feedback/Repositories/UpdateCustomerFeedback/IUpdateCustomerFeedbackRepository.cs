// --------------------------------------------------------------------------
// <copyright file="IUpdateCustomerFeedbackRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.10.20">Created</change>
// </history>

namespace Fanex.CRM.Feedback.Repositories
{
    using Fanex.CRM.Feedback.Criteria;
    using Fanex.CRM.Feedback.Models;
    using Fanex.Data.Repository;

    public interface IUpdateCustomerFeedbackRepository
    {
        CustomerFeedback GetFeedbackById(ICriteria criteria);

        void UpdateFeedback(UpdateCustomerFeedbackCriteria criteria);
    }
}
