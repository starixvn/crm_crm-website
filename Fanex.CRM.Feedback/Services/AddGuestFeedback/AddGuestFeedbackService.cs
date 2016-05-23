// --------------------------------------------------------------------------
// <copyright file="AddGuestFeedbackService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
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
    /// Add Guest Feedback Service.
    /// </summary>
    public class AddGuestFeedbackService : IAddGuestFeedbackService
    {
        private IAddGuestFeedbackRepository _addGuestFeedbackRepository;

        public AddGuestFeedbackService()
            : this(null)
        {
        }

        public AddGuestFeedbackService(
            IAddGuestFeedbackRepository addGuestFeedbackRepository)
        {
            _addGuestFeedbackRepository = addGuestFeedbackRepository ?? new AddGuestFeedbackRepository();
        }

        public Result AddGuestFeedback(AddGuestFeedbackCriteria criteria)
        {
            var result = _addGuestFeedbackRepository.AddGuestFeedback(criteria);
            if (result.Code == ErrorCodeConstants.NoneError)
            {
                result.Message = Labels.Feedback_AddCustomerFeedback_AddSuccess;
            }

            return result;
        }
    }
}
