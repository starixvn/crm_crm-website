using Fanex.CRM.Common.Controllers;
using Fanex.CRM.Common.Models;
using Fanex.CRM.PicAndHeadline.Criteria;
using Fanex.CRM.PicAndHeadline.Services;
using Fanex.CRM.Setting.Services;
using System.Web.Mvc;

namespace Fanex.CRM.PicAndHeadline.Controllers
{
    public class PicManagementController : BaseController
    {
        private readonly ISettingService _settingService;
        private readonly IPicReportService _picReportService;

        public PicManagementController()
           : this(null, null)
        {
        }

        public PicManagementController(
            ISettingService settingService,
            IPicReportService picReportService)
        {
            _settingService = settingService ?? new SettingService();
            _picReportService = picReportService ?? new PicReportService();
        }

        public ActionResult Index()
        {

            return View("~/Areas/PicAndHeadline/PicManagement/ViewList/Index.cshtml");
        }

        [HttpPost]
        public ActionResult AllSetting()
        {
            var allSetting = new
            {
                RoleListWithAll = _settingService.GetPicRoles(true),
                RoleList = _settingService.GetPicRoles(),
                CommunicationTools = _settingService.GetAllCommunicationTools()
            };

            return Json(allSetting, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetReport(GetReportCriteria criteria)
        {
            var report = _picReportService.GetReport(criteria);
            return Json(report, JsonRequestBehavior.AllowGet);
        }

        public ActionResult OpenInsertPic()
        {
            return View("~/Areas/PicAndHeadline/PicManagement/InsertPic/Index.cshtml");
        }

        [HttpPost]
        public ActionResult InsertPic(InsertPicCriteria criteria)
        {
            var result = _picReportService.InsertPic(criteria);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult OpenChangePicState(ChangePicStateCriteria criteria)
        {
            return View("~/Areas/PicAndHeadline/PicManagement/ChangePicState/Index.cshtml", criteria);
        }

        [HttpPost]
        public JsonResult ChangePicState(ChangePicStateCriteria criteria)
        {
           // CommandResult result = _feedbackSubjectManagementService.ChangeTemplateState(criteria);
            return Json(new CommandResult() { Message = "Ok", Code = 0 }, JsonRequestBehavior.AllowGet);
        }
    }
}
