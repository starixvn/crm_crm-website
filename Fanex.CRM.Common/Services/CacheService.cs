// --------------------------------------------------------------------------
// <copyright file="CacheService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Common.Services
{
    using Fanex.Caching;
    using Newtonsoft.Json;

    /// <summary>
    /// Caching service.
    /// </summary>
    public static class CacheService
    {
        private const int TIMEOUT = 300; // 5 minutes.

        private static readonly ICache Cache = Caching.CacheService.CreateInstance(CacheType.MemoryCache);

        /// <summary>
        /// Getting cache value.
        /// </summary>
        /// <typeparam name="T">Parameter type.</typeparam>
        /// <param name="key">Cache key.</param>
        /// <returns>Return value.</returns>
        public static T Get<T>(string key)
        {
            return DeepClone(Cache.Get<T>(key));
        }

        /// <summary>
        /// Setting cache value.
        /// </summary>
        /// <typeparam name="T">Parameter type.</typeparam>
        /// <param name="key">Cache key.</param>
        /// <param name="value">Return value.</param>
        public static void Set<T>(string key, T value)
        {
            Cache.Set(key, DeepClone(value), TIMEOUT);
        }

        /// <summary>
        /// Removing the cache.
        /// </summary>
        public static void Clear()
        {
            Cache.Clear();
        }

        /// <summary>
        /// Deletes the specified key.
        /// </summary>
        /// <param name="key">The key to remove.</param>
        public static void Delete(string key)
        {
            Cache.Remove(key);
        }

        private static T DeepClone<T>(T source)
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source));
        }
    }
}
