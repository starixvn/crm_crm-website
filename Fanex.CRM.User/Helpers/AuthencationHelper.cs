namespace Fanex.CRM.User.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Security;
    using Common.Services;
    using Fanex.CRM.Common.Models;
    using NAP.Core.Models;
    using NAP.Core.Web.Auth;
    using System.Security.Principal;
    public static class AuthencationHelper
    {
        public static void SignIn(CRMUser user)
        {
            var cache = HttpContext.Current.Response.Cache;
            cache.SetNoStore();
            cache.SetExpires(DateTime.Now.AddMinutes(-1));
            cache.SetCacheability(HttpCacheability.NoCache);
            DateTime expire = DateTime.Now.AddMinutes(FormsAuthentication.Timeout.TotalMinutes);
            string userData = (new AuthInfo
            {
                UserId = user.Id,
                OnUserId = user.OnUserId,
            }).ToXml();

            var ticket = new FormsAuthenticationTicket(
                1,
                user.Username,
                 DateTime.Now,
               expire,
                false,
                userData, "/");
            string hashTicket = FormsAuthentication.Encrypt(ticket);
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, hashTicket);

            HttpContext.Current.Response.Cookies.Set(cookie);

        }
        public static void ClearCookies()
        {
            var cache = HttpContext.Current.Response.Cache;
            cache.SetNoStore();
            cache.SetExpires(DateTime.Now.AddMinutes(-1));
            cache.SetCacheability(HttpCacheability.NoCache);
            var cookies = HttpContext.Current.Request.Cookies.AllKeys;
            foreach (string cookie in cookies)
            {
                var item = new HttpCookie(cookie)
                {
                    Expires = DateTime.Now.AddDays(-1),
                    HttpOnly = true
                };
                HttpContext.Current.Response.Cookies.Add(item);
            }
        }
        public static void AssignAppSession(CRMUser user, IList<MenuItem> menuItems)
        {
            AppSession.CurrentUserMenu = menuItems;
            AppSession.CurrentUser = user;
        }
    }
}
