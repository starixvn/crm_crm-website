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

    [NapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Read)]
    public class CustomerSettingDetailController : ReadOnlyBaseController
    {
        private readonly IPermissionService _permissionService;
        private readonly ICustomerDetailService _customerService;

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
            ViewBag.UserRole = AppSession.CurrentUser.UserRole;

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
            return Json(_customerService.GetAllSettings(AppSession.CurrentUser.UserRole), JsonRequestBehavior.AllowGet);
        }
    }
}