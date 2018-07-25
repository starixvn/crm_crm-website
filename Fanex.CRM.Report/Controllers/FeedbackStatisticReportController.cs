namespace Fanex.CRM.Report.Controllers
{
    using Common.Models;
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Controllers;
    using Fanex.CRM.Report.Criteria.FeedbackStatisticReport;
    using Fanex.CRM.Report.Services.FeedbackStatisticReport;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using Models.FeedbackStatisticReport;
    using Setting.Services;
    using System.Collections.Generic;
    using System.Web.Mvc;

    [NapAuth(FunctionKey = FunctionPermissions.FeedbackStatistics, Policy = Constant.Policy.Read)]
    public class FeedbackStatisticReportController : ReadOnlyBaseController
    {
        private readonly IFeedbackStatisticReportService _feedbackStatisticService;
        public readonly ISettingService _serviceSetting;

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
            if (criteria.IsValid())
            {
                var result = new
                {
                    Report = _feedbackStatisticService.FeedbackStatisticDetail(criteria),
                    MainCategories = _serviceSetting.GetAllMainCategoriesFromAllSettings(),
                };

                return Json(result, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                Report = new FeedbackStatisticDetail
                {
                    Title = string.Empty,
                    TotalItems = 0,
                    Feedbacks = new List<FeedbackDetail>()
                }
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExportFeedbackStatisticDetail(GetFeedbackStatisticDetailCriteria criteria)
        {
            if (criteria.IsValid())
            {
                var mainCategories = _serviceSetting.GetAllMainCategoriesFromAllSettings();
                _feedbackStatisticService.ExportFeedbackStatisticDetail(criteria, mainCategories);
            }

            return new EmptyResult();
        }
    }
}
