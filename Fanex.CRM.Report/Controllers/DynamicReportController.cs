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
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Controllers;
    using Fanex.CRM.Report.Criteria.DynamicReport;
    using Fanex.CRM.Report.Services.DynamicReport;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// Dynamic Report Controller.
    /// </summary>
    [NapAuth(FunctionKey = FunctionPermissions.DynamicReport, Policy = Constant.Policy.Read)]
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

        public ActionResult Index()
        {
            return View("~/Areas/Report/DynamicReport/Index.cshtml");
        }

        /// <summary>
        /// Gets all setting.
        /// </summary>
        /// <returns>All setting.</returns>
        [HttpPost]
        public ActionResult AllSetting()
        {
            var allSetting = new
            {
                PeriodTypeList = _dynamicReportService.AllPeriodType(),
                ProductList = _dynamicReportService.AllProduct(),
                SiteList = _dynamicReportService.AllSite(),
                StatusList = _dynamicReportService.AllStatus(),
                ResponseList = _dynamicReportService.AllResponse(),
                RoleList = _dynamicReportService.AllRole(),
                CommunicationToolList = _dynamicReportService.AllCommunicationTool(),
                MainCategoryList = _dynamicReportService.AllMainCategory(),
                ExtensionList = _dynamicReportService.AllExtension()
            };

            return Json(allSetting, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
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
