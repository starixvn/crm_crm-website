// --------------------------------------------------------------------------
// <copyright file="AddCustomerFeedbackRepository.cs" company="">
// Copyright (c) . All rights reserved.
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
    /// Add Customer Feedback Repository.
    /// </summary>
    public class AddCustomerFeedbackRepository : BaseRepository, IAddCustomerFeedbackRepository
    {
        public Result AddCustomerFeedback(AddCustomerFeedbackCriteria criteria)
        {
            return ExecuteNonQuery(criteria);
        }
    }
}
