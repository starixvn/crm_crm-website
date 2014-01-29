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
    using Fanex.CRM.Setting.Criteria;
    using Fanex.CRM.Setting.Models;

    public interface ISettingService
    {
        SettingInfo GetSettingById(GetSettingByIdCriteria criteria);

        List<SiteInfo> GetAllSite();

        List<CountryInfo> GetAllCountry();

        List<SettingInfo> GetAllLanguage();

        List<SettingInfo> GetAllExtension();

        List<SettingInfo> GetAllCommunicationTool();

        List<SettingInfo> GetAllMainCategory();

        List<SettingInfo> GetAllProduct();

        List<SettingInfo> GetAllStatus();

        List<SettingInfo> GetAllRole();

        List<SettingInfo> GetAllResponse();
    }
}
