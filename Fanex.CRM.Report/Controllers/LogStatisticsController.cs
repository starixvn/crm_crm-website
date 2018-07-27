namespace Fanex.CRM.Report.Controllers
{
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Controllers;
    using Fanex.CRM.Report.Criteria.LogStatistics;
    using Fanex.CRM.Report.Enums;
    using Fanex.CRM.Report.Excels.LogStatistics;
    using Fanex.CRM.Report.Models.LogStatistics;
    using Fanex.CRM.Report.Services.LogStatistics;
    using Fanex.CRM.Setting.Services;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using Models.StatisticsReportByDate;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web.Mvc;

    public class LogStatisticsController : ReadOnlyBaseController
    {
        private readonly ISettingService _serviceSetting;
        private readonly ILogStatisticsService _logStatisticsService;

        public LogStatisticsController()
            : this(null, null)
        {
        }

        public LogStatisticsController(
            ILogStatisticsService logStatisticsService, ISettingService serviceSetting)
        {
            _logStatisticsService = logStatisticsService ?? new LogStatisticsService();
            _serviceSetting = serviceSetting ?? new SettingService();
        }

        [NapAuth(FunctionKey = FunctionPermissions.LogStatistics, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            return View("~/Areas/Report/LogStatistics/Index.cshtml");
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermissions.LogStatistics, Policy = Constant.Policy.Read)]
        public ActionResult GetAllSettings()
        {
            var products = _serviceSetting.GetProductsOnly().Select(p => new
            {
                p.Id,
                Name = p.Value
            });
            var communicationTools = _serviceSetting.GetAllCommunicationTools().Select(p => new
            {
                p.Id,
                p.Name
            });
            var mainCategories = _serviceSetting.GetAllMainCategories().Select(p => new
            {
                p.Id,
                p.Name
            });

            var allSetting = new SettingForLogStatistics
            {
                CommunicationToolList = communicationTools,
                MainCategoryList = mainCategories,
                ProductList = products
            };

            return Json(allSetting, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermissions.LogStatistics, Policy = Constant.Policy.Read)]
        public ActionResult GetMainReport(LogStatisticsCriteria criteria)
        {
            if (criteria.IsValid())
            {
                return Json(new
                {
                    Success = true,
                    IBCReport = _logStatisticsService.GetReportData(new LogStatisticsCriteria
                    {
                        ProductId = criteria.ProductId,
                        MainCategoryId = criteria.MainCategoryId,
                        CommunicationId = criteria.CommunicationId,
                        FromDate = criteria.FromDate,
                        ToDate = criteria.ToDate,
                        ProductType = (int)ProductType.Detail,
                        CategoryType = (int)CategoryType.SubCategory,
                        SiteType = (int)LogStatisticsReportType.IBC
                    }),

                    LicenseeReport = _logStatisticsService.GetReportData(new LogStatisticsCriteria
                    {
                        ProductId = criteria.ProductId,
                        MainCategoryId = criteria.MainCategoryId,
                        CommunicationId = criteria.CommunicationId,
                        FromDate = criteria.FromDate,
                        ToDate = criteria.ToDate,
                        ProductType = (int)ProductType.Detail,
                        CategoryType = (int)CategoryType.SubCategory,
                        SiteType = (int)LogStatisticsReportType.Licensee
                    })
                });
            }

            return Json(new { Success = false });
        }

        [HttpPost]
        public ActionResult GetReportDetail(LogStatisticsReportDetailCriteria criteria)
        {
            if (criteria.IsValid())
            {
                var jsonResult = Json(new
                {
                    Report = _logStatisticsService.GetReportDetail(criteria),
                    MainCategories = _serviceSetting.GetAllMainCategoriesFromAllSettings(),
                    CommunicationToolList = _serviceSetting.GetAllCommunicationTools(),
                    ExtensionList = _serviceSetting.GetAllExtensions(),
                    ProductList = _serviceSetting.GetProducts(),
                    SubProductList = _serviceSetting.GetSubProducts(),
                });
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }

            return Json(new
            {
                Report = new StatisticsDetailReportByDateModel
                {
                    Name = string.Empty,
                    ChildReports = new List<StatisticsDetailbyDateModel>(),
                    TotalChildReports = 0
                }
            });
        }

        public void ExportReport(LogStatisticsCriteria criteria)
        {
            criteria.ProductType = (int)ProductType.Detail;
            criteria.CategoryType = (int)CategoryType.SubCategory;

            var report = new LogStatisticsViewModel();
            if (criteria.IsValid())
            {
                report = _logStatisticsService.GetReportData(criteria);
            }

            var excelFileName = Regex.Replace(report.Name, @"\W+", "_");

            LogStatisticsExcel logStatisticExcel = new LogStatisticsExcel(report, excelFileName);
            logStatisticExcel.BuildWorkBook();
        }

        public void ExportReportDetail(LogStatisticsReportDetailCriteria criteria)
        {
            if (criteria.IsValid())
            {
                criteria.PageIndex = 0;
                criteria.PageSize = 0;
                var reportDetail = _logStatisticsService.GetReportDetail(criteria);

                var excelFileName = Regex.Replace(reportDetail.Name, @"\W+", "_");

                var mainCategory = _serviceSetting.GetAllMainCategoriesFromAllSettings();
                var communicationToolList = _serviceSetting.GetAllCommunicationTools();
                var extensionList = _serviceSetting.GetAllExtensions();
                var productList = _serviceSetting.GetProducts();
                var subProductList = _serviceSetting.GetSubProducts();

                LogStatisticsReportDetailExcel reportDetailExcel = new LogStatisticsReportDetailExcel(
                    reportDetail,
                    excelFileName,
                    mainCategory,
                    communicationToolList,
                    extensionList,
                    productList,
                    subProductList
                );
                reportDetailExcel.BuildWorkBook();
            }
        }
    }
}