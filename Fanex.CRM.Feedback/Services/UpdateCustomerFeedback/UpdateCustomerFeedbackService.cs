// --------------------------------------------------------------------------
// <copyright file="UpdateCustomerFeedbackService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.10.20">Created</change>
// </history>

namespace Fanex.CRM.Feedback.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Feedback.Criteria;
    using Fanex.CRM.Feedback.Models;
    using Fanex.CRM.Feedback.Repositories;
    using Fanex.CRM.Feedback.Resources;
    using Fanex.CRM.Setting.Constants;
    using Fanex.CRM.Setting.Services;

    /// <summary>
    /// Update customer feedback service.
    /// </summary>
    public class UpdateCustomerFeedbackService : IUpdateCustomerFeedbackService
    {
        private readonly IUpdateCustomerFeedbackRepository _updateCustomerFeedbackRepsitory;
        private readonly ISettingService _settingService;

        public UpdateCustomerFeedbackService()
            : this(null, null)
        {
        }

        public UpdateCustomerFeedbackService(IUpdateCustomerFeedbackRepository updateCustomerFeedbackRepsitory, ISettingService settingService)
        {
            _updateCustomerFeedbackRepsitory = updateCustomerFeedbackRepsitory ?? new UpdateCustomerFeedbackRepository();
            _settingService = settingService ?? new SettingService();
        }

        /// <summary>
        /// Gets all settings.
        /// </summary>
        /// <param name="hasDeleteFeedbackPermission">If set to <c>true</c> [has delete feedback permission].</param>
        /// <returns>
        /// The object of all settings.
        /// </returns>
        public dynamic GetAllSettings(bool hasDeleteFeedbackPermission)
        {
            var countries = _settingService.AllCountry();
            countries.Insert(0, new CountryInfo { Id = 0, Name = Labels.Symbol_NA });

            var languages = _settingService.AllLanguage();
            languages.Insert(0, new SettingInfo { Id = 0, Name = Labels.Symbol_NA });

            var statusRawData = _settingService.AllStatus(ViewTypeConstants.CommunicationTool);
            if (!hasDeleteFeedbackPermission)
            {
                var deletedStatus = statusRawData.SingleOrDefault(s => string.Equals(s.Name, Labels.Deleted, StringComparison.OrdinalIgnoreCase));
                if (deletedStatus != null)
                {
                    statusRawData.Remove(deletedStatus);    
                }
            }

            return new
            {
                Titles = _settingService.AllTitle(),
                Sites = _settingService.AllSiteWithGroup(),
                Extensions = _settingService.AllExtension(),
                Communications = _settingService.AllCommunicationTool(),
                Categories = _settingService.AllMainCategory(),
                Products = _settingService.AllProduct(),
                Countries = countries,
                Languages = languages,
                StatusList = statusRawData,
                Roles = _settingService.AllRole(),
                Responses = _settingService.AllResponse(),
                LicenseeTypes = _settingService.AllLicenseeType()
            };
        }

        public CustomerFeedback GetFeedbackById(GetFeedbackByIdCriteria criteria)
        {
            return _updateCustomerFeedbackRepsitory.GetFeedbackById(criteria);
        }

        public void UpdateFeedback(UpdateCustomerFeedbackCriteria criteria)
        {
            _updateCustomerFeedbackRepsitory.UpdateFeedback(criteria);
        }
    }
}
