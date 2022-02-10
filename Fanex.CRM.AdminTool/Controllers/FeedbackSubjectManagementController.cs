namespace Fanex.CRM.AdminTool.Controllers
{
    using System.Web.Mvc;
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Controllers;
    using Fanex.NAP.Core.Business;
    using NAP.Core.Web.Attribute;

    [NapAuth(FunctionKey = FunctionPermissions.FBSubjectTemplateManagement, Policy = Constant.Policy.Read)]
    public class FeedbackSubjectManagementController : ReadOnlyBaseController
    {
        public ActionResult Index()
        {
            return new RedirectResult(string.Format("{0}/FeedbackSubject", ExternalDomain), false);
        }
    }
}