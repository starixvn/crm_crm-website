// --------------------------------------------------------------------------
// <copyright file="AddGuestFeedbackController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.06.03">Created</change>
// </history>
// --------------------------------------------------------------------------

namespace Fanex.CRM.Feedback.Controllers
{
    using System.Web.Mvc;
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Enumerations;
    using Fanex.CRM.Feedback.Criteria;
    using Fanex.CRM.Feedback.Resources;
    using Fanex.CRM.Feedback.Services.AddCustomerFeedback;
    using Fanex.CRM.Infrastructure.Constants;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using ONEbook.UM.Filters;

    /// <summary>
    /// Add Guest Feedback Controller.
    /// </summary>
    [NapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Read)]
    [CheckUMFilter]
    public class AddGuestFeedbackController : AddFeedbackController
    {
        private IAddGuestFeedbackService _feedbackService;

        public AddGuestFeedbackController()
            : this(null)
        {
        }

        public AddGuestFeedbackController(IAddGuestFeedbackService feedbackService)
        {
            _feedbackService = feedbackService ?? new AddGuestFeedbackService();
        }

        [NapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Host)]
        public ActionResult Index()
        {
            return View("~/Areas/Feedbacks/AddGuestFeedback/Index.cshtml");
        }

        /// <summary>
        /// Adds the feedback.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>
        /// Add result.
        /// </returns>
        [HttpPost]
        public JsonResult AddGuestFeedback(AddGuestFeedbackCriteria criteria)
        {
            var result = CheckAddCustomerFeedbackPermission(criteria);
            if (result.Code == ErrorCodeConstants.NoneError)
            {
                result = _feedbackService.AddGuestFeedback(criteria);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Checks the add customer feedback permission.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>
        /// Check result.
        /// </returns>
        private Result CheckAddCustomerFeedbackPermission(AddGuestFeedbackCriteria criteria)
        {
            if (!HasPermission(FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Constant.Policy.Host))
            {
                return new Result(ErrorCodeConstants.NoPermission, Labels.Feedback_AddCustomerFeedback_NoCreateFeedbackPermission);
            }

            if (criteria.Status.Id == (int)CRMStatus.Deleted &&
                !HasPermission(FunctionPermissions.DeleteFeedback.ToString(), Constant.Policy.Host))
            {
                return new Result(ErrorCodeConstants.NoPermission, Labels.Feedback_AddCustomerFeedback_NoDeletedFeedbackPermission);
            }

            if (!HasPermission(FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Constant.Policy.Host))
            {
                return new Result(ErrorCodeConstants.NoPermission, Labels.Feedback_AddCustomerFeedback_NoUpdateCustomerPermission);
            }

            return new Result();
        }
    }
}
