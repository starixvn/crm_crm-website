// --------------------------------------------------------------------------
// <copyright file="SettingRelationController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.06.03">Created</change>
// </history>
// --------------------------------------------------------------------------

namespace Fanex.CRM.AdminTool.Controllers
{
    using System.Web.Mvc;
    using Fanex.CRM.AdminTool.Criteria.SettingRelation;
    using Fanex.CRM.AdminTool.Services.SettingRelation;
    using Fanex.CRM.Common.Attributes;
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Controllers;
    using Fanex.CRM.Common.Enumerations;
    using Fanex.CRM.Common.Models;
    using Fanex.NAP.Core.Business;

    /// <summary>
    /// Setting Relation Controller.
    /// </summary>
    public class SettingRelationController : ReadOnlyBaseController
    {
        private ISettingRelationService _settingRelationService;

        public SettingRelationController()
            : this(null)
        {
        }

        public SettingRelationController(ISettingRelationService settingRelationService)
        {
            _settingRelationService = settingRelationService ?? new SettingRelationService();
        }

        [AuthenticateAction(AuthenticationType.Read, "Index")]
        public ActionResult Index()
        {
            return View("~/Areas/AdminTool/SettingRelation/ViewList/Index.cshtml");
        }

        [AuthenticateAction(AuthenticationType.Read, "Index")]
        public ActionResult OpenInsertRelationView(int settingId, string settingValue, string settingType)
        {
            return View(
                "~/Areas/AdminTool/SettingRelation/AddRelation/Index.cshtml",
                new Setting
                {
                    Id = settingId,
                    Value = settingValue,
                    SettingType = settingType
                });
        }

        [HttpPost]
        [AuthenticateAction(AuthenticationType.Read, "Index")]
        public JsonResult MainSetting()
        {
            var result = _settingRelationService.GetMainSettings();
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        [AuthenticateAction(AuthenticationType.Read, "Index")]
        public JsonResult SettingByType(GetSettingByTypeCriteria criteria)
        {
            var result = _settingRelationService.GetSettingByTypes(criteria);
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        [AuthenticateAction(AuthenticationType.Read, "Index")]
        public JsonResult GetMainSettingForAddRelation(GetMainSettingForAddRelationCriteria criteria)
        {
            var result = _settingRelationService.GetMainSettingForAddRelations(criteria);
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        [AuthenticateAction(AuthenticationType.Read, "Index")]
        public JsonResult MainSettingRelations(GetMainSettingRelationsCriteria criteria)
        {
            var result = _settingRelationService.MainSettingRelations(criteria);
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        [AjaxNapAuth(FunctionKey = FunctionPermissions.SettingRelationManagement, Policy = Constant.Policy.Host)]
        public JsonResult AddSettingRelations(AddSettingRelationsCriteria criteria)
        {
            var result = _settingRelationService.GetAddSettingRelations(criteria);
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }
    }
}
