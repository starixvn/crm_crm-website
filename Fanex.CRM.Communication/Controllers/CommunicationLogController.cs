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
    using System.Linq;
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
        private readonly ICommunicationLogService _communicationServiceLog;
        private readonly bool hasHostCreateUpdateFeedbackPermission;

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
            hasHostCreateUpdateFeedbackPermission = HasPermission(FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Constant.Policy.Host);
        }

        private static bool IsRelatedDeletedStatus(int currentStatus, bool isDeletedStatus)
        {
            bool isChangedStatusFromDeleted = isDeletedStatus && currentStatus != (int)CRMStatus.Deleted;
            bool isChangedStatusToDeleted = !isDeletedStatus && currentStatus == (int)CRMStatus.Deleted;

            return isChangedStatusFromDeleted || isChangedStatusToDeleted;
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
            var statusList = _communicationServiceLog.AllStatusForEdit;
            var hasDeleteFeedbackPermission = HasPermission(FunctionPermissions.DeleteFeedback, Constant.Policy.Host);
            if (!hasDeleteFeedbackPermission)
            {
                statusList = statusList.Where(s => s.Id != (int)CRMStatus.Deleted).ToList();
            }

            var allSetting = new
            {
                ExtensionList = _serviceSetting.GetAllExtensions(),
                CountryList = _serviceSetting.GetAllCountries(),
                CommunicationToolList = _serviceSetting.GetAllCommunicationTools(),
                LanguageList = _serviceSetting.GetAllLanguages(),
                MainCategoryList = _serviceSetting.GetAllMainCategories(),
                ProductList = _serviceSetting.GetAllProducts(),
                StatusList = statusList,
                SiteList = _serviceSetting.GetAllSites(),
                RoleList = _serviceSetting.GetAllRoles(),
                ResponseList = _serviceSetting.GetAllResponses(),
                LicenseeType = _serviceSetting.GetAllLicenseeTypes()
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
                ExtensionList = _serviceSetting.GetAllExtensions(),
                CountryList = _serviceSetting.GetAllCountries(),
                CommunicationToolList = _serviceSetting.GetAllCommunicationTools(),
                LanguageList = _serviceSetting.GetAllLanguages(),
                MainCategoryList = _serviceSetting.GetAllMainCategories(),
                ProductList = _serviceSetting.GetAllProducts(),
                StatusList = _communicationServiceLog.AllStatusForInsert,
                SiteList = _serviceSetting.GetAllSites(),
                RoleList = _serviceSetting.GetAllRoles(),
                ResponseList = _serviceSetting.GetAllResponses(),
                LicenseeType = _serviceSetting.GetAllLicenseeTypes(),
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
            var logInfo = _communicationServiceLog.GetLogsByRunningNumber(new GetLogByRunningNumberCriteria
            {
                RunningNumber = logRunningNumber
            });

            ViewBag.HasHostCreateUpdatePermission = HasUpdateFeedbackPermission(logInfo.Status.Id);
            ViewBag.HasReviewPermission = HasPermission(FunctionPermissions.ReviewFeedback, Constant.Policy.Host);
            ViewBag.IsDeletedFeedbackView = (logInfo.Status.Id == (int)CRMStatus.Deleted).ToString().ToLower();

            return View("~/Areas/Communication/EditLog/Index.cshtml", logInfo);
        }

        private bool HasUpdateFeedbackPermission(int currentStatus)
        {
            var isDeletedStatus = currentStatus == (int)CRMStatus.Deleted;
            if (isDeletedStatus)
            {
                return hasHostCreateUpdateFeedbackPermission && HasPermission(FunctionPermissions.DeleteFeedback, Constant.Policy.Host);
            }

            return hasHostCreateUpdateFeedbackPermission;
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
        public JsonResult UpdateLog(UpdateLogCriteria criteria, bool isDeletedStatus)
        {
            Result updateResult = CheckUpdateCustomerFeedbackPermission(criteria, isDeletedStatus);
            if (updateResult.Code == ErrorCodeConstants.NoneError)
            {
                updateResult = _communicationServiceLog.UpdateLog(criteria);
            }

            var jsonResult = Json(updateResult, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        private Result CheckUpdateCustomerFeedbackPermission(UpdateLogCriteria criteria, bool isDeletedStatus)
        {
            int permissionError = ErrorCodeConstants.NoPermission;
            if (!hasHostCreateUpdateFeedbackPermission)
            {
                return new Result(permissionError, Labels.Communication_Logs_NoUpdatePermission);
            }

            if (!HasPermission(FunctionPermissions.DeleteFeedback, Constant.Policy.Host) &&
                IsRelatedDeletedStatus(criteria.StatusId, isDeletedStatus))
            {
                return new Result(permissionError, Labels.Communication_Logs_NoDeletePermission);
            }

            if (!HasHostPermission(FunctionPermissions.ReviewFeedback) && 
                criteria.UpdateReviewedMode != UpdateReviewedModeConstants.NotChange)
            {
                return new Result(permissionError, Labels.Communication_Logs_NoReviewPermission);
            }

            return new Result();
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
            var allStatus = _communicationServiceLog.AllStatusForInsert;
            var jsonResult = Json(allStatus, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }
    }
}
