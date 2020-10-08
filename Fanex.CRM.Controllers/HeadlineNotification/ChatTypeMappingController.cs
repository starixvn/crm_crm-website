namespace Fanex.CRM.Controllers.HeadlineNotification
{
    using System.Web.Mvc;
    using Fanex.CRM.Common.Attributes;
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Controllers;
    using Fanex.CRM.Common.Services;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    [AllowCrossSite]
    [NapAuth(FunctionKey = FunctionPermissions.ChatTypeMapping, Policy = Constant.Policy.Read)]
    public class ChatTypeMappingController : ReadOnlyBaseController
    {
        public ActionResult Index()
        {
            return new RedirectResult(string.Format("{0}ChatTypeMapping", AppSetting.ExternalSiteHostUrl), false);
        }
    }
}