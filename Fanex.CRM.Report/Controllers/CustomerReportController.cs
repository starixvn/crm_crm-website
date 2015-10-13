// --------------------------------------------------------------------------
// <copyright file="CustomerReportController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.CRM.Report.Controllers
{
    using System.Web.Mvc;
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Controllers;
    using Fanex.CRM.Report.Criteria.CustomerReport;
    using Fanex.CRM.Report.Repositories.CustomerReport;
    using Fanex.CRM.Setting.Services;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// Customer Report Controller.
    /// </summary>
    [NapAuth(FunctionKey = FunctionPermissions.CustomerReport, Policy = Constant.Policy.Read)]
    public class CustomerReportController : BaseController
    {
        private readonly ICustomerReportService _customerReportService;
        private readonly ISettingService _settingService;

        public CustomerReportController()
            : this(null, null)
        {
        }

        public CustomerReportController(ICustomerReportService customerReportService, ISettingService settingService)
        {
            _customerReportService = customerReportService ?? new CustomerReportService();
            _settingService = settingService ?? new SettingService();
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
                SiteList = _settingService.GetAllSites(),
                TitleList = _settingService.GetAllTitles(),
                CountryList = _settingService.GetAllCountries(),
                LanguageList = _settingService.GetAllLanguages(),
                CustomerTypeList = _settingService.GetAllCustomerTypes(),
                VIPTypeList = _settingService.GetAllVIPTypes()
            };

            return Json(allSetting, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
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
