namespace Fanex.CRM.Report.Controllers
{
    using Common.Attributes;
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Controllers;
    using Fanex.CRM.Report.Criteria.DynamicReport;
    using Fanex.CRM.Report.Services.DynamicReport;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using Models;
    using Setting.Services;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class DynamicReportController : ReadOnlyBaseController
    {
        private readonly IDynamicReportService _dynamicReportService;
        private readonly ISettingService _serviceSetting;

        public DynamicReportController()
            : this(null, null)
        {
        }

        public DynamicReportController(IDynamicReportService dynamicReportService, ISettingService serviceSetting)
        {
            _dynamicReportService = dynamicReportService ?? new DynamicReportService();
            _serviceSetting = serviceSetting ?? new SettingService();
        }

        [NapAuth(FunctionKey = FunctionPermissions.DynamicReport, Policy = Constant.Policy.Read)]

        public ActionResult Index()
        {
            return View("~/Areas/Report/DynamicReport/Index.cshtml");
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermissions.DynamicReport, Policy = Constant.Policy.Read)]

        public ActionResult AllSetting()
        {
            var allSetting = new
            {
                PeriodTypeList = _dynamicReportService.GetAllPeriodTypes(),
                ProductList = _dynamicReportService.GetAllProducts(),
                SiteList = _dynamicReportService.GetAllSites(),
                StatusList = _dynamicReportService.GetAllStatuses(),
                ResponseList = _dynamicReportService.GetAllResponses(),
                RoleList = _dynamicReportService.GetAllRoles(),
                CommunicationToolList = _dynamicReportService.GetAllCommunicationTools(),
                MainCategoryList = _dynamicReportService.GetAllMainCategories(),
                ExtensionList = _dynamicReportService.GetAllExtensions()
            };

            return Json(allSetting, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [OverridableNapAuthAttribute(FunctionKey = FunctionPermissions.DynamicReport, Policy = Constant.Policy.Read)]

        public ActionResult GetDynamicReport(GetDynamicReportCriteria criteria)
        {
            if (criteria.IsValid())
            {
                var logs = _dynamicReportService.GetDynamicReport(criteria);
                var result = new
                {
                    MainCategories = _serviceSetting.GetAllMainCategoriesFromAllSettings(),
                    Logs = logs
                };

                return Json(result, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                Logs = new DynamicReportInfo
                {
                    ChildReports = new List<DynamicReportInfo>(),
                }
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExportDynamicReport(GetDynamicReportCriteria criteria)
        {
            if (criteria.IsValid())
            {
                var mainCategory = _serviceSetting.GetAllMainCategoriesFromAllSettings();
                _dynamicReportService.ExportDynamicReport(criteria, mainCategory);
            }

            return new EmptyResult();
        }
    }
}
