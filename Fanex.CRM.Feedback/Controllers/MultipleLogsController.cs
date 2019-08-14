namespace Fanex.CRM.Feedback.Controllers
{
    using System.Web.Mvc;
    using Common.Constants;
    using Common.Controllers;
    using Common.Enumerations;
    using Common.Services;
    using Criteria;
    using NAP.Core.Business;
    using NAP.Core.Web.Attribute;
    using Services;
    using Setting.Constants;
    using Setting.Services;

    public class MultipleLogsController : ReadOnlyBaseController
    {
        private readonly IMultipleLogsService _multipleLogsService;
        private readonly ISettingService _serviceSetting;

        public MultipleLogsController()
            : this(null, null)
        {
        }

        public MultipleLogsController(
            IMultipleLogsService multipleLogsService,
            ISettingService serviceSetting)
        {
            _multipleLogsService = multipleLogsService ?? new MultipleLogsService();
            _serviceSetting = serviceSetting ?? new SettingService();
        }

        [NapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Host)]
        public ActionResult Create()
        {
            ViewBag.UserRole = AppSession.CurrentUser.UserRole;
            ViewBag.CN88SiteId = SiteConstants.CN88;
            return View("~/Areas/Feedbacks/CreateMultipleLogs/Index.cshtml");
        }

        [NapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Read)]
        public ActionResult Edit(int feedbackId)
        {
            var log = _multipleLogsService.GetLogByFeedbackId(new GetLogByFeedbackIdCriteria
            {
                FeedbackId = feedbackId
            });

            ViewBag.HasUpdateMultipleLogsPermission = HasPermission(FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Constant.Policy.Host);
            ViewBag.HasReviewPermission = HasPermission(FunctionPermissions.ReviewFeedback, Constant.Policy.Host);
            ViewBag.UserRole = AppSession.CurrentUser.UserRole;

            return View("~/Areas/Feedbacks/EditMultipleLogs/Index.cshtml", log);
        }

        [NapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Read)]
        public ActionResult EditStatus(int feedbackId)
        {
            var log = _multipleLogsService.GetLogByFeedbackId(new GetLogByFeedbackIdCriteria
            {
                FeedbackId = feedbackId
            });

            ViewBag.HasUpdateMultipleLogsPermission = HasPermission(FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Constant.Policy.Host);
            ViewBag.HasReviewPermission = HasPermission(FunctionPermissions.ReviewFeedback, Constant.Policy.Host);

            return View("~/Areas/Feedbacks/EditMultipleLogStatuses/Index.cshtml", log);
        }

        [NapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Host)]
        [HttpPost]
        public ActionResult GetSettings()
        {
            var siteList = AppSession.CurrentUser.UserRole == UserRole.CN88Staff
                ? _serviceSetting.GetAllSitesCN88()
                : _serviceSetting.GetAllSites();
            var allSetting = new
            {
                ExtensionList = _serviceSetting.GetAllExtensions(),
                CountryList = _serviceSetting.GetAllCountries(),
                ProvinceList = _serviceSetting.GetAllProvinces(),
                CommunicationToolList = _serviceSetting.GetAllCommunicationTools(),
                LanguageList = _serviceSetting.GetAllLanguages(),
                StatusList = _multipleLogsService.AllStatusForInsert,
                SiteList = siteList,
                RoleList = _serviceSetting.GetAllRoles(),
                ResponseList = _serviceSetting.GetAllResponses(),
                LicenseeType = _serviceSetting.GetAllLicenseeTypes(),
                ProductNA = _serviceSetting.GetProductNA(),
                SubProductNA = _serviceSetting.GetSubProductNA(),
                BettypeNA = _serviceSetting.GetBettypeNA(),
                ExtensionNA = _serviceSetting.GetExtensionNA(),
                ProductList = _serviceSetting.GetProducts(),
                SubProductList = _serviceSetting.GetSubProducts(),
                BettypeList = _serviceSetting.GetBettypes(),
                MainCategoryList = _serviceSetting.GetMainCategories(),
                SubCategoryList = _serviceSetting.GetSubCategories(),
            };

            return Json(allSetting, JsonRequestBehavior.AllowGet);
        }

        [NapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Read)]
        [HttpPost]
        public ActionResult GetEditSettings()
        {
            var siteList = AppSession.CurrentUser.UserRole == UserRole.CN88Staff
               ? _serviceSetting.GetAllSitesCN88()
               : _serviceSetting.GetAllSites();

            var allSetting = new
            {
                ExtensionList = _serviceSetting.GetAllExtensions(),
                CountryList = _serviceSetting.GetAllCountries(),
                ProvinceList = _serviceSetting.GetAllProvinces(),
                CommunicationToolList = _serviceSetting.GetAllCommunicationTools(),
                LanguageList = _serviceSetting.GetAllLanguages(),
                StatusList = _multipleLogsService.AllStatusForEdit,
                SiteList = siteList,
                RoleList = _serviceSetting.GetAllRoles(),
                ResponseList = _serviceSetting.GetAllResponses(),
                LicenseeType = _serviceSetting.GetAllLicenseeTypes(),
                ProductNA = _serviceSetting.GetProductNA(),
                SubProductNA = _serviceSetting.GetSubProductNA(),
                BettypeNA = _serviceSetting.GetBettypeNA(),
                ExtensionNA = _serviceSetting.GetExtensionNA(),
                ProductList = _serviceSetting.GetProducts(),
                SubProductList = _serviceSetting.GetSubProducts(),
                BettypeList = _serviceSetting.GetBettypes(),
                MainCategoryList = _serviceSetting.GetMainCategories(),
                SubCategoryList = _serviceSetting.GetSubCategories(),
            };

            return Json(allSetting, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Host)]
        public ActionResult InsertMultipleLogs(InsertMultipleLogsCriteria criteria)
        {
            var insertResult = _multipleLogsService.InsertMultpleLogs(criteria);

            return Json(insertResult);
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Host)]
        public ActionResult UpdateMultipleLogs(UpdateMultipleLogsCriteria criteria)
        {
            var updateResult = _multipleLogsService.UpdateMultipleLogs(criteria);

            return Json(updateResult);
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Host)]
        public ActionResult UpdateMultipleLogStatuses(UpdateMultipleLogStatusesCriteria criteria)
        {
            var updateResult = _multipleLogsService.UpdateMultipleLogStatuses(criteria);

            return Json(updateResult);
        }
    }
}