namespace Fanex.CRM.Controllers
{
    using System.Web.Mvc;
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Controllers;
    using Fanex.CRM.Common.Services;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    [NapAuth(FunctionKey = FunctionPermissions.NotificationType, Policy = Constant.Policy.Read)]
    public class NotificationTypeController : ReadOnlyBaseController
    {
        public ActionResult Index()
        {
            return new RedirectResult(string.Format("{0}NotificationType", AppSetting.ExternalSiteHostUrl), false);
        }
    }
}