// --------------------------------------------------------------------------
// <copyright file="AddCustomerFeedbackService.cs" company="">
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
    using Fanex.CRM.Feedback.Repositories.AddCustomerFeedback;
    using Fanex.CRM.Feedback.Resources;
    using Fanex.CRM.Infrastructure.Constants;
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
            var result = _addCustomerFeedbackRepository.AddCustomerFeedback(criteria);
            if (result.Code == ErrorCodeConstants.NoneError)
            {
                result.Message = Labels.Feedback_AddCustomerFeedback_AddSuccess;
            }

            return result;
        }
    }
}
