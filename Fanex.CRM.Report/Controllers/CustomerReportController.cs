// --------------------------------------------------------------------------
// <copyright file="CustomerReportController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.CRM.Report.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Excels.CustomerReport;
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Controllers;
    using Fanex.CRM.Report.Criteria.CustomerReport;
    using Fanex.CRM.Report.Repositories.CustomerReport;
    using Fanex.CRM.Setting.Services;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using Resources;

    /// <summary>
    /// Customer Report Controller.
    /// </summary>
    [NapAuth(FunctionKey = FunctionPermissions.CustomerReport, Policy = Constant.Policy.Read)]
    public class CustomerReportController : ReadOnlyBaseController
    {
        private readonly ICustomerReportService _customerReportService;

        private readonly ISettingService _settingService;

        public CustomerReportController()
            : this(null, null)
        {
        }

        public CustomerReportController(
            ICustomerReportService customerReportService,
            ISettingService settingService)
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
            var languages = _settingService.GetAllLanguages().ToList();
            languages.Insert(0, new Common.Models.Setting() { Id = -1, Name = Labels.Symbol_NotApply, IsActive = true });

            var sites = _settingService.GetAllSites().ToList();
            sites.Insert(0, new Setting.Models.SiteInfo() { Id = -1, Name = Labels.Symbol_NotApply, IsActive = true });
            var allSetting = new
            {
                SiteList = sites,
                CountryList = _settingService.GetAllCountries(),
                LanguageList = languages,
                CustomerTypeList = _settingService.GetAllCustomerTypes(),
                VIPTypeList = _settingService.GetAllVIPTypes(),
                PositionList = _settingService.GetPositions()
            };

            return Json(allSetting, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetCustomerReport(GetCustomerReportCriteria criteria)
        {
            var customerReport = _customerReportService.GetCustomerReport(criteria);
            return Json(customerReport, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public void ExportCustomerReport(GetCustomerReportCriteria criteria)
        {
            criteria.PageIndex = 0;
            criteria.PageSize = 0;
            var data = _customerReportService.GetCustomerReport(criteria);
            var excel = new CustomerReportExcel(data, Labels.CustomerReport_Title);
            excel.BuildWorkBook();
        }
    }
}