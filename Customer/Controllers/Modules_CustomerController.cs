// --------------------------------------------------------------------------
// <copyright file="CustomerController.cs" company="Nexcel Solutions Vietnam">
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
    using Fanex.CRM.Web.Controllers;

    /// <summary>
    /// Customer Controller.
    /// </summary>
    public class Modules_CustomerController : BaseController
    {
        private ICustomerService _serviceInstance;

        public Modules_CustomerController()
            : this(null)
        {
        }

        public ActionResult GoToCustomerInfoDetail(string customerId)
        {
            return View("~/Areas/Customer/CustomerInfoDetail.cshtml");
        }

        public Modules_CustomerController(ICustomerService serviceInstance)
        {
            _serviceInstance = serviceInstance ?? new CustomerService();
        }

        [HttpPost]
        public JsonResult GetCustomerInfoByUsername(GetCustomerInfoByUsernameCriteria criteria)
        {
            var result = _serviceInstance.GetCustomerInfoByUsername(criteria);
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }
    }
}
