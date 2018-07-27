namespace Fanex.CRM.Report.Controllers
{
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Controllers;
    using Fanex.CRM.Common.Enumerations;
    using Fanex.CRM.Report.Criteria.StatisticByDateReport;
    using Fanex.CRM.Report.Services.StatisticByDateReport;
    using Fanex.CRM.Report.Services.WeeklyStatisticsReport;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using Setting.Services;
    using System.Web.Mvc;

    [NapAuth(FunctionKey = FunctionPermissions.WeeklyStatistics, Policy = Constant.Policy.Read)]
    public class WeeklyStatisticsReportController : ReadOnlyBaseController
    {
        private readonly IStatisticByDateReportService _weeklyStatisticsReportService;
        public readonly ISettingService _serviceSetting;

        public WeeklyStatisticsReportController()
            : this(null)
        {
        }

        public WeeklyStatisticsReportController(IStatisticByDateReportService weeklyStatisticsReportService)
        {
            _weeklyStatisticsReportService = weeklyStatisticsReportService ?? new WeeklyStatisticsReportService();
            _serviceSetting = new SettingService();
        }

        public ActionResult Index()
        {
            return View("~/Areas/Report/WeeklyStatisticsReport/Index.cshtml");
        }

        [HttpPost]
        public ActionResult MainReport(GetMainReportCriteria criteria)
        {
            var jsonResult = Json(new
            {
                IBCReport = _weeklyStatisticsReportService.GetMainReport(new GetMainReportCriteria
                {
                    FromDate = criteria.FromDate,
                    ToDate = criteria.ToDate,
                    SiteType = (int)ReportType.CreditSite
                }),
                LicenseeReport = _weeklyStatisticsReportService.GetMainReport(new GetMainReportCriteria
                {
                    FromDate = criteria.FromDate,
                    ToDate = criteria.ToDate,
                    SiteType = (int)ReportType.Licensee
                })
            });

            return jsonResult;
        }

        public void ExportIbcReport(GetMainReportCriteria criteria)
        {
            criteria.SiteType = (int)ReportType.CreditSite;
            _weeklyStatisticsReportService.ExportMainReport(criteria);
        }

        public void ExportLicenseeReport(GetMainReportCriteria criteria)
        {
            criteria.SiteType = (int)ReportType.Licensee;
            _weeklyStatisticsReportService.ExportMainReport(criteria);
        }

        [HttpPost]
        public ActionResult DetailReport(GetDetailReportCriteria criteria)
        {
            var result = new
            {
                Report = _weeklyStatisticsReportService.DetailReport(criteria),
                MainCategory = _serviceSetting.GetAllMainCategories(),
                CommunicationToolList = _serviceSetting.GetAllCommunicationTools(),
                ExtensionList = _serviceSetting.GetAllExtensions(),
                ProductList = _serviceSetting.GetProducts(),
                SubProductList = _serviceSetting.GetSubProducts()
            };
            var jsonResult = Json(result);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public void ExportDetailReport(GetDetailReportCriteria criteria)
        {
            var mainCategory = _serviceSetting.GetAllMainCategories();
            var communicationToolList = _serviceSetting.GetAllCommunicationTools();
            var extensionList = _serviceSetting.GetAllExtensions();
            var productList = _serviceSetting.GetProducts();
            var subProductList = _serviceSetting.GetSubProducts();
            _weeklyStatisticsReportService.ExportDetailReport(
                criteria,
                mainCategory,
                communicationToolList,
                extensionList,
                productList,
                subProductList);
        }
    }
}
