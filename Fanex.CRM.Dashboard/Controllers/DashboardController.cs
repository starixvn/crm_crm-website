// -----------------------------------------------------------------------
// <copyright file="DashboardController.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.Dashboard.Controllers
{
    using System.Web.Mvc;
    using Fanex.CRM.Infrastructure.Web.Controllers;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class DashboardController : BaseController
    {
        public DashboardController()
        {
        }

        public ActionResult Index()
        {
            return View("~/Areas/Dashboard/Index.cshtml");
        }
    }
}
