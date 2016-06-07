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
    using Fanex.CRM.Common.Attributes;
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Controllers;
    using Fanex.CRM.Report.Criteria.CasinoReport;
    using Fanex.CRM.Report.Services.CasinoReport;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// Casino Report Controller.
    /// </summary>
    public class CasinoReportController : ReadOnlyBaseController
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
        [NapAuth(FunctionKey = FunctionPermissions.CasinoDailyStatistics, Policy = Constant.Policy.Read)]
        public ActionResult CasinoDailyStatistics()
        {
            return View("~/Areas/Report/CasinoDailyStatistics/Index.cshtml");
        }

        [NapAuth(FunctionKey = FunctionPermissions.CasinoDailyStatistics, Policy = Constant.Policy.Read)]
        public ActionResult CasinoDailySummary(GetCasinoSummaryCriteria criteria)
        {
            var report = _casinoReportService.GetCasinoDailySummary(criteria);
            return Json(report, JsonRequestBehavior.AllowGet);
        }

        [NapAuth(FunctionKey = FunctionPermissions.CasinoDailyStatistics, Policy = Constant.Policy.Read)]
        public ActionResult ExportCasinoDailySummary(GetCasinoSummaryCriteria criteria)
        {
            _casinoReportService.ExportCasinoDailySummary(criteria);
            return new EmptyResult();
        }

        [NapAuth(FunctionKey = FunctionPermissions.CasinoDailyStatistics, Policy = Constant.Policy.Read)]
        public ActionResult CasinoDailyDetail(GetCasinoDetailCriteria criteria)
        {
            var report = _casinoReportService.GetCasinoDailyDetail(criteria);
            return Json(report, JsonRequestBehavior.AllowGet);
        }

        [NapAuth(FunctionKey = FunctionPermissions.CasinoDailyStatistics, Policy = Constant.Policy.Read)]
        public ActionResult ExportCasinoDailyDetail(GetCasinoDetailCriteria criteria)
        {
            _casinoReportService.ExportCasinoDailyDetail(criteria);
            return new EmptyResult();
        }

        // Casino Weekly Statistics.
        [NapAuth(FunctionKey = FunctionPermissions.CasinoWeeklyStatistics, Policy = Constant.Policy.Read)]
        public ActionResult CasinoWeeklyStatistics()
        {
            return View("~/Areas/Report/CasinoWeeklyStatistics/Index.cshtml");
        }

        [NapAuth(FunctionKey = FunctionPermissions.CasinoWeeklyStatistics, Policy = Constant.Policy.Read)]
        public ActionResult CasinoWeeklySummary(GetCasinoSummaryCriteria criteria)
        {
            var report = _casinoReportService.GetCasinoWeeklySummary(criteria);
            return Json(report, JsonRequestBehavior.AllowGet);
        }

        [NapAuth(FunctionKey = FunctionPermissions.CasinoWeeklyStatistics, Policy = Constant.Policy.Read)]
        public ActionResult ExportCasinoWeeklySummary(GetCasinoSummaryCriteria criteria)
        {
            _casinoReportService.ExportCasinoWeeklySummary(criteria);
            return new EmptyResult();
        }

        [NapAuth(FunctionKey = FunctionPermissions.CasinoWeeklyStatistics, Policy = Constant.Policy.Read)]
        public ActionResult CasinoWeeklyDetail(GetCasinoDetailCriteria criteria)
        {
            var report = _casinoReportService.GetCasinoWeeklyDetail(criteria);
            return Json(report, JsonRequestBehavior.AllowGet);
        }

        [NapAuth(FunctionKey = FunctionPermissions.CasinoWeeklyStatistics, Policy = Constant.Policy.Read)]
        public ActionResult ExportCasinoWeeklyDetail(GetCasinoDetailCriteria criteria)
        {
            criteria.ReportType = 3; // Weekly
            _casinoReportService.ExportCasinoDailyDetail(criteria);
            return new EmptyResult();
        }
    }
}
