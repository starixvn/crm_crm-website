using System.Web.Mvc;
using Fanex.CRM.Common.Constants;
using Fanex.CRM.Common.Controllers;
using Fanex.CRM.Criteria.PicAndHeadline;
using Fanex.CRM.Services;
using Fanex.CRM.Services.Interfaces;
using Fanex.NAP.Core.Business;
using Fanex.NAP.Core.Web.Attribute;

namespace Fanex.CRM.Controllers
{
    //[NapAuth(FunctionKey = FunctionPermissions.ChangeLog, Policy = Constant.Policy.Read)]
    public class ChangeLogController : BaseController
    {
        private readonly IChangeLogService changeLogService;

        public ChangeLogController() : this(null)
        {
        }

        public ChangeLogController(
            IChangeLogService changeLogService)
        {
            this.changeLogService = changeLogService ?? new ChangeLogService();
        }

        public ActionResult Index()
        {
            return View("~/Areas/PicAndHeadline/ChangeLog/Index.cshtml");
        }

        public ActionResult GetHeadlineDAReport(GetHeadlineDALogReportCriteria criteria)
        {
            var report = changeLogService.GetHeadlineDAReport(criteria);
            return Json(report, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPicReport(GetHeadlineDALogReportCriteria criteria)
        {
            var report = changeLogService.GetHeadlineDAReport(criteria);
            return Json(report, JsonRequestBehavior.AllowGet);
        }
    }
}