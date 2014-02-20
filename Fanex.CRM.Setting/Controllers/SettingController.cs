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
    using Fanex.CRM.Infrastructure.Web.Controllers;
    using Fanex.CRM.Setting.Criteria;
    using Fanex.CRM.Setting.Services.Implements;
    using Fanex.CRM.Setting.Services.Interfaces;

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
        public JsonResult GetAllSite()
        {
            var allSite = _settingServiceInstance.GetAllSite();
            var jsonResult = Json(allSite, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetAllTitle()
        {
            var allTitle = _settingServiceInstance.GetAllTitle();
            var jsonResult = Json(allTitle, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetAllCountry()
        {
            var allCountry = _settingServiceInstance.GetAllCountry();
            var jsonResult = Json(allCountry, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetAllLanguage()
        {
            var allLanguage = _settingServiceInstance.GetAllLanguage();
            var jsonResult = Json(allLanguage, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetAllExtension()
        {
            var allExtension = _settingServiceInstance.GetAllExtension();
            var jsonResult = Json(allExtension, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetAllCommunicationTool()
        {
            var allCommunicationTool = _settingServiceInstance.GetAllCommunicationTool();
            return Json(allCommunicationTool, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetAllMainCategory()
        {
            var allCategory = _settingServiceInstance.GetAllMainCategory();
            var jsonResult = Json(allCategory, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetAllProduct()
        {
            var allProduct = _settingServiceInstance.GetAllProduct();
            var jsonResult = Json(allProduct, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetAllStatus()
        {
            var allStatus = _settingServiceInstance.GetAllStatus();
            var jsonResult = Json(allStatus, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetAllRole()
        {
            var allRole = _settingServiceInstance.GetAllRole();
            var jsonResult = Json(allRole, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetAllResponse()
        {
            var allResponse = _settingServiceInstance.GetAllResponse();
            var jsonResult = Json(allResponse, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetNumberOfFeedbackSubjectTemplate()
        {
            var numberOfFeedbackSubjectTemplate = _settingServiceInstance.GetNumberOfFeedbackSubjectTemplate();
            var jsonResult = Json(numberOfFeedbackSubjectTemplate, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetFeedbackSubjectTemplateByPagingIndex(GetFeedbackSubjectTemplateByPagingIndexCriteria criteria)
        {
            var feedbackSubjectTemplateByPagingIndex = _settingServiceInstance.GetFeedbackSubjectTemplateByPagingIndex(criteria);
            var jsonResult = Json(feedbackSubjectTemplateByPagingIndex, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpGet]
        public JsonResult GetAllLogAction(GetActionCriteria criteria)
        {
            var actions = _settingServiceInstance.GetAllLogAction(criteria);
            return Json(actions, JsonRequestBehavior.AllowGet);
        }
    }
}
