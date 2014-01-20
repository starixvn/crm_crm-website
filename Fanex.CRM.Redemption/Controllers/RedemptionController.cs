// -----------------------------------------------------------------------
// <copyright file="RedemptionController.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.Redemption.Controllers
{
    using System.Web.Mvc;
    using Fanex.CRM.Infrastructure.Web.Controllers;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class RedemptionController : BaseController
    {
        public RedemptionController()
        {

        }

        public ActionResult Index()
        {
            return View("~/Areas/Redemption/Index.cshtml");
        }
    }
}
