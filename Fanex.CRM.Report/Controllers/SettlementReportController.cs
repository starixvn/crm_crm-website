namespace Fanex.CRM.Report.Controllers
{
    using System.Web.Mvc;
    using Fanex.CRM.Common.Attributes;
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Controllers;
    using Fanex.CRM.Report.Criteria.SettlementReport;
    using Fanex.CRM.Report.Services.SettlementReport;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    [NapAuth(FunctionKey = FunctionPermissions.SettlementIssuesReport, Policy = Constant.Policy.Read)]
    public class SettlementReportController : ReadOnlyBaseController
    {
        private readonly ISettlementReportService _settlementReportService;

        public SettlementReportController()
            : this(null)
        {
        }

        public SettlementReportController(ISettlementReportService settlementReportService)
        {
            _settlementReportService = settlementReportService ?? new SettlementReportService();
        }

        public ActionResult Index()
        {
            return View("~/Areas/Report/SettlementReport/Index.cshtml");
        }

        [HttpPost]
        [HandleAjaxError]
        public ActionResult GetSettlementReport(GetSettlementReportCriteria criteria)
        {
            var logs = _settlementReportService.GetSettlementReport(criteria);
            return Json(logs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExportSettlementReport(GetSettlementReportCriteria criteria)
        {
            _settlementReportService.ExportSettlementReport(criteria);
            return new EmptyResult();
        }
    }
}