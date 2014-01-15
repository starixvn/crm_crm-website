// --------------------------------------------------------------------------
// <copyright file="SSOController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2014.01.09">Created</change>
// </history>

namespace Fanex.CRM.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using Fanex.Common;
    using Fanex.CRM.Infrastructure.Common;
    using Fanex.NAP.Core.Configuration;
    using Fanex.NAP.Core.Criteria.User;
    using Fanex.NAP.Core.Model;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Core.Web.Auth;
    using Fanex.NAP.Core.Web.Session;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class UserLoginController: BaseController
    {
        private readonly int _timeExipration = 30;
        private IUserService _userNapService;
        private IMenuItemService _menuItemNapService;
        private ICurrentSession _napCurrentSession;

        public UserLoginController()
            : this(null, null, null)
        {
        }

        public UserLoginController(
            IUserService userService,
            IMenuItemService menuItemService,
            ICurrentSession currentSession)
        {
            _userNapService = userService ?? new UserService();
            _menuItemNapService = menuItemService ?? new MenuItemService();
            _napCurrentSession = currentSession ?? new CurrentSession();
        }

        /// <summary>
        /// Indexes the specified user identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="onuserid">The on user identifier.</param>
        /// <param name="time">The time value.</param>
        /// <returns>The index page.</returns>
        [AllowAnonymous]
        [HttpGet]
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public ActionResult SSO(string userId, string onuserid, string time)
        {
            Session.Clear();

            userId = Cryptography.CfsDeCrypt(userId.Replace(" ", "+"));
            onuserid = Cryptography.CfsDeCrypt(onuserid.Replace(" ", "+"));
            time = Cryptography.CfsDeCrypt(time.Replace(" ", "+"));
            string currentTime = _userNapService.UserSSOGetTime();

            var expiration = Convert.ToDateTime(currentTime) - Convert.ToDateTime(time);

            if (expiration.TotalSeconds <= _timeExipration)
            {
                //// do SSO check-in

                var criteria = new UserSSOCheckInUserCriteria
                {
                    UserId = Convert.ToInt32(userId),
                    SiteId = SiteId,
                    CheckOnUserId = Convert.ToInt32(onuserid),
                    OnUserId = 0,
                    IP = CurrentContext.IP,
                    NetSessionId = Session.SessionID,
                    LoginDate = DateTime.Now,
                    BrowserName = CurrentContext.BrowserName,
                    CountryCode = CurrentContext.CountryCode,
                    Domain = CurrentContext.DomainName,
                    Platform = CurrentContext.Platform
                };

                _userNapService.UserSSOCheckIn(ref criteria);

                //// do SSO check-in

                if (criteria.OnUserId != 0)
                {
                    var user = _userNapService.GetById(criteria.UserId) as User;

                    //// saves user session & auth info

                    _napCurrentSession.UserId = user.Id;
                    _napCurrentSession.Username = user.Username;
                    _napCurrentSession.OnUserId = criteria.OnUserId;
                    _napCurrentSession.SessionId = Session.SessionID.ToString();
                    _napCurrentSession.Level = _userNapService.GetUserSiteLevel(user.Id, this.SiteId);

                    AppSession.CurrentUser = user;
                    Fanex.CRM.Service.Interface.IPermissionService permissionService = new Fanex.CRM.Service.Impl.PermissionService();
                    AppSession.RoleAccess = permissionService.GetRoleAccess((new FunctionService()).GetByUserAndSiteIncludeGroup(
                        user.Id,
                        NapSettings.Settings.SiteId,
                        NapSettings.Settings.Environment));

                    if (this.SiteId != 0)
                    {
                        _napCurrentSession.MenuItemList = _menuItemNapService.GetByUserAndSiteIncludeGroup(
                                                                                user.Id,
                                                                                SiteId,
                                                                                NapSettings.Settings.Environment,
                                                                                true) as IList<MenuItem>;
                        AppSession.CurrentUserMenu = _napCurrentSession.MenuItemList;
                    }

                    CreateAuthInfo(user);

                    //// saves user session & auth info
                    return Redirect(FormsAuthentication.DefaultUrl);
                }
            }

            return Redirect(FormsAuthentication.LoginUrl);
        }

        /// <summary>
        /// Creates the authentication information.
        /// </summary>
        /// <param name="user">The user value.</param>
        private void CreateAuthInfo(User user)
        {
            var currentDomain = CurrentContext.GetCurrentDomain();

            var authInfo = new AuthInfo();

            authInfo.UserId = user.Id;
            authInfo.OnUserId = this._napCurrentSession.OnUserId;

            var authTicket = new FormsAuthenticationTicket(
                 1,
                user.Username,
                DateTime.Now,
                DateTime.Now.AddMinutes(20000),
                false,
                authInfo.ToXml(),
                currentDomain.Path);

            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket)
            {
                HttpOnly = true,
                Path = currentDomain.Path,
                Name = FormsAuthentication.FormsCookieName
            };

            Response.Cookies.Add(authCookie);
        }
    }
}