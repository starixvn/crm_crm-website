namespace Fanex.CRM.Report.Controllers
{
    using System.Web.Mvc;
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Controllers;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    [NapAuth(FunctionKey = FunctionPermissions.IssueStatistics, Policy = Constant.Policy.Read)]
    public class StatisticReportController : ReadOnlyBaseController
    {

        public ActionResult Index()
        {
            return new RedirectResult(string.Format("{0}/IssueStatistics", ExternalDomain), false);
        }
    }
}