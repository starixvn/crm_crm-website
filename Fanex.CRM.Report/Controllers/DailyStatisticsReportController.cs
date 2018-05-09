// --------------------------------------------------------------------------
// <copyright file="DailyStatisticsReportController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Controllers
{
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Controllers;
    using Fanex.CRM.Common.Enumerations;
    using Fanex.CRM.Report.Criteria.StatisticByDateReport;
    using Fanex.CRM.Report.Services.DailyStatisticsReport;
    using Fanex.CRM.Report.Services.StatisticByDateReport;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using Setting.Services;
    using System.Web.Mvc;

    /// <summary>
    /// Daily Statistics Report Controller.
    /// </summary>
    [NapAuth(FunctionKey = FunctionPermissions.DailyStatistics, Policy = Constant.Policy.Read)]
    public class DailyStatisticsReportController : ReadOnlyBaseController
    {
        private IStatisticByDateReportService _dailyStatisticsReportService;
        public ISettingService _serviceSetting;

        public DailyStatisticsReportController()
            : this(null)
        {
        }

        public DailyStatisticsReportController(IStatisticByDateReportService dailyStatisticsReportService)
        {
            _dailyStatisticsReportService = dailyStatisticsReportService ?? new DailyStatisticsReportService();
            _serviceSetting = new SettingService();
        }

        public ActionResult Index()
        {
            return View("~/Areas/Report/DailyStatisticsReport/Index.cshtml");
        }

        /// <summary>
        /// Gets the main report.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The main report.</returns>
        [HttpPost]
        public ActionResult MainReport(GetMainReportCriteria criteria)
        {
            var jsonResult = Json(new
            {
                IBCReport = _dailyStatisticsReportService.GetMainReport(new GetMainReportCriteria
                {
                    FromDate = criteria.FromDate,
                    ToDate = criteria.ToDate,
                    SiteType = (int)ReportType.CreditSite
                }),
                LicenseeReport = _dailyStatisticsReportService.GetMainReport(new GetMainReportCriteria
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
            _dailyStatisticsReportService.ExportMainReport(criteria);
        }

        public void ExportLicenseeReport(GetMainReportCriteria criteria)
        {
            criteria.SiteType = (int)ReportType.Licensee;
            _dailyStatisticsReportService.ExportMainReport(criteria);
        }

        [HttpPost]
        public ActionResult DetailReport(GetDetailReportCriteria criteria)
        {
            var result = new
            {
                Report = _dailyStatisticsReportService.DetailReport(criteria),
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
            _dailyStatisticsReportService.ExportDetailReport(
                criteria,
                mainCategory,
                communicationToolList,
                extensionList,
                productList,
                subProductList);
        }
    }
}
