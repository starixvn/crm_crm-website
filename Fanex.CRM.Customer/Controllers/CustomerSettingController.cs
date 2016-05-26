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
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.CRM.Common.Attributes;
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Controllers;
    using Fanex.CRM.Customer.Criteria;
    using Fanex.CRM.Customer.Services;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

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
        [AjaxNapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Host)]
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
            var jsonResult = Json(updateResult, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpGet]
        public JsonResult RelatedCustomerAutoSuggestion(string tag)
        {
            var result = _customerService.RelatedCustomerAutoSuggestion(
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
            var jsonResult = Json(_customerService.VerifyUsername(criteria));
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
    }
}
