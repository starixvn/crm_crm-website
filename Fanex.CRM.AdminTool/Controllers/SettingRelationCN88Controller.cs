namespace Fanex.CRM.AdminTool.Controllers
{
    using Fanex.CRM.AdminTool.Criteria.SettingRelation;
    using Fanex.CRM.AdminTool.Services.SettingRelation;
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Controllers;
    using Fanex.CRM.Common.Models;
    using Fanex.NAP.Core.Business;
    using NAP.Core.Web.Attribute;
    using System.Web.Mvc;

    [NapAuth(FunctionKey = FunctionPermissions.SettingRelationManagement, Policy = Constant.Policy.Read)]
    public class SettingRelationCN88Controller : ReadOnlyBaseController
    {
        private readonly ISettingRelationService _settingRelationService;

        public SettingRelationCN88Controller()
            : this(null)
        {
        }

        public SettingRelationCN88Controller(ISettingRelationService settingRelationService)
        {
            _settingRelationService = settingRelationService ?? new SettingRelationService();
        }

        public ActionResult Index()
        {
            return View("~/Areas/AdminTool/SettingRelationCN88/ViewList/Index.cshtml");
        }

        public ActionResult OpenInsertRelationView(int settingId, string settingValue, string settingType)
        {
            return View(
                "~/Areas/AdminTool/SettingRelationCN88/AddRelation/Index.cshtml",
                new Setting
                {
                    Id = settingId,
                    Value = settingValue,
                    SettingType = settingType
                });
        }

        [HttpPost]
        public JsonResult MainSetting()
        {
            var result = _settingRelationService.GetMainSettingCN88(new GetMainSettingCriteria());
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SettingByType(GetSettingByTypeCN88Criteria criteria)
        {
            var result = _settingRelationService.GetSettingByTypeCN88(criteria);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetMainSettingForAddRelation(GetMainSettingForAddRelationCriteria criteria)
        {
            var result = _settingRelationService.GetMainSettingForAddRelations(criteria);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult MainSettingRelations(GetMainSettingRelationsCriteria criteria)
        {
            var result = _settingRelationService.MainSettingRelationsCN88(criteria);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermissions.SettingRelationManagement, Policy = Constant.Policy.Host)]
        public JsonResult AddSettingRelations(AddSettingRelationsCriteria criteria)
        {
            var result = _settingRelationService.GetAddSettingRelations(criteria);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}