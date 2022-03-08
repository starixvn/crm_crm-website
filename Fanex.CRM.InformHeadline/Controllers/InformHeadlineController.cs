namespace Fanex.CRM.InformHeadline.Controllers
{
    using System.Web.Mvc;
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Controllers;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    public class InformHeadlineController : ReadOnlyBaseController
    {
        [NapAuth(FunctionKey = FunctionPermissions.InformHeadline, Policy = Constant.Policy.Read)]
        [HttpGet]
        public ActionResult Index()
        {
            return new RedirectResult(string.Format("{0}/InformHeadline", ExternalDomain), false);
        }
    }
}