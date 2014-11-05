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
    using System;
    using Fanex.CRM.Feedback.Criteria;
    using Fanex.CRM.Feedback.Repositories.AddCustomerFeedback;
    using Fanex.CRM.Feedback.Resources;
    using Fanex.CRM.Infrastructure.Constants;
    using Fanex.CRM.Infrastructure.Helpers;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Setting.Services.Implements;
    using Fanex.CRM.Setting.Services.Interfaces;

    /// <summary>
    /// Add Guest Feedback Service.
    /// </summary>
    public class AddGuestFeedbackService : IAddGuestFeedbackService
    {
        private ISettingService _serviceSetting;

        private IAddGuestFeedbackRepository _addGuestFeedbackRepository;

        public AddGuestFeedbackService()
            : this(null, null)
        {
        }

        public AddGuestFeedbackService(
            ISettingService serviceSetting,
            IAddGuestFeedbackRepository addGuestFeedbackRepository)
        {
            _serviceSetting = serviceSetting ?? new SettingService();
            _addGuestFeedbackRepository = addGuestFeedbackRepository ?? new AddGuestFeedbackRepository();
        }

        /// <summary>
        /// Adds the customer feedback.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Add result.</returns>
        public Result AddGuestFeedback(AddGuestFeedbackCriteria criteria)
        {
            try
            {
                var addResult = _addGuestFeedbackRepository.AddGuestFeedback(criteria);
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
