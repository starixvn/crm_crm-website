namespace Fanex.CRM.AdminTool.Controllers
{
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
            return new RedirectResult(string.Format("{0}/CommunicationSetting", ExternalDomain), false);
        }
    }
}