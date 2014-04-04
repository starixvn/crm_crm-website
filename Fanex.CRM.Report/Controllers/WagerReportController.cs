// --------------------------------------------------------------------------
// <copyright file="WagerReportController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.03.28">Created</change>
// </history>

namespace Fanex.CRM.Report.Controllers
{
    using System.Web.Mvc;
    using Fanex.CRM.Infrastructure.Web.Controllers;
    using Fanex.CRM.Report.Criteria.WagerReport;
    using Fanex.CRM.Report.Services.Implements.WagerReport;
    using Fanex.CRM.Report.Services.Interfaces;

    /// <summary>
    /// WagerReportController controller.
    /// </summary>
    public class WagerReportController : BaseController
    {
        private readonly IWagerReportService _wagerService;

        public WagerReportController()
            : this(null)
        {
        }

        public WagerReportController(IWagerReportService wagerService)
        {
            _wagerService = wagerService ?? new WagerReportService();
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View("~/Areas/Report/WagerReport/Index.cshtml");
        }

        [HttpGet]
        public ActionResult GetWagerReport(GetWagerReportCriteria criteria)
        {
            var report = _wagerService.GetWagerReport(criteria);
            return Json(report, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ExportWagerReportExcel(GetWagerReportCriteria criteria)
        {
            _wagerService.ExportWagerReportExcel(criteria);
            return new EmptyResult();
        }
    }
}
