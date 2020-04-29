namespace Fanex.CRM.Controllers
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Controllers;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Models.LicenseeMapping;
    using Fanex.CRM.Services;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    [NapAuth(FunctionKey = FunctionPermissions.SettingManagement, Policy = Constant.Policy.Host)]
    public class LicenseeMappingController : ReadOnlyBaseController
    {
        private readonly ILicenseeMappingService licenseeMappingService;

        public LicenseeMappingController() : this(null)
        {
        }

        public LicenseeMappingController(ILicenseeMappingService licenseeMappingService)
        {
            this.licenseeMappingService = licenseeMappingService ?? new LicenseeMappingService();
        }

        [HttpPost]
        public ActionResult GetLicensee()
        {
            IEnumerable<string> licensees = licenseeMappingService.GetLicensee();
            return Json(licensees);
        }

        [HttpPost]
        public ActionResult GetLicenseeManager(string username)
        {
            IEnumerable<LicenseeManager> licensees = licenseeMappingService.GetLicenseeManager(username);
            return Json(licensees);
        }

        [HttpPost]
        public ActionResult GetLicenseeMapping(string keyword = null)
        {
            IEnumerable<LicenseeMapping> licenseeMappings = licenseeMappingService.GetLicenseeMapping(keyword);
            return Json(licenseeMappings);
        }

        [HttpPost]
        public ActionResult UpdateLicenseeMapping(UpdateLicenseeMappingParam param)
        {
            CommandResult commandResult = licenseeMappingService.UpdateLicenseeMapping(param);
            return Json(commandResult);
        }
    }
}