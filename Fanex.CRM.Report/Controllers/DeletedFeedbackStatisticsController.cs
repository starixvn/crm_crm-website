namespace Fanex.CRM.Report.Controllers
{
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Controllers;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using System.Web.Mvc;

    public class DeletedFeedbackStatisticsController : ReadOnlyBaseController
    {
        [NapAuth(FunctionKey = FunctionPermissions.DeletedFeedbackStatistic, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            return new RedirectResult(string.Format("{0}/DeletedFeedbackStatistic", ExternalDomain), false);
        }
    }
}