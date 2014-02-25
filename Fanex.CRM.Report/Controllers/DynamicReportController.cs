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
    using Fanex.CRM.Report.Services.Implements;
    using Fanex.CRM.Report.Services.Interfaces;
    using Fanex.CRM.Setting.Services.Implements;
    using Fanex.CRM.Setting.Services.Interfaces;
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

        [NapAuth(FunctionKey = FunctionPermission.CRMReport, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            return View("~/Areas/Report/DynamicReport/Index.cshtml");
        }

        /// <summary>
        /// Gets all setting.
        /// </summary>
        /// <returns>All setting.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermission.CRMReport, Policy = Constant.Policy.Read)]
        public ActionResult GetAllSetting()
        {
            var allSetting = new
            {
                PeriodTypeList = _dynamicReportService.GetAllPeriodType(),
                ProductList = _dynamicReportService.GetAllProduct(),
                SiteList = _dynamicReportService.GetAllSite(),
                StatusList = _dynamicReportService.GetAllStatus(),
                ResponseList = _dynamicReportService.GetAllResponse(),
                //RoleList = _dynamicReportService.GetAllRole(),
                //CommunicationToolList = _dynamicReportService.GetAllCommunicationTool()
            };

            return Json(allSetting, JsonRequestBehavior.AllowGet);
        }
    }
}
