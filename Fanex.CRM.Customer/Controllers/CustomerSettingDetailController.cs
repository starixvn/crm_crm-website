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
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Controllers;
    using Fanex.CRM.Common.Services;
    using Fanex.CRM.Customer.Criteria.DetailInfo;
    using Fanex.CRM.Customer.Models;
    using Fanex.CRM.Customer.Services;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    
    /// <summary>
    /// Customer Setting Detail Controller.
    /// </summary>
    public class CustomerSettingDetailController : BaseController
    {
        private ICustomerDetailService _customerServiceInstance;

        public CustomerSettingDetailController()
            : this(null)
        {
        }

        public CustomerSettingDetailController(ICustomerDetailService customerServiceInstance)
        {
            _customerServiceInstance = customerServiceInstance ?? new CustomerDetailService();
        }

        [NapAuth(FunctionKey = FunctionPermissions.CustomerInfo, Policy = Constant.Policy.Read)]
        public ActionResult Index(string customerId)
        {
            CustomerInfo customerDetail = _customerServiceInstance.GetCustomerInfoDetailById(new GetCustomerInfoDetailByIdCriteria
            {
                CustomerId = customerId
            });

            return View("~/Areas/Customer/DetailInfo/CustomerInfoDetail.cshtml", customerDetail);
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermissions.CustomerInfo, Policy = Constant.Policy.Read)]
        public JsonResult GetCustomerInfoDetailById(GetCustomerInfoDetailByIdCriteria criteria)
        {
            var customerInfo = _customerServiceInstance.GetCustomerInfoDetailById(criteria);
            var jsonResult = Json(customerInfo, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        [AjaxNapAuth(FunctionKey = FunctionPermissions.CustomerInfo, Policy = Constant.Policy.Host)]
        public JsonResult UpdateCustomerInfoDetail(UpdateCustomerInfoDetailCriteria criteria)
        {
            var customerInfoDetail = _customerServiceInstance.UpdateCustomerInfoDetail(criteria);
            var jsonResult = Json(customerInfoDetail, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        public JsonResult AllSettings() 
        {
            return Json(_customerServiceInstance.AllSettings(), JsonRequestBehavior.AllowGet);
        }
    }
}
