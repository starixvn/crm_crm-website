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
    using Fanex.CRM.Common.Enumerations;
    using Fanex.CRM.Common.Services;
    using Fanex.CRM.Communication.Criteria;
    using Fanex.CRM.Communication.Resources;
    using Fanex.CRM.Communication.Services;
    using Fanex.CRM.Infrastructure.Constants;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Setting.Services;
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

        [NapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Read)]
        public ActionResult Index(int customerId)
        {
            ViewBag.CustomerInfoPolicy = GetPolicy(FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo);
            return View("~/Areas/Communication/ViewLog/CommunicationLog.cshtml", customerId);
        }

        /// <summary>
        /// Gets all setting for edit.
        /// </summary>
        /// <returns>All setting for edit.</returns>
        [NapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Read)]
        [HttpPost]
        public ActionResult AllSettingForEdit()
        {
            var allSetting = new
            {
                ExtensionList = _serviceSetting.AllExtension(),
                CountryList = _serviceSetting.AllCountry(),
                CommunicationToolList = _serviceSetting.AllCommunicationTool(),
                LanguageList = _serviceSetting.AllLanguage(),
                MainCategoryList = _serviceSetting.AllMainCategory(),
                ProductList = _serviceSetting.AllProduct(),
                StatusList = _communicationServiceLog.AllStatusForEdit,
                SiteList = _serviceSetting.AllSite(),
                RoleList = _serviceSetting.AllRole(),
                ResponseList = _serviceSetting.AllResponse(),
                LicenseeType = _serviceSetting.AllLicenseeType()
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
        [NapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Host)]
        [HttpPost]
        public ActionResult AllSettingForInsert(int custId)
        {
            var allSetting = new
            {
                ExtensionList = _serviceSetting.AllExtension(),
                CountryList = _serviceSetting.AllCountry(),
                CommunicationToolList = _serviceSetting.AllCommunicationTool(),
                LanguageList = _serviceSetting.AllLanguage(),
                MainCategoryList = _serviceSetting.AllMainCategory(),
                ProductList = _serviceSetting.AllProduct(),
                StatusList = _communicationServiceLog.AllStatusForInsert,
                SiteList = _serviceSetting.AllSite(),
                RoleList = _serviceSetting.AllRole(),
                ResponseList = _serviceSetting.AllResponse(),
                LicenseeType = _serviceSetting.AllLicenseeType(),
                DefaultSettings = _communicationServiceLog.GetCustomerDefaultSetting(new GetDefaultSettingsCriteria { CustId = custId })
            };

            return Json(allSetting, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the logs by customer identifier.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The logs by customer identifier.</returns>
        [NapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Read)]
        [HttpPost]
        public ActionResult GetLogsByCustomerId(GetLogsByCustomerIdCriteria criteria)
        {
            var logs = _communicationServiceLog.GetLogsByCustomerId(criteria);
            var jsonResult = Json(logs, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [NapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Read)]
        public ActionResult OpenEditView(int logRunningNumber)
        {
            ViewBag.HasReviewPermission = HasPermission(FunctionPermissions.ReviewFeedback, Constant.Policy.Host);
            ViewBag.HasHostCreateUpdatePermission = HasPermission(FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Constant.Policy.Host);
            var logInfo = _communicationServiceLog.GetLogsByRunningNumber(new GetLogByRunningNumberCriteria
            {
                RunningNumber = logRunningNumber
            });

            return View("~/Areas/Communication/EditLog/Index.cshtml", logInfo);
        }

        [NapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Host)]
        public ActionResult OpenInsertView(int customerId)
        {
            return View("~/Areas/Communication/InsertLog/Index.cshtml", customerId);
        }

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
        [AjaxNapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Host)]
        public JsonResult UpdateLog(UpdateLogCriteria criteria)
        {
            Result updateResult;
            if (criteria.UpdateReviewedMode != UpdateReviewedModeConstants.NotChange &&
                !HasHostPermission(FunctionPermissions.ReviewFeedback))
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
        [NapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Host)]
        public JsonResult InsertLog(InsertLogCriteria criteria)
        {
            Result insertResult = _communicationServiceLog.InsertLog(criteria);
            var jsonResult = Json(insertResult, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [NapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Read)]
        [HttpPost]
        public JsonResult AllStatus()
        {
            var allStatus = _communicationServiceLog.AllStatusForEdit;
            var jsonResult = Json(allStatus, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }
    }
}
