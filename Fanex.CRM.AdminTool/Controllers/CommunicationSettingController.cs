namespace Fanex.CRM.AdminTool.Controllers
{
    using System.Web.Mvc;
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Controllers;
    using Fanex.CRM.Setting.Services;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    [NapAuth(FunctionKey = FunctionPermissions.DefaultCommunicationSetting, Policy = Constant.Policy.Read)]
    public class CommunicationSettingController : ReadOnlyBaseController
    {
        private readonly ISettingService serviceSetting;

        public CommunicationSettingController() : this(null)
        {
        }

        public CommunicationSettingController(ISettingService serviceSetting)
        {
            this.serviceSetting = serviceSetting ?? new SettingService();
        }

        public ActionResult Index()
        {
            ViewBag.IsHost = HasHostPermission(FunctionPermissions.DefaultCommunicationSetting);
            return View("~/Areas/AdminTool/CommunicationToolSetting/Index.cshtml");
        }

        public ActionResult GetAllSettings()
        {
            var settings = new
            {
                CommunicationTools = serviceSetting.GetAllCommunicationTools(),
                MainCategories = serviceSetting.GetMainCategories(),
                SubCategories = serviceSetting.GetSubCategories(),
            };

            return Json(settings, JsonRequestBehavior.AllowGet);
        }
    }
}