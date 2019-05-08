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

    public static class AuthencationHelper
    {
        public static void AssignAuthenticationCookie(string cookiePath, CRMUser user)
        {
            HttpContext.Current.Response.Cookies.Add(CreateAuthenticationCookie(cookiePath, user));
        }

        public static void AssignAppSession(CRMUser user, IList<MenuItem> menuItems)
        {
            AppSession.CurrentUserMenu = menuItems;
            AppSession.CurrentUser = user;
        }

        private static HttpCookie CreateAuthenticationCookie(string cookiePath, CRMUser user)
        {
            string cookieName = FormsAuthentication.FormsCookieName;

            string userData = (new AuthInfo
            {
                UserId = user.Id,
                OnUserId = user.OnUserId,
            }).ToXml();

            var currentTime = DateTime.Now;
            var formAuthentiationTicket = new FormsAuthenticationTicket(
                1,
                user.Username,
                currentTime,
                currentTime.AddMinutes(FormsAuthentication.Timeout.Minutes),
                false,
                userData,
                cookiePath);

            string encryptTicket = FormsAuthentication.Encrypt(formAuthentiationTicket);

            return new HttpCookie(cookieName, encryptTicket)
            {
                Path = cookiePath,
                Name = cookieName,
                HttpOnly = true
            };
        }
    }
}
