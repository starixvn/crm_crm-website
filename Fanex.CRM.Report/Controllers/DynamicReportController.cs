// --------------------------------------------------------------------------
// <copyright file="DynamicReportController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Controllers
{
    using Common.Attributes;
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Controllers;
    using Fanex.CRM.Report.Criteria.DynamicReport;
    using Fanex.CRM.Report.Services.DynamicReport;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using Setting.Services;
    using System.Web.Mvc;

    /// <summary>
    /// Dynamic Report Controller.
    /// </summary>
    public class DynamicReportController : ReadOnlyBaseController
    {
        private IDynamicReportService _dynamicReportService;
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

        /// <summary>
        /// Gets all setting.
        /// </summary>
        /// <returns>All setting.</returns>
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
            var logs = _dynamicReportService.GetDynamicReport(criteria);
            var result = new
            {
                MainCategory = _serviceSetting.GetAllMainCategories(),
                Logs = logs
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExportDynamicReport(GetDynamicReportCriteria criteria)
        {
            var mainCategory = _serviceSetting.GetAllMainCategories();
            _dynamicReportService.ExportDynamicReport(criteria, mainCategory);
            return new EmptyResult();
        }
    }
}
