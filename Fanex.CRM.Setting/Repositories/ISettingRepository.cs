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
        IEnumerable<Setting> GetAllSettings(GetAllSettingCriteria criteria);

        IEnumerable<T> GetSettingRelations<T>(GetSettingRelationByNameCriteria criteria) where T : SettingRelation, new();

        IEnumerable<Country> GetAllCountries(GetAllCountryCriteria criteria);

        IEnumerable<Setting> GetAllSettingRelations(GetAllSettingRelationCriteria criteria);

        IEnumerable<Setting> GetAllLanguages(GetAllLanguageCriteria criteria);

        IEnumerable<Setting> GetAllCommunicationTools(GetAllCommunicationToolCriteria criteria);

        IEnumerable<Setting> GetAllMainCategory(GetAllMainCategoryCriteria criteria);

        IEnumerable<Setting> GetAllSubCategory(GetAllSubCategoryCriteria criteria);

        IEnumerable<BaseModel> GetAllStatus(GetAllStatusCriteria criteria);

        IEnumerable<Status> GetStatuses(GetAllStatusCriteria criteria);

        IEnumerable<FeedbackTemplate> GetFeedbackSubjectTemplateById(GetFeedbackSubjectTemplateByIdCriteria criteria);

        IEnumerable<BaseModel> GetAllLogActions(GetActionCriteria criteria);

        IEnumerable<Product> GetAllBetType(GetAllBetTypeCriteria criteria);

        IEnumerable<BaseModel> GetAllStatusInformHeadline(GetAllStatusInformHeadlineCriteria criteria);

        IEnumerable<BaseModel> GetAllLicenseeTypes(GetAllLicenseeTypeCriteria criteria);

        IEnumerable<BaseModel> GetAllCurrencies(GetAllCurrencyCriteria criteria);

        IEnumerable<BaseModel> GetAllCurrencyRedemptions(GetAllCurrencyRedemptionCriteria criteria);

        IEnumerable<Station> GetStations(GetStationsCriteria criteria);

        IEnumerable<Product> GetSubProducts(SubProductCriteria criteria);
    }
}
