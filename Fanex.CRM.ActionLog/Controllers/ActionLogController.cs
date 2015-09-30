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
    using Fanex.CRM.ActionLog.Services;
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Controllers;
    using Fanex.CRM.Common.Enumerations;
    using Fanex.CRM.Common.Services;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// ActionLog Controller controller.
    /// </summary>
    [NapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Read)]
    public class ActionLogController : BaseController
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

        /// <summary>
        /// Gets the activity log.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The JSON result.</returns>
        [HttpPost]
        public ActionResult GetActivityLog(GetActivityLogCriteria criteria)
        {
            var activityLogs = _actionLogService.GetActivityLogs(criteria);

            return Json(
                new 
                { 
                    ActivityLogs = activityLogs,
                    TotalItems = activityLogs.TotalItems
                },
                JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AuthenticateAction(AuthenticationType.Read, "SettingLog")]
        public ActionResult SettingLog()
        {
            return View("~/Areas/ActionLog/SettingLog/Index.cshtml");
        }

        /// <summary>
        /// Gets the setting log.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The JSON result.</returns>
        [HttpPost]
        [AuthenticateAction(AuthenticationType.Read, "SettingLog")]
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
