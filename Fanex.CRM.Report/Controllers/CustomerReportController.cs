// --------------------------------------------------------------------------
// <copyright file="CustomerReportController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Controllers
{
    using System.Web.Mvc;
    using Fanex.CRM.Infrastructure.Common;
    using Fanex.CRM.Infrastructure.Web.Controllers;
    using Fanex.CRM.Report.Criteria.CustomerReport;
    using Fanex.CRM.Report.Repositories.Implements.CustomerReport;
    using Fanex.CRM.Report.Repositories.Interfaces;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// Customer Report Controller.
    /// </summary>
    public class CustomerReportController : BaseController
    {
        private ICustomerReportService _customerReportService;

        public CustomerReportController()
            : this(null)
        {
        }

        public CustomerReportController(ICustomerReportService customerReportService)
        {
            _customerReportService = customerReportService ?? new CustomerReportService();
        }

        [NapAuth(FunctionKey = FunctionPermission.CRMReport, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            return View("~/Areas/Report/CustomerReport/Index.cshtml");
        }

        /// <summary>
        /// Gets all setting.
        /// </summary>
        /// <returns>All setting.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermission.CRMReport, Policy = Constant.Policy.Read)]
        public ActionResult GetAllSetting()
        {
            var allSetting = new
            {
                SiteList = _customerReportService.GetAllSite(),
                TitleList = _customerReportService.GetAllTitle(),
                CountryList = _customerReportService.GetAllCountry(),
                LanguageList = _customerReportService.GetAllLanguage(),
                CustomerTypeList = _customerReportService.GetAllCustomerType(),
                VIPTypeList = _customerReportService.GetAllVIPType()
            };

            return Json(allSetting, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermission.CRMReport, Policy = Constant.Policy.Read)]
        public ActionResult GetCustomerReport(GetCustomerReportCriteria criteria)
        {
            var customerReport = _customerReportService.GetCustomerReport(criteria);
            var jsonResult = Json(customerReport, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        public ActionResult ExportCustomerReport(GetCustomerReportCriteria criteria)
        {
            _customerReportService.ExportCustomerReport(criteria);
            return new EmptyResult();
        }
    }
}
