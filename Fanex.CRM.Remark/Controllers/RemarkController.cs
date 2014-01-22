// -----------------------------------------------------------------------
// <copyright file="RemarkController.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.Remark.Controllers
{
    using System.Web.Mvc;
    using Fanex.CRM.Infrastructure.Web.Controllers;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class RemarkController : BaseController
    {
        public RemarkController()
        {

        }

        public ActionResult Index(){
            return View("~/Areas/Remark/Index.cshtml");
        }
    }
}
