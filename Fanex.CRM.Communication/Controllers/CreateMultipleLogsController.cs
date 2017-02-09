namespace Fanex.CRM.Communication.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Common.Constants;
    using Common.Controllers;
    using Common.Enumerations;
    using Criteria;
    using Infrastructure.Constants;
    using Infrastructure.Models;
    using NAP.Core.Business;
    using NAP.Core.Web.Attribute;
    using Resources;
    using Services;
    using Setting.Services;

    public class CreateMultipleLogsController : ReadOnlyBaseController
    {
        private readonly ICommunicationLogService _communicationServiceLog;

        private readonly ISettingService _serviceSetting;

        public CreateMultipleLogsController()
            : this(null, null)
        {
        }

        public CreateMultipleLogsController(
            ICommunicationLogService communicationServiceLog,
            ISettingService serviceSetting)
        {
            _communicationServiceLog = communicationServiceLog ?? new CommunicationLogService();
            _serviceSetting = serviceSetting ?? new SettingService();
        }

        private static bool IsRelatedDeletedStatus(int currentStatus, bool isDeletedStatus)
        {
            var isChangedStatusFromDeleted = isDeletedStatus && currentStatus != (int)CRMStatus.Deleted;
            var isChangedStatusToDeleted = !isDeletedStatus && currentStatus == (int)CRMStatus.Deleted;

            return isChangedStatusFromDeleted || isChangedStatusToDeleted;
        }

        private bool HasHostCreateUpdateFeedbackPermission()
        {
            return HasPermission(FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Constant.Policy.Host);
        }

        [NapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Read)]
        public ActionResult Index(int customerId)
        {
            ViewBag.CustomerInfoPolicy = GetPolicy(FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo);
            return View("~/Areas/Communication/CreateMultipleLogs/Index.cshtml", customerId);
        }

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
                SiteList = _serviceSetting.GetAllSiteWithGroup().Where(s => s.IsActive),
                RoleList = _serviceSetting.GetAllRoles(),
                ResponseList = _serviceSetting.GetAllResponses(),
                LicenseeType = _serviceSetting.GetAllLicenseeTypes()
            };

            return Json(allSetting, JsonRequestBehavior.AllowGet);
        }

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

        [NapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Read)]
        [HttpPost]
        public ActionResult GetLogsByCustomerId(GetLogsByCustomerIdCriteria criteria)
        {
            var logs = _communicationServiceLog.GetLogsByCustomerId(criteria);
            return Json(logs, JsonRequestBehavior.AllowGet);
        }

        private bool HasUpdateFeedbackPermission(int currentStatus)
        {
            var isDeletedStatus = currentStatus == (int)CRMStatus.Deleted;
            if (isDeletedStatus)
            {
                return HasHostCreateUpdateFeedbackPermission() && HasPermission(FunctionPermissions.DeleteFeedback, Constant.Policy.Host);
            }

            return HasHostCreateUpdateFeedbackPermission();
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Host)]
        public JsonResult UpdateLog(UpdateLogCriteria criteria, bool isDeletedStatus)
        {
            var updateResult = CheckUpdateCustomerFeedbackPermission(criteria, isDeletedStatus);
            if (updateResult.Code == ErrorCodeConstants.NoneError)
            {
                updateResult = _communicationServiceLog.UpdateLog(criteria);
            }

            return Json(updateResult, JsonRequestBehavior.AllowGet);
        }

        private Result CheckUpdateCustomerFeedbackPermission(UpdateLogCriteria criteria, bool isDeletedStatus)
        {
            var permissionError = ErrorCodeConstants.NoPermission;
            if (!HasHostCreateUpdateFeedbackPermission())
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
        
        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Host)]
        public JsonResult InsertLog(InsertLogCriteria criteria)
        {
            var insertResult = _communicationServiceLog.InsertLog(criteria);
            return Json(insertResult, JsonRequestBehavior.AllowGet);
        }

        [NapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Read)]
        [HttpPost]
        public JsonResult AllStatus()
        {
            var allStatus = _communicationServiceLog.AllStatusForInsert;
            return Json(allStatus, JsonRequestBehavior.AllowGet);
        }
    }
}
