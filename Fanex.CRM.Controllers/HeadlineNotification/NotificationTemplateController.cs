namespace Fanex.CRM.Controllers
{
    using System;
    using System.Web.Mvc;
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Controllers;
    using Fanex.CRM.Common.Helpers;
    using Fanex.CRM.Common.Services;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    [NapAuth(FunctionKey = FunctionPermissions.NotificationTemplate, Policy = Constant.Policy.Read)]
    public class NotificationTemplateController : ReadOnlyBaseController
    {
        public ActionResult Index()
        {
            SerilogHelper.WriteInformation(string.Format("notification template old - begin at: {0}", DateTime.Now));
            return new RedirectResult(string.Format("{0}NotificationTemplate", AppSetting.ExternalSiteHostUrl));
        }
    }
}