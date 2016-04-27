// --------------------------------------------------------------------------
// <copyright file="AddFeedbackController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.06.03">Created</change>
// </history>
// --------------------------------------------------------------------------

namespace Fanex.CRM.Feedback.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.CRM.Common.Attributes;
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Controllers;
    using Fanex.CRM.Common.Enumerations;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Feedback.Models;
    using Fanex.CRM.Feedback.Resources;
    using Fanex.CRM.Setting.Constants;
    using Fanex.CRM.Setting.Services;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// Add Feedback Controller.
    /// </summary>
    [NapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Read)]
    public class AddFeedbackController : BaseController
    {
        public AddFeedbackController()
            : this(null)
        {
        }

        public AddFeedbackController(ISettingService serviceSetting)
        {
            ServiceSetting = serviceSetting ?? new SettingService();
        }

        public ISettingService ServiceSetting { get; set; }

        [HttpPost]
        public ActionResult AllSetting()
        {
            var settings = AllSettingToAddFeedback();
            bool hasCreateFeedbackPermission = HasPermission(FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Constant.Policy.Host);
            if (hasCreateFeedbackPermission)
            {
                settings.StatusList = settings.StatusList.Where(s => !s.Name.Equals("Deleted", System.StringComparison.OrdinalIgnoreCase));
            }

            return Json(settings, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets all setting for edit.
        /// </summary>
        /// <returns>All setting to add feedback.</returns>
        public AllSettingToAddFeedback AllSettingToAddFeedback()
        {
            var languages = ServiceSetting.GetAllLanguages().ToList();
            languages.Insert(
                0,
                new Setting
                {
                    Id = 0,
                    Name = Labels.Symbol_NA
                });

            var countries = ServiceSetting.GetAllCountries().ToList();
            countries.Insert(
                0,
                new Country
                {
                    Id = 0,
                    Name = Labels.Symbol_NA
                });

            var statusList = ServiceSetting.GetAllStatuses(ViewTypeConstants.CommunicationTool)
                                        .Where(s => s.Id != (int)CRMStatus.Deleted).ToList();
            var settings = new AllSettingToAddFeedback
            {
                ExtensionList = ServiceSetting.GetAllExtensions(),
                CountryList = countries,
                CommunicationToolList = ServiceSetting.GetAllCommunicationTools(),
                LanguageList = languages,
                MainCategoryList = ServiceSetting.GetAllMainCategories(),
                ProductList = ServiceSetting.GetAllProducts(),
                SiteList = ServiceSetting.GetAllSiteWithGroup(),
                ResponseList = ServiceSetting.GetAllResponses(),
                StatusList = statusList,
                TitleList = ServiceSetting.GetAllTitles(),
                LicenseeType = ServiceSetting.GetAllLicenseeTypes()
            };

            return settings;
        }
    }
}
