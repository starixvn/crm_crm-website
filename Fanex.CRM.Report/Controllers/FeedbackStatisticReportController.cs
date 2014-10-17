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
    using Fanex.CRM.Infrastructure.Web.Controllers;
    using Fanex.CRM.Report.Criteria.FeedbackStatisticReport;
    using Fanex.CRM.Report.Services.Implements.FeedbackStatisticReport;
    using Fanex.CRM.Report.Services.Interfaces;

    /// <summary>
    /// Feedback statistic report controller.
    /// </summary>
    public class FeedbackStatisticReportController : BaseController
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

        [AuthenticateAction(AuthenticateAction.AuthenticationType.Read, "Index")]
        public ActionResult Index()
        {
            var settings = _feedbackStatisticService.GetFeedbackStatisticFilterType();
            return View("~/Areas/Report/FeedbackStatistics/Index.cshtml", settings);
        }

        [AuthenticateAction(AuthenticateAction.AuthenticationType.Read, "Index")]
        public ActionResult SearchFeedbackStatistic(GetFeedbackStatisticCriteria criteria)
        {
            var report = _feedbackStatisticService.GetFeedbackStatistic(criteria);
            return Json(report, JsonRequestBehavior.AllowGet);
        }

        [AuthenticateAction(AuthenticateAction.AuthenticationType.Read, "Index")]
        public ActionResult ExportFeedbackStatistic(GetFeedbackStatisticCriteria criteria)
        {
            _feedbackStatisticService.ExportFeedbackStatistic(criteria);
            return new EmptyResult();
        }

        [AuthenticateAction(AuthenticateAction.AuthenticationType.Read, "FeedbackStatisticDetail")]
        public ActionResult FeedbackStatisticDetail(GetFeedbackStatisticDetailCriteria criteria)
        {
            return View("~/Areas/Report/FeedbackStatistics/FeedbackStatisticDetail/Index.cshtml", criteria);
        }

        [AuthenticateAction(AuthenticateAction.AuthenticationType.Read, "FeedbackStatisticDetail")]
        public ActionResult GetFeedbackStatisticDetail(GetFeedbackStatisticDetailCriteria criteria)
        {
            var report = _feedbackStatisticService.FeedbackStatisticDetail(criteria);
            return Json(report, JsonRequestBehavior.AllowGet);
        }

        [AuthenticateAction(AuthenticateAction.AuthenticationType.Read, "FeedbackStatisticDetail")]
        public ActionResult ExportFeedbackStatisticDetail(GetFeedbackStatisticDetailCriteria criteria)
        {
            _feedbackStatisticService.ExportFeedbackStatisticDetail(criteria);
            return new EmptyResult();
        }
    }
}
