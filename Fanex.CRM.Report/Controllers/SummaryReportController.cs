namespace Fanex.CRM.Report.Controllers
{
    using System.Web.Mvc;
    using Fanex.CRM.Common.Attributes;
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Controllers;
    using Fanex.CRM.Report.Criteria.SummaryReport;
    using Fanex.CRM.Report.Services.SummaryReport;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    [NapAuth(FunctionKey = FunctionPermissions.CategoriesSummaryReport, Policy = Constant.Policy.Read)]
    public class SummaryReportController : ReadOnlyBaseController
    {
        private readonly ISummaryReportService _summaryService;

        public SummaryReportController()
            : this(null)
        {
        }

        public SummaryReportController(ISummaryReportService summaryService)
        {
            _summaryService = summaryService ?? new SummaryReportService();
        }

        [HttpGet]
        public ActionResult AllSettings()
        {
            return Json(_summaryService.AllSettings(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View("~/Areas/Report/SummaryReport/Index.cshtml");
        }

        [HttpGet]
        [HandleAjaxError]
        public ActionResult GetSummaryReport(GetSummaryReportCriteria criteria)
        {
            var report = _summaryService.GetSummaryReport(criteria);
            return Json(report, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ExportSummaryReportExcel(GetSummaryReportCriteria criteria)
        {
            _summaryService.ExportSummaryReportExcel(criteria);
            return new EmptyResult();
        }
    }
}