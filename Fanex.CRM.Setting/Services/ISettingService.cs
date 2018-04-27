// --------------------------------------------------------------------------
// <copyright file="ISettingService.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Setting.Services
{
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Setting.Criteria;
    using Fanex.CRM.Setting.Models;
    using System.Collections.Generic;

    public interface ISettingService
    {
        IEnumerable<Setting> GetAllSettings();

        Setting GetSettingById(GetSettingByIdCriteria criteria);

        IEnumerable<SiteInfo> GetAllSites(GetAllSiteCriteria criteria = null);

        IEnumerable<SiteSetting> GetAllSiteWithGroup();

        IEnumerable<SettingRelation> GetAllTitles();

        IEnumerable<Country> GetAllCountries();

        IEnumerable<Setting> GetAllLanguages();

        IEnumerable<SettingRelation> GetAllExtensions();

        IEnumerable<Setting> GetAllCommunicationTools();

        IEnumerable<Setting> GetAllMainCategories();

        IEnumerable<Setting> GetAllMainCategoriesFromAllSettings();

        IEnumerable<Product> GetAllSubProducts();

        IEnumerable<BaseModel> GetAllStatuses(int statusType);

        IEnumerable<Status> GetInformHeadlineStatuses();

        IEnumerable<BaseModel> GetAllCurrencyRedemptions(GetAllCurrencyRedemptionCriteria criteria);

        IEnumerable<SettingRelation> GetAllRoles();

        IEnumerable<SettingRelation> GetAllResponses();

        IEnumerable<Setting> GetAllCustomerTypes();

        IEnumerable<Setting> GetAllVIPTypes();

        IEnumerable<Setting> GetAllSiteTypes();

        IEnumerable<BaseModel> GetAllLicenseeTypes();

        IEnumerable<BaseModel> GetAllCurrencies(GetAllCurrencyCriteria criteria, bool hasDefaultOption = true);

        int NumberOfFeedbackSubjectTemplate();

        IEnumerable<FeedbackTemplate> GetFeedbackSubjectTemplateByPagingIndex(GetFeedbackSubjectTemplateByPagingIndexCriteria criteria);

        IEnumerable<BaseModel> GetAllLogActions(GetActionCriteria criteria);

        InformHeadlineSetting GetAllInformHeadlineSettings();

        BaseModel[] GetFeedbackStatisticFilterTypes();

        void ClearCache();

        IEnumerable<Setting> GetMainSettings();

        IEnumerable<Station> GetStations();

        IEnumerable<Product> GetAllProducts();

        IEnumerable<Product> GetProductsOnly();

        IEnumerable<Setting> GetPositions();

        Product GetProductNA();

        Product GetSubProductNA();

        Product GetBettypeNA();

        SettingRelation GetExtensionNA();

        IEnumerable<Product> GetProducts();

        IEnumerable<Product> GetSubProducts();

        IEnumerable<Product> GetBettypes();
    }
}
