// -----------------------------------------------------------------------
// <copyright file="LogStatistic.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.Report.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web.Mvc;
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Controllers;
    using Fanex.CRM.Common.Enumerations;
    using Fanex.CRM.Common.Services;
    using Fanex.CRM.Report.Criteria.LogStatistic;
    using Fanex.CRM.Report.Enums;
    using Fanex.CRM.Report.Excels.LogStatistic;
    using Fanex.CRM.Report.Models.LogStatistic;
    using Fanex.CRM.Report.Services.LogStatistic;
    using Fanex.CRM.Setting.Services;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    public class LogStatisticController : BaseController
    {
        private readonly ISettingService _serviceSetting;
        private readonly ILogStatisticService _logStatisticService;

        public LogStatisticController()
            : this(null, null)
        { 
        }

        public LogStatisticController(
            ILogStatisticService logStatisticService,
            ISettingService serviceSetting)
        {
            _logStatisticService = logStatisticService ?? new LogStatisticService();
            _serviceSetting = serviceSetting ?? new SettingService();
        }

        [NapAuth(FunctionKey = FunctionPermissions.LogStatistic, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            return View("~/Areas/Report/LogStatistic/Index.cshtml");
        }

        [HttpPost]
        [AjaxNapAuth(FunctionKey = FunctionPermissions.LogStatistic, Policy = Constant.Policy.Read)]
        public ActionResult GetAllSettings()
        {
            var productSettings = new List<int>() { 8000, 8001, 8003, 8009, 8010, 8013, 8014 };
            var communicationSettings = new List<int>() { 1000, 1001, 1003, 1006, 1007 };
            var mainCategorySettings = new List<int>() { 3000, 3001, 3003, 3008, 3009 };
            var products = _serviceSetting.AllProduct().Where(p => productSettings.Contains(p.Id)).Select(p => new
                {
                    Id = p.Id,
                    Name = p.Value
                });
            var communicationTools = _serviceSetting.AllCommunicationTool().Where(p => communicationSettings.Contains(p.Id)).Select(p => new
                {
                    Id = p.Id,
                    Name = p.Name
                });
            var mainCategories = _serviceSetting.AllMainCategory().Where(p => mainCategorySettings.Contains(p.Id)).Select(p => new
                {
                    Id = p.Id,
                    Name = p.Name
                });

            var allSetting = new SettingForLogStatistic
            {
                CommunicationToolList = communicationTools,
                MainCategoryList = mainCategories,
                ProductList = products
            };

            return Json(allSetting, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AjaxNapAuth(FunctionKey = FunctionPermissions.LogStatistic, Policy = Constant.Policy.Read)]
        public ActionResult GetMainReport(LogStatisticCriteria criteria)
        {
            return Json(new
            {
                IBCReport = _logStatisticService.GetReportData(new LogStatisticCriteria
                {
                    ProductId = criteria.ProductId,
                    MainCategoryId = criteria.MainCategoryId,
                    CommunicationId = criteria.CommunicationId,
                    FromDate = criteria.FromDate,
                    ToDate = criteria.ToDate,
                    ProductType = (int)ProductType.Detail,
                    CategoryType = (int)CategoryType.SubCategory,
                    SiteType = (int)LogStatisticReportType.IBC
                }),

                LicenseeReport = _logStatisticService.GetReportData(new LogStatisticCriteria
                {
                    ProductId = criteria.ProductId,
                    MainCategoryId = criteria.MainCategoryId,
                    CommunicationId = criteria.CommunicationId,
                    FromDate = criteria.FromDate,
                    ToDate = criteria.ToDate,
                    ProductType = (int)ProductType.Detail,
                    CategoryType = (int)CategoryType.SubCategory,
                    SiteType = (int)LogStatisticReportType.Licensee
                })
            });
        }

        public void ExportReport(LogStatisticCriteria criteria)
        {
            criteria.ProductType = (int)ProductType.Detail;
            criteria.CategoryType = (int)CategoryType.SubCategory;

            var report = _logStatisticService.GetReportData(criteria);

            var excelFileName = Regex.Replace(report.Name, @"\W+", "_");

            LogStatisticExcel logStatisticExcel = new LogStatisticExcel(report, excelFileName);
            logStatisticExcel.BuildWorkBook();
        }
    }
}
