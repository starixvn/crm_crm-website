namespace Fanex.CRM.Feedback.Controllers
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Common.Models;
    using Common.Services;
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Controllers;
    using Fanex.CRM.Common.Excel;
    using Fanex.CRM.Feedback.Criteria;
    using Fanex.CRM.Feedback.Excels;
    using Fanex.CRM.Feedback.Excels.LatestFeedback;
    using Fanex.CRM.Feedback.Resources;
    using Fanex.CRM.Feedback.Services;
    using Fanex.CRM.Feedback.ViewModels;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using Setting.Services;

    public class FeedbacksController : ReadOnlyBaseController
    {
        private readonly IFeedbackService _feedbackService;
        private readonly ISettingService _serviceSetting;

        public FeedbacksController()
            : this(null, null)
        {
        }

        public FeedbacksController(IFeedbackService feedbackService, ISettingService serviceSetting)
        {
            _feedbackService = feedbackService ?? new FeedbackService();
            _serviceSetting = serviceSetting ?? new SettingService();
        }

        [HttpGet]
        public ActionResult AllSetting()
        {
            var hasPermission = HasReadPermission(FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo) ||
                                HasReadPermission(FunctionPermissions.DeleteFeedback);
            dynamic settings = null;
            if (hasPermission)
            {
                settings = _feedbackService.GetAllSettings();
            }

            return Json(settings, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [NapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Read)]
        public ActionResult ManageFeedback(ManageFeedbackViewModel model)
        {
            ViewBag.CreateUpdateFeedbackPermission = HasPermission(FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Constant.Policy.Host);
            ViewBag.UserRole = AppSession.CurrentUser.UserRole;
            return View("~/Areas/Feedbacks/ManageFeedback/Index.cshtml", model);
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Read)]
        public ActionResult SearchManageFeedback(ManageFeedbackCriteria criteria)
        {
            if (criteria.IsValid())
            {
                var feedbacks = _feedbackService.GetManageFeedbacks(criteria);
                var report = new
                {
                    Feedbacks = feedbacks,
                    TotalItems = criteria.TotalRows,
                    MainCategories = _serviceSetting.GetAllMainCategoriesFromAllSettings()
                };
                return Json(report, JsonRequestBehavior.DenyGet);
            }

            return Json(new { Feedbacks = new List<FeedbackDetail>() }, JsonRequestBehavior.DenyGet);
        }

        [HttpGet]
        [NapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Read)]
        public ActionResult LatestFeedback(int custId = 0, string custName = null)
        {
            ViewBag.CustId = custId;
            ViewBag.Username = custName;
            ViewBag.CreateUpdateFeedbackPermission = HasPermission(FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Constant.Policy.Host);
            return View("~/Areas/Feedbacks/LatestFeedback/Index.cshtml", _feedbackService.GetTopItems);
        }

        [HttpGet]
        [NapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Read)]
        public ActionResult GetLatestFeedback(LatestFeedbackCriteria criteria)
        {
            var feedbacks = _feedbackService.GetLatestFeedbackReport(criteria);
            var fromCustomerText = criteria.CustId != 0 ?
                                    string.Format(Labels.FromCustomer, criteria.Username) :
                                    string.Empty;
            var title = string.Format(Labels.LatestFeedbackReportTitle, criteria.Top, fromCustomerText);

            var jsonResult = Json(
                new
                {
                    Title = title.Trim(),
                    Feedbacks = feedbacks,
                    Settings = _feedbackService.GetAllSettings(),
                    MainCategories = _serviceSetting.GetAllMainCategoriesFromAllSettings()
                },
                JsonRequestBehavior.AllowGet);

            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpGet]
        [NapAuth(FunctionKey = FunctionPermissions.DeleteFeedback, Policy = Constant.Policy.Read)]
        public ActionResult DeletedFeedback()
        {
            return View("~/Areas/Feedbacks/DeletedFeedback/Index.cshtml");
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermissions.DeleteFeedback, Policy = Constant.Policy.Read)]
        public ActionResult SearchDeletedFeedback(DeletedFeedbackCriteria criteria)
        {
            if (criteria.IsValid())
            {
                var feedbacks = _feedbackService.GetDeletedFeedbackReport(criteria);
                var report = new
                {
                    Feedbacks = feedbacks,
                    TotalItems = criteria.TotalItems,
                    MainCategories = _serviceSetting.GetAllMainCategoriesFromAllSettings()
                };
                return Json(report, JsonRequestBehavior.DenyGet);
            }

            return Json(new { Feedbacks = new List<Models.DeletedFeedback>() }, JsonRequestBehavior.DenyGet);
        }

        [HttpGet]
        [NapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Read)]
        public ActionResult CreateMultipleLogs()
        {
            return View("~/Areas/Feedbacks/ManageFeedback/Index.cshtml");
        }

        public void ExportLatestFeedbacks(LatestFeedbackCriteria criteria)
        {
            var fromCustomerText = criteria.CustId != 0 ?
                                    string.Format(Labels.FromCustomer, criteria.Username) :
                                    string.Empty;
            var title = string.Format(Labels.LatestFeedbackReportTitle, criteria.Top, fromCustomerText);
            var feedbacks = _feedbackService.GetLatestFeedbackReport(criteria);

            var mainCategories = _serviceSetting.GetAllMainCategoriesFromAllSettings();
            BaseExcel latestFeedbackExcel = new LatestFeedbackExcel(feedbacks, title.Trim(), mainCategories);
            latestFeedbackExcel.BuildWorkBook();
        }

        public void ExportManageFeedbacks(ManageFeedbackCriteria criteria)
        {
            if (criteria.IsValid())
            {
                criteria.PageIndex = 0;
                var feedbacks = _feedbackService.GetManageFeedbacks(criteria);

                var fromCustomerText = string.IsNullOrEmpty(criteria.Username) ?
                                        string.Empty :
                                        string.Format(Labels.FromCustomer, criteria.Username);
                var title = Labels.Feedback + " " + fromCustomerText;

                var mainCategories = _serviceSetting.GetAllMainCategoriesFromAllSettings();
                BaseExcel manageFeedbackExcel = new FeedbackExcel(feedbacks, mainCategories, title.Trim());
                manageFeedbackExcel.BuildWorkBook();
            }
        }

        public void ExportDeletedFeedbacks(DeletedFeedbackCriteria criteria)
        {
            if (criteria.IsValid())
            {
                criteria.IsExportExcel = true;
                var feedbacks = _feedbackService.GetDeletedFeedbackReport(criteria);

                var mainCategories = _serviceSetting.GetAllMainCategoriesFromAllSettings();
                BaseExcel deletedFeedbackExcel = new FeedbackExcel(feedbacks, mainCategories, Labels.DeletedFeedbackTitle);
                deletedFeedbackExcel.BuildWorkBook();
            }
        }

        [HttpPost]
        public ActionResult CheckFeedbackDuplication(CheckFeedbackDuplicationCriteria criteria)
        {
            var result = _feedbackService.CheckFeedbackDuplication(criteria);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}