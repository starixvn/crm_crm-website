// --------------------------------------------------------------------------
// <copyright file="ISettingRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Setting.Repositories
{
    using System.Collections.Generic;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Setting.Criteria;
    using Fanex.CRM.Setting.Models;
    using Fanex.Data.Repository;

    public interface ISettingRepository
    {
        IEnumerable<SettingInfo> GetAllSettings(GetAllSettingCriteria criteria);

        IEnumerable<SettingInfo> GetSettingRelations(GetSettingRelationByNameCriteria criteria);

        IEnumerable<CountryInfo> GetAllCountries(GetAllCountryCriteria criteria);

        IEnumerable<SettingInfo> GetAllSettingRelations(GetAllSettingRelationCriteria criteria);

        IEnumerable<SettingInfo> GetAllLanguages(GetAllLanguageCriteria criteria);

        IEnumerable<SettingInfo> GetAllCommunicationTools(GetAllCommunicationToolCriteria criteria);

        IEnumerable<SettingInfo> GetAllMainCategory(GetAllMainCategoryCriteria criteria);

        IEnumerable<SettingInfo> GetAllSubCategory(GetAllSubCategoryCriteria criteria);

        IEnumerable<SettingInfo> GetAllStatus(GetAllStatusCriteria criteria);

        IEnumerable<Status> GetStatuses(GetAllStatusCriteria criteria);

        IEnumerable<FeedbackTemplate> GetFeedbackSubjectTemplateById(GetFeedbackSubjectTemplateByIdCriteria criteria);

        IEnumerable<BaseModel> GetAllLogActions(GetActionCriteria criteria);

        IEnumerable<SettingInfo> GetAllBetType(GetAllBetTypeCriteria criteria);

        IEnumerable<BaseModel> GetAllStatusInformHeadline(GetAllStatusInformHeadlineCriteria criteria);

        IEnumerable<BaseModel> GetAllLicenseeTypes(GetAllLicenseeTypeCriteria criteria);

        IEnumerable<BaseModel> GetAllCurrencies(GetAllCurrencyCriteria criteria);

        IEnumerable<BaseModel> GetAllCurrencyRedemptions(GetAllCurrencyRedemptionCriteria criteria);

        IEnumerable<Station> GetStations(GetStationsCriteria criteria);
    }
}
