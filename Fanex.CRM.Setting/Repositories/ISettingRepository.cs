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
        List<SettingInfo> GetAllSetting(GetAllSettingCriteria criteria);

        List<SettingInfo> GetSettingRelationByName(GetSettingRelationByNameCriteria criteria);

        List<CountryInfo> GetAllCountry(GetAllCountryCriteria criteria);

        List<SettingInfo> GetAllSettingRelation(GetAllSettingRelationCriteria criteria);

        List<SettingInfo> GetAllLanguage(GetAllLanguageCriteria criteria);

        List<SettingInfo> GetAllCommunicationTool(GetAllCommunicationToolCriteria criteria);

        List<SettingInfo> GetAllMainCategory(GetAllMainCategoryCriteria criteria);

        List<SettingInfo> GetAllSubCategory(GetAllSubCategoryCriteria criteria);

        List<SettingInfo> GetAllStatus(GetAllStatusCriteria criteria);

        List<FeedbackTemplate> GetFeedbackSubjectTemplateById(GetFeedbackSubjectTemplateByIdCriteria criteria);

        List<SettingInfo> GetAllLogAction(ICriteria criteria);

        List<SettingInfo> GetAllBetType(GetAllBetTypeCriteria criteria);

        List<BaseModel> GetAllStatusInformHeadline(GetAllStatusInformHeadlineCriteria criteria);

        List<BaseModel> GetAllLicenseeType(GetAllLicenseeTypeCriteria criteria);

        List<BaseModel> GetAllCurrency(GetAllCurrencyCriteria criteria);

        List<SettingInfo> GetAllCurrencyRedemption(GetAllCurrencyRedemptionCriteria criteria);
    }
}
