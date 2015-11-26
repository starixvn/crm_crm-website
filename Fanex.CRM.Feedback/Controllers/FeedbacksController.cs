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
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Controllers;
    using Fanex.CRM.Feedback.Criteria;
    using Fanex.CRM.Feedback.Resources;
    using Fanex.CRM.Feedback.Services;
    using Fanex.CRM.Feedback.ViewModels;
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
        public ActionResult LatestFeedback()
        {
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
            return Json(
                new
                {
                    Title = string.Format(
                        Labels.LatestFeedbackReportTitle,
                        criteria.Top,
                        criteria.CustId != 0 ? ("From " + criteria.Username) : string.Empty),
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
    }
}
