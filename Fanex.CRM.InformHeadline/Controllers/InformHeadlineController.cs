// -----------------------------------------------------------------------
// <copyright file="InformHeadlineController.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.InformHeadline.Controllers
{
    using System.Web.Mvc;
    using Fanex.CRM.Common.Controllers;
    using Fanex.NAP.Core.Web.Attribute;
    using Fanex.CRM.Common.Constants;
    using Fanex.NAP.Core.Business;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class InformHeadlineController : BaseController
    {
        public InformHeadlineController()            
        {
        }

        [NapAuth(FunctionKey = FunctionPermissions.InformHeadline, Policy = Constant.Policy.Read)]
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.HasSearchPermission = HasHostPermission(FunctionPermissions.InformHeadlineContact);
            return View("~/Areas/InformHeadline/ViewList/Index.cshtml");
        }
    }
}
