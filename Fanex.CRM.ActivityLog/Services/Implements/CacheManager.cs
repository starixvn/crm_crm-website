// --------------------------------------------------------------------------
// <copyright file="CacheManager.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.03.04">Created</change>
// </history>

namespace Fanex.CRM.ActionLog.Services.Implements
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CRM.ActionLog.Resources;
    using Fanex.CRM.Infrastructure.Service.Implements;
    using Fanex.CRM.Setting.Models;
    using Fanex.CRM.Setting.Services.Implements;
    using Fanex.CRM.Setting.Services.Interfaces;

    /// <summary>
    /// CacheManager class.
    /// </summary>
    public static class CacheManager
    {
        private const string GetAllSettingSessionKey = "Fanex.CRM.ActionLog.GetAllSettingSessionKey";

        private static readonly ISettingService SettingServiceInstance;

        static CacheManager()
        {
            SettingServiceInstance = SettingServiceInstance ?? new SettingService();
        }

        /// <summary>
        /// Gets all setting.
        /// </summary>
        /// <returns>The hash table.</returns>
        public static IDictionary<string, string> GetAllSetting()
        {
            string key = GetAllSettingSessionKey;
            return Get<IDictionary<string, string>>(
                key,
                () =>
                {
                    var countries = SettingServiceInstance.GetAllCountry()
                        .Select(c => new SettingInfo { Id = c.Id, Value = c.Name, Type = "Country" });

                    var settings = SettingServiceInstance.GetAllSetting();
                    settings.AddRange(countries);

                    // Add default N/A
                    settings.Add(new SettingInfo { Id = 0, Value = Labels.NA });

                    return ListToHash(settings);
                });
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
                settingHash.Add(setting.Id.ToString(), setting.Value);
            }

            return settingHash;
        }
    }
}
