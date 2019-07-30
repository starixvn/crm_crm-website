namespace Fanex.CRM.Common.Services
{
    using Fanex.Caching;
    using Newtonsoft.Json;

    public static class CacheService
    {
        private const int TIMEOUT = 300; // 5 minutes.

        private static readonly ICache Cache = Caching.CacheService.CreateInstance(CacheType.MemoryCache);

        public static T Get<T>(string key)
        {
            return DeepClone(Cache.Get<T>(GetKeyForLanguage(key)));
        }

        public static void Set<T>(string key, T value)
        {
            Cache.Set(GetKeyForLanguage(key), DeepClone(value), TIMEOUT);
        }

        public static void Clear()
        {
            Cache.Clear();
        }

        public static void Delete(string key)
        {
            Cache.Remove(GetKeyForLanguage(key));
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