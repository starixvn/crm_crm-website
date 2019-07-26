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
    public class SettingManagementCN88Controller : ReadOnlyBaseController
    {
        private readonly ISettingManagementService _settingManagementService;

        public SettingManagementCN88Controller()
            : this(null)
        {
        }

        public SettingManagementCN88Controller(ISettingManagementService settingManagementService)
        {
            _settingManagementService = settingManagementService ?? new SettingManagementService();
        }

        public ActionResult Index()
        {
            return View("~/Areas/AdminTool/SettingManagementCN88/ViewList/Index.cshtml");
        }

        public ActionResult OpenInsertNewSettingView()
        {
            return View("~/Areas/AdminTool/SettingManagementCN88/InsertSetting/Index.cshtml");
        }

        public ActionResult OpenInsertSubSettingView(GetSettingNameCriteria criteria)
        {
            var settingName = _settingManagementService.GetSettingName(criteria);
            return View("~/Areas/AdminTool/SettingManagementCN88/InsertSubSetting/Index.cshtml", settingName);
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
                "~/Areas/AdminTool/SettingManagementCN88/DeleteSetting/Index.cshtml",
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
                "~/Areas/AdminTool/SettingManagementCN88/EditSetting/Index.cshtml",
                _settingManagementService.GetSettingCN88ById(new GetSettingCN88ByIdCriteria
                {
                    SettingId = settingId
                }));
        }

        [HttpPost]
        public JsonResult MainSetting()
        {
            var result = _settingManagementService.GetMainSettingCN88(new GetMainSettingCriteria());
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ParentSetting()
        {
            var result = _settingManagementService.GetParentSettings();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SettingByType(GetSettingByTypeCN88Criteria criteria)
        {
            var result = _settingManagementService.GetSettingByTypeCN88(criteria);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermissions.SettingManagement, Policy = Constant.Policy.Host)]
        public JsonResult InsertSetting(InsertSettingCN88Criteria criteria)
        {
            var result = _settingManagementService.InsertSettingCN88(criteria);
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
        public JsonResult EditSetting(EditSettingCN88Criteria criteria)
        {
            var result = _settingManagementService.EditSettingCN88(criteria);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}