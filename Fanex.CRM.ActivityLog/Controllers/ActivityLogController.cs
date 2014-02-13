// -----------------------------------------------------------------------
// <copyright file="ActivityLogController.cs" company="Nexcel Solutions Vietnam">
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
    public class ActivityLogController : BaseController
    {
        public ActivityLogController()
        {
        }

        [NapAuth(FunctionKey = FunctionPermission.ViewActivityLog, Policy = Constant.Policy.Read)]
        public ActionResult Index(int customerId)
        {
            return View("~/Areas/ActionLog/ActivityLog/Index.cshtml");
        }
    }
}
