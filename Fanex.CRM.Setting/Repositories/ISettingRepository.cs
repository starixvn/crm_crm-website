// --------------------------------------------------------------------------
// <copyright file="ISettingRepository.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Setting.Repositories
{
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Setting.Criteria;
    using Fanex.CRM.Setting.Models;
    using System.Collections.Generic;

    public interface ISettingRepository
    {
        IList<Setting> GetAllSettings(GetAllSettingCriteria criteria);

        IList<T> GetSettingRelations<T>(GetSettingRelationByNameCriteria criteria) where T : SettingRelation, new();

        IList<Country> GetAllCountries(GetAllCountryCriteria criteria);

        IEnumerable<Setting> GetAllSettingRelations(GetAllSettingRelationCriteria criteria);

        IList<Setting> GetAllLanguages(GetAllLanguageCriteria criteria);

        IList<Setting> GetAllCommunicationTools(GetAllCommunicationToolCriteria criteria);

        IList<Setting> GetAllMainCategory(GetAllMainCategoryCriteria criteria);

        IList<Setting> GetAllSubCategory(GetAllSubCategoryCriteria criteria);

        IList<BaseModel> GetAllStatus(GetAllStatusCriteria criteria);

        IEnumerable<Status> GetStatuses(GetAllStatusCriteria criteria);

        IEnumerable<FeedbackTemplate> GetFeedbackSubjectTemplateById(GetFeedbackSubjectTemplateByIdCriteria criteria);

        IList<BaseModel> GetAllLogActions(GetActionCriteria criteria);

        IEnumerable<Product> GetAllBetType(GetAllBetTypeCriteria criteria);

        IEnumerable<BaseModel> GetAllStatusInformHeadline(GetAllStatusInformHeadlineCriteria criteria);

        IList<BaseModel> GetAllLicenseeTypes(GetAllLicenseeTypeCriteria criteria);

        IList<BaseModel> GetAllCurrencies(GetAllCurrencyCriteria criteria);

        IList<BaseModel> GetAllCurrencyRedemptions(GetAllCurrencyRedemptionCriteria criteria);

        IEnumerable<Station> GetStations(GetStationsCriteria criteria);

        IEnumerable<Product> GetSubProducts(SubProductCriteria criteria);
    }
}
