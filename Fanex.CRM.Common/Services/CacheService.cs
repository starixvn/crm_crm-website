namespace Fanex.CRM.Common.Services
{
    using Fanex.Caching;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;

    public static class CacheService
    {
        private const int TIMEOUT = 5; // 5 minutes.

        public static T Get<T>(string key)
        {
            return DeepClone(ConfiguredCacheService.Get<T>(GetKeyForLanguage(key)));
        }

        public static void Set<T>(string key, T value)
        {
            ConfiguredCacheService.Set(GetKeyForLanguage(key), DeepClone(value), new CacheItemOptions().SetSlidingExpiration(TimeSpan.FromMinutes(TIMEOUT)));
        }

        public static void ClearCache(List<string> SessionKeys)
        {

            foreach(var key in SessionKeys)
            {
                if(ConfiguredCacheService.Contain(key))
                {
                    ConfiguredCacheService.Remove(GetKeyForLanguage(key));
                }
            }
        }

        public static void Delete(string key)
        {
            ConfiguredCacheService.Remove(GetKeyForLanguage(key));
        }

        private static string GetKeyForLanguage(string key)
        {
            var language = AppSession.CurrentUser.Language;
            return language != null ? key + language : key;
        }

        private static T DeepClone<T>(T source)
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source));
        }
    }
}