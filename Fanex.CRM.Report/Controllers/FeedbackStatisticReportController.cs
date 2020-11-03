namespace Fanex.CRM.Report.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Common.Enumerations;
    using Common.Models;
    using Common.Services;
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Controllers;
    using Fanex.CRM.Report.Criteria.FeedbackStatisticReport;
    using Fanex.CRM.Report.Services.FeedbackStatisticReport;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using Models.FeedbackStatisticReport;
    using Setting.Services;

    [NapAuth(FunctionKey = FunctionPermissions.FeedbackStatistics, Policy = Constant.Policy.Read)]
    public class FeedbackStatisticReportController : ReadOnlyBaseController
    {
        private readonly IFeedbackStatisticReportService _feedbackStatisticService;
        private readonly ISettingService _serviceSetting;

        public FeedbackStatisticReportController() : this(null, null)
        {
        }

        public FeedbackStatisticReportController(
            IFeedbackStatisticReportService feedbackStatisticService,
            ISettingService serviceSetting)
        {
            _feedbackStatisticService = feedbackStatisticService ?? new FeedbackStatisticReportService();
            _serviceSetting = serviceSetting ?? new SettingService();
        }

        public ActionResult Index()
        {
            var settings = (AppSession.CurrentUser.UserRole == UserRole.CN88Staff)
                ? _feedbackStatisticService.GetFeedbackStatisticFilterTypeCN88()
                : _feedbackStatisticService.GetFeedbackStatisticFilterType();

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
                    MainCategories = _serviceSetting.GetAllMainCategoriesFromAllSettings().Where(x => x.IsActive),
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
                var mainCategories = _serviceSetting.GetAllMainCategoriesFromAllSettings().Where(x => x.IsActive);
                _feedbackStatisticService.ExportFeedbackStatisticDetail(criteria, mainCategories);
            }

            return new EmptyResult();
        }
    }
}