// -----------------------------------------------------------------------
// <copyright file="InformHeadlineController.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.InformHeadline.Controllers
{
    using System.Web.Mvc;
    using Fanex.CRM.Common.Controllers;
    using Fanex.CRM.InformHeadline.Services;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class InformHeadlineController : BaseController
    {
        private readonly IInformHeadlineService _informHeadlineService;

        public InformHeadlineController()
            : this(null)
        {
        }

        public InformHeadlineController(IInformHeadlineService informHeadlineService)
        {
            _informHeadlineService = informHeadlineService ?? new InformHeadlineService();
        }

        // [AuthenticateAction(AuthenticationType.Read, "Index")]
        public ActionResult Index()
        {
            return View("~/Areas/InformHeadline/ViewList/Index.cshtml");
        }
    }
}
