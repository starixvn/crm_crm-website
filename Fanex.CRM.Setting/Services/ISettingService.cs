// --------------------------------------------------------------------------
// <copyright file="ISettingService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Setting.Services
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Setting.Criteria;
    using Fanex.CRM.Setting.Models;

    public interface ISettingService
    {
        IEnumerable<Setting> GetAllSettings();

        Setting GetSettingById(GetSettingByIdCriteria criteria);

        IEnumerable<SiteInfo> GetAllSites(GetAllSiteCriteria criteria = null);

        IEnumerable<BaseModel> GetAllSiteWithGroup();

        IEnumerable<Setting> GetAllTitles(bool hasItemAll = false);

        IEnumerable<Country> GetAllCountries();

        IEnumerable<Setting> GetAllLanguages();

        IEnumerable<Setting> GetAllExtensions();

        IEnumerable<Setting> GetAllCommunicationTools();

        IEnumerable<Setting> GetAllMainCategories();

        IEnumerable<Setting> GetAllProducts();

        IEnumerable<Setting> GetAllStatuses(int statusType);

        IEnumerable<Status> GetInformHeadlineStatuses();

        IEnumerable<BaseModel> GetAllCurrencyRedemptions(GetAllCurrencyRedemptionCriteria criteria);

        IEnumerable<Setting> GetAllRoles();

        IEnumerable<Setting> GetAllResponses();

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
    }
}
