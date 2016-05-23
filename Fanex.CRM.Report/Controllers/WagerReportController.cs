// --------------------------------------------------------------------------
// <copyright file="WagerReportController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.03.28">Created</change>
// </history>

namespace Fanex.CRM.Report.Controllers
{
    using System.Web.Mvc;
    using Fanex.CRM.Common.Attributes;
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Controllers;
    using Fanex.CRM.Common.Enumerations;
    using Fanex.CRM.Infrastructure.Attributes;
    using Fanex.CRM.Report.Criteria.WagerReport;
    using Fanex.CRM.Report.Services.WagerReport;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// WagerReportController controller.
    /// </summary>
    [NapAuth(FunctionKey = FunctionPermissions.WagersRelatedReport, Policy = Constant.Policy.Read)]
    public class WagerReportController : BaseController
    {
        private readonly IWagerReportService _wagerService;

        public WagerReportController()
            : this(null)
        {
        }

        public WagerReportController(IWagerReportService wagerService)
        {
            _wagerService = wagerService ?? new WagerReportService();
        }

        [HttpGet]
        [AuthenticateAction(AuthenticationType.Read, "Index")]
        public ActionResult Index()
        {
            return View("~/Areas/Report/WagerReport/Index.cshtml");
        }

        [HttpGet]
        [AuthenticateAction(AuthenticationType.Read, "Index")]
        [HandleAjaxError]
        public ActionResult GetWagerReport(GetWagerReportCriteria criteria)
        {
            var report = _wagerService.GetWagerReport(criteria);
            return Json(report, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AuthenticateAction(AuthenticationType.Read, "Index")]
        public ActionResult ExportWagerReportExcel(GetWagerReportCriteria criteria)
        {
            _wagerService.ExportWagerReportExcel(criteria);
            return new EmptyResult();
        }
    }
}
