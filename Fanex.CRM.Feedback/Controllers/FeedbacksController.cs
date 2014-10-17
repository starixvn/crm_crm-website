// --------------------------------------------------------------------------
// <copyright file="FeedbacksController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.06.03">Created</change>
// </history>
// --------------------------------------------------------------------------

namespace Fanex.CRM.Feedback.Controllers
{
    using System.Web.Mvc;
    using Fanex.CRM.Feedback.Criteria;
    using Fanex.CRM.Feedback.Resources;
    using Fanex.CRM.Feedback.Services;
    using Fanex.CRM.Feedback.ViewModels;
    using Fanex.CRM.Infrastructure.Common;
    using Fanex.CRM.Infrastructure.Web.Controllers;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// Feedbacks Controller.
    /// </summary>
    public class FeedbacksController : BaseController
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
        [NapAuth(FunctionKey = FunctionPermission.EditFeedback, Policy = Constant.Policy.Read)]
        public ActionResult GetAllSetting()
        {
            var settings = _feedbackService.GetAllSetting();
            return Json(settings, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [NapAuth(FunctionKey = FunctionPermission.EditFeedback, Policy = Constant.Policy.Read)]
        public ActionResult ManageFeedback(ManageFeedbackViewModel model)
        {
            return View("~/Areas/Feedbacks/ManageFeedback/Index.cshtml", model);
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermission.EditFeedback, Policy = Constant.Policy.Read)]
        public ActionResult SearchManageFeedback(ManageFeedbackCriteria criteria)
        {
            var feedbacks = _feedbackService.GetManageFeedback(criteria);
            var report = new { Feedbacks = feedbacks, TotalItems = criteria.TotalRows };
            return Json(report, JsonRequestBehavior.DenyGet);
        }

        [HttpGet]
        public ActionResult LatestFeedback(int? custId, string custName)
        {
            ViewBag.CustId = custId ?? 0;
            ViewBag.Username = custName;
            return View("~/Areas/Feedbacks/LatestFeedback/Index.cshtml", _feedbackService.GetTop);
        }

        /// <summary>
        /// Gets the latest feedback.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The action result.</returns>
        [HttpGet]
        public ActionResult GetLatestFeedback(LatestFeedbackCriteria criteria)
        {
            var feedbacks = _feedbackService.GetLatestFeedback(criteria);
            return Json(
                new
                {
                    Title = string.Format(
                        Labels.LatestFeedbackReportTitle,
                        criteria.Top,
                        criteria.CustId > 0 ? ("From " + criteria.Username) : string.Empty),
                    Feedbacks = feedbacks
                },
                JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [NapAuth(FunctionKey = FunctionPermission.DeleteFeedback, Policy = Constant.Policy.Host)]
        public ActionResult DeletedFeedback()
        {
            return View("~/Areas/Feedbacks/DeletedFeedback/Index.cshtml");
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermission.DeleteFeedback, Policy = Constant.Policy.Host)]
        public ActionResult SearchDeletedFeedback(DeletedFeedbackCriteria criteria)
        {
            var feedbacks = _feedbackService.GetDeletedFeedbackReport(criteria);
            var report = new { Feedbacks = feedbacks, TotalItems = criteria.TotalItems };
            return Json(report, JsonRequestBehavior.DenyGet);
        }
    }
}
