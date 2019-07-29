namespace Fanex.CRM.Report.Controllers
{
    using System.Web.Mvc;
    using Fanex.CRM.Common.ActionResults;
    using Fanex.CRM.Common.Attributes;
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Controllers;
    using Fanex.CRM.Report.Criteria.StatisticReport;
    using Fanex.CRM.Report.Services.StatisticReport;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    [NapAuth(FunctionKey = FunctionPermissions.IssueStatistics, Policy = Constant.Policy.Read)]
    public class StatisticReportController : ReadOnlyBaseController
    {
        private readonly IStatisticReportService _statisticReportService;

        public StatisticReportController()
            : this(null)
        {
        }

        public StatisticReportController(IStatisticReportService statisticReportrtService)
        {
            _statisticReportService = statisticReportrtService ?? new StatisticReportService();
        }

        public ActionResult Index()
        {
            return View("~/Areas/Report/StatisticReport/Index.cshtml");
        }

        [HttpPost]
        public ActionResult AllSetting()
        {
            var allSetting = new
            {
                PeriodTypeList = _statisticReportService.AllPeriodType(),
                MainCategoryList = _statisticReportService.AllMainCategory()
            };

            return Json(allSetting, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [HandleAjaxError]
        public JsonWithoutNullField GetStatisticReport(GetStatisticReportCriteria criteria)
        {
            var logs = _statisticReportService.StatisticReport(criteria);
            return new JsonWithoutNullField(logs);
        }

        public ActionResult ExportStatisticReport(GetStatisticReportCriteria criteria)
        {
            _statisticReportService.ExportStatisticReport(criteria);
            return new EmptyResult();
        }
    }
}