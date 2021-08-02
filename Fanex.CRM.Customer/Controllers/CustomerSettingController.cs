namespace Fanex.CRM.Customer.Controllers
{
    using System.Web.Mvc;
    using Common.Services;
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Controllers;
    using Fanex.CRM.Customer.Criteria;
    using Fanex.CRM.Customer.Services;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    [NapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Read)]
    public class CustomerSettingController : BaseController
    {
        private readonly ICustomerService _customerService;

        public CustomerSettingController()
            : this(null)
        {
        }

        public CustomerSettingController(ICustomerService customerServiceInstance)
        {
            _customerService = customerServiceInstance ?? new CustomerService();
        }

        public ActionResult ShortCustomerInfo()
        {
            ViewBag.CustomerInfoPolicy = GetPolicy(FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo);
            ViewBag.UserName = Request.Params["username"];
            ViewBag.UserRole = AppSession.CurrentUser.UserRole;

            return View("~/Areas/Customer/ShortInfo/CustomerInfo.cshtml");
        }

        [HttpPost]
        public JsonResult GetCustomerInfoByUsername(GetCustomerInfoByUsernameCriteria criteria)
        {
            var customerInfo = _customerService.GetCustomerInfoByUsername(criteria);
            return Json(customerInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetUplineInfoOfUser(GetUplineInfoOfUserCriteria criteria)
        {
            var uplineInfo = _customerService.GetUplineInfoOfUser(criteria);
            return Json(uplineInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Host)]
        public JsonResult UpdateCustomerInfo(UpdateCustomerInfoCriteria criteria)
        {
            var updateResult = _customerService.UpdateCustomerInfo(criteria);
            return Json(updateResult, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult CustomerAutoSuggestion(CustomerSuggestionCriteria criteria)
        {
            var jsonResult = new
            {
                query = criteria.Query,
                suggestions = _customerService.QueryCustomerByUsername(criteria)
            };

            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateCustomerSuggestion(UpdateCustomerSuggestionCriteria criteria)
        {
            if (criteria.IsValid())
            {
                var updateResult = _customerService.UpdateCustomerSuggestion(criteria);
                return Json(updateResult, JsonRequestBehavior.AllowGet);
            }

            return Json(false, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public JsonResult VerifyUsername(VerifyRelatedCustomerCriteria criteria)
        {
            return Json(_customerService.VerifyUsername(criteria), JsonRequestBehavior.AllowGet);
        }
    }
}