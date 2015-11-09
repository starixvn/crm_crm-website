// --------------------------------------------------------------------------
// <copyright file="CacheManager.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.03.04">Created</change>
// </history>

namespace Fanex.CRM.ActionLog.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CRM.ActionLog.Resources;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Common.Services;
    using Fanex.CRM.Setting.Criteria;
    using Fanex.CRM.Setting.Services;

    /// <summary>
    /// CacheManager class.
    /// </summary>
    internal static class CacheManager
    {
        private const string GetAllSettingSessionKey = "Fanex.CRM.ActionLog.GetAllSettingSessionKey";

        public static IDictionary<string, string> AllSettings
        {
            get
            {
                var result = CacheService.Get<IDictionary<string, string>>(GetAllSettingSessionKey);
                if (result == null)
                {
                    result = GetAllSettings();
                    CacheService.Set(GetAllSettingSessionKey, result);
                }

                return result;
            }
        }

        private static IDictionary<string, string> GetAllSettings()
        {
            var settingService = new SettingService();

            var countries = settingService.GetAllCountries()
                .Select(c => new Setting { Id = c.Id, Value = c.Name, SettingType = "Country" });

            var currencies = settingService.GetAllCurrencies(new GetAllCurrencyCriteria())
                .Select(c => new Setting { Id = c.Id, Value = c.Name, SettingType = "Currency" });

            var settings = settingService.GetAllSettings().ToList();
            settings.AddRange(countries);
            settings.AddRange(currencies);

            // Add default N/A
            settings.Add(new Setting { Id = 0, Value = Labels.NA, SettingType = string.Empty });

            return settings.ToDictionary(x => x.Id + x.SettingType.ToUpperInvariant(), x => x.Value);            
        }
    }
}
