// --------------------------------------------------------------------------
// <copyright file="CustomerSettingDetailController.cs" company="">
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
    using Fanex.CRM.Common.Services;
    using Fanex.CRM.Customer.Criteria.DetailInfo;
    using Fanex.CRM.Customer.Models;
    using Fanex.CRM.Customer.Services;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using System.Web.Mvc;

    /// <summary>
    /// Customer Setting Detail Controller.
    /// </summary>
    [NapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Read)]
    public class CustomerSettingDetailController : ReadOnlyBaseController
    {
        private readonly IPermissionService _permissionService;
        private ICustomerDetailService _customerService;

        public CustomerSettingDetailController()
            : this(null, null)
        {
        }

        public CustomerSettingDetailController(IPermissionService permissionService, ICustomerDetailService customerService)
        {
            _permissionService = permissionService ?? new PermissionService();
            _customerService = customerService ?? new CustomerDetailService();
        }

        public ActionResult Index(string customerId)
        {
            ViewBag.CustomerInfoPolicy = _permissionService.GetPolicy(FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo);
            ViewBag.UpdateHeadlineInfoPolicy = _permissionService.GetPolicy(FunctionPermissions.Updateheadlineinfo);

            CustomerInfo customerDetail = _customerService.GetCustomerInfoDetailById(new GetCustomerInfoDetailByIdCriteria
            {
                CustomerId = customerId
            });

            return View("~/Areas/Customer/DetailInfo/CustomerInfoDetail.cshtml", customerDetail);
        }

        [HttpPost]
        public JsonResult GetCustomerInfoDetailById(GetCustomerInfoDetailByIdCriteria criteria)
        {
            var customerInfo = _customerService.GetCustomerInfoDetailById(criteria);
            return Json(customerInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Host)]
        public JsonResult UpdateCustomerInfoDetail(UpdateCustomerInfoDetailCriteria criteria)
        {
            criteria.IsUpdateHeadlineInfo = _permissionService.HasHostPermission(FunctionPermissions.Updateheadlineinfo);
            var result = _customerService.UpdateCustomerInfoDetail(criteria);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AllSettings()
        {
            return Json(_customerService.GetAllSettings(), JsonRequestBehavior.AllowGet);
        }
    }
}