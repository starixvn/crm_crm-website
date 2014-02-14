// -----------------------------------------------------------------------
// <copyright file="SettingLogController.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.ActionLog.Controllers
{
    using System.Web.Mvc;
    using Fanex.CRM.Infrastructure.Common;
    using Fanex.CRM.Infrastructure.Web.Controllers;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SettingLogController : BaseController
    {
        public SettingLogController()
        {
        }

        [NapAuth(FunctionKey = FunctionPermission.ViewActivityLog, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            return View("~/Areas/ActionLog/SettingLog/Index.cshtml");
        }
    }
}
