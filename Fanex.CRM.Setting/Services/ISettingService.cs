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
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Setting.Criteria;
    using Fanex.CRM.Setting.Models;

    public interface ISettingService
    {
        List<SettingInfo> AllSetting();

        SettingInfo SettingById(GetSettingByIdCriteria criteria);

        List<SiteInfo> AllSite();

        IList<BaseModel> AllSiteWithGroup();

        List<SettingInfo> AllTitle();

        List<CountryInfo> AllCountry();

        List<SettingInfo> AllLanguage();

        List<SettingInfo> AllExtension();

        List<SettingInfo> AllCommunicationTool();

        List<SettingInfo> AllMainCategory();

        List<SettingInfo> AllProduct();

        List<SettingInfo> AllStatus(int statusType);

        List<BaseModel> AllCurrencyRedemption(GetAllCurrencyRedemptionCriteria criteria);

        List<SettingInfo> AllRole();

        List<SettingInfo> AllResponse();

        List<SettingInfo> AllCustomerType();

        List<SettingInfo> AllVIPType();

        List<SettingInfo> AllSiteType();

        List<BaseModel> AllLicenseeType();

        List<BaseModel> GetAllCurrency(GetAllCurrencyCriteria criteria);

        int NumberOfFeedbackSubjectTemplate();

        List<FeedbackTemplate> FeedbackSubjectTemplateByPagingIndex(GetFeedbackSubjectTemplateByPagingIndexCriteria criteria);

        List<SettingInfo> AllLogAction(GetActionCriteria criteria);

        InformHeadlineSetting AllInformHeadlineSettings();

        BaseModel[] FeedbackStatisticFilterTypeList();

        void ResetAllCache();

        List<SettingInfo> MainSetting();
    }
}
