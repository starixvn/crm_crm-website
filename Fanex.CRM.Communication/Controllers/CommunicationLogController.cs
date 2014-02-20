// --------------------------------------------------------------------------
// <copyright file="CommunicationLogController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Communication.Controllers
{
    using System.Web.Mvc;
    using Fanex.CRM.Communication.Criteria;
    using Fanex.CRM.Communication.Models;
    using Fanex.CRM.Communication.Services.Implements;
    using Fanex.CRM.Communication.Services.Interfaces;
    using Fanex.CRM.Infrastructure.Common;
    using Fanex.CRM.Infrastructure.Web.Controllers;
    using Fanex.CRM.Setting.Services.Implements;
    using Fanex.CRM.Setting.Services.Interfaces;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// Communication Log Controller.
    /// </summary>
    public class CommunicationLogController : BaseController
    {
        private ICommunicationLogService _communicationServiceLog;

        private ISettingService _serviceSetting;

        public CommunicationLogController()
            : this(null, null)
        {
        }

        public CommunicationLogController(
            ICommunicationLogService communicationServiceLog,
            ISettingService serviceSetting)
        {
            _communicationServiceLog = communicationServiceLog ?? new CommunicationLogService();
            _serviceSetting = serviceSetting ?? new SettingService();
        }

        [NapAuth(FunctionKey = FunctionPermission.EditFeedback, Policy = Constant.Policy.Read)]
        public ActionResult Index(int customerId)
        {
            return View("~/Areas/Communication/ViewLog/CommunicationLog.cshtml", customerId);
        }

        /// <summary>
        /// Gets all setting for edit.
        /// </summary>
        /// <returns>All setting for edit.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermission.EditFeedback, Policy = Constant.Policy.Read)]
        public ActionResult GetAllSettingForEdit()
        {
            var allSetting = new
            {
                ExtensionList = _serviceSetting.GetAllExtension(),
                CountryList = _serviceSetting.GetAllCountry(),
                CommunicationToolList = _serviceSetting.GetAllCommunicationTool(),
                LanguageList = _serviceSetting.GetAllLanguage(),
                MainCategoryList = _serviceSetting.GetAllMainCategory(),
                ProductList = _serviceSetting.GetAllProduct(),
                StatusList = _communicationServiceLog.GetAllStatus(),
                SiteList = _serviceSetting.GetAllSite(),
                RoleList = _serviceSetting.GetAllRole(),
                ResponseList = _serviceSetting.GetAllResponse()
            };

            return Json(allSetting, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermission.EditFeedback, Policy = Constant.Policy.Read)]
        public ActionResult GetLogsByCustomerId(GetLogsByCustomerIdCriteria criteria)
        {
            var logs = _communicationServiceLog.GetLogsByCustomerId(criteria);
            var jsonResult = Json(logs, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [NapAuth(FunctionKey = FunctionPermission.EditFeedback, Policy = Constant.Policy.Host)]
        public ActionResult OpenEditView(int logRunningNumber)
        {
            var logInfo = _communicationServiceLog.GetLogsByRunningNumber(new GetLogByRunningNumberCriteria
            {
                RunningNumber = logRunningNumber
            });

            return View("~/Areas/Communication/EditLog/Index.cshtml", logInfo);
        }

        [NapAuth(FunctionKey = FunctionPermission.EditFeedback, Policy = Constant.Policy.Host)]
        public ActionResult OpenInsertView(int customerId)
        {
            return View("~/Areas/Communication/InsertLog/Index.cshtml", customerId);
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermission.EditFeedback, Policy = Constant.Policy.Host)]
        public JsonResult UpdateLog(UpdateLogCriteria criteria)
        {
            var updateResult = _communicationServiceLog.UpdateLog(criteria);
            var jsonResult = Json(updateResult, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermission.EditFeedback, Policy = Constant.Policy.Host)]
        public JsonResult InsertLog(InsertLogCriteria criteria)
        {
            var insertResult = _communicationServiceLog.InsertLog(criteria);
            var jsonResult = Json(insertResult, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetAllStatus()
        {
            var allStatus = _communicationServiceLog.GetAllStatus();
            var jsonResult = Json(allStatus, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }
    }
}
