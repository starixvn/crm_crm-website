// --------------------------------------------------------------------------
// <copyright file="CasinoReportController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.07.31">Created</change>
// </history>

namespace Fanex.CRM.Report.Controllers
{
    using System.Web.Mvc;
    using Fanex.CRM.Common.Controllers;
    using Fanex.CRM.Common.Services;
    using Fanex.CRM.Report.Criteria.CasinoReport;
    using Fanex.CRM.Report.Services.CasinoReport;

    /// <summary>
    /// Casino Report Controller.
    /// </summary>
    public class CasinoReportController : BaseController
    {
        private readonly ICasinoReportService _casinoReportService;

        public CasinoReportController()
            : this(null)
        { 
        }
        
        public CasinoReportController(ICasinoReportService casinoReportService)
        {
            _casinoReportService = casinoReportService ?? new CasinoReportService();
        }

        // Casino Daily Statistics.
        [AuthenticateAction(AuthenticateAction.AuthenticationType.Read, "CasinoDailyStatistics")]
        public ActionResult CasinoDailyStatistics()
        {
            return View("~/Areas/Report/CasinoDailyStatistics/Index.cshtml");
        }

        [AuthenticateAction(AuthenticateAction.AuthenticationType.Read, "CasinoDailyStatistics")]
        public ActionResult CasinoDailySummary(GetCasinoSummaryCriteria criteria)
        {
            var report = _casinoReportService.GetCasinoDailySummary(criteria);
            return Json(report, JsonRequestBehavior.AllowGet);
        }

        [AuthenticateAction(AuthenticateAction.AuthenticationType.Read, "CasinoDailyStatistics")]
        public ActionResult ExportCasinoDailySummary(GetCasinoSummaryCriteria criteria)
        {
            _casinoReportService.ExportCasinoDailySummary(criteria);
            return new EmptyResult();
        }

        [AuthenticateAction(AuthenticateAction.AuthenticationType.Read, "CasinoDailyStatistics")]
        public ActionResult CasinoDailyDetail(GetCasinoDetailCriteria criteria)
        {
            var report = _casinoReportService.GetCasinoDailyDetail(criteria);
            return Json(report, JsonRequestBehavior.AllowGet);
        }

        [AuthenticateAction(AuthenticateAction.AuthenticationType.Read, "CasinoDailyStatistics")]
        public ActionResult ExportCasinoDailyDetail(GetCasinoDetailCriteria criteria)
        {
            _casinoReportService.ExportCasinoDailyDetail(criteria);
            return new EmptyResult();
        }

        // Casino Weekly Statistics.
        [AuthenticateAction(AuthenticateAction.AuthenticationType.Read, "CasinoWeeklyStatistics")]
        public ActionResult CasinoWeeklyStatistics()
        {
            return View("~/Areas/Report/CasinoWeeklyStatistics/Index.cshtml");
        }

        [AuthenticateAction(AuthenticateAction.AuthenticationType.Read, "CasinoWeeklyStatistics")]
        public ActionResult CasinoWeeklySummary(GetCasinoSummaryCriteria criteria)
        {
            var report = _casinoReportService.GetCasinoWeeklySummary(criteria);
            return Json(report, JsonRequestBehavior.AllowGet);
        }

        [AuthenticateAction(AuthenticateAction.AuthenticationType.Read, "CasinoWeeklyStatistics")]
        public ActionResult ExportCasinoWeeklySummary(GetCasinoSummaryCriteria criteria)
        {
            _casinoReportService.ExportCasinoWeeklySummary(criteria);
            return new EmptyResult();
        }

        [AuthenticateAction(AuthenticateAction.AuthenticationType.Read, "CasinoWeeklyStatistics")]
        public ActionResult CasinoWeeklyDetail(GetCasinoDetailCriteria criteria)
        {
            var report = _casinoReportService.GetCasinoWeeklyDetail(criteria);
            return Json(report, JsonRequestBehavior.AllowGet);
        }
    }
}
