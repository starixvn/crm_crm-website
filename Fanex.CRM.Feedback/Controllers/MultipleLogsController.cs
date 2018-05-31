namespace Fanex.CRM.Feedback.Controllers
{
    using Common.Constants;
    using Common.Controllers;
    using Criteria;
    using NAP.Core.Business;
    using NAP.Core.Web.Attribute;
    using Services;
    using Setting.Services;
    using System.Web.Mvc;

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

            return View("~/Areas/Feedbacks/EditMultipleLogs/Index.cshtml", log);
        }

        [NapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Host)]
        [HttpPost]
        public ActionResult GetSettings()
        {
            var allSetting = new
            {
                ExtensionList = _serviceSetting.GetAllExtensions(),
                CountryList = _serviceSetting.GetAllCountries(),
                CommunicationToolList = _serviceSetting.GetAllCommunicationTools(),
                LanguageList = _serviceSetting.GetAllLanguages(),
                StatusList = _multipleLogsService.AllStatusForInsert,
                SiteList = _serviceSetting.GetAllSites(),
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
            var allSetting = new
            {
                ExtensionList = _serviceSetting.GetAllExtensions(),
                CountryList = _serviceSetting.GetAllCountries(),
                CommunicationToolList = _serviceSetting.GetAllCommunicationTools(),
                LanguageList = _serviceSetting.GetAllLanguages(),
                StatusList = _multipleLogsService.AllStatusForEdit,
                SiteList = _serviceSetting.GetAllSites(),
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
    }
}
