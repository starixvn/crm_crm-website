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
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Controllers;
    using Fanex.CRM.Communication.Criteria;
    using Fanex.CRM.Communication.Resources;
    using Fanex.CRM.Communication.Services.Implements;
    using Fanex.CRM.Communication.Services.Interfaces;
    using Fanex.CRM.Infrastructure.Constants;
    using Fanex.CRM.Infrastructure.Models;
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

        public ActionResult Index(int customerId)
        {
            if (HasPermission(FunctionPermission.EditFeedback, Constant.Policy.Read) ||
                HasPermission(FunctionPermission.CreateFeedback, Constant.Policy.Read))
            {
                return View("~/Areas/Communication/ViewLog/CommunicationLog.cshtml", customerId);
            }

            return Redirect("~/Common/AccessDenied");
        }

        /// <summary>
        /// Gets all setting for edit.
        /// </summary>
        /// <returns>All setting for edit.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermission.EditFeedback, Policy = Constant.Policy.Read)]
        public ActionResult GetAllSetting_Edit()
        {
            var allSetting = new
            {
                ExtensionList = _serviceSetting.GetAllExtension(),
                CountryList = _serviceSetting.GetAllCountry(),
                CommunicationToolList = _serviceSetting.GetAllCommunicationTool(),
                LanguageList = _serviceSetting.GetAllLanguage(),
                MainCategoryList = _serviceSetting.GetAllMainCategory(),
                ProductList = _serviceSetting.GetAllProduct(),
                StatusList = _communicationServiceLog.GetAllStatus_Edit(),
                SiteList = _serviceSetting.GetAllSite(),
                RoleList = _serviceSetting.GetAllRole(),
                ResponseList = _serviceSetting.GetAllResponse()
            };

            return Json(allSetting, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets all setting for edit.
        /// </summary>
        /// <param name="custId">The customer ID.</param>
        /// <returns>
        /// All setting for edit.
        /// </returns>
        [HttpPost]
        public ActionResult GetAllSetting_Insert(int custId)
        {
            var allSetting = new
            {
                ExtensionList = _serviceSetting.GetAllExtension(),
                CountryList = _serviceSetting.GetAllCountry(),
                CommunicationToolList = _serviceSetting.GetAllCommunicationTool(),
                LanguageList = _serviceSetting.GetAllLanguage(),
                MainCategoryList = _serviceSetting.GetAllMainCategory(),
                ProductList = _serviceSetting.GetAllProduct(),
                StatusList = _communicationServiceLog.GetAllStatus_Insert(),
                SiteList = _serviceSetting.GetAllSite(),
                RoleList = _serviceSetting.GetAllRole(),
                ResponseList = _serviceSetting.GetAllResponse(),
                DefaultSettings = _communicationServiceLog.GetCustomerDefaultSetting(new GetDefaultSettingsCriteria { CustId = custId }),
            };

            return Json(allSetting, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the logs by customer identifier.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The logs by customer identifier.</returns>
        [HttpPost]
        public ActionResult GetLogsByCustomerId(GetLogsByCustomerIdCriteria criteria)
        {
            if (!HasPermission(FunctionPermission.EditFeedback, Constant.Policy.Read) &&
                !HasPermission(FunctionPermission.CreateFeedback, Constant.Policy.Read))
            {
                return null;
            }

            var logs = _communicationServiceLog.GetLogsByCustomerId(criteria);
            var jsonResult = Json(logs, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [NapAuth(FunctionKey = FunctionPermission.EditFeedback, Policy = Constant.Policy.Read)]
        public ActionResult OpenEditView(int logRunningNumber)
        {
            var logInfo = _communicationServiceLog.GetLogsByRunningNumber(new GetLogByRunningNumberCriteria
            {
                RunningNumber = logRunningNumber
            });

            return View("~/Areas/Communication/EditLog/Index.cshtml", logInfo);
        }

        [NapAuth(FunctionKey = FunctionPermission.CreateFeedback, Policy = Constant.Policy.Host)]
        public ActionResult OpenInsertView(int customerId)
        {
            return View("~/Areas/Communication/InsertLog/Index.cshtml", customerId);
        }

        [NapAuth(FunctionKey = FunctionPermission.EditFeedback, Policy = Constant.Policy.Read)]
        public ActionResult ViewDetailById(int logId)
        {
            var logInfo = _communicationServiceLog.GetLogsByRunningNumber(new GetLogByRunningNumberCriteria
            {
                RunningNumber = logId
            });

            return View("~/Areas/Communication/ViewDetailById/Index.cshtml", logInfo);
        }

        /// <summary>
        /// Updates the log.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Update Result.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermission.EditFeedback, Policy = Constant.Policy.Host)]
        public JsonResult UpdateLog(UpdateLogCriteria criteria)
        {
            Result updateResult;
            if (criteria.UpdateReviewedMode != UpdateReviewedModeConstants.NotChange &&
                !HasHostPermission(FunctionPermission.ReviewFeedback))
            {
                updateResult = new Result
                {
                    Code = ErrorCodeConstants.NoPermission,
                    Message = Labels.Communication_Logs_NoReviewPermission
                };

                return Json(updateResult, JsonRequestBehavior.AllowGet);
            }

            updateResult = _communicationServiceLog.UpdateLog(criteria);
            var jsonResult = Json(updateResult, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        /// <summary>
        /// Inserts the log.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Insert result.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermission.CreateFeedback, Policy = Constant.Policy.Host)]
        public JsonResult InsertLog(InsertLogCriteria criteria)
        {
            Result insertResult = _communicationServiceLog.InsertLog(criteria);
            var jsonResult = Json(insertResult, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetAllStatus()
        {
            var allStatus = _communicationServiceLog.GetAllStatus_Edit();
            var jsonResult = Json(allStatus, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }
    }
}
