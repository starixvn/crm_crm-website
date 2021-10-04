namespace Fanex.CRM.ActionLog.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CRM.ActionLog.Resources;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Common.Services;
    using Fanex.CRM.Setting.Criteria;
    using Fanex.CRM.Setting.Services;

    internal static class CacheManager
    {
        private const string GetAllSettingSessionKey = "Fanex.CRM.ActionLog.GetAllSettingSessionKey";

        public static IDictionary<string, string> AllSettings
        {
            get
            {
                var result = RedisCacheService.Get<IDictionary<string, string>>(GetAllSettingSessionKey);
                if (result == null)
                {
                    result = GetAllSettings();
                    RedisCacheService.Set(GetAllSettingSessionKey, result);
                }

                return result;
            }
        }

        private static IDictionary<string, string> GetAllSettings()
        {
            var settingService = new SettingService();

            var countries = settingService.GetAllCountries().Where(x => x.IsActive)
                .Select(c => new Setting { Id = c.Id, Value = c.Name, SettingType = "Country" });

            var currencies = settingService.GetAllCurrencies(new GetAllCurrencyCriteria())
                .Select(c => new Setting { Id = c.Id, Value = c.Name, SettingType = "Currency" });

            var settings = settingService.GetAllSettings();
            settings.AddRange(countries);
            settings.AddRange(currencies);

            // Add default N/A
            settings.Add(new Setting { Id = 0, Value = Labels.NA, SettingType = string.Empty });

            return settings.GroupBy(x => x.Id).Select(x => x.First())
                .ToDictionary(x => x.Id + x.SettingType.ToUpperInvariant(), x => x.Value);
        }
    }
}