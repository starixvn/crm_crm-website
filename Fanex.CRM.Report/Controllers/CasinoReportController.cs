// --------------------------------------------------------------------------
// <copyright file="CasinoReportController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.07.31">Created</change>
// </history>

namespace Fanex.CRM.Report.Controllers
{
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Controllers;
    using Fanex.CRM.Report.Criteria.CasinoReport;
    using Fanex.CRM.Report.Services.CasinoReport;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using Setting.Services;
    using System.Web.Mvc;

    /// <summary>
    /// Casino Report Controller.
    /// </summary>
    public class CasinoReportController : ReadOnlyBaseController
    {
        private readonly ICasinoReportService _casinoReportService;
        private readonly ISettingService _serviceSetting;

        public CasinoReportController()
            : this(null)
        {
        }

        public CasinoReportController(ICasinoReportService casinoReportService)
        {
            _casinoReportService = casinoReportService ?? new CasinoReportService();
            _serviceSetting = new SettingService();
        }

        // Casino Daily Statistics.
        [NapAuth(FunctionKey = FunctionPermissions.CasinoDailyStatistics, Policy = Constant.Policy.Read)]
        public ActionResult CasinoDailyStatistics()
        {
            return View("~/Areas/Report/CasinoDailyStatistics/Index.cshtml");
        }

        [NapAuth(FunctionKey = FunctionPermissions.CasinoDailyStatistics, Policy = Constant.Policy.Read)]
        public ActionResult CasinoDailySummary(GetCasinoSummaryCriteria criteria)
        {
            var report = _casinoReportService.GetCasinoDailySummary(criteria);
            return Json(report, JsonRequestBehavior.AllowGet);
        }

        [NapAuth(FunctionKey = FunctionPermissions.CasinoDailyStatistics, Policy = Constant.Policy.Read)]
        public ActionResult ExportCasinoDailySummary(GetCasinoSummaryCriteria criteria)
        {
            _casinoReportService.ExportCasinoDailySummary(criteria);
            return new EmptyResult();
        }

        [NapAuth(FunctionKey = FunctionPermissions.CasinoDailyStatistics, Policy = Constant.Policy.Read)]
        public ActionResult CasinoDailyDetail(GetCasinoDetailCriteria criteria)
        {
            var result = new
            {
                Report = _casinoReportService.GetCasinoDailyDetail(criteria),
                MainCategory = _serviceSetting.GetAllMainCategories(),
                CommunicationToolList = _serviceSetting.GetAllCommunicationTools(),
                ExtensionList = _serviceSetting.GetAllExtensions(),
                ProductList = _serviceSetting.GetProducts(),
                SubProductList = _serviceSetting.GetSubProducts()

            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [NapAuth(FunctionKey = FunctionPermissions.CasinoDailyStatistics, Policy = Constant.Policy.Read)]
        public ActionResult ExportCasinoDailyDetail(GetCasinoDetailCriteria criteria)
        {
            var mainCategory = _serviceSetting.GetAllMainCategories();
            var communicationToolList = _serviceSetting.GetAllCommunicationTools();
            var extensionList = _serviceSetting.GetAllExtensions();
            var productList = _serviceSetting.GetProducts();
            var subProductList = _serviceSetting.GetSubProducts();
            _casinoReportService.ExportCasinoDailyDetail(
                criteria,
                mainCategory,
                communicationToolList,
                extensionList,
                productList,
                subProductList);
            return new EmptyResult();
        }

        // Casino Weekly Statistics.
        [NapAuth(FunctionKey = FunctionPermissions.CasinoWeeklyStatistics, Policy = Constant.Policy.Read)]
        public ActionResult CasinoWeeklyStatistics()
        {
            return View("~/Areas/Report/CasinoWeeklyStatistics/Index.cshtml");
        }

        [NapAuth(FunctionKey = FunctionPermissions.CasinoWeeklyStatistics, Policy = Constant.Policy.Read)]
        public ActionResult CasinoWeeklySummary(GetCasinoSummaryCriteria criteria)
        {
            var report = _casinoReportService.GetCasinoWeeklySummary(criteria);
            return Json(report, JsonRequestBehavior.AllowGet);
        }

        [NapAuth(FunctionKey = FunctionPermissions.CasinoWeeklyStatistics, Policy = Constant.Policy.Read)]
        public ActionResult ExportCasinoWeeklySummary(GetCasinoSummaryCriteria criteria)
        {
            _casinoReportService.ExportCasinoWeeklySummary(criteria);
            return new EmptyResult();
        }

        [NapAuth(FunctionKey = FunctionPermissions.CasinoWeeklyStatistics, Policy = Constant.Policy.Read)]
        public ActionResult CasinoWeeklyDetail(GetCasinoDetailCriteria criteria)
        {
            var result = new
            {
                Report = _casinoReportService.GetCasinoWeeklyDetail(criteria),
                MainCategory = _serviceSetting.GetAllMainCategories(),
                CommunicationToolList = _serviceSetting.GetAllCommunicationTools(),
                ExtensionList = _serviceSetting.GetAllExtensions(),
                ProductList = _serviceSetting.GetProducts(),
                SubProductList = _serviceSetting.GetSubProducts()

            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [NapAuth(FunctionKey = FunctionPermissions.CasinoWeeklyStatistics, Policy = Constant.Policy.Read)]
        public ActionResult ExportCasinoWeeklyDetail(GetCasinoDetailCriteria criteria)
        {
            var mainCategory = _serviceSetting.GetAllMainCategories();
            var communicationToolList = _serviceSetting.GetAllCommunicationTools();
            var extensionList = _serviceSetting.GetAllExtensions();
            var productList = _serviceSetting.GetProducts();
            var subProductList = _serviceSetting.GetSubProducts();
            criteria.ReportType = 3; // Weekly
            _casinoReportService.ExportCasinoDailyDetail(
                criteria,
                mainCategory,
                communicationToolList,
                extensionList,
                productList,
                subProductList);
            return new EmptyResult();
        }
    }
}
