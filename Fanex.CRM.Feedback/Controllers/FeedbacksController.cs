// --------------------------------------------------------------------------
// <copyright file="FeedbacksController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.06.03">Created</change>
// </history>
// --------------------------------------------------------------------------

namespace Fanex.CRM.Feedback.Controllers
{
    using System.Web.Mvc;
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

    /// <summary>
    /// Feedbacks Controller.
    /// </summary>
    public class FeedbacksController : ReadOnlyBaseController
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbacksController()
            : this(null)
        {
        }

        public FeedbacksController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService ?? new FeedbackService();
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
            return View("~/Areas/Feedbacks/ManageFeedback/Index.cshtml", model);
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Read)]
        public ActionResult SearchManageFeedback(ManageFeedbackCriteria criteria)
        {
            var feedbacks = _feedbackService.GetManageFeedbacks(criteria);
            var report = new { Feedbacks = feedbacks, TotalItems = criteria.TotalRows };
            return Json(report, JsonRequestBehavior.DenyGet);
        }

        [HttpGet]
        [NapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Read)]
        public ActionResult LatestFeedback(int custId = 0, string custName = null)
        {
            ViewBag.CustId = custId;
            ViewBag.Username = custName;
            return View("~/Areas/Feedbacks/LatestFeedback/Index.cshtml", _feedbackService.GetTopItems);
        }

        /// <summary>
        /// Gets the latest feedback.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The action result.</returns>
        [HttpGet]
        [NapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Read)]
        public ActionResult GetLatestFeedback(LatestFeedbackCriteria criteria)
        {
            var feedbacks = _feedbackService.GetLatestFeedbackReport(criteria);
            var fromCustomerText = criteria.CustId != 0 ?
                                    string.Format(Labels.FromCustomer, criteria.Username) :
                                    string.Empty;
            var title = string.Format(Labels.LatestFeedbackReportTitle, criteria.Top, fromCustomerText);

            return Json(
                new
                {
                    Title = title.Trim(),
                    Feedbacks = feedbacks
                },
                JsonRequestBehavior.AllowGet);
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
            var feedbacks = _feedbackService.GetDeletedFeedbackReport(criteria);
            var report = new { Feedbacks = feedbacks, TotalItems = criteria.TotalItems };
            return Json(report, JsonRequestBehavior.DenyGet);
        }

        public void ExportLatestFeedbacks(LatestFeedbackCriteria criteria)
        {
            var fromCustomerText = criteria.CustId != 0 ?
                                    string.Format(Labels.FromCustomer, criteria.Username) :
                                    string.Empty;
            var title = string.Format(Labels.LatestFeedbackReportTitle, criteria.Top, fromCustomerText);
            var feedbacks = _feedbackService.GetLatestFeedbackReport(criteria);

            BaseExcel latestFeedbackExcel = new LatestFeedbackExcel(feedbacks, title.Trim());
            latestFeedbackExcel.BuildWorkBook();
        }

        public void ExportManageFeedbacks(ManageFeedbackCriteria criteria)
        {
            criteria.PageIndex = 0;
            var feedbacks = _feedbackService.GetManageFeedbacks(criteria);

            var fromCustomerText = string.IsNullOrEmpty(criteria.Username) ?
                                    string.Empty :
                                    string.Format(Labels.FromCustomer, criteria.Username);
            var title = Labels.Feedback + " " + fromCustomerText;

            BaseExcel manageFeedbackExcel = new FeedbackExcel(feedbacks, title.Trim());
            manageFeedbackExcel.BuildWorkBook();
        }

        public void ExportDeletedFeedbacks(DeletedFeedbackCriteria criteria)
        {
            criteria.IsExportExcel = true;
            var feedbacks = _feedbackService.GetDeletedFeedbackReport(criteria);

            BaseExcel deletedFeedbackExcel = new FeedbackExcel(feedbacks, Labels.DeletedFeedbackTitle);
            deletedFeedbackExcel.BuildWorkBook();
        }
    }
}