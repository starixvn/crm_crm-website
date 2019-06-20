namespace Fanex.CRM.Feedback.Services
{
    using Common.Enumerations;
    using Common.Services;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Feedback.Criteria;
    using Fanex.CRM.Feedback.Models;
    using Fanex.CRM.Feedback.Repositories;
    using Fanex.CRM.Feedback.Resources;
    using Fanex.CRM.Setting.Constants;
    using Fanex.CRM.Setting.Services;
    using System;
    using System.Linq;

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

        public dynamic GetAllSettings(bool hasDeleteFeedbackPermission, UserRole userRole)
        {
            var countries = _settingService.GetAllCountries();
            if (AppSession.CurrentUser.UserRole != UserRole.CN88Staff)
            {
                countries.Insert(0, new Country { Id = 0, Name = Labels.Symbol_NA });
            }

            var languages = _settingService.GetAllLanguages();
            languages.Insert(0, new Setting { Id = 0, Name = Labels.Symbol_NA });

            var statusRawData = _settingService.GetAllStatuses(ViewTypeConstants.CommunicationTool);
            if (!hasDeleteFeedbackPermission)
            {
                var deletedStatus = statusRawData.SingleOrDefault(s => string.Equals(s.Name, Labels.Deleted, StringComparison.OrdinalIgnoreCase));
                if (deletedStatus != null)
                {
                    statusRawData.Remove(deletedStatus);
                }
            }

            var sites = userRole == UserRole.CN88Staff
                ? _settingService.GetAllSiteWithGroupCN88()
                : _settingService.GetAllSiteWithGroup().Where(s => s.IsActive);
            return new
            {
                Titles = _settingService.GetAllTitles(),
                Sites = sites,
                ExtensionList = _settingService.GetAllExtensions(),
                CommunicationToolList = _settingService.GetAllCommunicationTools(),
                Countries = countries,
                Provinces = _settingService.GetAllProvinces(),
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
            if (AppSession.CurrentUser.UserRole == UserRole.CN88Staff)
            {
                feedback.Country.Id = CountryConstants.CN88CountryChina;
            }

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
