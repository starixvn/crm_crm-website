namespace Fanex.CRM.Report.Controllers
{
    using System.Web.Mvc;
    using Common.Enumerations;
    using Common.Services;
    using Excels.CustomerReport;
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Controllers;
    using Fanex.CRM.Report.Criteria.CustomerReport;
    using Fanex.CRM.Report.Repositories.CustomerReport;
    using Fanex.CRM.Setting.Services;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using Resources;

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
            ViewBag.UserRole = AppSession.CurrentUser.UserRole;
            return View("~/Areas/Report/CustomerReport/Index.cshtml");
        }

        [HttpPost]
        public ActionResult AllSetting()
        {
            var languages = _settingService.GetAllLanguages();
            languages.Insert(0, new Common.Models.Setting { Id = -1, Name = Labels.Symbol_NotApply, IsActive = true });

            var sites = (AppSession.CurrentUser.UserRole == UserRole.CN88Staff)
                ? _settingService.GetAllSitesCN88()
                : _settingService.GetAllSitesWithNA();

            var positions = (AppSession.CurrentUser.UserRole == UserRole.CN88Staff)
                ? _settingService.GetPositionsCN88()
                : _settingService.GetPositions(false);

            var allSetting = new
            {
                SiteList = sites,
                CountryList = _settingService.GetAllCountries(),
                LanguageList = languages,
                CustomerTypeList = _settingService.GetAllCustomerTypes(),
                VIPTypeList = _settingService.GetAllVIPTypes(),
                PositionList = positions
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