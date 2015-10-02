// --------------------------------------------------------------------------
// <copyright file="AddCustomerFeedbackRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Feedback.Repositories.AddCustomerFeedback
{
    using Fanex.Common;
    using Fanex.CRM.Common.Repositories;
    using Fanex.CRM.Feedback.Criteria;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.Data;

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
