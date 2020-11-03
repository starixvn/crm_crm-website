namespace Fanex.CRM.Setting.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Common.Enumerations;
    using Common.Services;
    using Fanex.CRM.Common.Controllers;
    using Fanex.CRM.Setting.Constants;
    using Fanex.CRM.Setting.Criteria;
    using Fanex.CRM.Setting.Services;

    public class SettingController : ReadOnlyBaseController
    {
        private readonly ISettingService _settingServiceInstance;

        public SettingController() : this(null)
        {
        }

        public SettingController(ISettingService settingServiceInstance)
        {
            _settingServiceInstance = settingServiceInstance ?? new SettingService();
        }

        [HttpPost]
        public JsonResult AllSite()
        {
            var userRole = AppSession.CurrentUser.UserRole;
            var allSite = userRole == UserRole.CN88Staff
                ? _settingServiceInstance.GetAllSitesCN88()
                : _settingServiceInstance.GetAllSites();
            return Json(allSite, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AllTitle()
        {
            var allTitle = _settingServiceInstance.GetAllTitles();
            return Json(allTitle, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AllCountry()
        {
            var allCountry = _settingServiceInstance.GetAllCountries().Where(x => x.IsActive);
            return Json(allCountry, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AllProvince()
        {
            var allProvince = _settingServiceInstance.GetAllProvinces();

            return Json(allProvince, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AllLanguage()
        {
            var allLanguage = _settingServiceInstance.GetAllLanguages().Where(x => x.IsActive);
            return Json(allLanguage, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AllExtension()
        {
            var allExtension = _settingServiceInstance.GetAllExtensions();
            return Json(allExtension, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AllCommunicationTool()
        {
            var allCommunicationTool = _settingServiceInstance.GetAllCommunicationTools().Where(x => x.IsActive);
            return Json(allCommunicationTool, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AllMainCategory()
        {
            var allCategory = _settingServiceInstance.GetAllMainCategories().Where(x => x.IsActive);
            return Json(allCategory, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AllProduct()
        {
            var allProduct = _settingServiceInstance.GetAllSubProducts();
            return Json(allProduct, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AllStatus()
        {
            var allStatus = _settingServiceInstance.GetAllStatuses(ViewTypeConstants.All);
            return Json(allStatus, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AllRole()
        {
            var allRole = _settingServiceInstance.GetAllRoles();
            return Json(allRole, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AllResponse()
        {
            var allResponse = _settingServiceInstance.GetAllResponses();
            return Json(allResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult NumberOfFeedbackSubjectTemplate()
        {
            var numberOfFeedbackSubjectTemplate = _settingServiceInstance.NumberOfFeedbackSubjectTemplate();
            return Json(numberOfFeedbackSubjectTemplate, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetFeedbackSubjectTemplateByPagingIndex(GetFeedbackSubjectTemplateByPagingIndexCriteria criteria)
        {
            var feedbackSubjectTemplateByPagingIndex = _settingServiceInstance.GetFeedbackSubjectTemplateByPagingIndex(criteria);
            return Json(feedbackSubjectTemplateByPagingIndex, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAllLogAction(GetActionCriteria criteria)
        {
            var actions = _settingServiceInstance.GetAllLogActions(criteria);
            return Json(actions, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetSourceSettingMappings(GetSourceSettingMappingCriteria criteria)
        {
            var sourceMappings = _settingServiceInstance.GetSettingMappings(criteria);
            return Json(sourceMappings, JsonRequestBehavior.AllowGet);
        }
    }
}