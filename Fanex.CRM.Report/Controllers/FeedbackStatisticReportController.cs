// --------------------------------------------------------------------------
// <copyright file="FeedbackStatisticReportController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.10.10">Created</change>
// </history>

namespace Fanex.CRM.Report.Controllers
{
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Controllers;
    using Fanex.CRM.Report.Criteria.FeedbackStatisticReport;
    using Fanex.CRM.Report.Services.FeedbackStatisticReport;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using Setting.Services;
    using System.Web.Mvc;

    /// <summary>
    /// Feedback statistic report controller.
    /// </summary>
    [NapAuth(FunctionKey = FunctionPermissions.FeedbackStatistics, Policy = Constant.Policy.Read)]
    public class FeedbackStatisticReportController : ReadOnlyBaseController
    {
        private readonly IFeedbackStatisticReportService _feedbackStatisticService;
        public ISettingService _serviceSetting { get; set; }

        public FeedbackStatisticReportController()
            : this(null)
        {
        }

        public FeedbackStatisticReportController(IFeedbackStatisticReportService feedbackStatisticService)
        {
            _feedbackStatisticService = feedbackStatisticService ?? new FeedbackStatisticReportService();
            _serviceSetting = new SettingService();
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
            var result = new
            {
                Report = _feedbackStatisticService.FeedbackStatisticDetail(criteria),
                MainCategory = _serviceSetting.GetAllMainCategories()
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExportFeedbackStatisticDetail(GetFeedbackStatisticDetailCriteria criteria)
        {
            _feedbackStatisticService.ExportFeedbackStatisticDetail(criteria);
            return new EmptyResult();
        }
    }
}
