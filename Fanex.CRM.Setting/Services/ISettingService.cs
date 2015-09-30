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
        IEnumerable<SettingInfo> GetAllSettings();

        SettingInfo GetSettingById(GetSettingByIdCriteria criteria);

        IEnumerable<SiteInfo> GetAllSites(GetAllSiteCriteria criteria = null);

        IEnumerable<BaseModel> GetAllSiteWithGroup();

        IEnumerable<SettingInfo> GetAllTitles();

        IEnumerable<CountryInfo> GetAllCountries();

        IEnumerable<SettingInfo> GetAllLanguages();

        IEnumerable<SettingInfo> GetAllExtensions();

        IEnumerable<SettingInfo> GetAllCommunicationTools();

        IEnumerable<SettingInfo> GetAllMainCategories();

        IEnumerable<SettingInfo> GetAllProducts();

        IEnumerable<SettingInfo> GetAllStatuses(int statusType);

        IEnumerable<Status> GetInformHeadlineStatuses();

        IEnumerable<BaseModel> GetAllCurrencyRedemptions(GetAllCurrencyRedemptionCriteria criteria);

        IEnumerable<SettingInfo> GetAllRoles();

        IEnumerable<SettingInfo> GetAllResponses();

        IEnumerable<SettingInfo> GetAllCustomerTypes();

        IEnumerable<SettingInfo> GetAllVIPTypes();

        IEnumerable<SettingInfo> GetAllSiteTypes();

        IEnumerable<BaseModel> GetAllLicenseeTypes();

        IEnumerable<BaseModel> GetAllCurrencies(GetAllCurrencyCriteria criteria, bool hasDefaultOption = true);

        int NumberOfFeedbackSubjectTemplate();

        IEnumerable<FeedbackTemplate> GetFeedbackSubjectTemplateByPagingIndex(GetFeedbackSubjectTemplateByPagingIndexCriteria criteria);

        IEnumerable<BaseModel> GetAllLogActions(GetActionCriteria criteria);

        InformHeadlineSetting GetAllInformHeadlineSettings();

        BaseModel[] GetFeedbackStatisticFilterTypes();

        void ClearCache();

        IEnumerable<SettingInfo> GetMainSettings();

        IEnumerable<Station> GetStations();
    }
}
