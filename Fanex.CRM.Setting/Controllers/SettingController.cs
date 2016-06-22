// --------------------------------------------------------------------------
// <copyright file="SettingController.cs" company="">
// Copyright (c) . All rights reserved.
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
    public class SettingController : ReadOnlyBaseController
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
            var allSite = _settingServiceInstance.GetAllSiteWithGroup();
            return Json(allSite, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AllTitle()
        {
            var allTitle = _settingServiceInstance.GetAllTitles();
            return Json(allTitle, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AllCountry()
        {
            var allCountry = _settingServiceInstance.GetAllCountries();
            return Json(allCountry, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AllLanguage()
        {
            var allLanguage = _settingServiceInstance.GetAllLanguages();
            return Json(allLanguage, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AllExtension()
        {
            var allExtension = _settingServiceInstance.GetAllExtensions();
            return Json(allExtension, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AllCommunicationTool()
        {
            var allCommunicationTool = _settingServiceInstance.GetAllCommunicationTools();
            return Json(allCommunicationTool, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AllMainCategory()
        {
            var allCategory = _settingServiceInstance.GetAllMainCategories();
            return Json(allCategory, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AllProduct()
        {
            var allProduct = _settingServiceInstance.GetAllSubProducts();
            return Json(allProduct, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AllStatus()
        {
            var allStatus = _settingServiceInstance.GetAllStatuses(ViewTypeConstants.All);
            return Json(allStatus, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AllRole()
        {
            var allRole = _settingServiceInstance.GetAllRoles();
            return Json(allRole, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AllResponse()
        {
            var allResponse = _settingServiceInstance.GetAllResponses();
            return Json(allResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult NumberOfFeedbackSubjectTemplate()
        {
            var numberOfFeedbackSubjectTemplate = _settingServiceInstance.NumberOfFeedbackSubjectTemplate();
            return Json(numberOfFeedbackSubjectTemplate, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetFeedbackSubjectTemplateByPagingIndex(GetFeedbackSubjectTemplateByPagingIndexCriteria criteria)
        {
            var feedbackSubjectTemplateByPagingIndex = _settingServiceInstance.GetFeedbackSubjectTemplateByPagingIndex(criteria);
            return Json(feedbackSubjectTemplateByPagingIndex, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAllLogAction(GetActionCriteria criteria)
        {
            var actions = _settingServiceInstance.GetAllLogActions(criteria);
            return Json(actions, JsonRequestBehavior.AllowGet);
        }
    }
}