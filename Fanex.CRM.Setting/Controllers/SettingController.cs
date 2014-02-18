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
        private ISettingService _serviceInstance;

        public SettingController()
            : this(null)
        {
        }

        public SettingController(ISettingService serviceInstance)
        {
            _serviceInstance = serviceInstance ?? new SettingService();
        }

        [HttpPost]
        public JsonResult GetAllSite()
        {
            var result = _serviceInstance.GetAllSite();
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetAllTitle()
        {
            var result = _serviceInstance.GetAllTitle();
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetAllCountry()
        {
            var result = _serviceInstance.GetAllCountry();
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetAllLanguage()
        {
            var result = _serviceInstance.GetAllLanguage();
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetAllExtension()
        {
            var result = _serviceInstance.GetAllExtension();
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetAllCommunicationTool()
        {
            var result = _serviceInstance.GetAllCommunicationTool();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetAllMainCategory()
        {
            var result = _serviceInstance.GetAllMainCategory();
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetAllProduct()
        {
            var result = _serviceInstance.GetAllProduct();
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetAllStatus()
        {
            var result = _serviceInstance.GetAllStatus();
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetAllRole()
        {
            var result = _serviceInstance.GetAllRole();
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetAllResponse()
        {
            var result = _serviceInstance.GetAllResponse();
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetNumberOfFeedbackSubjectTemplate()
        {
            var result = _serviceInstance.GetNumberOfFeedbackSubjectTemplate();
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetFeedbackSubjectTemplateByPagingIndex(GetFeedbackSubjectTemplateByPagingIndexCriteria criteria)
        {
            var result = _serviceInstance.GetFeedbackSubjectTemplateByPagingIndex(criteria);
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }
    }
}
