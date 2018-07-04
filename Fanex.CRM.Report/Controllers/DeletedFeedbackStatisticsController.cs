// -----------------------------------------------------------------------
// <copyright file="DeletedFeedbackStatisticsController.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.Report.Controllers
{
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Controllers;
    using Fanex.CRM.Common.Excel;
    using Fanex.CRM.Report.Criteria.DeletedFeedbackStatistics;
    using Fanex.CRM.Report.Excels.DeletedFeedbackStatistics;
    using Fanex.CRM.Report.Services.DeletedFeedbackStatistics;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using Setting.Services;
    using System.Text.RegularExpressions;
    using System.Web.Mvc;

    public class DeletedFeedbackStatisticsController : ReadOnlyBaseController
    {
        private readonly IDeletedFeedbackStatisticsService _deletedFeedbackStatisticsService;
        private readonly ISettingService _serviceSetting;

        public DeletedFeedbackStatisticsController()
            : this(null, null)
        {
        }

        public DeletedFeedbackStatisticsController(
            IDeletedFeedbackStatisticsService deletedFeedbackStatisticsService,
            ISettingService serviceSetting)
        {
            _deletedFeedbackStatisticsService = deletedFeedbackStatisticsService ?? new DeletedFeedbackStatisticsService();
            _serviceSetting = serviceSetting ?? new SettingService();
        }

        [NapAuth(FunctionKey = FunctionPermissions.DeletedFeedbackStatistic, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            return View("~/Areas/Report/DeletedFeedbackStatistics/Index.cshtml");
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermissions.DeletedFeedbackStatistic, Policy = Constant.Policy.Read)]
        public ActionResult GetReport(DeletedFeedbackStatisticsCriteria criteria)
        {
            return Json(new
            {
                ReportData = _deletedFeedbackStatisticsService.GetReport(criteria),
                MainCategories = _serviceSetting.GetAllMainCategoriesFromAllSettings()
            });
        }

        public void ExportReport(DeletedFeedbackStatisticsCriteria criteria)
        {
            var report = _deletedFeedbackStatisticsService.GetReport(criteria);
            var mainCategories = _serviceSetting.GetAllMainCategoriesFromAllSettings();

            var excelFileName = Regex.Replace(report.Name, @"\W+", "_");
            BaseExcel deletedFeedbackStatisticsExcel = new DeletedFeedbackStatisticsExcel(report, excelFileName, mainCategories);
            deletedFeedbackStatisticsExcel.BuildWorkBook();
        }
    }
}