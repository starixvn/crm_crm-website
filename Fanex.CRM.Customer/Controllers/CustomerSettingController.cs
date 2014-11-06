// --------------------------------------------------------------------------
// <copyright file="CustomerSettingController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Customer.Controllers
{
    using System.Web.Mvc;
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Controllers;
    using Fanex.CRM.Customer.Criteria;
    using Fanex.CRM.Customer.Services;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// Customer Setting Controller.
    /// </summary>
    public class CustomerSettingController : BaseController
    {
        private ICustomerService _customerServiceInstance;

        public CustomerSettingController()
            : this(null)
        {
        }

        public CustomerSettingController(ICustomerService customerServiceInstance)
        {
            _customerServiceInstance = customerServiceInstance ?? new CustomerService();
        }

        [NapAuth(FunctionKey = FunctionPermission.CustomerInfo, Policy = Constant.Policy.Read)]
        public ActionResult ShortCustomerInfo()
        {
            return View("~/Areas/Customer/ShortInfo/CustomerInfo.cshtml");
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermission.CustomerInfo, Policy = Constant.Policy.Read)]
        public JsonResult GetCustomerInfoByUsername(GetCustomerInfoByUsernameCriteria criteria)
        {
            var customerInfo = _customerServiceInstance.GetCustomerInfoByUsername(criteria);
            var jsonResult = Json(customerInfo, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermission.CustomerInfo, Policy = Constant.Policy.Read)]
        public JsonResult GetUplineInfoOfUser(GetUplineInfoOfUserCriteria criteria)
        {
            var uplineInfo = _customerServiceInstance.GetUplineInfoOfUser(criteria);
            var jsonResult = Json(uplineInfo, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermission.CustomerInfo, Policy = Constant.Policy.Host)]
        public JsonResult UpdateCustomerInfo(UpdateCustomerInfoCriteria criteria)
        {
            var updateResult = _customerServiceInstance.UpdateCustomerInfo(criteria);
            var jsonResult = Json(updateResult, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpGet]
        public JsonResult CustomerAutoSuggestion(CustomerSuggestionCriteria criteria)
        {
            var jsonResult = new
            {
                query = criteria.Query,
                suggestions = _customerServiceInstance.QueryCustomerByUsername(criteria)
            };

            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateCustomerSuggestion(UpdateCustomerSuggestionCriteria criteria)
        {
            var updateResult = _customerServiceInstance.UpdateCustomerSuggestion(criteria);
            var jsonResult = Json(updateResult, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        /// <summary>
        /// The related customer automatic suggestion.
        /// </summary>
        /// <param name="tag">The tag value.</param>
        /// <returns>Suggestion result.</returns>
        [HttpGet]
        public JsonResult RelatedCustomerAutoSuggestion(string tag)
        {
            var result = _customerServiceInstance.RelatedCustomerAutoSuggestion(
                new RelatedCustomerAutoSuggestionCriteria
                {
                    CustomerNamePharse = tag
                });
            JsonResult jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        /// <summary>
        /// Verifies the name of the user.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>
        /// Verify result.
        /// </returns>
        [HttpPost]
        public JsonResult VerifyUsername(VerifyRelatedCustomerCriteria criteria)
        {
            var jsonResult = Json(_customerServiceInstance.VerifyUsername(criteria));
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
    }
}
