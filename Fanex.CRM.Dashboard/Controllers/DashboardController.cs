// --------------------------------------------------------------------------
// <copyright file="DashboardController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Dashboard.Controllers
{
    using System.Web.Mvc;
    using Fanex.CRM.Dashboard.Criteria;
    using Fanex.CRM.Dashboard.Services.Implements;
    using Fanex.CRM.Dashboard.Services.Interfaces;
    using Fanex.CRM.Infrastructure.Common;
    using Fanex.CRM.Infrastructure.Web.Controllers;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// Dashboard Controller.
    /// </summary>
    public class DashboardController : BaseController
    {
        private IDashboardService _serviceInstance;

        public DashboardController()
            : this(null)
        {
        }

        public DashboardController(IDashboardService serviceInstance)
        {
            _serviceInstance = serviceInstance ?? new DashboardService();
        }

        public ActionResult Index(int customerId)
        {
            return View("~/Areas/Dashboard/ViewInfo/Index.cshtml", customerId);
        }

        [ChildActionOnly]
        public PartialViewResult GetCustomerRemarksView()
        {
            return PartialView("~/Areas/Dashboard/ViewInfo/CustomerRemarks.cshtml");
        }

        [ChildActionOnly]
        public PartialViewResult GetCustomerInfoView()
        {
            return PartialView("~/Areas/Dashboard/ViewInfo/CustomerInfo.cshtml");
        }

        [ChildActionOnly]
        public PartialViewResult GetRedemptionLogsView()
        {
            return PartialView("~/Areas/Dashboard/ViewInfo/RedemptionLogs.cshtml");
        }

        [ChildActionOnly]
        public PartialViewResult GetCommunicationLogsView()
        {
            return PartialView("~/Areas/Dashboard/ViewInfo/CommunicationLogs.cshtml");
        }

        [NapAuth(FunctionKey = FunctionPermission.CustomerInfo, Policy = Constant.Policy.Host)]
        public ActionResult OpenEditVerifyFlagView(int customerId)
        {
            var customerInfo = _serviceInstance.GetCustomerInfo(new GetCustomerInfoCriteria
            {
                CustomerId = customerId
            });

            return View("~/Areas/Dashboard/VerifyFlag/Index.cshtml", customerInfo);
        }

        [HttpPost]
        public ActionResult GetCustomerRemarks(GetCustomerRemarksCriteria criteria)
        {
            var result = _serviceInstance.GetCustomerRemarks(criteria);
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        public ActionResult GetCustomerInfo(GetCustomerInfoCriteria criteria)
        {
            var result = _serviceInstance.GetCustomerInfo(criteria);
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        public ActionResult GetRedemptionLogs(GetRedemptionLogsCriteria criteria)
        {
            var result = _serviceInstance.GetRedemptionLogs(criteria);
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        public ActionResult GetCommunicationLogs(GetCommunicationLogsCriteria criteria)
        {
            var result = _serviceInstance.GetCommunicationLogs(criteria);
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        public ActionResult UpdateCustomerVerifyFlag(UpdateCustomerVerifyFlagCriteria criteria)
        {
            var result = _serviceInstance.UpdateCustomerVerifyFlag(criteria);
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }
    }
}
