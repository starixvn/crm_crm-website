using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Fanex.CRM.Common.Controllers;
using Fanex.CRM.Common.Models;
using Fanex.CRM.Criteria.PicAndHeadline;
using Fanex.CRM.Models.PicAndHeadline;
using Fanex.CRM.Services;
using Fanex.CRM.Setting.Services;

namespace Fanex.CRM.Controllers
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
            CommandResult result = _picReportService.ChangePicState(criteria);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult OpenEditPic(
            int picId,
            string username,
            int roleId,
            string remark,
            IEnumerable<Dictionary<string, string>> communicationTools)
        {
            var comTools = communicationTools == null ?
                new List<PicCommunicationTool> { new PicCommunicationTool { Type = -1, Value = string.Empty } } :
                communicationTools.Select(c => new PicCommunicationTool
                {
                    Type = int.Parse(c["SettingId"]),
                    Value = c["Value"]
                });

            var criteria = new EditPicCriteria
            {
                PicId = picId,
                Username = username,
                RoleId = roleId,
                Remark = remark,
                CommunicationTools = comTools
            };

            return View("~/Areas/PicAndHeadline/PicManagement/EditPic/Index.cshtml", criteria);
        }

        [HttpPost]
        public ActionResult EditPic(EditPicCriteria criteria)
        {
            var result = _picReportService.EditPic(criteria);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}