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
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Controllers;
    using Fanex.CRM.Report.Criteria.CustomerReport;
    using Fanex.CRM.Report.Repositories.CustomerReport;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// Customer Report Controller.
    /// </summary>
    [NapAuth(FunctionKey = FunctionPermissions.CustomerReport, Policy = Constant.Policy.Read)]
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

        public ActionResult Index()
        {
            return View("~/Areas/Report/CustomerReport/Index.cshtml");
        }

        /// <summary>
        /// Gets all setting.
        /// </summary>
        /// <returns>All setting.</returns>
        [HttpPost]
        public ActionResult AllSetting()
        {
            var allSetting = new
            {
                SiteList = _customerReportService.AllSite(),
                TitleList = _customerReportService.AllTitle(),
                CountryList = _customerReportService.AllCountry(),
                LanguageList = _customerReportService.AllLanguage(),
                CustomerTypeList = _customerReportService.AllCustomerType(),
                VIPTypeList = _customerReportService.AllVIPType()
            };

            return Json(allSetting, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetCustomerReport(GetCustomerReportCriteria criteria)
        {
            var customerReport = _customerReportService.CustomerReport(criteria);
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
