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
    using Fanex.CRM.Common.Attributes;
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Controllers;
    using Fanex.CRM.Common.Enumerations;
    using Fanex.CRM.Infrastructure.Attributes;
    using Fanex.CRM.Report.Criteria.SettlementReport;
    using Fanex.CRM.Report.Services.SettlementReport;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using ONEbook.UM.Filters;

    /// <summary>
    /// Settlement Report Controller.
    /// </summary>
    [NapAuth(FunctionKey = FunctionPermissions.SettlementIssuesReport, Policy = Constant.Policy.Read)]
    [CheckUMFilter]
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

        [AuthenticateAction(AuthenticationType.Read, "Index")]
        public ActionResult Index()
        {
            return View("~/Areas/Report/SettlementReport/Index.cshtml");
        }

        [HttpPost]
        [HandleAjaxError]
        [AuthenticateAction(AuthenticationType.Read, "Index")]
        public ActionResult GetSettlementReport(GetSettlementReportCriteria criteria)
        {
            var logs = _settlementReportService.GetSettlementReport(criteria);
            var jsonResult = Json(logs, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        public ActionResult ExportSettlementReport(GetSettlementReportCriteria criteria)
        {
            _settlementReportService.ExportSettlementReport(criteria);
            return new EmptyResult();
        }
    }
}
