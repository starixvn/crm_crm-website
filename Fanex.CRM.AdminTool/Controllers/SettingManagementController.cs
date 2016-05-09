// --------------------------------------------------------------------------
// <copyright file="SettingManagementController.cs" company="Nexcel Solutions Vietnam">
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
    using Fanex.CRM.AdminTool.Criteria.SettingManagement;
    using Fanex.CRM.AdminTool.Resources;
    using Fanex.CRM.AdminTool.Services.SettingManagement;
    using Fanex.CRM.Common.Attributes;
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Controllers;
    using Fanex.CRM.Common.Enumerations;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.NAP.Core.Business;
    using ONEbook.UM.Filters;

    /// <summary>
    /// Setting Management Controller.
    /// </summary>
    [CheckUMFilter]
    public class SettingManagementController : BaseController
    {
        private ISettingManagementService _settingManagementService;

        public SettingManagementController()
            : this(null)
        {
        }

        public SettingManagementController(ISettingManagementService settingManagementService)
        {
            _settingManagementService = settingManagementService ?? new SettingManagementService();
        }

        [AuthenticateAction(AuthenticationType.Read, "Index")]
        public ActionResult Index()
        {
            return View("~/Areas/AdminTool/SettingManagement/ViewList/Index.cshtml");
        }

        [AuthenticateAction(AuthenticationType.Read, "Index")]
        public ActionResult OpenInsertNewSettingView()
        {
            return View("~/Areas/AdminTool/SettingManagement/InsertSetting/Index.cshtml");
        }

        [AuthenticateAction(AuthenticationType.Read, "Index")]
        public ActionResult OpenInsertSubSettingView(string settingType)
        {
            return View(
                "~/Areas/AdminTool/SettingManagement/InsertSubSetting/Index.cshtml",
                 new Setting
                {
                    SettingType = settingType
                });
        }

        [AuthenticateAction(AuthenticationType.Read, "Index")]
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

        [AuthenticateAction(AuthenticationType.Read, "Index")]
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
        [AuthenticateAction(AuthenticationType.Read, "Index")]
        public JsonResult MainSetting()
        {
            var result = _settingManagementService.GetMainSettings();
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        [AuthenticateAction(AuthenticationType.Read, "Index")]
        public JsonResult ParentSetting()
        {
            var result = _settingManagementService.GetParentSettings();
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        [AuthenticateAction(AuthenticationType.Read, "Index")]
        public JsonResult SettingByType(GetSettingByTypeCriteria criteria)
        {
            var result = _settingManagementService.SettingByType(criteria);
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        [AjaxNapAuth(FunctionKey = FunctionPermissions.SettingManagement, Policy = Constant.Policy.Host)]
        public JsonResult InsertSetting(InsertSettingCriteria criteria)
        {
            var result = _settingManagementService.InsertSetting(criteria);
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        [AjaxNapAuth(FunctionKey = FunctionPermissions.SettingManagement, Policy = Constant.Policy.Host)]
        public JsonResult DeleteSetting(DeleteSettingCriteria criteria)
        {
            Result result = _settingManagementService.DeleteSetting(criteria);
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        [AjaxNapAuth(FunctionKey = FunctionPermissions.SettingManagement, Policy = Constant.Policy.Host)]
        public JsonResult EditSetting(EditSettingCriteria criteria)
        {
            var result = _settingManagementService.EditSetting(criteria);
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }
    }
}
