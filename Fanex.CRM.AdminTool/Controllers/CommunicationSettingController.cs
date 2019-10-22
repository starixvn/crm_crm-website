namespace Fanex.CRM.AdminTool.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.CRM.AdminTool.Criteria.CommunicationToolSetting;
    using Fanex.CRM.AdminTool.Models.CommunicationToolSetting;
    using Fanex.CRM.AdminTool.Services;
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Controllers;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Common.Resources;
    using Fanex.CRM.Setting.Services;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    [NapAuth(FunctionKey = FunctionPermissions.DefaultCommunicationSetting, Policy = Constant.Policy.Read)]
    public class CommunicationSettingController : ReadOnlyBaseController
    {
        private readonly ISettingService settingService;
        private readonly ICommunicationToolSettingService communicationToolSettingService;

        public CommunicationSettingController() : this(null, null)
        {
        }

        public CommunicationSettingController(
            ISettingService settingService,
            ICommunicationToolSettingService communicationToolSettingService)
        {
            this.settingService = settingService ?? new SettingService();
            this.communicationToolSettingService = communicationToolSettingService ?? new CommunicationToolSettingService();
        }

        public ActionResult Index()
        {
            ViewBag.IsHost = HasHostPermission(FunctionPermissions.DefaultCommunicationSetting);
            return View("~/Areas/AdminTool/CommunicationToolSetting/Index.cshtml");
        }

        public JsonResult GetAllSettings()
        {
            var settings = new
            {
                CommunicationTools = settingService.GetAllCommunicationTools(),
                MainCategories = settingService.GetMainCategories(),
                SubCategories = settingService.GetSubCategories()
            };

            return Json(settings, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCommunicationToolSetting(GetCommunicationToolSettingCriteria criteria)
        {
            Tuple<IEnumerable<CommunicationToolSettingItem>, int> tuple = communicationToolSettingService.GetCommunicationToolSetting(criteria);
            return Json(new { Items = tuple.Item1, TotalItems = tuple.Item2 }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermissions.DefaultCommunicationSetting, Policy = Constant.Policy.Host)]
        public ActionResult CreateCommunicationToolSetting(CreateCommunicationToolSettingCriteria criteria)
        {
            CommandResult commandResult = communicationToolSettingService.Create(criteria);
            if (commandResult.IsSuccessful)
            {
                commandResult.Message = Labels.Message_InsertNewSettingSuccessfully;
            }
            return Json(commandResult, JsonRequestBehavior.AllowGet);
        }
    }
}