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
    public static class CacheManager
    {
        private const string GetAllSettingSessionKey = "Fanex.CRM.ActionLog.GetAllSettingSessionKey";

        private static readonly ISettingService SettingServiceInstance = new SettingService();

        /// <summary>
        /// Gets all settings.
        /// </summary>
        /// <value>
        /// All settings.
        /// </value>
        public static IDictionary<string, string> AllSettings
        {
            get
            {
                string key = GetAllSettingSessionKey;
                return Get<IDictionary<string, string>>(
                    key,
                    () =>
                    {
                        var countries = SettingServiceInstance.AllCountry()
                            .Select(c => new SettingInfo { Id = c.Id, Value = c.Name, SettingType = "Country" });

                        var currencies = SettingServiceInstance.GetAllCurrency(new GetAllCurrencyCriteria())
                            .Select(c => new SettingInfo { Id = c.Id, Value = c.Name, SettingType = "Currency" });

                        var settings = SettingServiceInstance.AllSetting();
                        settings.AddRange(countries);
                        settings.AddRange(currencies);

                        // Add default N/A
                        settings.Add(new SettingInfo { Id = 0, Value = Labels.NA, SettingType = string.Empty });

                        return ListToHash(settings);
                    });
            }
        }

        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <typeparam name="T">T template.</typeparam>
        /// <param name="key">The key string.</param>
        /// <param name="acquire">The acquire.</param>
        /// <returns>The T result.</returns>
        private static T Get<T>(string key, Func<T> acquire)
        {
            T result = CacheService.Get<T>(key);
            if (result != null)
            {
                return result;
            }

            result = acquire();
            if (result != null)
            {
                CacheService.Set<T>(key, result);
            }

            return result;
        }

        /// <summary>
        /// Convert list to hash table.
        /// </summary>
        /// <param name="settingInfos">The setting list.</param>
        /// <returns>
        /// The hash table.
        /// </returns>
        private static IDictionary<string, string> ListToHash(IList<SettingInfo> settingInfos)
        {
            if (settingInfos == null)
            {
                return null;
            }

            IDictionary<string, string> settingHash = new Dictionary<string, string>(128);
            foreach (SettingInfo setting in settingInfos)
            {
                settingHash.Add(setting.Id.ToString() + setting.SettingType.ToUpperInvariant(), setting.Value);
            }

            return settingHash;
        }
    }
}
