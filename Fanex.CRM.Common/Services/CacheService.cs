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
    using System;
    using Fanex.Caching;
    using Fanex.CRM.Infrastructure.Helpers;

    /// <summary>
    /// Caching service.
    /// </summary>
    public static class CacheService
    {
        private static readonly ICache Cache = Caching.CacheService.CreateInstance(CacheType.MemoryCache);

        /// <summary>
        /// Getting cache value.
        /// </summary>
        /// <param name="key">Cache key.</param>
        /// <returns>Return value.</returns>
        public static object Get(string key)
        {
            try
            {
                return Cache.Get<object>(key);
            }
            catch (Exception exception)
            {
                LogExtensions.ExceptionHandler(exception);
                throw;
            }
        }

        /// <summary>
        /// Setting cache value.
        /// </summary>
        /// <param name="key">Cache key.</param>
        /// <param name="value">The value that saving to cache.</param>
        public static void Set(string key, object value)
        {
            try
            {
                Cache.Set(key, value);
            }
            catch (Exception exception)
            {
                LogExtensions.ExceptionHandler(exception);
                throw;
            }
        }

        /// <summary>
        /// Getting cache value.
        /// </summary>
        /// <typeparam name="T">Parameter type.</typeparam>
        /// <param name="key">Cache key.</param>
        /// <returns>Return value.</returns>
        public static T Get<T>(string key)
        {
            return Cache.Get<T>(key);
        }

        /// <summary>
        /// Setting cache value.
        /// </summary>
        /// <typeparam name="T">Parameter type.</typeparam>
        /// <param name="key">Cache key.</param>
        /// <param name="value">Return value.</param>
        public static void Set<T>(string key, T value)
        {
            try
            {
                Cache.Set<T>(key, value);
            }
            catch (Exception exception)
            {
                LogExtensions.ExceptionHandler(exception);
                throw;
            }
        }

        /// <summary>
        /// Setting cache value.
        /// </summary>
        /// <param name="key">Cache key.</param>
        /// <param name="value">The value that saving to cache.</param>
        /// <param name="expire">Expire time.</param>
        public static void Set(string key, object value, TimeSpan expire)
        {
            try
            {
                Cache.Set(key, value, expire);
            }
            catch (Exception exception)
            {
                LogExtensions.ExceptionHandler(exception);
                throw;
            }
        }

        /// <summary>
        /// Setting cache value.
        /// </summary>
        /// <typeparam name="T">Parameter type.</typeparam>
        /// <param name="key">Cache key.</param>
        /// <param name="value">The value that saving to cache.</param>
        /// <param name="expire">Expire time.</param>
        public static void Set<T>(string key, T value, TimeSpan expire)
        {
            Cache.Set<T>(key, value, expire);
        }

        /// <summary>
        /// Removing the cache.
        /// </summary>
        public static void Delete()
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
    }
}
