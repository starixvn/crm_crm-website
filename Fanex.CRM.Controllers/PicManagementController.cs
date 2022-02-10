using System.Web.Mvc;
using Fanex.CRM.Common.Constants;
using Fanex.CRM.Common.Controllers;
using Fanex.NAP.Core.Business;
using Fanex.NAP.Core.Web.Attribute;

namespace Fanex.CRM.Controllers
{
    [NapAuth(FunctionKey = FunctionPermissions.PICManagement, Policy = Constant.Policy.Read)]
    public class PicManagementController : BaseController
    {

        public ActionResult Index()
        {
            return Redirect(string.Format("{0}/PicManagement", ExternalDomain));
        }
    }
}