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
            var countries = _settingService.GetAllCountries().ToList();
            countries.Insert(0, new Country { Id = 0, Name = Labels.Symbol_NA });

            var languages = _settingService.GetAllLanguages().ToList();
            languages.Insert(0, new Setting { Id = 0, Name = Labels.Symbol_NA });

            var statusRawData = _settingService.GetAllStatuses(ViewTypeConstants.CommunicationTool).ToList();
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
                Titles = _settingService.GetAllTitles(),
                Sites = _settingService.GetAllSiteWithGroup().Where(s => s.IsActive),
                Extensions = _settingService.GetAllExtensions(),
                Communications = _settingService.GetAllCommunicationTools(),
                Categories = _settingService.GetAllMainCategories(),
                Products = _settingService.GetAllProducts(),
                Countries = countries,
                Languages = languages,
                StatusList = statusRawData,
                Roles = _settingService.GetAllRoles(),
                Responses = _settingService.GetAllResponses(),
                LicenseeTypes = _settingService.GetAllLicenseeTypes()
            };
        }

        public CustomerFeedback GetFeedbackById(GetFeedbackByIdCriteria criteria)
        {           
            var feedback = _updateCustomerFeedbackRepsitory.GetFeedbackById(criteria);

            UpdateSettingNameForFeedback(feedback);

            return feedback;
        }

        private void UpdateSettingNameForFeedback(CustomerFeedback feedback)
        {
            var settings = _settingService.GetAllSettings();
            var siteSettings = _settingService.GetAllSiteWithGroup().Where(s => !s.IsActive);

            var title = settings.FirstOrDefault(s => s.Id == feedback.CustomerInfo.Title.Id);
            feedback.CustomerInfo.Title.Name = title == null ? null : title.Value;

            var site = siteSettings.FirstOrDefault(s => s.Id == feedback.CustomerInfo.SiteSetting.Id);
            feedback.CustomerInfo.SiteSetting.Name = site == null ? null : site.Name;

            var licenseeType = settings.FirstOrDefault(s => s.Id == feedback.CustomerInfo.LicenseeType.Id);
            feedback.CustomerInfo.LicenseeType.Name = licenseeType == null ? Labels.Symbol_NA : licenseeType.Value;

            var communication = settings.FirstOrDefault(s => s.Id == feedback.Communication.Id);
            feedback.Communication.Name = communication == null ? null : communication.Value;

            var product = settings.FirstOrDefault(s => s.Id == feedback.Product.Id);
            feedback.Product.Name = product == null ? null : product.Value;

            var subProduct = settings.FirstOrDefault(s => s.Id == feedback.SubProduct.Id);
            feedback.SubProduct.Name = subProduct == null ? Labels.Symbol_NA : subProduct.Value;

            var language = settings.FirstOrDefault(s => s.Id == feedback.Language.Id);
            feedback.Language.Name = language == null ? Labels.Symbol_NA : language.Value;

            var bettype = settings.FirstOrDefault(s => s.Id == feedback.BetType.Id);
            feedback.BetType.Name = bettype == null ? Labels.Symbol_NA : bettype.Value;           

            feedback.Response.Name = settings.First(s => s.Id == feedback.Response.Id).Value;
            feedback.Category.Name = settings.First(s => s.Id == feedback.Category.Id).Value;
            feedback.Status.Name = settings.First(s => s.Id == feedback.Status.Id).Value;
            feedback.Extension.Name = settings.First(s => s.Id == feedback.Extension.Id).Value;
            feedback.Role.Name = settings.First(s => s.Id == feedback.Role.Id).Value;
            feedback.SubCategory.Name = settings.First(s => s.Id == feedback.SubCategory.Id).Value;                      
        }

        public void UpdateFeedback(UpdateCustomerFeedbackCriteria criteria)
        {
            _updateCustomerFeedbackRepsitory.UpdateFeedback(criteria);
        }
    }
}
