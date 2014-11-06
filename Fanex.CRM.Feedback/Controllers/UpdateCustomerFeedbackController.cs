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
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Controllers;
    using Fanex.CRM.Common.Enumerations;
    using Fanex.CRM.Feedback.Criteria;
    using Fanex.CRM.Feedback.Resources;
    using Fanex.CRM.Feedback.Services;
    using Fanex.CRM.Infrastructure.Constants;
    using Fanex.CRM.Infrastructure.Models;
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

        [NapAuth(FunctionKey = FunctionPermissions.EditFeedback, Policy = Constant.Policy.Read)]
        public ActionResult Index(int id)
        {
            ViewBag.FeedbackId = id;
            ViewBag.HasReviewFeedbackPermission = HasPermission(FunctionPermissions.ReviewFeedback, Constant.Policy.Host);
            return View("~/Areas/Feedbacks/UpdateCustomerFeedback/Index.cshtml");
        }

        [NapAuth(FunctionKey = FunctionPermissions.EditFeedback, Policy = Constant.Policy.Read)]
        public ActionResult AllSettings()
        {
            bool hasDeleteFeedbackPermission = HasPermission(FunctionPermissions.DeleteFeedback, Constant.Policy.Host);
            return Json(_updateCustomerFeedbackService.GetAllSettings(hasDeleteFeedbackPermission), JsonRequestBehavior.AllowGet);
        }

        [NapAuth(FunctionKey = FunctionPermissions.EditFeedback, Policy = Constant.Policy.Read)]
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
        public ActionResult UpdateFeedback(UpdateCustomerFeedbackCriteria criteria)
        {
            Result result = CheckUpdateCustomerFeedbackPermission(criteria);
            if (result.Code == ErrorCodeConstants.NoneError)
            {
                _updateCustomerFeedbackService.UpdateFeedback(criteria);
                result = new Result(criteria.ErrorCode, criteria.ErrorMessage);
            }

            return Json(result, JsonRequestBehavior.DenyGet);
        }

        /// <summary>
        /// Checks the update customer feedback permission.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The permission result.</returns>
        private Result CheckUpdateCustomerFeedbackPermission(UpdateCustomerFeedbackCriteria criteria)
        {
            int permissionError = ErrorCodeConstants.NoPermission;
            if (!HasPermission(FunctionPermissions.EditFeedback, Constant.Policy.Host))
            {
                return new Result(permissionError, Labels.Feedback_UpdateCustomerFeedback_NoUpdateFeedbackPermission);
            }

            if (criteria.OverrideCRMInfo && !HasPermission(FunctionPermissions.CustomerInfo, Constant.Policy.Host))
            {
                return new Result(permissionError, Labels.Feedback_AddCustomerFeedback_NoOverrideCustomerInfoPermission);
            }

            if (criteria.UpdateReviewMode != (int)RevewMode.NotUpdate &&
                !HasPermission(FunctionPermissions.ReviewFeedback, Constant.Policy.Host))
            {
                return new Result(permissionError, Labels.NoPermissionOnReviewFunction);
            }

            if (criteria.Status.Id == (int)CRMStatus.Deleted &&
                    !HasPermission(FunctionPermissions.DeleteFeedback, Constant.Policy.Host))
            {
                return new Result(permissionError, Labels.Feedback_AddCustomerFeedback_NoDeletedFeedbackPermission);
            }

            return new Result();
        }
    }
}
