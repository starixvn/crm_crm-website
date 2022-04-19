namespace Fanex.CRM.Dashboard.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Common.Services;
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Controllers;
    using Fanex.CRM.Common.Extensions;
    using Fanex.CRM.Dashboard.Criteria;
    using Fanex.CRM.Dashboard.Services;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using Setting.Services;

    public class DashboardController : ReadOnlyBaseController
    {
        private readonly IDashboardService _dashboardServiceInstance;
        private readonly ISettingService _serviceSetting;

        public DashboardController() : this(null, null)
        {
        }

        public DashboardController(
            IDashboardService dashboardServiceInstance,
            ISettingService serviceSetting)
        {
            _dashboardServiceInstance = dashboardServiceInstance ?? new DashboardService();
            _serviceSetting = serviceSetting ?? new SettingService();
        }

        public ActionResult Index(int customerId = 0)
        {
            if (customerId == 0)
            {
                return View("~/Views/Home/BlankPage.cshtml");
            }
            ViewBag.UserRole = AppSession.CurrentUser.UserRole;
            ViewBag.CreateUpdateFeedbackPermission = HasPermission(FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Constant.Policy.Host);

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
            ViewBag.HasCreateUpdateFeedbackPermission = HasPermission(FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Constant.Policy.Host);
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
            return Json(remarks, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetNewRemarks(GetCustomerNewRemarksCriteria criteria)
        {
            var remarks = _dashboardServiceInstance.GetCustomerNewRemarks(criteria);
            foreach(var remark in remarks)
            {
                remark.CreatedDateString = DateTime.MinValue != remark.CreatedDate ? remark.CreatedDate.ToLongFormatedDateTime() : string.Empty;
            }
            return Json(remarks, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetCustomerInfo(GetCustomerInfoCriteria criteria)
        {
            var customerInfo = _dashboardServiceInstance.GetCustomerInfo(criteria);
            var allsites = _serviceSetting.GetAllSites();
            var siteCredits = allsites.Where(x => x.Name == "Credit");
            var creditSite = siteCredits.Where(x => x.ChildSites.Any(y => customerInfo.SiteID == y.Id)).ToList();
            return Json(new { customerInfo , isCreditSite = creditSite.Count > 0}, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetRedemptionLogs(GetRedemptionLogsCriteria criteria)
        {
            var redemptionLogs = _dashboardServiceInstance.GetRedemptionLogs(criteria);
            return Json(redemptionLogs, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetCommunicationLogs(GetCommunicationLogsCriteria criteria)
        {
            var communicationLogs = _dashboardServiceInstance.GetCommunicationLogs(criteria);
            var result = new
            {
                CommunicationLogs = communicationLogs,
                MainCategories = _serviceSetting.GetAllMainCategoriesFromAllSettings()
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Host)]
        public ActionResult UpdateCustomerVerifyFlag(UpdateCustomerVerifyFlagCriteria criteria)
        {
            var updateResult = _dashboardServiceInstance.UpdateCustomerVerifyFlag(criteria);
            return Json(updateResult, JsonRequestBehavior.AllowGet);
        }
    }
}