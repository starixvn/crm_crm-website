namespace Fanex.CRM.Feedback.Controllers
{
    using System;
    using System.Web.Mvc;
    using Common.Services;
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Enumerations;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Feedback.Criteria;
    using Fanex.CRM.Feedback.Resources;
    using Fanex.CRM.Feedback.Services.AddCustomerFeedback;
    using Fanex.CRM.Infrastructure.Constants;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    [NapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Read)]
    public class AddGuestFeedbackController : AddFeedbackController
    {
        private readonly IAddGuestFeedbackService _feedbackService;

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
            ViewBag.UserRole = AppSession.CurrentUser.UserRole;
            return View("~/Areas/Feedbacks/AddGuestFeedback/Index.cshtml");
        }

        [HttpPost]
        public JsonResult AddGuestFeedback(AddGuestFeedbackCriteria criteria)
        {
            try
            {
                var result = CheckAddCustomerFeedbackPermission(criteria);
                if (result.Code == ErrorCodeConstants.NoneError)
                {
                    result = _feedbackService.AddGuestFeedback(criteria);
                }

                return Json(result, JsonRequestBehavior.AllowGet);
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

        private CommandResult CheckAddCustomerFeedbackPermission(AddGuestFeedbackCriteria criteria)
        {
            if (!HasPermission(FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Constant.Policy.Host))
            {
                return new CommandResult(ErrorCodeConstants.NoPermission, Labels.Feedback_AddCustomerFeedback_NoCreateFeedbackPermission);
            }

            if (criteria.Status.Id == (int)CRMStatus.Deleted &&
                !HasPermission(FunctionPermissions.DeleteFeedback, Constant.Policy.Host))
            {
                return new CommandResult(ErrorCodeConstants.NoPermission, Labels.Feedback_AddCustomerFeedback_NoDeletedFeedbackPermission);
            }

            if (!HasPermission(FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Constant.Policy.Host))
            {
                return new CommandResult(ErrorCodeConstants.NoPermission, Labels.Feedback_AddCustomerFeedback_NoUpdateCustomerPermission);
            }

            return new CommandResult();
        }
    }
}