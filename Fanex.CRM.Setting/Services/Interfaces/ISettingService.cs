// --------------------------------------------------------------------------
// <copyright file="ISettingService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Setting.Services.Interfaces
{
    using System.Collections.Generic;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Setting.Criteria;
    using Fanex.CRM.Setting.Models;

    public interface ISettingService
    {
        List<SettingInfo> GetAllSetting();

        SettingInfo GetSettingById(GetSettingByIdCriteria criteria);

        List<SiteInfo> GetAllSite();

        IList<BaseModel> GetAllSiteWithGroup();

        List<SettingInfo> GetAllTitle();

        List<CountryInfo> GetAllCountry();

        List<SettingInfo> GetAllLanguage();

        List<SettingInfo> GetAllExtension();

        List<SettingInfo> GetAllCommunicationTool();

        List<SettingInfo> GetAllMainCategory();

        List<SettingInfo> GetAllProduct();

        List<SettingInfo> GetAllStatus(GetAllStatusCriteria criteria);

        List<SettingInfo> GetAllRole();

        List<SettingInfo> GetAllResponse();

        List<SettingInfo> GetAllCustomerType();

        List<SettingInfo> GetAllVIPType();

        List<SettingInfo> GetAllSiteType();

        int GetNumberOfFeedbackSubjectTemplate();

        List<FeedbackTemplate> GetFeedbackSubjectTemplateByPagingIndex(GetFeedbackSubjectTemplateByPagingIndexCriteria criteria);

        List<SettingInfo> GetAllLogAction(GetActionCriteria criteria);

        InformHeadlineSetting GetAllInformHeadlineSettings();

        BaseModel[] GetFeedbackStatisticFilterType();

        void ResetAllCache();

        List<SettingInfo> GetMainSetting();

        List<SettingInfo> GetAllFeedbackStatus();
    }
}
