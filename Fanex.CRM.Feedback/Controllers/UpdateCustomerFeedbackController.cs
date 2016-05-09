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
    using Fanex.CRM.Common.Services;
    using Fanex.CRM.Feedback.Criteria;
    using Fanex.CRM.Feedback.Resources;
    using Fanex.CRM.Feedback.Services;
    using Fanex.CRM.Infrastructure.Constants;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using ONEbook.UM.Filters;

    /// <summary>
    /// Update customer feedback controller.
    /// </summary>
    [CheckUMFilter]
    public class UpdateCustomerFeedbackController : BaseController
    {
        private readonly IUpdateCustomerFeedbackService _updateCustomerFeedbackService;
        private readonly bool _hasHostCreateUpdateFeedbackPermission;
        private readonly bool _hasHostDeleteFeedbackPermission;

        public UpdateCustomerFeedbackController()
            : this(null)
        { 
        }

        public UpdateCustomerFeedbackController(IUpdateCustomerFeedbackService updateCustomerFeedbackService)
        {
            _updateCustomerFeedbackService = updateCustomerFeedbackService ?? new UpdateCustomerFeedbackService();
            _hasHostCreateUpdateFeedbackPermission = HasPermission(FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Constant.Policy.Host);
            _hasHostDeleteFeedbackPermission = HasPermission(FunctionPermissions.DeleteFeedback, Constant.Policy.Host);
        }

        private static bool IsRelatedDeletedStatus(int currentStatus, bool isDeletedStatus)
        {
            bool isChangedStatusFromDeleted = isDeletedStatus && currentStatus != (int)CRMStatus.Deleted;
            bool isChangedStatusToDeleted = !isDeletedStatus && currentStatus == (int)CRMStatus.Deleted;

            return isChangedStatusFromDeleted || isChangedStatusToDeleted;
        }

        public ActionResult Index(int id, bool isDeletedFeedbackView = false)
        {
            ViewBag.FeedbackId = id;
            ViewBag.HasReviewFeedbackPermission = HasPermission(FunctionPermissions.ReviewFeedback, Constant.Policy.Host);
            ViewBag.HasUpdateFeedbackPermission = HasUpdateFeedbackPermission(isDeletedFeedbackView);
            ViewBag.IsDeletedFeedbackView = isDeletedFeedbackView.ToString().ToLower();
            ViewBag.IsShowDeletedStatus = (isDeletedFeedbackView || _hasHostDeleteFeedbackPermission).ToString().ToLower();

            return View("~/Areas/Feedbacks/UpdateCustomerFeedback/Index.cshtml");
        }

        private bool HasUpdateFeedbackPermission(bool isDeletedFeedbackView)
        {
            if (isDeletedFeedbackView)
            {
                return _hasHostCreateUpdateFeedbackPermission && _hasHostDeleteFeedbackPermission;
            }

            return _hasHostCreateUpdateFeedbackPermission;
        }

        public ActionResult AllSettings()
        {
            dynamic settings = null;
            var hasDeleteFeedbackPermission = HasReadPermission(FunctionPermissions.DeleteFeedback);
            settings = _updateCustomerFeedbackService.GetAllSettings(hasDeleteFeedbackPermission);

            return Json(settings, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetFeedback(GetFeedbackByIdCriteria criteria)
        {
            return Json(_updateCustomerFeedbackService.GetFeedbackById(criteria), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Updates the feedback.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The result object.</returns>
        [NapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Host)]
        [HttpPost]
        public ActionResult UpdateFeedback(UpdateCustomerFeedbackCriteria criteria, bool isDeletedStatus)
        {
            Result result = CheckUpdateCustomerFeedbackPermission(criteria, isDeletedStatus);
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
        private Result CheckUpdateCustomerFeedbackPermission(UpdateCustomerFeedbackCriteria criteria, bool isDeletedStatus)
        {
            int permissionError = ErrorCodeConstants.NoPermission;
            if (!_hasHostCreateUpdateFeedbackPermission)
            {
                return new Result(permissionError, Labels.Feedback_UpdateCustomerFeedback_NoUpdateFeedbackPermission);
            }

            if (!_hasHostDeleteFeedbackPermission && IsRelatedDeletedStatus(criteria.Status.Id, isDeletedStatus))
            {
                return new Result(permissionError, Labels.Feedback_AddCustomerFeedback_NoDeletedFeedbackPermission);
            }

            if (!HasPermission(FunctionPermissions.ReviewFeedback, Constant.Policy.Host) &&
                criteria.UpdateReviewMode != (int)ReviewMode.NotUpdate)
            {
                return new Result(permissionError, Labels.NoPermissionOnReviewFunction);
            }

            return new Result();
        }
    }
}
