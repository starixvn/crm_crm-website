// --------------------------------------------------------------------------
// <copyright file="DailyStatisticsReportController.cs" company="Nexcel Solutions Vietnam">
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
    using Fanex.CRM.Report.Services.DailyStatisticsReport;
    using Fanex.CRM.Report.Services.StatisticByDateReport;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// Daily Statistics Report Controller.
    /// </summary>
    public class DailyStatisticsReportController : BaseController
    {
        private IStatisticByDateReportService _dailyStatisticsReportService;

        public DailyStatisticsReportController()
            : this(null)
        {
        }

        public DailyStatisticsReportController(IStatisticByDateReportService dailyStatisticsReportService)
        {
            _dailyStatisticsReportService = dailyStatisticsReportService ?? new DailyStatisticsReportService();
        }

        [NapAuth(FunctionKey = FunctionPermissions.CRMReport, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            return View("~/Areas/Report/DailyStatisticsReport/Index.cshtml");
        }

        /// <summary>
        /// Gets the main report.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The main report.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermissions.CRMReport, Policy = Constant.Policy.Read)]
        public ActionResult MainReport(GetMainReportCriteria criteria)
        {
            var jsonResult = Json(new
            {
                IBCReport = _dailyStatisticsReportService.MainReport(new GetMainReportCriteria
                {
                    FromDate = criteria.FromDate,
                    ToDate = criteria.ToDate,
                    SiteType = (int)ReportType.CreditSite
                }),
                LicenseeReport = _dailyStatisticsReportService.MainReport(new GetMainReportCriteria
                {
                    FromDate = criteria.FromDate,
                    ToDate = criteria.ToDate,
                    SiteType = (int)ReportType.Licensee
                })
            });

            return jsonResult;
        }

        [NapAuth(FunctionKey = FunctionPermissions.CRMReport, Policy = Constant.Policy.Read)]
        public void ExportIbcReport(GetMainReportCriteria criteria)
        {
            criteria.SiteType = (int)ReportType.CreditSite;
            _dailyStatisticsReportService.ExportMainReport(criteria);
        }

        [NapAuth(FunctionKey = FunctionPermissions.CRMReport, Policy = Constant.Policy.Read)]
        public void ExportLicenseeReport(GetMainReportCriteria criteria)
        {
            criteria.SiteType = (int)ReportType.Licensee;
            _dailyStatisticsReportService.ExportMainReport(criteria);
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermissions.CRMReport, Policy = Constant.Policy.Read)]
        public ActionResult DetailReport(GetDetailReportCriteria criteria)
        {
            var result = _dailyStatisticsReportService.DetailReport(criteria);
            var jsonResult = Json(result);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [NapAuth(FunctionKey = FunctionPermissions.CRMReport, Policy = Constant.Policy.Read)]
        public void ExportDetailReport(GetDetailReportCriteria criteria)
        {
            _dailyStatisticsReportService.ExportDetailReport(criteria);
        }
    }
}
