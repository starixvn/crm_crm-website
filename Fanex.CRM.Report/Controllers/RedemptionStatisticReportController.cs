// --------------------------------------------------------------------------
// <copyright file="RedemptionStatisticReportController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ken.Nguyen" date="2014.11.24">Created</change>
// </history>

namespace Fanex.CRM.Report.Controllers
{
    using System.Web.Mvc;
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Controllers;
    using Fanex.CRM.Report.Criteria.DynamicReport;
    using Fanex.CRM.Report.Services.DynamicReport;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using Fanex.CRM.Report.Criteria.RedemptionStatisticsReport;
    using Fanex.Excel.Extensions;

    /// <summary>
    /// Feedback statistic report controller.
    /// </summary>
    public class RedemptionStatisticReportController : BaseController
    {
        private readonly IRedemptionStatisticsService _redemptionStatisticsService;

        public RedemptionStatisticReportController()
            : this(null)
        {
        }

        public RedemptionStatisticReportController(IRedemptionStatisticsService redemptionStatisticsService)
        {
            _redemptionStatisticsService = redemptionStatisticsService ?? new RedemptionStatisticsService();
        }

        /// <summary>
        /// Gets all setting.
        /// </summary>
        /// <returns>All setting.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermissions.CRMReport, Policy = Constant.Policy.Read)]
        public ActionResult AllSetting()
        {
            var allSetting = new
            {
                StatusList = _redemptionStatisticsService.AllStatus(),
                CurrencyList = _redemptionStatisticsService.AllCurrencyRedemption(),
                ReportByList = _redemptionStatisticsService.AllReportBy()
            };

            return Json(allSetting, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermissions.CRMReport, Policy = Constant.Policy.Read)]
        public ActionResult GetRedemptionStisticsSummaryReport(GetRedemptionStatisticsSummaryCriteria criteria)
        {
            criteria.Subject = criteria.Subject ?? string.Empty;

            var summaryReport = _redemptionStatisticsService.GetRedemptionSummaryReport(criteria);
            var jsonResult = Json(summaryReport, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermissions.CRMReport, Policy = Constant.Policy.Read)]
        public ActionResult GetRedemptionStisticsSummaryReportV3(GetRedemptionStatisticsSummaryCriteria criteria)
        {
            criteria.Subject = criteria.Subject ?? string.Empty;

            var summaryReport = _redemptionStatisticsService.GetRedemptionSummaryReport(criteria);
            var jsonResult = Json(summaryReport, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermissions.CRMReport, Policy = Constant.Policy.Read)]
        public ActionResult GetRedemptionStisticsSummaryReportV2(GetRedemptionStatisticsSummaryCriteria criteria)
        {

            criteria.Subject = criteria.Subject ?? string.Empty;

            return new JsonNetResult(_redemptionStatisticsService.GetRedemptionSummaryReportV2(criteria));
            

            
            //criteria.Subject = criteria.Subject ?? string.Empty;

            //var summaryReport = _redemptionStatisticsService.GetRedemptionSummaryReport(criteria);
            //var jsonResult = Json(summaryReport, JsonRequestBehavior.AllowGet);
            //return jsonResult;
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermissions.CRMReport, Policy = Constant.Policy.Read)]
        public ActionResult GetRedemptionStisticsDetailReport(GetRedemptionStatisticsDetailCriteria criteria)
        {
            var detailReport = _redemptionStatisticsService.GetRedemptionDetailReport(criteria);
            var jsonResult = Json(detailReport, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [NapAuth(FunctionKey = FunctionPermissions.CRMReport, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            //var settings = _redemptionStatisticsService.GetFeedbackStatisticFilterType();
            return View("~/Areas/Report/RedemptionStatistics/Index.cshtml", null);
        }
    }
}
