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
    using Fanex.CRM.Infrastructure.Common;
    using Fanex.CRM.Infrastructure.Web.Controllers;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// ActionLogController controller.
    /// </summary>
    public class ActionLogController : BaseController
    {
        [NapAuth(FunctionKey = FunctionPermission.ViewActivityLog, Policy = Constant.Policy.Read)]
        public ActionResult ActivityLog(int customerId)
        {
            return View("~/Areas/ActionLog/ActivityLog/Index.cshtml");
        }

        [NapAuth(FunctionKey = FunctionPermission.ViewActivityLog, Policy = Constant.Policy.Read)]
        public ActionResult SettingLog()
        {
            return View("~/Areas/ActionLog/SettingLog/Index.cshtml");
        }
    }
}
