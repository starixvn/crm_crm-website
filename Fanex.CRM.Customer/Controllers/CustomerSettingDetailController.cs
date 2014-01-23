// --------------------------------------------------------------------------
// <copyright file="CustomerSettingDetailController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Customer.Controllers
{
    using System.Web.Mvc;
    using Fanex.CRM.Customer.Criteria.DetailInfo;
    using Fanex.CRM.Customer.Models;
    using Fanex.CRM.Customer.Services.Implements;
    using Fanex.CRM.Customer.Services.Interfaces;
    using Fanex.CRM.Infrastructure.Common;
    using Fanex.CRM.Infrastructure.Web.Controllers;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;


    /// <summary>
    /// Customer Setting Detail Controller.
    /// </summary>
    public class CustomerSettingDetailController : BaseController
    {
        private ICustomerDetailService _serviceInstance;

        public CustomerSettingDetailController()
            : this(null)
        {
        }

        public CustomerSettingDetailController(ICustomerDetailService serviceInstance)
        {
            _serviceInstance = serviceInstance ?? new CustomerDetailService();
        }

        [NapAuth(FunctionKey = FunctionPermission.CustomerInfo, Policy = Constant.Policy.Read)]
        public ActionResult Index(string customerId)
        {
            CustomerInfo customerDetail = _serviceInstance.GetCustomerInfoDetailById(new GetCustomerInfoDetailByIdCriteria
            {
                CustomerId = customerId
            });

            return View("~/Areas/Customer/DetailInfo/CustomerInfoDetail.cshtml", customerDetail);
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermission.CustomerInfo, Policy = Constant.Policy.Read)]
        public JsonResult GetCustomerInfoDetailById(GetCustomerInfoDetailByIdCriteria criteria)
        {
            var result = _serviceInstance.GetCustomerInfoDetailById(criteria);
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermission.CustomerInfo, Policy = Constant.Policy.Host)]
        public JsonResult UpdateCustomerInfoDetail(UpdateCustomerInfoDetailCriteria criteria)
        {
            var result = _serviceInstance.UpdateCustomerInfoDetail(criteria);
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }
    }
}
