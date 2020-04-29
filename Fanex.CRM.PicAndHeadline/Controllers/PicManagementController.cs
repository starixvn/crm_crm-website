using Fanex.CRM.Common.Controllers;
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

            return View("~/Areas/PicAndHeadline/PicManagement/Index.cshtml");
        }

        [HttpPost]
        public ActionResult AllSetting()
        {
            var allSetting = new
            {
                RoleList = _settingService.GetPicRoles()                
            };

            return Json(allSetting, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetReport(GetReportCriteria criteria)
        {
            var report = _picReportService.GetReport(criteria);
            return Json(report, JsonRequestBehavior.AllowGet);
        }
    }
}
