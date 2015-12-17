// --------------------------------------------------------------------------
// <copyright file="AddCustomerFeedbackController.cs" company="Nexcel Solutions Vietnam">
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
    using Fanex.CRM.Feedback.Criteria;
    using Fanex.CRM.Feedback.Models;
    using Fanex.CRM.Feedback.Resources;
    using Fanex.CRM.Feedback.Services.AddCustomerFeedback;
    using Fanex.CRM.Infrastructure.Constants;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// Add Customer Feedback Controller.
    /// </summary>
    [NapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Read)]
    public class AddCustomerFeedbackController : AddFeedbackController
    {
        private IAddCustomerFeedbackService _feedbackService;

        public AddCustomerFeedbackController()
            : this(null)
        {
        }

        public AddCustomerFeedbackController(IAddCustomerFeedbackService feedbackService)
        {
            _feedbackService = feedbackService ?? new AddCustomerFeedbackService();
        }

        [NapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Host)]
        public ActionResult Index(int custid, string custName)
        {
            var customerInfo = new CustomerInfo
            {
                Id = custid,
                Name = custName
            };
            return View("~/Areas/Feedbacks/AddCustomerFeedback/Index.cshtml", customerInfo);
        }

        /// <summary>
        /// Gets all setting for edit.
        /// </summary>
        /// <returns>All setting for edit.</returns>
        [HttpPost]
        public ActionResult AllSettingToAddCustomerFeedback()
        {
            var allSetting = AllSettingToAddFeedback();
            allSetting.RoleList = ServiceSetting.GetAllRoles();
            return Json(allSetting, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Adds the feedback.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>
        /// Add result.
        /// </returns>
        [HttpPost]
        public JsonResult AddCustomerFeedback(AddCustomerFeedbackCriteria criteria)
        {
            var insertResult = CheckAddCustomerFeedbackPermission();
            if (insertResult.Code == ErrorCodeConstants.NoneError)
            {
                insertResult = _feedbackService.AddCustomerFeedback(criteria);
            }

            return Json(insertResult, JsonRequestBehavior.AllowGet);
        }

        private Result CheckAddCustomerFeedbackPermission()
        {
            if (!HasPermission(FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Constant.Policy.Host))
            {
                return new Result(ErrorCodeConstants.NoPermission, Labels.Feedback_AddCustomerFeedback_NoCreateFeedbackPermission);
            }

            return new Result();
        }
    }
}