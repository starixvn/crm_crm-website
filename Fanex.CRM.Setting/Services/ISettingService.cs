namespace Fanex.CRM.Setting.Services
{
    using System.Collections.Generic;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Setting.Criteria;
    using Fanex.CRM.Setting.Models;

    public interface ISettingService
    {
        List<Setting> GetAllSettings();

        Setting GetSettingById(GetSettingByIdCriteria criteria);

        IList<SiteInfo> GetAllSites(GetAllSiteCriteria criteria = null);

        IList<SiteInfo> GetAllSitesCN88();

        IList<SiteSetting> GetAllSiteWithGroup();

        IList<SiteSetting> GetAllSiteWithGroupCN88();

        IList<SettingRelation> GetAllTitles();

        IList<Country> GetAllCountries();

        IList<Province> GetAllProvinces();

        IList<Setting> GetAllLanguages();

        IList<SettingRelation> GetAllExtensions();

        IList<Setting> GetAllCommunicationTools();

        IList<Setting> GetAllMainCategories();

        IList<Setting> GetAllMainCategoriesFromAllSettings();

        IList<Product> GetAllSubProducts();

        List<BaseModel> GetAllStatuses(int statusType);

        IList<Status> GetInformHeadlineStatuses();

        List<BaseModel> GetAllCurrencyRedemptions(GetAllCurrencyRedemptionCriteria criteria);

        IList<SettingRelation> GetAllRoles();

        IList<SettingRelation> GetAllResponses();

        IList<Setting> GetAllCustomerTypes();

        IList<Setting> GetAllVIPTypes();

        IList<Setting> GetAllSiteTypes();

        IList<BaseModel> GetAllLicenseeTypes();

        IList<BaseModel> GetAllCurrencies(GetAllCurrencyCriteria criteria, bool hasDefaultOption = true);

        int NumberOfFeedbackSubjectTemplate();

        IList<FeedbackTemplate> GetFeedbackSubjectTemplateByPagingIndex(GetFeedbackSubjectTemplateByPagingIndexCriteria criteria);

        IList<BaseModel> GetAllLogActions(GetActionCriteria criteria);

        InformHeadlineSetting GetAllInformHeadlineSettings();

        BaseModel[] GetFeedbackStatisticFilterTypes();

        BaseModel[] GetFeedbackStatisticFilterTypesCN88();

        void ClearCache();

        IList<Setting> GetMainSettings();

        IList<Station> GetStations();

        IList<Product> GetAllProducts();

        IList<Product> GetProductsOnly();

        IList<Setting> GetPositions();

        Product GetProductNA();

        Product GetSubProductNA();

        Product GetBettypeNA();

        SettingRelation GetExtensionNA();

        IList<Product> GetProducts();

        IList<Product> GetSubProducts();

        IList<Product> GetBettypes();

        IList<Setting> GetMainCategories();

        IList<Setting> GetSubCategories();

        IList<SiteInfo> GetAllSitesWithNA();

        IList<Setting> GetPositionsCN88();
    }
}