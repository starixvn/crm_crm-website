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
                var deleteStatus = settings.StatusList.Find(s => s.Name == "Deleted");
                if (deleteStatus != null)
                {
                    settings.StatusList.Remove(deleteStatus);
                }
            }

            return Json(settings, JsonRequestBehavior.AllowGet);
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

            var statusList = ServiceSetting.AllStatus(ViewTypeConstants.CommunicationTool)
                                        .Where(s => s.Id != (int)CRMStatus.Deleted).ToList();
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
                StatusList = statusList,
                TitleList = ServiceSetting.AllTitle(),
                LicenseeType = ServiceSetting.AllLicenseeType()
            };

            return allSetting;
        }
    }
}
