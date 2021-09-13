namespace Fanex.CRM.Feedback.Controllers
{
    using Common.Services;
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

        public ISettingService ServiceSetting { get; set; }

        public AddFeedbackController(ISettingService serviceSetting)
        {
            ServiceSetting = serviceSetting ?? new SettingService();
        }


        [HttpPost]
        public ActionResult AllSetting()
        {
            var settings = AllSettingToAddFeedback(UserRole.NormalStaff);
            bool hasCreateFeedbackPermission = HasPermission(FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Constant.Policy.Host);
            if (hasCreateFeedbackPermission)
            {
                settings.StatusList = settings.StatusList.Where(s => !s.Name.Equals("Deleted", System.StringComparison.OrdinalIgnoreCase));
            }

            return Json(settings, JsonRequestBehavior.AllowGet);
        }

        public AllSettingToAddFeedback AllSettingToAddFeedback(UserRole userRole)
        {
            var languages = ServiceSetting.GetAllLanguages().Where(x => x.IsActive).ToList();
            languages.Insert(
                0,
                new Setting
                {
                    Id = 0,
                    Name = Labels.Symbol_NA,
                    IsActive = true
                });

            var countries = ServiceSetting.GetAllCountries().Where(x => x.IsActive).ToList();
            if (AppSession.CurrentUser.UserRole != UserRole.CN88Staff)
            {
                countries.Insert(
                    0,
                    new Country
                    {
                        Id = 0,
                        Name = Labels.Symbol_NA,
                        IsActive = true
                    });
            }

            var statusList = ServiceSetting.GetAllStatuses(ViewTypeConstants.CommunicationTool)
                                        .Where(s => s.Id != (int)CRMStatus.Deleted).ToList();
            var settings = new AllSettingToAddFeedback
            {
                ExtensionList = ServiceSetting.GetAllExtensions(),
                CountryList = countries.Where(x => x.IsActive),
                ProvinceList = ServiceSetting.GetAllProvinces(),
                CommunicationToolList = ServiceSetting.GetAllCommunicationTools().Where(x => x.IsActive),
                LanguageList = languages.Where(x => x.IsActive),
                SiteList = (userRole == UserRole.CN88Staff) ? ServiceSetting.GetAllSiteWithGroupCN88() : ServiceSetting.GetAllSiteWithGroup(),
                ResponseList = ServiceSetting.GetAllResponses().Where(x => x.IsActive),
                StatusList = statusList,
                TitleList = ServiceSetting.GetAllTitles().Where(x => x.IsActive),
                LicenseeType = ServiceSetting.GetAllLicenseeTypes(),
                ProductNA = ServiceSetting.GetProductNA(),
                SubProductNA = ServiceSetting.GetSubProductNA(),
                BettypeNA = ServiceSetting.GetBettypeNA(),
                ExtensionNA = ServiceSetting.GetExtensionNA(),
                ProductList = ServiceSetting.GetProducts().Where(x => x.IsActive),
                SubProductList = ServiceSetting.GetSubProducts().Where(x => x.IsActive),
                BettypeList = ServiceSetting.GetBettypes().Where(x => x.IsActive),
                MainCategoryList = ServiceSetting.GetMainCategories().Where(x => x.IsActive),
                SubCategoryList = ServiceSetting.GetSubCategories().Where(x => x.IsActive),
            };

            return settings;
        } 
    }
}
