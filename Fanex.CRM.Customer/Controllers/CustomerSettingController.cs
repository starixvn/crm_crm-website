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
    using Fanex.CRM.Customer.Criteria;
    using Fanex.CRM.Customer.Services.Implements;
    using Fanex.CRM.Customer.Services.Interfaces;
    using Fanex.CRM.Infrastructure.Web.Controllers;

    /// <summary>
    /// Customer Setting Controller.
    /// </summary>
    public class CustomerSettingController : BaseController
    {
        private ICustomerService _serviceInstance;

        public CustomerSettingController()
            : this(null)
        {
        }

        public CustomerSettingController(ICustomerService serviceInstance)
        {
            _serviceInstance = serviceInstance ?? new CustomerService();
        }

        public ActionResult GoToCustomerInfoDetail(string customerId)
        {
            return View("~/Areas/Customer/CustomerInfoDetail.cshtml");
        }

        [HttpPost]
        public JsonResult GetCustomerInfoByUsername(GetCustomerInfoByUsernameCriteria criteria)
        {
            var result = _serviceInstance.GetCustomerInfoByUsername(criteria);
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetUplineInfoOfUser(GetUplineInfoOfUserCriteria criteria)
        {
            var result = _serviceInstance.GetUplineInfoOfUser(criteria);
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        public JsonResult UpdateCustomerSuggestion(UpdateCustomerSuggestionCriteria criteria)
        {
            var result = _serviceInstance.UpdateCustomerSuggestion(criteria);
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        public JsonResult UpdateCustomerInfo(UpdateCustomerInfoCriteria criteria)
        {
            var result = _serviceInstance.UpdateCustomerInfo(criteria);
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }
    }
}
