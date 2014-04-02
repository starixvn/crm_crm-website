// --------------------------------------------------------------------------
// <copyright file="SettlementReportController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Controllers
{
    using System.Web.Mvc;
    using Fanex.CRM.Infrastructure.Web.Controllers;
    using Fanex.CRM.Report.Criteria.SettlementReport;
    using Fanex.CRM.Report.Services.Implements.SettlementReport;
    using Fanex.CRM.Report.Services.Interfaces;

    /// <summary>
    /// Settlement Report Controller.
    /// </summary>
    public class SettlementReportController : BaseController
    {
        private ISettlementReportService _settlementReportService;

        public SettlementReportController()
            : this(null)
        {
        }

        public SettlementReportController(ISettlementReportService settlementReportService)
        {
            _settlementReportService = settlementReportService ?? new SettlementReportService();
        }

        [AuthenticateAction(AuthenticateAction.AuthenticationType.Read, "Index")]
        public ActionResult Index()
        {
            return View("~/Areas/Report/SettlementReport/Index.cshtml");
        }

        [HttpPost]
        [HandleAjaxError]
        [AuthenticateAction(AuthenticateAction.AuthenticationType.Read, "Index")]
        public ActionResult GetSettlementReport(GeSettlementReportCriteria criteria)
        {
            var logs = _settlementReportService.GetSettlementReport(criteria);
            var jsonResult = Json(logs, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        public ActionResult ExportSettlementReport(GeSettlementReportCriteria criteria)
        {
            _settlementReportService.ExportSettlementReport(criteria);
            return new EmptyResult();
        }
    }
}
