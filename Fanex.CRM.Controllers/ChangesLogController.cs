using System;
using System.Linq;
using System.Web.Mvc;
using Fanex.CRM.Common.Constants;
using Fanex.CRM.Common.Controllers;
using Fanex.CRM.Criteria.PicAndHeadline;
using Fanex.CRM.Resources;
using Fanex.CRM.Services;
using Fanex.CRM.Services.Interfaces;
using Fanex.NAP.Core.Business;
using Fanex.NAP.Core.Web.Attribute;

namespace Fanex.CRM.Controllers
{
    [NapAuth(FunctionKey = FunctionPermissions.ChangesLog, Policy = Constant.Policy.Read)]
    public class ChangesLogController : BaseController
    {
        private const int EndHour = 23;
        private const int EndMinuteSecond = 59;
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
            return new RedirectResult(string.Format("{0}/ChangesLog", ExternalDomain), false);
        }

        public ActionResult GetHeadlineDAReport(GetHeadlineDALogReportCriteria criteria)
        {
            criteria.ToDate = criteria.ToDate.AddHours(EndHour).AddMinutes(EndMinuteSecond).AddSeconds(EndMinuteSecond);
            var report = changesLogService.GetHeadlineDAReport(criteria);
            return Json(report, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPicReport(GetPicLogReportCriteria criteria)
        {
            criteria.ToDate = criteria.ToDate.AddHours(EndHour).AddMinutes(EndMinuteSecond).AddSeconds(EndMinuteSecond);
            var report = changesLogService.GetPicReport(criteria);
            return Json(report, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetActionTypes()
        {
            var criteria = new GetActionTypeCriteria();
            var actionTypes = changesLogService.GetActionType(criteria);
            return Json(actionTypes, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetLogActions()
        {
            var logActions = Enum.GetValues(typeof(LogAction)).Cast<LogAction>().Select(action => $"\"{action.ToString()}\": {(int)action}");
            var result = $"{{ {string.Join(", ", logActions)} }}";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}