// -----------------------------------------------------------------------
// <copyright file="InformHeadlineController.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.InformHeadline.Controllers
{
    using System.Web.Mvc;
    using Fanex.CRM.Common.Controllers;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class InformHeadlineController : BaseController
    {
        public InformHeadlineController()            
        {
        }

        // [AuthenticateAction(AuthenticationType.Read, "Index")]
        public ActionResult Index()
        {
            return View("~/Areas/InformHeadline/ViewList/Index.cshtml");
        }
    }
}
