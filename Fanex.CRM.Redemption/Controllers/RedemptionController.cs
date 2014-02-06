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

        [ActionName("New")]
        public ActionResult InsertRedemption()
        {
            return View("~/Areas/Redemption/InsertRedemption.cshtml");
        }

        [ActionName("edit")]
        public ActionResult EditRedemption()
        {
            return View("~/Areas/Redemption/EditRedemption.cshtml");
        }
    }
}
