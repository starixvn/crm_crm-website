// --------------------------------------------------------------------------
// <copyright file="UpdateCustomerFeedbackService.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.10.20">Created</change>
// </history>

namespace Fanex.CRM.Feedback.Services
{
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Feedback.Criteria;
    using Fanex.CRM.Feedback.Models;
    using Fanex.CRM.Feedback.Repositories;
    using Fanex.CRM.Feedback.Resources;
    using Fanex.CRM.Setting.Constants;
    using Fanex.CRM.Setting.Services;
    using System;
    using System.Linq;

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
                ExtensionList = _settingService.GetAllExtensions(),
                CommunicationToolList = _settingService.GetAllCommunicationTools(),
                Countries = countries,
                Languages = languages,
                StatusList = statusRawData,
                Roles = _settingService.GetAllRoles(),
                Responses = _settingService.GetAllResponses(),
                LicenseeTypes = _settingService.GetAllLicenseeTypes(),
                ProductNA = _settingService.GetProductNA(),
                SubProductNA = _settingService.GetSubProductNA(),
                BettypeNA = _settingService.GetBettypeNA(),
                ExtensionNA = _settingService.GetExtensionNA(),
                ProductList = _settingService.GetProducts(),
                SubProductList = _settingService.GetSubProducts(),
                BettypeList = _settingService.GetBettypes(),
                MainCategoryList = _settingService.GetMainCategories(),
                SubCategoryList = _settingService.GetSubCategories(),
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
            feedback.CustomerInfo.SiteSetting.AliasName = site == null ? null : site.AliasName;

            var licenseeType = settings.FirstOrDefault(s => s.Id == feedback.CustomerInfo.LicenseeType.Id);
            feedback.CustomerInfo.LicenseeType.Name = licenseeType == null ? Labels.Symbol_NA : licenseeType.Value;

            var language = settings.FirstOrDefault(s => s.Id == feedback.Language.Id);
            feedback.Language.Name = language == null ? Labels.Symbol_NA : language.Value;

            feedback.Response.Name = settings.First(s => s.Id == feedback.Response.Id).Value;
            feedback.Status.Name = settings.First(s => s.Id == feedback.Status.Id).Value;
            feedback.Role.Name = settings.First(s => s.Id == feedback.Role.Id).Value;
        }

        public void UpdateFeedback(UpdateCustomerFeedbackCriteria criteria)
        {
            _updateCustomerFeedbackRepsitory.UpdateFeedback(criteria);
        }
    }
}
