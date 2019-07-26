namespace Fanex.CRM.Setting.Repositories
{
    using System.Collections.Generic;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Setting.Criteria;
    using Fanex.CRM.Setting.Models;

    public interface ISettingRepository
    {
        List<Setting> GetAllSettings(GetAllSettingCriteria criteria);

        IList<T> GetSettingRelations<T>(GetSettingRelationByNameCriteria criteria) where T : SettingRelation, new();

        IList<Country> GetAllCountries(GetAllCountryCriteria criteria);

        IList<Province> GetAllProvinces(GetAllProvinceCriteria criteria);

        IEnumerable<Setting> GetAllSettingRelations(GetAllSettingRelationCriteria criteria);

        IList<Setting> GetAllLanguages(GetAllLanguageCriteria criteria);

        IList<Setting> GetAllCommunicationTools(GetAllCommunicationToolCriteria criteria);

        IList<Setting> GetAllMainCategory(GetAllMainCategoryCriteria criteria);

        IList<Setting> GetAllSubCategory(GetAllSubCategoryCriteria criteria);

        IList<BaseModel> GetAllStatus(GetAllStatusCriteria criteria);

        IList<BaseModel> GetAllLogActions(GetActionCriteria criteria);

        IEnumerable<Product> GetAllBetType(GetAllBetTypeCriteria criteria);

        IEnumerable<BaseModel> GetAllStatusInformHeadline(GetAllStatusInformHeadlineCriteria criteria);

        IList<BaseModel> GetAllLicenseeTypes(GetAllLicenseeTypeCriteria criteria);

        IList<BaseModel> GetAllCurrencies(GetAllCurrencyCriteria criteria);

        IList<BaseModel> GetAllCurrencyRedemptions(GetAllCurrencyRedemptionCriteria criteria);

        IEnumerable<Product> GetSubProducts(SubProductCriteria criteria);
    }
}