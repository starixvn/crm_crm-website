namespace Fanex.CRM.AdminTool.Controllers
{
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Controllers;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using System.Web.Mvc;
    [NapAuth(FunctionKey = FunctionPermissions.JobSettingManagement, Policy = Constant.Policy.Read)]
    public class JobSettingManagementController : ReadOnlyBaseController
    {
        public ActionResult Index()
        {
            return new RedirectResult(string.Format("{0}/JobSettingManagement", ExternalDomain), false);
        }
    }
}
