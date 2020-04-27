namespace Fanex.CRM.AdminTool.Controllers
{
    using System.Web.Mvc;
    using Fanex.CRM.AdminTool.Criteria.SettingManagement;
    using Fanex.CRM.AdminTool.Resources;
    using Fanex.CRM.AdminTool.Services.SettingManagement;
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Controllers;
    using Fanex.CRM.Common.Models;
    using Fanex.NAP.Core.Business;
    using NAP.Core.Web.Attribute;

    [NapAuth(FunctionKey = FunctionPermissions.SettingManagement, Policy = Constant.Policy.Read)]
    public class SettingManagementController : ReadOnlyBaseController
    {
        private readonly ISettingManagementService _settingManagementService;

        public SettingManagementController()
            : this(null)
        {
        }

        public SettingManagementController(ISettingManagementService settingManagementService)
        {
            _settingManagementService = settingManagementService ?? new SettingManagementService();
        }

        public ActionResult Index()
        {
            return View("~/Areas/AdminTool/SettingManagement/Index.cshtml");
        }

        public ActionResult OpenInsertNewSettingView()
        {
            return View("~/Areas/AdminTool/SettingManagement/InsertSetting/Index.cshtml");
        }

        public ActionResult OpenInsertSubSettingView(string settingType)
        {
            return View(
                "~/Areas/AdminTool/SettingManagement/InsertSubSetting/Index.cshtml",
                 new Setting
                 {
                     SettingType = settingType
                 });
        }

        public ActionResult OpenDeleteSettingView(
            string settingType,
            int settingId,
            string settingValue,
            bool isActive)
        {
            var messageConfirm = string.Format(Labels.SettingManagement_ActiveSetting_Message, settingType, settingValue);

            if (isActive)
            {
                messageConfirm = string.Format(Labels.SettingManagement_DeactiveSetting_Message, settingType, settingValue);
            }

            ViewBag.Message = messageConfirm;

            return View(
                "~/Areas/AdminTool/SettingManagement/DeleteSetting/Index.cshtml",
                new Setting
                {
                    SettingType = settingType,
                    Id = settingId,
                    Value = settingValue,
                    IsActive = isActive
                });
        }

        public ActionResult OpenEditSettingView(int settingId)
        {
            return View(
                "~/Areas/AdminTool/SettingManagement/EditSetting/Index.cshtml",
                _settingManagementService.GetSettingById(new GetSettingByIdCriteria
                {
                    SettingId = settingId
                }));
        }

        [HttpPost]
        public JsonResult MainSetting()
        {
            var result = _settingManagementService.GetMainSettings();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ParentSetting()
        {
            var result = _settingManagementService.GetParentSettings();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SettingByType(GetSettingByTypeCriteria criteria)
        {
            var result = _settingManagementService.SettingByType(criteria);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermissions.SettingManagement, Policy = Constant.Policy.Host)]
        public JsonResult InsertSetting(InsertSettingCriteria criteria)
        {
            var result = _settingManagementService.InsertSetting(criteria);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermissions.SettingManagement, Policy = Constant.Policy.Host)]
        public JsonResult DeleteSetting(DeleteSettingCriteria criteria)
        {
            CommandResult result = _settingManagementService.DeleteSetting(criteria);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermissions.SettingManagement, Policy = Constant.Policy.Host)]
        public JsonResult EditSetting(EditSettingCriteria criteria)
        {
            var result = _settingManagementService.EditSetting(criteria);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}