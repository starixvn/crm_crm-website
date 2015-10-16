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
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Controllers;
    using Fanex.CRM.Common.Services;
    using Fanex.CRM.Dashboard.Criteria;
    using Fanex.CRM.Dashboard.Services;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// Dashboard Controller.
    /// </summary>
    public class DashboardController : BaseController
    {
        private IDashboardService _dashboardServiceInstance;

        public DashboardController()
            : this(null)
        {
        }

        public DashboardController(IDashboardService dashboardServiceInstance)
        {
            _dashboardServiceInstance = dashboardServiceInstance ?? new DashboardService();
        }
                
        public ActionResult Index(int customerId = 0)
        {
            if (customerId == 0)
            {
                return View("~/Views/Home/BlankPage.cshtml");
            }

            return View("~/Areas/Dashboard/ViewInfo/Index.cshtml", customerId);
        }

        [ChildActionOnly]
        public PartialViewResult CustomerRemarksView()
        {
            return PartialView("~/Areas/Dashboard/ViewInfo/CustomerRemarks.cshtml");
        }

        [ChildActionOnly]
        public PartialViewResult CustomerInfoView()
        {
            ViewBag.HasCreateUpdateFeedbackPermission = HasPermission(FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Constant.Policy.Host);
            return PartialView("~/Areas/Dashboard/ViewInfo/CustomerInfo.cshtml");
        }

        [ChildActionOnly]
        public PartialViewResult RedemptionLogsView()
        {
            return PartialView("~/Areas/Dashboard/ViewInfo/RedemptionLogs.cshtml");
        }

        [ChildActionOnly]
        public PartialViewResult CommunicationLogsView()
        {
            return PartialView("~/Areas/Dashboard/ViewInfo/CommunicationLogs.cshtml");
        }

        [NapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Read)]
        public ActionResult OpenEditVerifyFlagView(int customerId)
        {
            var customerInfo = _dashboardServiceInstance.GetCustomerInfo(new GetCustomerInfoCriteria
            {
                CustomerId = customerId
            });

            return View("~/Areas/Dashboard/VerifyFlag/Index.cshtml", customerInfo);
        }

        [HttpPost]
        public ActionResult GetCustomerRemarks(GetCustomerRemarksCriteria criteria)
        {
            var remarks = _dashboardServiceInstance.GetCustomerRemarks(criteria);
            var jsonResult = Json(remarks, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        public ActionResult GetCustomerInfo(GetCustomerInfoCriteria criteria)
        {
            var customerInfo = _dashboardServiceInstance.GetCustomerInfo(criteria);
            var jsonResult = Json(customerInfo, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        public ActionResult GetRedemptionLogs(GetRedemptionLogsCriteria criteria)
        {
            var redemptionLogs = _dashboardServiceInstance.GetRedemptionLogs(criteria);
            var jsonResult = Json(redemptionLogs, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        public ActionResult GetCommunicationLogs(GetCommunicationLogsCriteria criteria)
        {
            var communicationLogs = _dashboardServiceInstance.GetCommunicationLogs(criteria);
            var jsonResult = Json(communicationLogs, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        [CRMAuth]
        [NapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Host)]
        public ActionResult UpdateCustomerVerifyFlag(UpdateCustomerVerifyFlagCriteria criteria)
        {
            var updateResult = _dashboardServiceInstance.UpdateCustomerVerifyFlag(criteria);
            var jsonResult = Json(updateResult, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }
    }
}
