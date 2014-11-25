// --------------------------------------------------------------------------
// <copyright file="SettingController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Setting.Controllers
{
    using System.Web.Mvc;
    using Fanex.CRM.Common.Controllers;
    using Fanex.CRM.Setting.Constants;
    using Fanex.CRM.Setting.Criteria;
    using Fanex.CRM.Setting.Services;

    /// <summary>
    /// Setting Controller.
    /// </summary>
    public class SettingController : BaseController
    {
        private ISettingService _settingServiceInstance;

        public SettingController()
            : this(null)
        {
        }

        public SettingController(ISettingService settingServiceInstance)
        {
            _settingServiceInstance = settingServiceInstance ?? new SettingService();
        }

        [HttpPost]
        public JsonResult AllSite()
        {
            var allSite = _settingServiceInstance.AllSite();
            var jsonResult = Json(allSite, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        public JsonResult AllTitle()
        {
            var allTitle = _settingServiceInstance.AllTitle();
            var jsonResult = Json(allTitle, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        public JsonResult AllCountry()
        {
            var allCountry = _settingServiceInstance.AllCountry();
            var jsonResult = Json(allCountry, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        public JsonResult AllLanguage()
        {
            var allLanguage = _settingServiceInstance.AllLanguage();
            var jsonResult = Json(allLanguage, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        public JsonResult AllExtension()
        {
            var allExtension = _settingServiceInstance.AllExtension();
            var jsonResult = Json(allExtension, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        public JsonResult AllCommunicationTool()
        {
            var allCommunicationTool = _settingServiceInstance.AllCommunicationTool();
            return Json(allCommunicationTool, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AllMainCategory()
        {
            var allCategory = _settingServiceInstance.AllMainCategory();
            var jsonResult = Json(allCategory, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        public JsonResult AllProduct()
        {
            var allProduct = _settingServiceInstance.AllProduct();
            var jsonResult = Json(allProduct, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        public JsonResult AllStatus()
        {
            var allStatus = _settingServiceInstance.AllStatus(ViewTypeConstants.All);
            var jsonResult = Json(allStatus, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        public JsonResult AllRole()
        {
            var allRole = _settingServiceInstance.AllRole();
            var jsonResult = Json(allRole, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        public JsonResult AllResponse()
        {
            var allResponse = _settingServiceInstance.AllResponse();
            var jsonResult = Json(allResponse, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        public JsonResult NumberOfFeedbackSubjectTemplate()
        {
            var numberOfFeedbackSubjectTemplate = _settingServiceInstance.NumberOfFeedbackSubjectTemplate();
            var jsonResult = Json(numberOfFeedbackSubjectTemplate, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetFeedbackSubjectTemplateByPagingIndex(GetFeedbackSubjectTemplateByPagingIndexCriteria criteria)
        {
            var feedbackSubjectTemplateByPagingIndex = _settingServiceInstance.FeedbackSubjectTemplateByPagingIndex(criteria);
            var jsonResult = Json(feedbackSubjectTemplateByPagingIndex, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpGet]
        public JsonResult GetAllLogAction(GetActionCriteria criteria)
        {
            var actions = _settingServiceInstance.AllLogAction(criteria);
            return Json(actions, JsonRequestBehavior.AllowGet);
        }
    }
}
