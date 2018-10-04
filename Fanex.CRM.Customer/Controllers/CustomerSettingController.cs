// --------------------------------------------------------------------------
// <copyright file="CustomerSettingController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Customer.Controllers
{
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Controllers;
    using Fanex.CRM.Customer.Criteria;
    using Fanex.CRM.Customer.Services;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using System.Web.Mvc;

    /// <summary>
    /// Customer Setting Controller.
    /// </summary>
    [NapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Read)]
    public class CustomerSettingController : BaseController
    {
        private ICustomerService _customerService;

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
            var updateResult = _customerService.UpdateCustomerSuggestion(criteria);
            return Json(updateResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult VerifyUsername(VerifyRelatedCustomerCriteria criteria)
        {
            return Json(_customerService.VerifyUsername(criteria), JsonRequestBehavior.AllowGet);
        }
    }
}