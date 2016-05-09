// -----------------------------------------------------------------------
// <copyright file="DeletedFeedbackStatisticsController.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.Report.Controllers
{
    using System.Text.RegularExpressions;
    using System.Web.Mvc;
    using Fanex.CRM.Common.Attributes;
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Controllers;
    using Fanex.CRM.Common.Excel;
    using Fanex.CRM.Report.Criteria.DeletedFeedbackStatistics;
    using Fanex.CRM.Report.Excels.DeletedFeedbackStatistics;
    using Fanex.CRM.Report.Services.DeletedFeedbackStatistics;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using ONEbook.UM.Filters;

    [CheckUMFilter]
    public class DeletedFeedbackStatisticsController : BaseController
    {
        private readonly IDeletedFeedbackStatisticsService _deletedFeedbackStatisticsService;

        public DeletedFeedbackStatisticsController()
            : this(null)
        {
        }

        public DeletedFeedbackStatisticsController(IDeletedFeedbackStatisticsService deletedFeedbackStatisticsService)
        {
            _deletedFeedbackStatisticsService = deletedFeedbackStatisticsService ?? new DeletedFeedbackStatisticsService();
        }

        [NapAuth(FunctionKey = FunctionPermissions.DeletedFeedbackStatistic, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            return View("~/Areas/Report/DeletedFeedbackStatistics/Index.cshtml");
        }

        [HttpPost]
        [AjaxNapAuth(FunctionKey = FunctionPermissions.DeletedFeedbackStatistic, Policy = Constant.Policy.Read)]
        public ActionResult GetReport(DeletedFeedbackStatisticsCriteria criteria)
        {
            return Json(new 
            {
                ReportData = _deletedFeedbackStatisticsService.GetReport(criteria)
            });
        }

        public void ExportReport(DeletedFeedbackStatisticsCriteria criteria)
        {
            var report = _deletedFeedbackStatisticsService.GetReport(criteria);

            var excelFileName = Regex.Replace(report.Name, @"\W+", "_");
            BaseExcel deletedFeedbackStatisticsExcel = new DeletedFeedbackStatisticsExcel(report, excelFileName);
            deletedFeedbackStatisticsExcel.BuildWorkBook();
        }
    }
}
