// --------------------------------------------------------------------------
// <copyright file="FeedbackStatisticReportController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.10.10">Created</change>
// </history>

namespace Fanex.CRM.Report.Controllers
{
    using System.Web.Mvc;
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Controllers;
    using Fanex.CRM.Report.Criteria.FeedbackStatisticReport;
    using Fanex.CRM.Report.Services.FeedbackStatisticReport;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// Feedback statistic report controller.
    /// </summary>
    [NapAuth(FunctionKey = FunctionPermissions.FeedbackStatistics, Policy = Constant.Policy.Read)]
    public class FeedbackStatisticReportController : ReadOnlyBaseController
    {
        private readonly IFeedbackStatisticReportService _feedbackStatisticService;

        public FeedbackStatisticReportController()
            : this(null)
        {
        }

        public FeedbackStatisticReportController(IFeedbackStatisticReportService feedbackStatisticService)
        {
            _feedbackStatisticService = feedbackStatisticService ?? new FeedbackStatisticReportService();
        }

        public ActionResult Index()
        {
            var settings = _feedbackStatisticService.GetFeedbackStatisticFilterType();
            return View("~/Areas/Report/FeedbackStatistics/Index.cshtml", settings);
        }

        public ActionResult SearchFeedbackStatistic(GetFeedbackStatisticCriteria criteria)
        {
            var report = _feedbackStatisticService.GetFeedbackStatistic(criteria);
            return Json(report, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExportFeedbackStatistic(GetFeedbackStatisticCriteria criteria)
        {
            _feedbackStatisticService.ExportFeedbackStatistic(criteria);
            return new EmptyResult();
        }

        public ActionResult FeedbackStatisticDetail(GetFeedbackStatisticDetailCriteria criteria)
        {
            return View("~/Areas/Report/FeedbackStatistics/FeedbackStatisticDetail/Index.cshtml", criteria);
        }

        public ActionResult GetFeedbackStatisticDetail(GetFeedbackStatisticDetailCriteria criteria)
        {
            var report = _feedbackStatisticService.FeedbackStatisticDetail(criteria);
            return Json(report, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExportFeedbackStatisticDetail(GetFeedbackStatisticDetailCriteria criteria)
        {
            _feedbackStatisticService.ExportFeedbackStatisticDetail(criteria);
            return new EmptyResult();
        }
    }
}
