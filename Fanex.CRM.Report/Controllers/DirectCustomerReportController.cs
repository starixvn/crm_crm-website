using Fanex.CRM.Common.Constants;
using Fanex.CRM.Common.Controllers;
using Fanex.CRM.Report.Criteria.DirectCustomerReport;
using Fanex.CRM.Report.Excels.CustomerReport;
using Fanex.CRM.Report.Repositories.CustomerReport;
using Fanex.CRM.Report.Resources;
using Fanex.NAP.Core.Business;
using Fanex.NAP.Core.Web.Attribute;
using System.Web.Mvc;

namespace Fanex.CRM.Report.Controllers
{
    [NapAuth(FunctionKey = FunctionPermissions.DirectCustomerReport, Policy = Constant.Policy.Read)]
    public class DirectCustomerReportController : ReadOnlyBaseController
    {
        private readonly IDirectCustomerReportService _directCustomerReportService;

        public DirectCustomerReportController()
            : this(null)
        {
        }

        public DirectCustomerReportController(IDirectCustomerReportService ditectCustomerReportService)
        {
            _directCustomerReportService = ditectCustomerReportService ?? new DirectCustomerReportService();
        }

        public ActionResult Index()
        {
            return View("~/Areas/Report/DirectCustomerReport/Index.cshtml");
        }

        [HttpPost]
        public ActionResult GetReport(DirectCustomerReportCriteria criteria)
        {
            var customerReport = _directCustomerReportService.GetReport(criteria);
            return Json(customerReport, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public void ExportReport(DirectCustomerReportCriteria criteria)
        {
            criteria.PageIndex = 0;
            criteria.PageSize = 0;
            var data = _directCustomerReportService.GetReport(criteria);
            var excel = new DirectCustomerReportExcel(data, Labels.DirectCustomerReportTitle);
            excel.BuildWorkBook();
        }
    }
}
