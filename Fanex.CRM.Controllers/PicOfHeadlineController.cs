namespace Fanex.CRM.Controllers
{
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Controllers;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using System.Web.Mvc;

    [NapAuth(FunctionKey = FunctionPermissions.PICAssignment, Policy = Constant.Policy.Read)]
    public class PicOfHeadlineController : BaseController
    {
        public ActionResult Index()
        {
            return Redirect(string.Format("{0}/PicAssignment", ExternalDomain));
        }
    }
}