// --------------------------------------------------------------------------
// <copyright file="StatisticReportController.cs" company="Nexcel Solutions Vietnam">
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
    using Fanex.CRM.Infrastructure.ActionResults;
    using Fanex.CRM.Infrastructure.Attributes;
    using Fanex.CRM.Report.Criteria.StatisticReport;
    using Fanex.CRM.Report.Services.StatisticReport;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// Statistic Report Controller.
    /// </summary>
    [NapAuth(FunctionKey = FunctionPermissions.IssueStatistics, Policy = Constant.Policy.Read)]
    public class StatisticReportController : ReadOnlyBaseController
    {
        private IStatisticReportService _statisticReportService;

        public StatisticReportController()
            : this(null)
        {
        }

        public StatisticReportController(IStatisticReportService statisticReportrtService)
        {
            _statisticReportService = statisticReportrtService ?? new StatisticReportService();
        }

        [AuthenticateAction(AuthenticationType.Read, "Index")]
        public ActionResult Index()
        {
            return View("~/Areas/Report/StatisticReport/Index.cshtml");
        }

        /// <summary>
        /// Gets all setting.
        /// </summary>
        /// <returns>All setting.</returns>
        [HttpPost]
        [AuthenticateAction(AuthenticationType.Read, "Index")]
        public ActionResult AllSetting()
        {
            var allSetting = new
            {
                PeriodTypeList = _statisticReportService.AllPeriodType(),
                MainCategoryList = _statisticReportService.AllMainCategory()
            };

            return Json(allSetting, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [HandleAjaxError]
        [AuthenticateAction(AuthenticationType.Read, "Index")]
        public JsonWithoutNullField GetStatisticReport(GetStatisticReportCriteria criteria)
        {
            var logs = _statisticReportService.StatisticReport(criteria);
            return new JsonWithoutNullField(logs);
        }

        public ActionResult ExportStatisticReport(GetStatisticReportCriteria criteria)
        {
            _statisticReportService.ExportStatisticReport(criteria);
            return new EmptyResult();
        }
    }
}
