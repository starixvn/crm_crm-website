namespace Fanex.CRM.Feedback.Controllers
{
    using Common.Services;
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
    using System;
    using System.Web.Mvc;

    public class UpdateCustomerFeedbackController : ReadOnlyBaseController
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

        private static bool IsRelatedDeletedStatus(int currentStatus, bool isDeletedStatus)
        {
            bool isChangedStatusFromDeleted = isDeletedStatus && currentStatus != (int)CRMStatus.Deleted;
            bool isChangedStatusToDeleted = !isDeletedStatus && currentStatus == (int)CRMStatus.Deleted;

            return isChangedStatusFromDeleted || isChangedStatusToDeleted;
        }

        private bool HasHostCreateUpdateFeedbackPermission()
        {
            return HasPermission(FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Constant.Policy.Host);
        }

        private bool HasHostDeleteFeedbackPermission()
        {
            return HasPermission(FunctionPermissions.DeleteFeedback, Constant.Policy.Host);
        }

        public ActionResult Index(int id, bool isDeletedFeedbackView = false)
        {
            ViewBag.FeedbackId = id;
            ViewBag.HasReviewFeedbackPermission = HasPermission(FunctionPermissions.ReviewFeedback, Constant.Policy.Host);
            ViewBag.HasUpdateFeedbackPermission = HasUpdateFeedbackPermission(isDeletedFeedbackView);
            ViewBag.IsDeletedFeedbackView = isDeletedFeedbackView.ToString().ToLower();
            ViewBag.IsShowDeletedStatus = (isDeletedFeedbackView || HasHostDeleteFeedbackPermission()).ToString().ToLower();
            ViewBag.UserRole = AppSession.CurrentUser.UserRole;

            return View("~/Areas/Feedbacks/UpdateCustomerFeedback/Index.cshtml");
        }

        private bool HasUpdateFeedbackPermission(bool isDeletedFeedbackView)
        {
            if (isDeletedFeedbackView)
            {
                return HasHostCreateUpdateFeedbackPermission() && HasHostDeleteFeedbackPermission();
            }

            return HasHostCreateUpdateFeedbackPermission();
        }

        public ActionResult AllSettings()
        {
            dynamic settings = null;
            var hasDeleteFeedbackPermission = HasReadPermission(FunctionPermissions.DeleteFeedback);
            settings = _updateCustomerFeedbackService.GetAllSettings(hasDeleteFeedbackPermission, AppSession.CurrentUser.UserRole);

            return Json(settings, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetFeedback(GetFeedbackByIdCriteria criteria)
        {
            return Json(_updateCustomerFeedbackService.GetFeedbackById(criteria), JsonRequestBehavior.AllowGet);
        }

        [NapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Host)]
        [HttpPost]
        public ActionResult UpdateFeedback(UpdateCustomerFeedbackCriteria criteria, bool isDeletedStatus)
        {
            try
            {
                Result result = CheckUpdateCustomerFeedbackPermission(criteria, isDeletedStatus);
                if (result.Code == ErrorCodeConstants.NoneError)
                {
                    _updateCustomerFeedbackService.UpdateFeedback(criteria);
                    result = new Result(criteria.ErrorCode, criteria.ErrorMessage);
                }

                return Json(result, JsonRequestBehavior.DenyGet);
            }
            catch (Exception exception)
            {
                if (AppSetting.EnableManualLogging)
                {
                    LoggingHelper.Error(exception, criteria);
                }

                throw;
            }
        }

        private Result CheckUpdateCustomerFeedbackPermission(UpdateCustomerFeedbackCriteria criteria, bool isDeletedStatus)
        {
            int permissionError = ErrorCodeConstants.NoPermission;
            if (!HasHostCreateUpdateFeedbackPermission())
            {
                return new Result(permissionError, Labels.Feedback_UpdateCustomerFeedback_NoUpdateFeedbackPermission);
            }

            if (!HasHostDeleteFeedbackPermission() && IsRelatedDeletedStatus(criteria.Status.Id, isDeletedStatus))
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