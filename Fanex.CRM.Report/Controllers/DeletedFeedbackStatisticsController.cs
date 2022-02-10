namespace Fanex.CRM.Report.Controllers
{
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Controllers;
    using Fanex.CRM.Common.Excel;
    using Fanex.CRM.Report.Criteria.DeletedFeedbackStatistics;
    using Fanex.CRM.Report.Excels.DeletedFeedbackStatistics;
    using Fanex.CRM.Report.Services.DeletedFeedbackStatistics;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using Models.DeletedFeedbackStatistics;
    using Setting.Services;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
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