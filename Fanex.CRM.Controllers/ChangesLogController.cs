using System.Web.Mvc;
using Fanex.CRM.Common.Constants;
using Fanex.CRM.Common.Controllers;
using Fanex.NAP.Core.Business;
using Fanex.NAP.Core.Web.Attribute;

namespace Fanex.CRM.Controllers
{
    [NapAuth(FunctionKey = FunctionPermissions.ChangesLog, Policy = Constant.Policy.Read)]
    public class ChangesLogController : BaseController
    {
        public ActionResult Index()
        {
            return new RedirectResult(string.Format("{0}/ChangesLog", ExternalDomain), false);
        }
    }
}