namespace Fanex.CRM.Feedback.Controllers
{
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Controllers;
    using Fanex.CRM.Common.Enumerations;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Feedback.Models;
    using Fanex.CRM.Feedback.Resources;
    using Fanex.CRM.Setting.Constants;
    using Fanex.CRM.Setting.Services;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using System.Linq;
    using System.Web.Mvc;

    [NapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Read)]
    public class AddFeedbackController : ReadOnlyBaseController
    {
        public AddFeedbackController()
            : this(null)
        {
        }

        public AddFeedbackController(ISettingService serviceSetting)
        {
            ServiceSetting = serviceSetting ?? new SettingService();
        }

        public ISettingService ServiceSetting { get; set; }

        [HttpPost]
        public ActionResult AllSetting()
        {
            var settings = AllSettingToAddFeedback();
            bool hasCreateFeedbackPermission = HasPermission(FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Constant.Policy.Host);
            if (hasCreateFeedbackPermission)
            {
                settings.StatusList = settings.StatusList.Where(s => !s.Name.Equals("Deleted", System.StringComparison.OrdinalIgnoreCase));
            }

            return Json(settings, JsonRequestBehavior.AllowGet);
        }

        public AllSettingToAddFeedback AllSettingToAddFeedback()
        {
            var languages = ServiceSetting.GetAllLanguages();
            languages.Insert(
                0,
                new Setting
                {
                    Id = 0,
                    Name = Labels.Symbol_NA
                });

            var countries = ServiceSetting.GetAllCountries();
            countries.Insert(
                0,
                new Country
                {
                    Id = 0,
                    Name = Labels.Symbol_NA
                });

            var statusList = ServiceSetting.GetAllStatuses(ViewTypeConstants.CommunicationTool)
                                        .Where(s => s.Id != (int)CRMStatus.Deleted).ToList();
            var settings = new AllSettingToAddFeedback
            {
                ExtensionList = ServiceSetting.GetAllExtensions(),
                CountryList = countries,
                CommunicationToolList = ServiceSetting.GetAllCommunicationTools(),
                LanguageList = languages,
                SiteList = ServiceSetting.GetAllSiteWithGroup(),
                ResponseList = ServiceSetting.GetAllResponses(),
                StatusList = statusList,
                TitleList = ServiceSetting.GetAllTitles(),
                LicenseeType = ServiceSetting.GetAllLicenseeTypes(),
                ProductNA = ServiceSetting.GetProductNA(),
                SubProductNA = ServiceSetting.GetSubProductNA(),
                BettypeNA = ServiceSetting.GetBettypeNA(),
                ExtensionNA = ServiceSetting.GetExtensionNA(),
                ProductList = ServiceSetting.GetProducts(),
                SubProductList = ServiceSetting.GetSubProducts(),
                BettypeList = ServiceSetting.GetBettypes(),
                MainCategoryList = ServiceSetting.GetMainCategories(),
                SubCategoryList = ServiceSetting.GetSubCategories(),
            };

            return settings;
        }
    }
}
