// --------------------------------------------------------------------------
// <copyright file="AddCustomerFeedbackService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.06.03">Created</change>
// </history>
// --------------------------------------------------------------------------

namespace Fanex.CRM.Feedback.Services.AddCustomerFeedback
{
    using System;
    using Fanex.CRM.Feedback.Criteria;
    using Fanex.CRM.Feedback.Repositories.AddCustomerFeedback;
    using Fanex.CRM.Feedback.Resources;
    using Fanex.CRM.Infrastructure.Constants;
    using Fanex.CRM.Infrastructure.Helpers;
    using Fanex.CRM.Infrastructure.Models;

    /// <summary>
    /// Add Customer Feedback Service.
    /// </summary>
    public class AddCustomerFeedbackService : IAddCustomerFeedbackService
    {
        private IAddCustomerFeedbackRepository _addCustomerFeedbackRepository;

        public AddCustomerFeedbackService()
            : this(null)
        {
        }

        public AddCustomerFeedbackService(
            IAddCustomerFeedbackRepository addCustomerFeedbackRepository)
        {
            _addCustomerFeedbackRepository = addCustomerFeedbackRepository ?? new AddCustomerFeedbackRepository();
        }

        /// <summary>
        /// Adds the customer feedback.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Add result.</returns>
        public Result AddCustomerFeedback(AddCustomerFeedbackCriteria criteria)
        {
            try
            {
                var addResult = _addCustomerFeedbackRepository.AddCustomerFeedback(criteria);
                if (addResult != null && addResult.Code == ErrorCodeConstants.NoneError)
                {
                    addResult.Message = Labels.Feedback_AddCustomerFeedback_AddSuccess;
                }

                return addResult;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }
    }
}
