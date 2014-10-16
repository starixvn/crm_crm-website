// --------------------------------------------------------------------------
// <copyright file="WeeklyStatisticsReportController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Controllers
{
    using System.Web.Mvc;
    using Fanex.CRM.Infrastructure.Common;
    using Fanex.CRM.Infrastructure.Web.Controllers;
    using Fanex.CRM.Report.Criteria.WeeklyStatisticsReport;
    using Fanex.CRM.Report.Services.WeeklyStatisticsReport;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// Weekly Statistics Report Controller.
    /// </summary>
    public class WeeklyStatisticsReportController : BaseController
    {
        private IWeeklyStatisticsReportService _weeklyStatisticsReportService;

        public WeeklyStatisticsReportController()
            : this(null)
        {
        }

        public WeeklyStatisticsReportController(IWeeklyStatisticsReportService weeklyStatisticsReportService)
        {
            _weeklyStatisticsReportService = weeklyStatisticsReportService ?? new WeeklyStatisticsReportService();
        }

        [NapAuth(FunctionKey = FunctionPermission.CRMReport, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            return View("~/Areas/Report/WeeklyStatisticsReport/Index.cshtml");
        }

        /// <summary>
        /// Gets the main report.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The main report.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermission.CRMReport, Policy = Constant.Policy.Read)]
        public ActionResult MainReport(GetMainReportCriteria criteria)
        {
            var ibcReport = _weeklyStatisticsReportService.MainReport(new GetMainReportCriteria
            {
                FromDate = criteria.FromDate,
                ToDate = criteria.ToDate,
                SiteType = (int)Enumerations.ReportTypes.CreditSite
            });

            var licenseeReport = _weeklyStatisticsReportService.MainReport(new GetMainReportCriteria
            {
                FromDate = criteria.FromDate,
                ToDate = criteria.ToDate,
                SiteType = (int)Enumerations.ReportTypes.Licensee
            });

            var jsonResult = Json(new
            {
                IBCReport = ibcReport,
                LicenseeReport = licenseeReport
            });

            return jsonResult;
        }

        [NapAuth(FunctionKey = FunctionPermission.CRMReport, Policy = Constant.Policy.Read)]
        public void ExportIbcReport(GetMainReportCriteria criteria)
        {
            criteria.SiteType = (int)Enumerations.ReportTypes.CreditSite;
            _weeklyStatisticsReportService.ExportMainReport(criteria);
        }

        [NapAuth(FunctionKey = FunctionPermission.CRMReport, Policy = Constant.Policy.Read)]
        public void ExportLicenseeReport(GetMainReportCriteria criteria)
        {
            criteria.SiteType = (int)Enumerations.ReportTypes.Licensee;
            _weeklyStatisticsReportService.ExportMainReport(criteria);
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermission.CRMReport, Policy = Constant.Policy.Read)]
        public ActionResult DetailReport(GetDetailReportCriteria criteria)
        {
            var result = _weeklyStatisticsReportService.DetailReport(criteria);
            var jsonResult = Json(result);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [NapAuth(FunctionKey = FunctionPermission.CRMReport, Policy = Constant.Policy.Read)]
        public void ExportDetailReport(GetDetailReportCriteria criteria)
        {
            _weeklyStatisticsReportService.ExportDetailReport(criteria);
        }
    }
}
