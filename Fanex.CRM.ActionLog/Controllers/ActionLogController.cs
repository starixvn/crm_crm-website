namespace Fanex.CRM.ActionLog.Controllers
{
    using System.Web.Mvc;
    using Fanex.CRM.ActionLog.Criteria;
    using Fanex.CRM.ActionLog.Models;
    using Fanex.CRM.ActionLog.Services;
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Controllers;
    using Fanex.CRM.Common.Models;
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
            PagedList<ActionLog> activityLogs = _actionLogService.GetActivityLogs(criteria);

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
            return new RedirectResult(string.Format("{0}/SettingLog", ExternalDomain), false);
        }
    }
}