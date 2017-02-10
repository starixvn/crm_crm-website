namespace Fanex.CRM.Communication.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Common.Constants;
    using Common.Controllers;
    using Common.Enumerations;
    using Criteria;
    using Infrastructure.Constants;
    using Infrastructure.Models;
    using NAP.Core.Business;
    using NAP.Core.Web.Attribute;
    using Resources;
    using Services;
    using Setting.Services;

    public class CreateMultipleLogsController : ReadOnlyBaseController
    {
        private readonly ICommunicationLogService _communicationServiceLog;

        private readonly ISettingService _serviceSetting;

        public CreateMultipleLogsController()
            : this(null, null)
        {
        }

        public CreateMultipleLogsController(
            ICommunicationLogService communicationServiceLog,
            ISettingService serviceSetting)
        {
            _communicationServiceLog = communicationServiceLog ?? new CommunicationLogService();
            _serviceSetting = serviceSetting ?? new SettingService();
        }

        [NapAuth(FunctionKey = FunctionPermissions.CreateMultipleLogs, Policy = Constant.Policy.Host)]
        public ActionResult Index()
        {
            return View("~/Areas/Communication/CreateMultipleLogs/Index.cshtml");
        }

        [NapAuth(FunctionKey = FunctionPermissions.CreateMultipleLogs, Policy = Constant.Policy.Host)]
        [HttpPost]
        public ActionResult GetSettings()
        {
            var allSetting = new
            {
                ExtensionList = _serviceSetting.GetAllExtensions(),
                CountryList = _serviceSetting.GetAllCountries(),
                CommunicationToolList = _serviceSetting.GetAllCommunicationTools(),
                LanguageList = _serviceSetting.GetAllLanguages(),
                MainCategoryList = _serviceSetting.GetAllMainCategories(),
                ProductList = _serviceSetting.GetAllProducts(),
                StatusList = _communicationServiceLog.AllStatusForInsert,
                SiteList = _serviceSetting.GetAllSites(),
                RoleList = _serviceSetting.GetAllRoles(),
                ResponseList = _serviceSetting.GetAllResponses(),
                LicenseeType = _serviceSetting.GetAllLicenseeTypes()
            };

            return Json(allSetting, JsonRequestBehavior.AllowGet);
        }
    }
}
