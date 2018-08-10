namespace Fanex.CRM.Report.Controllers
{
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Controllers;
    using Fanex.CRM.Common.Extensions;
    using Fanex.CRM.Report.Criteria.RedemptionStatisticsReport;
    using Fanex.CRM.Report.Resources;
    using Fanex.CRM.Report.Services.DynamicReport;
    using Fanex.Excel.Extensions;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using System.Web.Mvc;

    [NapAuth(FunctionKey = FunctionPermissions.RedemptionStatistics, Policy = Constant.Policy.Read)]
    public class RedemptionStatisticsReportController : ReadOnlyBaseController
    {
        private readonly IRedemptionStatisticsService _redemptionStatisticsService;

        public RedemptionStatisticsReportController()
            : this(null)
        {
        }

        public RedemptionStatisticsReportController(IRedemptionStatisticsService redemptionStatisticsService)
        {
            _redemptionStatisticsService = redemptionStatisticsService ?? new RedemptionStatisticsService();
        }

        [HttpPost]
        public ActionResult AllSetting()
        {
            var allSetting = new
            {
                StatusList = _redemptionStatisticsService.AllStatus(),
                CurrencyList = _redemptionStatisticsService.AllCurrencyRedemption(),
                ReportByList = _redemptionStatisticsService.AllReportBy()
            };

            return Json(allSetting, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetRedemptionStatisticsSummaryReport(GetRedemptionStatisticsSummaryCriteria criteria)
        {
            criteria.Subject = criteria.Subject ?? string.Empty;
            return new JsonNetResult(_redemptionStatisticsService.GetRedemptionSummaryReport(criteria));
        }

        [HttpPost]
        public ActionResult GetRedemptionStatisticsDetailReport(GetRedemptionStatisticsDetailCriteria criteria)
        {
            if (criteria.IsValid())
            {
                criteria.Subject = criteria.Subject ?? string.Empty;
                criteria.StatusId = criteria.StatusId ?? string.Empty;
                return new JsonNetResult(_redemptionStatisticsService.GetRedemptionDetailReport(criteria));
            }

            return new JsonNetResult();
        }

        [HttpGet]
        public ActionResult ExportSummaryReport(GetRedemptionStatisticsSummaryCriteria criteria)
        {
            var excelFileName = this.GetSummaryFileName(criteria);
            criteria.Subject = criteria.Subject ?? string.Empty;

            NpoiExtensions.ResponseStreamExcel(_redemptionStatisticsService.GenerateSummaryExcelWorkbook(criteria), excelFileName);

            return new EmptyResult();
        }

        [HttpGet]
        public ActionResult ExportDetailReport(GetRedemptionStatisticsDetailCriteria criteria)
        {
            if (criteria.IsValid())
            {
                criteria.Subject = criteria.Subject ?? string.Empty;
                criteria.StatusId = criteria.StatusId ?? string.Empty;

                NpoiExtensions.ResponseStreamExcel(_redemptionStatisticsService.GenerateDetailExcelWorkbook(criteria), Labels.RedemptionStatisticsDetailReportFilename);
            }

            return new EmptyResult();
        }

        public ActionResult Index()
        {
            return View("~/Areas/Report/RedemptionStatistics/Index.cshtml");
        }

        private string GetSummaryFileName(GetRedemptionStatisticsSummaryCriteria criteria)
        {
            string pattern = "yyyyMMdd";
            return string.Format(
                Labels.RedemptionStatisticsReportFilename,
                criteria.FromDate.ToString(pattern),
                criteria.ToDate.ToString(pattern));
        }
    }
}