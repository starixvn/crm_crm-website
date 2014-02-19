// --------------------------------------------------------------------------
// <copyright file="ActionLogController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.02.14">Created</change>
// </history>

namespace Fanex.CRM.ActionLog.Controllers
{
    using System.Web.Mvc;
    using Fanex.CRM.ActionLog.Criteria;
    using Fanex.CRM.ActionLog.Services.Implements;
    using Fanex.CRM.ActionLog.Services.Interfaces;
    using Fanex.CRM.Infrastructure.Common;
    using Fanex.CRM.Infrastructure.Web.Controllers;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// ActionLogController controller.
    /// </summary>
    public class ActionLogController : BaseController
    {
        private readonly IActionLogService _serviceInstance;

        public ActionLogController()
            : this(null)
        { 
        }

        public ActionLogController(IActionLogService serviceInstance)
        {
            _serviceInstance = serviceInstance ?? new ActionLogService();
        }

        [NapAuth(FunctionKey = FunctionPermission.ViewActivityLog, Policy = Constant.Policy.Read)]
        [HttpGet]
        public ActionResult ActivityLog(int customerId)
        {
            return View("~/Areas/ActionLog/ActivityLog/Index.cshtml", customerId);
        }

        [NapAuth(FunctionKey = FunctionPermission.ViewActivityLog, Policy = Constant.Policy.Read)]
        [HttpPost]
        public ActionResult GetActivityLog(GetActivityLogCriteria criteria)
        {
            var activityLogs = _serviceInstance.GetActivityLogs(criteria);
            return Json(
                new 
                { 
                    ActivityLogs = activityLogs,
                    TotalItems = activityLogs.TotalItems
                },
                JsonRequestBehavior.AllowGet);
        }

        [NapAuth(FunctionKey = FunctionPermission.ViewActivityLog, Policy = Constant.Policy.Read)]
        [HttpGet]
        public ActionResult SettingLog()
        {
            return View("~/Areas/ActionLog/SettingLog/Index.cshtml");
        }

        [NapAuth(FunctionKey = FunctionPermission.ViewActivityLog, Policy = Constant.Policy.Read)]
        [HttpPost]
        public ActionResult GetSettingLog(GetSettingLogCriteria criteria)
        {
            var settingLogs = _serviceInstance.GetSettingLogs(criteria);
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
