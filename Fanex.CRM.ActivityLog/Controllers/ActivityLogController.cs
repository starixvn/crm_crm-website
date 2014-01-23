// -----------------------------------------------------------------------
// <copyright file="ActivityLogController.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.ActivityLog.Controllers
{
    using Fanex.CRM.Infrastructure.Web.Controllers;
    using System.Web.Mvc;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ActivityLogController : BaseController
    {
        public ActivityLogController()
        {

        }

        public ActionResult Index()
        {
            return View("~/Areas/ActivityLog/Index.cshtml");
        }
    }
}
