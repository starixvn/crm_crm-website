// --------------------------------------------------------------------------
// <copyright file="SummaryReportController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// <change who="Kan.Nguyen" date="2014.04.08">Created</change>
// </history>

namespace Fanex.CRM.Report.Controllers
{
    using System.Web.Mvc;
    using Fanex.CRM.Common.Controllers;
    using Fanex.CRM.Common.Enumerations;
    using Fanex.CRM.Common.Services;
    using Fanex.CRM.Infrastructure.Services;
    using Fanex.CRM.Report.Criteria.SummaryReport;
    using Fanex.CRM.Report.Services.SummaryReport;

    /// <summary>
    /// SummaryReportController controller.
    /// </summary>
    public class SummaryReportController : BaseController
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
        [AuthenticateAction(AuthenticationType.Read, "Index")]
        public ActionResult AllSettings()
        {
            return Json(_summaryService.AllSettings(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AuthenticateAction(AuthenticationType.Read, "Index")]
        public ActionResult Index()
        {
            return View("~/Areas/Report/SummaryReport/Index.cshtml");
        }

        [HttpGet]
        [AuthenticateAction(AuthenticationType.Read, "Index")]
        [HandleAjaxError]
        public ActionResult GetSummaryReport(GetSummaryReportCriteria criteria)
        {
            var report = _summaryService.GetSummaryReport(criteria);
            return Json(report, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AuthenticateAction(AuthenticationType.Read, "Index")]
        public ActionResult ExportSummaryReportExcel(GetSummaryReportCriteria criteria)
        {
            _summaryService.ExportSummaryReportExcel(criteria);
            return new EmptyResult();
        }
    }
}