// --------------------------------------------------------------------------
// <copyright file="UpdateCustomerFeedbackController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.10.20">Created</change>
// </history>

namespace Fanex.CRM.Feedback.Controllers
{
    using System.Web.Mvc;
    using Fanex.CRM.Feedback.Criteria;
    using Fanex.CRM.Feedback.Resources;
    using Fanex.CRM.Feedback.Services;
    using Fanex.CRM.Infrastructure.Common;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Infrastructure.Web.Controllers;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// Update customer feedback controller.
    /// </summary>
    public class UpdateCustomerFeedbackController : BaseController
    {
        private readonly IUpdateCustomerFeedbackService _updateCustomerFeedbackService;

        public UpdateCustomerFeedbackController()
            : this(null)
        { 
        }

        public UpdateCustomerFeedbackController(IUpdateCustomerFeedbackService updateCustomerFeedbackService)
        {
            _updateCustomerFeedbackService = updateCustomerFeedbackService ?? new UpdateCustomerFeedbackService();
        }

        [NapAuth(FunctionKey = FunctionPermission.EditFeedback, Policy = Constant.Policy.Read)]
        public ActionResult Index(int id)
        {
            ViewBag.FeedbackId = id;
            ViewBag.HasReviewFeedbackPermission = HasPermission(FunctionPermission.ReviewFeedback, Constant.Policy.Host);
            return View("~/Areas/Feedbacks/UpdateCustomerFeedback/Index.cshtml");
        }

        [NapAuth(FunctionKey = FunctionPermission.EditFeedback, Policy = Constant.Policy.Read)]
        public ActionResult GetSettings()
        {
            bool hasDeleteFeedbackPermission = HasPermission(FunctionPermission.DeleteFeedback, Constant.Policy.Host);
            return Json(_updateCustomerFeedbackService.GetAllSettings(hasDeleteFeedbackPermission), JsonRequestBehavior.AllowGet);
        }

        [NapAuth(FunctionKey = FunctionPermission.EditFeedback, Policy = Constant.Policy.Read)]
        public ActionResult GetFeedback(GetFeedbackByIdCriteria criteria)
        {
            return Json(_updateCustomerFeedbackService.GetFeedbackById(criteria), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Updates the feedback.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The result object.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermission.EditFeedback, Policy = Constant.Policy.Host)]
        public ActionResult UpdateFeedback(UpdateCustomerFeedbackCriteria criteria)
        {
            JsonRequestBehavior jsonBehavior = JsonRequestBehavior.DenyGet;
            if (!HasPermission(FunctionPermission.ReviewFeedback, Constant.Policy.Host) &&
                    criteria.UpdateReviewMode != (int)Enumerations.Functions.RevewMode.NotUpdate)
            {
                return Json(new Result((int)Enumerations.ErrorCode.UpdateCustomerInfo, Labels.NoPermissionOnReviewFunction), jsonBehavior);
            }

            if (criteria.Status.Id == (int)Enumerations.Functions.CRMStatus.Deleted &&
                    !HasPermission(FunctionPermission.DeleteFeedback, Constant.Policy.Host))
            {
                return Json(
                    new Result((int)Enumerations.ErrorCode.DeleteFeedback, Labels.Feedback_AddCustomerFeedback_NoDeletedFeedbackPermission),
                    jsonBehavior);
            }

            _updateCustomerFeedbackService.UpdateFeedback(criteria);
            return Json(new Result(criteria.ErrorCode, criteria.ErrorMessage), jsonBehavior);
        }
    }
}
