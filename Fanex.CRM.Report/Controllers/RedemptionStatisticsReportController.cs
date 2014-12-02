// --------------------------------------------------------------------------
// <copyright file="RedemptionStatisticsReportController.cs" company="Nexcel Solutions Vietnam">
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
    using Fanex.CRM.Common.Extensions;
    using Fanex.CRM.Infrastructure.Extensions;
    using Fanex.CRM.Report.Criteria.RedemptionStatisticsReport;
    using Fanex.CRM.Report.Resources;
    using Fanex.CRM.Report.Services.DynamicReport;
    using Fanex.Excel;
    using Fanex.Excel.Extensions;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// Feedback statistic report controller.
    /// </summary>
    public class RedemptionStatisticsReportController : BaseController
    {
        private readonly IRedemptionStatisticsService _redemptionStatisticsService;

        public RedemptionStatisticsReportController()
            : this(null)
        {
        }

        public RedemptionStatisticsReportController(IRedemptionStatisticsService redemptionStatisticsService)
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
        public ActionResult GetRedemptionStatisticsSummaryReport(GetRedemptionStatisticsSummaryCriteria criteria)
        {
            criteria.Subject = criteria.Subject ?? string.Empty;
            return new JsonNetResult(_redemptionStatisticsService.GetRedemptionSummaryReport(criteria));
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermissions.CRMReport, Policy = Constant.Policy.Read)]
        public ActionResult GetRedemptionStatisticsDetailReport(GetRedemptionStatisticsDetailCriteria criteria)
        {
            criteria.Subject = criteria.Subject ?? string.Empty;
            criteria.StatusId = criteria.StatusId ?? string.Empty;
            return new JsonNetResult(_redemptionStatisticsService.GetRedemptionDetailReport(criteria));
        }

        [HttpGet]
        public ActionResult ExportSummaryReport(GetRedemptionStatisticsSummaryCriteria criteria)
        {
            criteria.Subject = criteria.Subject ?? string.Empty;
            FanexExcel result = new FanexExcel(_redemptionStatisticsService.GetRedemptionSummaryReport(criteria));
            NpoiExcelExtensions.ResponseStreamExcel(result.ToExcel2003(), this.GetSummaryFileName(criteria));
            return new EmptyResult();
        }

        [HttpGet]
        public ActionResult ExportDetailReport(GetRedemptionStatisticsDetailCriteria criteria)
        {
            criteria.Subject = criteria.Subject ?? string.Empty;
            criteria.StatusId = criteria.StatusId ?? string.Empty;
            FanexExcel result = new FanexExcel(_redemptionStatisticsService.GetRedemptionDetailReport(criteria));
            NpoiExcelExtensions.ResponseStreamExcel(result.ToExcel2003(), Labels.RedemptionStatisticsDetailReportTitle);
            return new EmptyResult();
        }

        [NapAuth(FunctionKey = FunctionPermissions.CRMReport, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            return View("~/Areas/Report/RedemptionStatistics/Index.cshtml");
        }

        private string GetSummaryFileName(GetRedemptionStatisticsSummaryCriteria criteria)
        {
            return string.Format(
                Labels.RedemptionStatisticsReportTitle,
                criteria.FromDate.ToShortFormatedDateWithSlash(),
                criteria.ToDate.ToShortFormatedDateWithSlash());
        }
    }
}
