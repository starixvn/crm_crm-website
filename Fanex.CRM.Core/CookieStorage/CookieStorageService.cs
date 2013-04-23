
namespace Fanex.CRM.Infrastructure.CookieStorage
{
    using System;
    using System.Web;

    public class CookieStorageService : ICookieStorageService
    {
        /// <summary>
        /// add value to cookie
        /// </summary>
        /// <param name="key">not null or empty key</param>
        /// <param name="value">string value</param>
        /// <param name="expires"></param>
        public void Add(string key, string value, DateTime expires)
        {
            if (!string.IsNullOrEmpty(key) && HttpContext.Current != null)
            {
                HttpContext.Current.Response.Cookies[key].Value = value;
                HttpContext.Current.Response.Cookies[key].Expires = expires;
            }
        }

        /// <summary>
        /// Get value from cookie
        /// </summary>
        /// <param name="key">not null or empty key</param>
        /// <returns>string</returns>
        public string Get(string key)
        {
            if (!string.IsNullOrEmpty(key) && HttpContext.Current != null)
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies[key];
                if (cookie != null)
                    return cookie.Value;
            }

            return string.Empty;
        }
    }
}