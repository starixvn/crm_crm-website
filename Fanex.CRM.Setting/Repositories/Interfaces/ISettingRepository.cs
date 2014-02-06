// --------------------------------------------------------------------------
// <copyright file="ISettingRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Setting.Repositories.Interfaces
{
    using System.Collections.Generic;
    using Fanex.CRM.Setting.Criteria;
    using Fanex.CRM.Setting.Models;

    public interface ISettingRepository
    {
        List<SettingInfo> GetAllSetting(GetAllSettingCriteria criteria);

        List<SettingInfo> GetSettingRelationByName(GetSettingRelationByNameCriteria criteria);

        List<CountryInfo> GetAllCountry(GetAllCountryCriteria criteria);

        List<SettingInfo> GetAllSettingRelation(GetAllSettingRelationCriteria criteria);

        List<FeedbackTemplate> GetFeedbackSubjectTemplateById(GetFeedbackSubjectTemplateByIdCriteria criteria);
    }
}
