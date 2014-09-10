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
    using System.Collections.Generic;
    using Fanex.CRM.Feedback.Criteria;
    using Fanex.CRM.Feedback.Repositories.AddCustomerFeedback;
    using Fanex.CRM.Feedback.Resources;
    using Fanex.CRM.Infrastructure.Common;
    using Fanex.CRM.Infrastructure.Logging;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Setting.Constants;
    using Fanex.CRM.Setting.Criteria;
    using Fanex.CRM.Setting.Models;
    using Fanex.CRM.Setting.Services.Implements;
    using Fanex.CRM.Setting.Services.Interfaces;

    /// <summary>
    /// Add Customer Feedback Service.
    /// </summary>
    public class AddCustomerFeedbackService : IAddCustomerFeedbackService
    {
        private ISettingService _serviceSetting;

        private IAddCustomerFeedbackRepository _addCustomerFeedbackRepository;

        public AddCustomerFeedbackService()
            : this(null, null)
        {
        }

        public AddCustomerFeedbackService(
            ISettingService serviceSetting,
            IAddCustomerFeedbackRepository addCustomerFeedbackRepository)
        {
            _serviceSetting = serviceSetting ?? new SettingService();
            _addCustomerFeedbackRepository = addCustomerFeedbackRepository ?? new AddCustomerFeedbackRepository();
        }

        /// <summary>
        /// Gets all status.
        /// </summary>
        /// <returns>
        /// All status.
        /// </returns>
        public List<SettingInfo> GetAllStatus()
        {
            try
            {
                var allStatusResult = new List<SettingInfo>();
                var allStatusSetting = _serviceSetting.GetAllStatus(new GetAllStatusCriteria
                {
                    ViewType = ViewTypeConstants.CommunicationTool
                });

                if (allStatusSetting == null)
                {
                    return allStatusResult;
                }

                foreach (var status in allStatusSetting)
                {
                    if (IsIncludedStatus(status.Name))
                    {
                        allStatusResult.Add(status);
                    }
                }

                return allStatusResult;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
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

        private bool IsIncludedStatus(string statusName)
        {
            return string.Equals(statusName, Labels.Checked, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(statusName, Labels.Solved, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(statusName, Labels.Closed, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(statusName, Labels.Pending, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(statusName, Labels.Deleted, StringComparison.OrdinalIgnoreCase);
        }
    }
}
