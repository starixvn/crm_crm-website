namespace Fanex.CRM.Report.Controllers
{
    using System.Web.Mvc;
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Controllers;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    public class LogStatisticsController : ReadOnlyBaseController
    {

        [NapAuth(FunctionKey = FunctionPermissions.LogStatistics, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            return new RedirectResult(string.Format("{0}/LogStatistics", ExternalDomain), false);
        }
    }
}