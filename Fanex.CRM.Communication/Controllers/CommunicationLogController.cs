namespace Fanex.CRM.Communication.Controllers
{
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;
    using Common.Services;
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Controllers;
    using Fanex.CRM.Common.Enumerations;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Communication.Criteria;
    using Fanex.CRM.Communication.Resources;
    using Fanex.CRM.Communication.Services;
    using Fanex.CRM.Setting.Services;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using Setting.Constants;

    public class CommunicationLogController : ReadOnlyBaseController
    {
        private readonly ICommunicationLogService _communicationServiceLog;

        private readonly ISettingService _serviceSetting;

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

        private static bool IsRelatedDeletedStatus(int currentStatus, bool isDeletedStatus)
        {
            bool isChangedStatusFromDeleted = isDeletedStatus && currentStatus != (int)CRMStatus.Deleted;
            bool isChangedStatusToDeleted = !isDeletedStatus && currentStatus == (int)CRMStatus.Deleted;

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
            return View("~/Areas/Communication/ViewLog/CommunicationLog.cshtml", customerId);
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
            var siteList = AppSession.CurrentUser.UserRole == UserRole.CN88Staff
              ? _serviceSetting.GetAllSitesCN88()
              : _serviceSetting.GetAllSites();

            var allSetting = new
            {
                ExtensionList = _serviceSetting.GetAllExtensions(),
                CountryList = _serviceSetting.GetAllCountries(),
                ProvinceList = _serviceSetting.GetAllProvinces(),
                CommunicationToolList = _serviceSetting.GetAllCommunicationTools(),
                LanguageList = _serviceSetting.GetAllLanguages(),
                StatusList = statusList,
                SiteList = siteList,
                RoleList = _serviceSetting.GetAllRoles(),
                ResponseList = _serviceSetting.GetAllResponses(),
                LicenseeType = _serviceSetting.GetAllLicenseeTypes(),
                ProductNA = _serviceSetting.GetProductNA(),
                SubProductNA = _serviceSetting.GetSubProductNA(),
                BettypeNA = _serviceSetting.GetBettypeNA(),
                ExtensionNA = _serviceSetting.GetExtensionNA(),
                ProductList = _serviceSetting.GetProducts(),
                SubProductList = _serviceSetting.GetSubProducts(),
                BettypeList = _serviceSetting.GetBettypes(),
                MainCategoryList = _serviceSetting.GetMainCategories(),
                SubCategoryList = _serviceSetting.GetSubCategories(),
            };

            return Json(allSetting, JsonRequestBehavior.AllowGet);
        }

        [NapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Host)]
        [HttpPost]
        public ActionResult AllSettingForInsert(int custId)
        {
            var siteList = AppSession.CurrentUser.UserRole == UserRole.CN88Staff
               ? _serviceSetting.GetAllSitesCN88()
               : _serviceSetting.GetAllSites();

            var allSetting = new
            {
                ExtensionList = _serviceSetting.GetAllExtensions(),
                CountryList = _serviceSetting.GetAllCountries(),
                ProvinceList = _serviceSetting.GetAllProvinces(),
                CommunicationToolList = _serviceSetting.GetAllCommunicationTools(),
                LanguageList = _serviceSetting.GetAllLanguages(),
                StatusList = _communicationServiceLog.AllStatusForInsert,
                SiteList = siteList,
                RoleList = _serviceSetting.GetAllRoles(),
                ResponseList = _serviceSetting.GetAllResponses(),
                LicenseeType = _serviceSetting.GetAllLicenseeTypes(),
                DefaultSettings = _communicationServiceLog.GetCustomerDefaultSetting(new GetDefaultSettingsCriteria { CustId = custId }),
                ProductNA = _serviceSetting.GetProductNA(),
                SubProductNA = _serviceSetting.GetSubProductNA(),
                BettypeNA = _serviceSetting.GetBettypeNA(),
                ExtensionNA = _serviceSetting.GetExtensionNA(),
                ProductList = _serviceSetting.GetProducts(),
                SubProductList = _serviceSetting.GetSubProducts(),
                BettypeList = _serviceSetting.GetBettypes(),
                MainCategoryList = _serviceSetting.GetMainCategories(),
                SubCategoryList = _serviceSetting.GetSubCategories(),
            };

            return Json(allSetting, JsonRequestBehavior.AllowGet);
        }

        [NapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Read)]
        [HttpPost]
        public ActionResult GetLogsByCustomerId(GetLogsByCustomerIdCriteria criteria)
        {
            var logs = _communicationServiceLog.GetLogsByCustomerId(criteria);
            var result = new
            {
                MainCategories = _serviceSetting.GetAllMainCategoriesFromAllSettings(),
                Logs = logs
            };

            return Json(result, JsonRequestBehavior.AllowGet);
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
            ViewBag.UserRole = AppSession.CurrentUser.UserRole;

            return View("~/Areas/Communication/EditLog/Index.cshtml", logInfo);
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

        [NapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Host)]
        public ActionResult OpenInsertView(int customerId)
        {
            ViewBag.customerId = customerId;
            ViewBag.UserRole = AppSession.CurrentUser.UserRole;
            ViewBag.CN88SiteId = SiteConstants.CN88;
            ViewBag.CN88MemberId = RoleIdConstants.CN88Member;
            return View("~/Areas/Communication/InsertLog/Index.cshtml");
        }

        [NapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Host)]
        public ActionResult OpenDuplicateView(int logRunningNumber)
        {
            var logInfo = _communicationServiceLog.GetLogsByRunningNumber(new GetLogByRunningNumberCriteria
            {
                RunningNumber = logRunningNumber
            });
            ViewBag.customerId = logInfo.CustomerId;
            ViewBag.UserRole = AppSession.CurrentUser.UserRole;
            ViewBag.CN88SiteId = SiteConstants.CN88;
            ViewBag.CN88MemberId = RoleIdConstants.CN88Member;

            return View("~/Areas/Communication/InsertLog/Index.cshtml", logInfo);
        }

        public ActionResult ViewDetailById(int logId)
        {
            var logInfo = _communicationServiceLog.GetLogsByRunningNumber(new GetLogByRunningNumberCriteria
            {
                RunningNumber = logId
            });

            return View("~/Areas/Communication/ViewDetailById/Index.cshtml", logInfo);
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Host)]
        public JsonResult UpdateLog(UpdateLogCriteria criteria, bool isDeletedStatus)
        {
            CommandResult updateResult = CheckUpdateCustomerFeedbackPermission(criteria, isDeletedStatus);
            if (updateResult.IsSuccessful)
            {
                updateResult = _communicationServiceLog.UpdateLog(criteria);
            }

            return Json(updateResult, JsonRequestBehavior.AllowGet);
        }

        private CommandResult CheckUpdateCustomerFeedbackPermission(UpdateLogCriteria criteria, bool isDeletedStatus)
        {
            const int permissionError = (int)HttpStatusCode.Forbidden;
            if (!HasHostCreateUpdateFeedbackPermission())
            {
                return new CommandResult(permissionError, Labels.Communication_Logs_NoUpdatePermission);
            }

            if (!HasPermission(FunctionPermissions.DeleteFeedback, Constant.Policy.Host) &&
                IsRelatedDeletedStatus(criteria.StatusId, isDeletedStatus))
            {
                return new CommandResult(permissionError, Labels.Communication_Logs_NoDeletePermission);
            }

            if (!HasHostPermission(FunctionPermissions.ReviewFeedback) &&
                criteria.UpdateReviewedMode != UpdateReviewedModeConstants.NotChange)
            {
                return new CommandResult(permissionError, Labels.Communication_Logs_NoReviewPermission);
            }

            return new CommandResult();
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Host)]
        public JsonResult InsertLog(InsertLogCriteria criteria)
        {
            CommandResult insertResult = _communicationServiceLog.InsertLog(criteria);
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