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
    using Fanex.CRM.Infrastructure.Web.Controllers;
    using Fanex.CRM.Report.Criteria.DynamicReport;
    using Fanex.CRM.Report.Services.Implements.StatisticReport;
    using Fanex.CRM.Report.Services.Interfaces;

    /// <summary>
    /// Statistic Report Controller.
    /// </summary>
    public class StatisticReportController : BaseController
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

        [AuthenticateAction(AuthenticateAction.AuthenticationType.Read, "Index")]
        public ActionResult Index()
        {
            return View("~/Areas/Report/StatisticReport/Index.cshtml");
        }

        /// <summary>
        /// Gets all setting.
        /// </summary>
        /// <returns>All setting.</returns>
        [HttpPost]
        [AuthenticateAction(AuthenticateAction.AuthenticationType.Read, "Index")]
        public ActionResult GetAllSetting()
        {
            var allSetting = new
            {
                PeriodTypeList = _statisticReportService.GetAllPeriodType(),
                MainCategoryList = _statisticReportService.GetAllMainCategory()
            };

            return Json(allSetting, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [HandleAjaxError]
        [AuthenticateAction(AuthenticateAction.AuthenticationType.Read, "Index")]
        public ActionResult GetStatisticReport(GetStatisticReportCriteria criteria)
        {
            var logs = _statisticReportService.GetStatisticReport(criteria);
            var jsonResult = Json(logs, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }
    }
}
