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
    using Fanex.CRM.AdminTool.Services.SettingManagement;
    using Fanex.CRM.AdminTool.Criteria.FeedbackSubjectManagement;
    using Fanex.CRM.AdminTool.Services.FeedbackSubjectManagement;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Infrastructure.Web.Controllers;
    using Fanex.CRM.Setting.Models;

    /// <summary>
    /// Setting Management Controller.
    /// </summary>
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

        [AuthenticateAction(AuthenticateAction.AuthenticationType.Read, "Index")]
        public ActionResult Index()
        {
            return View("~/Areas/AdminTool/SettingManagement/ViewList/Index.cshtml");
        }

        [AuthenticateAction(AuthenticateAction.AuthenticationType.Read, "Index")]
        public ActionResult OpenInsertNewSettingView()
        {
            return View("~/Areas/AdminTool/SettingManagement/InsertSetting/Index.cshtml");
        }

        [AuthenticateAction(AuthenticateAction.AuthenticationType.Read, "Index")]
        public ActionResult OpenInsertSubSettingView(string settingType)
        {
            return View(
                "~/Areas/AdminTool/SettingManagement/InsertSubSetting/Index.cshtml",
                 new SettingInfo
                {
                    Type = settingType
                });
        }

        [AuthenticateAction(AuthenticateAction.AuthenticationType.Read, "Index")]
        public ActionResult OpenDeleteSettingView(
            string settingType,
            int settingId,
            string settingValue)
        {
            return View(
                "~/Areas/AdminTool/SettingManagement/DeleteSetting/Index.cshtml",
                new SettingInfo
                {
                    Type = settingType,
                    Id = settingId,
                    Value = settingValue
                });
        }

        [AuthenticateAction(AuthenticateAction.AuthenticationType.Read, "Index")]
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
        [AuthenticateAction(AuthenticateAction.AuthenticationType.Read, "Index")]
        public JsonResult GetMainSetting()
        {
            var result = _settingManagementService.GetMainSetting();
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        [AuthenticateAction(AuthenticateAction.AuthenticationType.Read, "Index")]
        public JsonResult GetParentSetting()
        {
            var result = _settingManagementService.GetParentSetting();
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        [AuthenticateAction(AuthenticateAction.AuthenticationType.Read, "Index")]
        public JsonResult SettingByType(GetSettingByTypeCriteria criteria)
        {
            var result = _settingManagementService.SettingByType(criteria);
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        [AuthenticateAction(AuthenticateAction.AuthenticationType.Host, "Index")]
        public JsonResult InsertSetting(InsertSettingCriteria criteria)
        {
            var result = _settingManagementService.InsertSetting(criteria);
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        [AuthenticateAction(AuthenticateAction.AuthenticationType.Host, "Index")]
        public JsonResult DeleteSetting(DeleteSettingCriteria criteria)
        {
            Result result = _settingManagementService.DeleteSetting(criteria);
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        [AuthenticateAction(AuthenticateAction.AuthenticationType.Host, "Index")]
        public JsonResult EditSetting(EditSettingCriteria criteria)
        {
            var result = _settingManagementService.EditSetting(criteria);
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }
    }
}
