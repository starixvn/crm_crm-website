namespace Fanex.CRM.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Controllers;
    using Fanex.CRM.Common.Enumerations;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Common.Services;
    using Fanex.CRM.Criteria.MultipleLogs;
    using Fanex.CRM.Models;
    using Fanex.CRM.Models.MultipleLogs;
    using Fanex.CRM.Resources;
    using Fanex.CRM.Services;
    using Fanex.CRM.Setting.Constants;
    using Fanex.CRM.Setting.Services;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    public class MultipleLogsController : ReadOnlyBaseController
    {
        private readonly IMultipleLogsService multipleLogsService;
        private readonly ISettingService serviceSetting;

        public MultipleLogsController()
            : this(null, null)
        {
        }

        public MultipleLogsController(
            IMultipleLogsService multipleLogsService,
            ISettingService serviceSetting)
        {
            this.multipleLogsService = multipleLogsService ?? new MultipleLogsService();
            this.serviceSetting = serviceSetting ?? new SettingService();
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Host)]
        public ActionResult VerifyCustomer(VerifyCustomerCriteria criteria)
        {
            IEnumerable<VerifyCustomerResult> customers = multipleLogsService.VerifyCustomer(criteria);
            return Json(customers, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Host)]
        public ActionResult VerifyRelatedCustomer(VerifyRelatedCustomerCriteria criteria)
        {
            IEnumerable<VerifyRelatedCustomerResult> customers = multipleLogsService.VerifyRelatedCustomer(criteria);
            return Json(customers, JsonRequestBehavior.AllowGet);
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
            var log = multipleLogsService.GetLogByFeedbackId(new GetLogByFeedbackIdCriteria
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
            var log = multipleLogsService.GetLogByFeedbackId(new GetLogByFeedbackIdCriteria
            {
                FeedbackId = feedbackId
            });

            ViewBag.HasUpdateMultipleLogsPermission = HasPermission(FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Constant.Policy.Host);
            ViewBag.HasReviewPermission = HasPermission(FunctionPermissions.ReviewFeedback, Constant.Policy.Host);

            return View("~/Areas/Feedbacks/EditMultipleLogStatuses/Index.cshtml", log);
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Host)]
        public ActionResult GetSettings()
        {
            IEnumerable<BaseModel> statues = serviceSetting.GetAllStatuses(ViewTypeConstants.CommunicationTool);
            var allSetting = new
            {
                ExtensionList = serviceSetting.GetAllExtensions(),
                CommunicationToolList = serviceSetting.GetAllCommunicationTools().Where(x => x.IsActive),
                ProvinceList = serviceSetting.GetAllProvinces(),
                StatusList = statues.Where(s => !string.Equals(s.Name, Labels.Deleted, StringComparison.OrdinalIgnoreCase)),
                RoleList = serviceSetting.GetAllRoles().Where(x => x.IsActive),
                ResponseList = serviceSetting.GetAllResponses().Where(x => x.IsActive),
                ProductNA = serviceSetting.GetProductNA(),
                SubProductNA = serviceSetting.GetSubProductNA(),
                BettypeNA = serviceSetting.GetBettypeNA(),
                ExtensionNA = serviceSetting.GetExtensionNA(),
                ProductList = serviceSetting.GetProducts().Where(x => x.IsActive),
                SubProductList = serviceSetting.GetSubProducts().Where(x => x.IsActive),
                BettypeList = serviceSetting.GetBettypes().Where(x => x.IsActive),
                MainCategoryList = serviceSetting.GetMainCategories().Where(x => x.IsActive),
                SubCategoryList = serviceSetting.GetSubCategories().Where(x => x.IsActive),
            };

            return Json(allSetting, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Read)]
        public ActionResult GetEditSettings()
        {
            var siteList = AppSession.CurrentUser.UserRole == UserRole.CN88Staff
               ? serviceSetting.GetAllSitesCN88()
               : serviceSetting.GetAllSites();

            var allSetting = new
            {
                ExtensionList = serviceSetting.GetAllExtensions(),
                CountryList = serviceSetting.GetAllCountries().Where(x => x.IsActive),
                ProvinceList = serviceSetting.GetAllProvinces(),
                CommunicationToolList = serviceSetting.GetAllCommunicationTools().Where(x => x.IsActive),
                LanguageList = serviceSetting.GetAllLanguages().Where(x => x.IsActive),
                StatusList = serviceSetting.GetAllStatuses(ViewTypeConstants.CommunicationTool),
                SiteList = siteList,
                RoleList = serviceSetting.GetAllRoles().Where(x => x.IsActive),
                ResponseList = serviceSetting.GetAllResponses().Where(x => x.IsActive),
                LicenseeType = serviceSetting.GetAllLicenseeTypes(),
                ProductNA = serviceSetting.GetProductNA(),
                SubProductNA = serviceSetting.GetSubProductNA(),
                BettypeNA = serviceSetting.GetBettypeNA(),
                ExtensionNA = serviceSetting.GetExtensionNA(),
                ProductList = serviceSetting.GetProducts().Where(x => x.IsActive),
                SubProductList = serviceSetting.GetSubProducts().Where(x => x.IsActive),
                BettypeList = serviceSetting.GetBettypes().Where(x => x.IsActive),
                MainCategoryList = serviceSetting.GetMainCategories().Where(x => x.IsActive),
                SubCategoryList = serviceSetting.GetSubCategories().Where(x => x.IsActive)
            };

            return Json(allSetting, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Host)]
        public ActionResult CheckNotExistDefaultCommunicationToolSetting(CheckNotExistCommunicationToolSettingCriteria criteria)
        {
            IEnumerable<VerifyCustomer> customers = multipleLogsService.CheckNotExistCommunicationToolSetting(criteria);
            return Json(customers, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Host)]
        public ActionResult CreateMultipleLogs(CreateMultipleLogsCriteria criteria)
        {
            CommandResult commandResult = multipleLogsService.Create(criteria);
            return Json(commandResult);
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Host)]
        public ActionResult UpdateMultipleLogs(UpdateMultipleLogsCriteria criteria)
        {
            CommandResult commandResult = multipleLogsService.Update(criteria);
            return Json(commandResult);
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Host)]
        public ActionResult UpdateMultipleLogStatuses(UpdateMultipleLogStatusCriteria criteria)
        {
            CommandResult commandResult = multipleLogsService.UpdateMultipleLogStatus(criteria);
            return Json(commandResult);
        }
    }
}