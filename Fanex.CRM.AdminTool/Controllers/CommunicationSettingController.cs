namespace Fanex.CRM.AdminTool.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Controllers;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    [NapAuth(FunctionKey = FunctionPermissions.DefaultCommunicationSetting, Policy = Constant.Policy.Read)]
    public class CommunicationSettingController : ReadOnlyBaseController
    {
        public ActionResult Index()
        {
            return Redirect(string.Format("{0}/CommunicationSetting", ExternalDomain));
        }
    }
}