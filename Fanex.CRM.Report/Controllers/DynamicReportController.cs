// --------------------------------------------------------------------------
// <copyright file="DynamicReportController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Controllers
{
    using System.Web.Mvc;
    using Fanex.CRM.Infrastructure.Common;
    using Fanex.CRM.Infrastructure.Web.Controllers;
    using Fanex.CRM.Report.Criteria.DynamicReport;
    using Fanex.CRM.Report.Services.Implements;
    using Fanex.CRM.Report.Services.Interfaces;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// Dynamic Report Controller.
    /// </summary>
    public class DynamicReportController : BaseController
    {
        private IDynamicReportService _dynamicReportService;

        public DynamicReportController()
            : this(null)
        {
        }

        public DynamicReportController(IDynamicReportService dynamicReportService)
        {
            _dynamicReportService = dynamicReportService ?? new DynamicReportService();
        }

        [NapAuth(FunctionKey = FunctionPermission.DynamicReport, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            return View("~/Areas/Report/DynamicReport/Index.cshtml");
        }

        /// <summary>
        /// Gets all setting.
        /// </summary>
        /// <returns>All setting.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermission.DynamicReport, Policy = Constant.Policy.Read)]
        public ActionResult GetAllSetting()
        {
            var allSetting = new
            {
                PeriodTypeList = _dynamicReportService.GetAllPeriodType(),
                ProductList = _dynamicReportService.GetAllProduct(),
                SiteList = _dynamicReportService.GetAllSite(),
                StatusList = _dynamicReportService.GetAllStatus(),
                ResponseList = _dynamicReportService.GetAllResponse(),
                RoleList = _dynamicReportService.GetAllRole(),
                CommunicationToolList = _dynamicReportService.GetAllCommunicationTool(),
                MainCategoryList = _dynamicReportService.GetAllMainCategory(),
                ExtensionList = _dynamicReportService.GetAllExtension()
            };

            return Json(allSetting, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermission.DynamicReport, Policy = Constant.Policy.Read)]
        public ActionResult GetDynamicReport(GetDynamicReportCriteria criteria)
        {
            var logs = _dynamicReportService.GetDynamicReport(criteria);
            var jsonResult = Json(logs, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        public ActionResult ExportDynamicReport(GetDynamicReportCriteria criteria)
        {
            _dynamicReportService.ExportDynamicReport(criteria);
            return new EmptyResult();
        }
    }
}
