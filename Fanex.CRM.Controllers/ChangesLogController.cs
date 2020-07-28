using System.Collections.Generic;
using System.Web.Mvc;
using Fanex.CRM.Common.Constants;
using Fanex.CRM.Common.Controllers;
using Fanex.CRM.Common.Models;
using Fanex.CRM.Criteria.PicAndHeadline;
using Fanex.CRM.Services;
using Fanex.CRM.Services.Interfaces;
using Fanex.NAP.Core.Business;
using Fanex.NAP.Core.Web.Attribute;

namespace Fanex.CRM.Controllers
{
    //[NapAuth(FunctionKey = FunctionPermissions.ChangesLog, Policy = Constant.Policy.Read)]
    public class ChangesLogController : BaseController
    {
        private readonly IChangesLogService changesLogService;

        public ChangesLogController() : this(null)
        {
        }

        public ChangesLogController(
            IChangesLogService changesLogService)
        {
            this.changesLogService = changesLogService ?? new ChangesLogService();
        }

        public ActionResult Index()
        {
            return View("~/Areas/PicAndHeadline/ChangesLog/Index.cshtml");
        }

        public ActionResult GetHeadlineDAReport(GetHeadlineDALogReportCriteria criteria)
        {
            var report = changesLogService.GetHeadlineDAReport(criteria);
            return Json(report, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPicReport(GetPicLogReportCriteria criteria)
        {
            var report = changesLogService.GetPicReport(criteria);
            return Json(report, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetActionTypes()
        {
            var criteria = new GetActionTypeCriteria();
            var actionTypes = changesLogService.GetActionType(criteria);

            return Json(actionTypes, JsonRequestBehavior.AllowGet);
        }
    }
}