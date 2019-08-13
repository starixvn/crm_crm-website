namespace Fanex.CRM.ActionLog.Controllers
{
    using System.Web.Mvc;
    using Fanex.CRM.ActionLog.Criteria;
    using Fanex.CRM.ActionLog.Services;
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Controllers;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    [NapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Read)]
    public class ActionLogController : ReadOnlyBaseController
    {
        private readonly IActionLogService _actionLogService;

        public ActionLogController()
            : this(null)
        {
        }

        public ActionLogController(IActionLogService actionLogService)
        {
            _actionLogService = actionLogService ?? new ActionLogService();
        }

        [HttpGet]
        public ActionResult ActivityLog(int customerId)
        {
            return View("~/Areas/ActionLog/ActivityLog/Index.cshtml", customerId);
        }

        [HttpPost]
        public ActionResult GetActivityLog(GetActivityLogCriteria criteria)
        {
            var activityLogs = _actionLogService.GetActivityLogs(criteria);

            var jsonResult = Json(
                new
                {
                    ActivityLogs = activityLogs,
                    TotalItems = activityLogs.TotalItems
                },
                JsonRequestBehavior.AllowGet);

            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpGet]
        [NapAuth(FunctionKey = FunctionPermissions.SettingLog, Policy = Constant.Policy.Read)]
        public ActionResult SettingLog()
        {
            return View("~/Areas/ActionLog/SettingLog/Index.cshtml");
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermissions.SettingLog, Policy = Constant.Policy.Read)]
        public ActionResult GetSettingLog(GetSettingLogCriteria criteria)
        {
            var settingLogs = _actionLogService.GetSettingLogs(criteria);

            return Json(
                new
                {
                    SettingLogs = settingLogs,
                    TotalItems = settingLogs.TotalItems
                },
                JsonRequestBehavior.AllowGet);
        }
    }
}