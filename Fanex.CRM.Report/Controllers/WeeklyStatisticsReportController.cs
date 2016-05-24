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
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Controllers;
    using Fanex.CRM.Common.Enumerations;
    using Fanex.CRM.Report.Criteria.StatisticByDateReport;
    using Fanex.CRM.Report.Services.StatisticByDateReport;
    using Fanex.CRM.Report.Services.WeeklyStatisticsReport;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// Weekly Statistics Report Controller.
    /// </summary>
    [NapAuth(FunctionKey = FunctionPermissions.WeeklyStatistics, Policy = Constant.Policy.Read)]
    public class WeeklyStatisticsReportController : ReadOnlyBaseController
    {
        private IStatisticByDateReportService _weeklyStatisticsReportService;

        public WeeklyStatisticsReportController()
            : this(null)
        {
        }

        public WeeklyStatisticsReportController(IStatisticByDateReportService weeklyStatisticsReportService)
        {
            _weeklyStatisticsReportService = weeklyStatisticsReportService ?? new WeeklyStatisticsReportService();
        }

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
        public ActionResult MainReport(GetMainReportCriteria criteria)
        {
            var jsonResult = Json(new
            {
                IBCReport = _weeklyStatisticsReportService.GetMainReport(new GetMainReportCriteria
                {
                    FromDate = criteria.FromDate,
                    ToDate = criteria.ToDate,
                    SiteType = (int)ReportType.CreditSite
                }),
                LicenseeReport = _weeklyStatisticsReportService.GetMainReport(new GetMainReportCriteria
                {
                    FromDate = criteria.FromDate,
                    ToDate = criteria.ToDate,
                    SiteType = (int)ReportType.Licensee
                })
            });

            return jsonResult;
        }

        public void ExportIbcReport(GetMainReportCriteria criteria)
        {
            criteria.SiteType = (int)ReportType.CreditSite;
            _weeklyStatisticsReportService.ExportMainReport(criteria);
        }

        public void ExportLicenseeReport(GetMainReportCriteria criteria)
        {
            criteria.SiteType = (int)ReportType.Licensee;
            _weeklyStatisticsReportService.ExportMainReport(criteria);
        }

        [HttpPost]
        public ActionResult DetailReport(GetDetailReportCriteria criteria)
        {
            var result = _weeklyStatisticsReportService.DetailReport(criteria);
            var jsonResult = Json(result);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public void ExportDetailReport(GetDetailReportCriteria criteria)
        {
            _weeklyStatisticsReportService.ExportDetailReport(criteria);
        }
    }
}
