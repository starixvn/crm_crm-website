using System.Linq;
using System.Web.Mvc;
using Fanex.CRM.Common.Constants;
using Fanex.CRM.Common.Controllers;
using Fanex.CRM.Common.Models;
using Fanex.CRM.Criteria.PicAndHeadline;
using Fanex.CRM.Report.Excels.PICManagement;
using Fanex.CRM.Report.Resources;
using Fanex.CRM.Services;
using Fanex.CRM.Setting.Constants;
using Fanex.CRM.Setting.Services;
using Fanex.NAP.Core.Business;
using Fanex.NAP.Core.Web.Attribute;

namespace Fanex.CRM.Controllers
{
    [NapAuth(FunctionKey = FunctionPermissions.PICManagement, Policy = Constant.Policy.Read)]
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
            ViewBag.IsHost = HasHostPermission(FunctionPermissions.PICManagement);
            return View("~/Areas/PicAndHeadline/PicManagement/Index.cshtml");
        }

        [HttpPost]
        public ActionResult AllSetting()
        {
            var allSetting = new
            {
                RoleListWithAll = _settingService.GetPicSettings(SettingTypeConstants.PicRole, true, true),
                RoleList = _settingService.GetPicSettings(SettingTypeConstants.PicRole, true),
                CommunicationTools = _settingService.GetPicSettings(SettingTypeConstants.PicCommunicationTool, true),
                Languages = _settingService.GetPicSettings(SettingTypeConstants.Language, true),
                Countries = _settingService.GetAllCountries()
            };

            return Json(allSetting, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetReport(GetReportCriteria criteria)
        {
            var report = _picReportService.GetReport(criteria);
            return Json(report, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public void ExportExcel(GetReportCriteria criteria)
        {
            criteria.PageIndex = 0;
            criteria.PageSize = 0;
            var communicationTools = _settingService.GetPicSettings(SettingTypeConstants.PicCommunicationTool, true).Select(x => new BaseModel { Id = x.Id, Name = x.Value });
            var report = _picReportService.GetReport(criteria);
            var excel = new PICManagementExcel(report, communicationTools, Labels.PICManagement);
            excel.BuildWorkBook();
        }

        [HttpPost]
        public ActionResult InsertPic(InsertPicCriteria criteria)
        {
            var result = _picReportService.InsertPic(criteria);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ChangePicState(ChangePicStateCriteria criteria)
        {
            CommandResult result = _picReportService.ChangePicState(criteria);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EditPic(EditPicCriteria criteria)
        {
            var result = _picReportService.EditPic(criteria);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}