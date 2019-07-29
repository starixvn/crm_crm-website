namespace Fanex.CRM.Feedback.Controllers
{
    using System;
    using System.Net;
    using System.Web.Mvc;
    using Common.Services;
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Feedback.Criteria;
    using Fanex.CRM.Feedback.Models;
    using Fanex.CRM.Feedback.Resources;
    using Fanex.CRM.Feedback.Services.AddCustomerFeedback;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    [NapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Read)]
    public class AddCustomerFeedbackController : AddFeedbackController
    {
        private readonly IAddCustomerFeedbackService _feedbackService;

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
            ViewBag.UserRole = AppSession.CurrentUser.UserRole;

            return View("~/Areas/Feedbacks/AddCustomerFeedback/Index.cshtml", customerInfo);
        }

        [HttpPost]
        public ActionResult AllSettingToAddCustomerFeedback()
        {
            var allSetting = AllSettingToAddFeedback(AppSession.CurrentUser.UserRole);
            allSetting.RoleList = ServiceSetting.GetAllRoles();
            return Json(allSetting, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddCustomerFeedback(AddCustomerFeedbackCriteria criteria)
        {
            try
            {
                var insertResult = CheckAddCustomerFeedbackPermission();
                if (insertResult.IsSuccessful)
                {
                    insertResult = _feedbackService.AddCustomerFeedback(criteria);
                }

                return Json(insertResult, JsonRequestBehavior.AllowGet);
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

        private CommandResult CheckAddCustomerFeedbackPermission()
        {
            if (!HasPermission(FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Constant.Policy.Host))
            {
                return new CommandResult((int)HttpStatusCode.Forbidden, Labels.Feedback_AddCustomerFeedback_NoCreateFeedbackPermission);
            }

            return new CommandResult();
        }
    }
}