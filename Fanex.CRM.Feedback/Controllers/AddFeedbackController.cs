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
    using System.Web.Mvc;
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Controllers;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Feedback.Models;
    using Fanex.CRM.Feedback.Resources;
    using Fanex.CRM.Setting.Services;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// Add Feedback Controller.
    /// </summary>
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
        [NapAuth(FunctionKey = FunctionPermission.EditFeedback, Policy = Constant.Policy.Read)]
        public ActionResult AllSetting()
        {
            return Json(AllSettingToAddFeedback(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets all setting for edit.
        /// </summary>
        /// <returns>All setting to add feedback.</returns>
        public AllSettingToAddFeedback AllSettingToAddFeedback()
        {
            var languageList = ServiceSetting.AllLanguage();
            languageList.Insert(
                0,
                new SettingInfo
                {
                    Id = 0,
                    Name = Labels.Symbol_NA
                });

            var countryList = ServiceSetting.AllCountry();
            countryList.Insert(
                0,
                new CountryInfo
                {
                    Id = 0,
                    Name = Labels.Symbol_NA
                });

            var allSetting = new AllSettingToAddFeedback
            {
                ExtensionList = ServiceSetting.AllExtension(),
                CountryList = countryList,
                CommunicationToolList = ServiceSetting.AllCommunicationTool(),
                LanguageList = languageList,
                MainCategoryList = ServiceSetting.AllMainCategory(),
                ProductList = ServiceSetting.AllProduct(),
                SiteList = ServiceSetting.AllSite(),
                ResponseList = ServiceSetting.AllResponse(),
                StatusList = ServiceSetting.AllFeedbackStatus(),
                TitleList = ServiceSetting.AllTitle()
            };

            return allSetting;
        }
    }
}
