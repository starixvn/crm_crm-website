// -----------------------------------------------------------------------
// <copyright file="UserLoginController.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// ----------------------------------------------------------------------- 

namespace Fanex.CRM.User.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using Fanex.Common;
    using Fanex.CRM.Infrastructure.Common;
    using Fanex.CRM.Infrastructure.Logging;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Infrastructure.Web.Controllers;
    using Fanex.CRM.User.Resources;
    using Fanex.NAP.Core.Configuration;
    using Fanex.NAP.Core.Criteria.User;
    using Fanex.NAP.Core.Model;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Core.Web;
    using Fanex.NAP.Core.Web.Auth;
    using Fanex.NAP.Core.Web.Session;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class UserLoginController : BaseController
    {
        private const string LogInViewURL = "~/Areas/User/LogIn/Index.cshtml";
        private readonly int _timeExipration = 30;
        private IUserService _userNapService;
        private IMenuItemService _menuItemNapService;
        private ICurrentSession _napCurrentSession;
        private IGroupService _napGroupService;
        private ISiteService _napSiteService;
        private IFunctionService _napFunctionService;

        public UserLoginController()
            : this(null, null, null, null, null, null)
        {
        }

        public UserLoginController(
            IUserService userService,
            IGroupService groupService,
            ISiteService siteService,
            IMenuItemService menuItemService,
            ICurrentSession currentSession,
            IFunctionService functionService)
        {
            _userNapService = userService ?? new UserService();
            _menuItemNapService = menuItemService ?? new MenuItemService();
            _napCurrentSession = currentSession ?? new CurrentSession();

            _napGroupService = groupService ?? new GroupService();
            _napSiteService = siteService ?? new SiteService();
            _napFunctionService = functionService ?? new FunctionService();
        }

        public ActionResult Index()
        {
            return View(LogInViewURL);
        }

        /// <summary>
        /// Sign in action.
        /// </summary>
        /// <returns>Sign in result.</returns>
        public RedirectResult SignIn()
        {
            if (AppSession.CurrentUserMenu == null || AppSession.CurrentUser == null)
            {
                return new RedirectResult(AppSetting.LoginUrl, true);
            }
            else
            {
                return new RedirectResult("~/Home/Index");
            }
        }

        /// <summary>
        /// Lon In Action.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>The log in result.</returns>
        [HttpPost]
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public ActionResult Index(string username, string password)
        {
            try
            {
                Session.Clear();
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    ViewBag.Message = Labels.LogIn_InvalidInput;
                    return View(LogInViewURL);
                }

                var user = _userNapService.GetByUsername(username.Trim()) as Fanex.NAP.Core.Model.User;
                if (user == null)
                {
                    ViewBag.Message = ViewResources.Invalid_Username_or_Password;
                    return View(LogInViewURL);
                }

                password = Cryptography.CfsEnCode(password);
                if (password != user.Password)
                {
                    ViewBag.Message = ViewResources.Invalid_Username_or_Password;
                    return View(LogInViewURL);
                }

                var group = user.Group == null ? null : _napGroupService.GetById(user.Group.Id);

                // gets all sites of logging-in user
                var siteLevelList = _napSiteService.GetSiteLevelByUser(user.Id, Environment);
                if ((group != null && !group.IsActive) ||
                    !siteLevelList.Where(x => x.IsActive).Any())
                {
                    // user account was suspended on all sites
                    if (siteLevelList.Count() > 0)
                    {
                        ViewBag.Message = ViewResources.Your_account_was_suspended;
                    }
                    else
                    {
                        // logging-in user belongs to no site
                        ViewBag.Message = ViewResources.You_have_no_authorization;
                    }

                    return View(LogInViewURL);
                }

                var criteria = new UserCheckInUserCriteria
                {
                    UserId = user.Id,
                    SiteId = this.SiteId,
                    NetSessionId = Session.SessionID,
                    Ip = CurrentContext.IP,
                    LoginDate = DateTime.Now,
                    BrowserName = CurrentContext.BrowserName,
                    CountryCode = CurrentContext.CountryCode,
                    Domain = CurrentContext.DomainName,
                    Platform = CurrentContext.Platform
                };

                _userNapService.UserCheckIn(ref criteria);

                _napCurrentSession.Level = _userNapService.GetUserSiteLevel(user.Id, this.SiteId);
                _napCurrentSession.UserId = user.Id;
                _napCurrentSession.Username = username;
                _napCurrentSession.OnUserId = criteria.OnUserId;
                _napCurrentSession.SessionId = Session.SessionID.ToString();
                if (this.SiteId != 0)
                {
                    CreateUserMenu(user);
                }

                CreateAuthInfo(user);
                AppSession.CurrentUser = user;
                AppSession.RoleAccess = GetRoleAccess((new FunctionService()).GetByUserAndSiteIncludeGroup(
                    user.Id,
                    NapSettings.Settings.SiteId,
                    NapSettings.Settings.Environment));
                if (AppSession.CurrentUserMenu == null ||
                    !AppSession.CurrentUserMenu.Where(m => m.Parent == null).Any())
                {
                    // user has no menu permission
                    DoSignOut();
                    ViewBag.Message = Labels.You_have_no_menu_permission;
                    return View(LogInViewURL);
                }

                if (!this.IsSiteLogin)
                {
                    // user has access to multiple sites
                    if (siteLevelList.Count() > 1)
                    {
                        return RedirectToAction("Site", "UserSite");
                    }

                    return RedirectToAction("GoToSite", "UserSite", new { id = siteLevelList.First().Site.Id });
                }

                // checks if logging-in user is active on this site
                bool userIsActiveOnThisSite = false;
                foreach (var siteLevel in siteLevelList)
                {
                    if (siteLevel.Site.Id == this.SiteId && siteLevel.IsActive)
                    {
                        userIsActiveOnThisSite = true;
                        break;
                    }
                }

                if (!userIsActiveOnThisSite)
                {
                    DoSignOut();
                    ViewBag.Message = ViewResources.Your_account_was_suspended;
                    return View(LogInViewURL);
                }

                return Redirect(FormsAuthentication.DefaultUrl);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
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

                    AppSession.RoleAccess = GetRoleAccess((new FunctionService()).GetByUserAndSiteIncludeGroup(
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
        /// Log out action.
        /// </summary>
        /// <returns>Log out result.</returns>
        public ActionResult LogOut()
        {
            var result = new Result();
            try
            {
                _userNapService.UserLogOut(new LogOutUserCriteria
                {
                    OnUserId = _napCurrentSession.OnUserId
                });

                ClearingLoginCookie();
            }
            catch (Exception exception)
            {
                LogExtensions.ExceptionHandler(exception);
            }

            return new RedirectResult(AppSetting.LoginUrl);
        }

        private void CreateUserMenu(User user)
        {
            _napCurrentSession.MenuItemList = (IList<MenuItem>)_menuItemNapService.GetByUserAndSiteIncludeGroup(
                user.Id,
                SiteId,
                Environment,
                true);
            AppSession.CurrentUserMenu = _napCurrentSession.MenuItemList;
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

        private void DoSignOut()
        {
            _userNapService.UserLogOut(new LogOutUserCriteria
            {
                OnUserId = _napCurrentSession.OnUserId
            });
            FormsAuthentication.SignOut();
            _napCurrentSession.ClearAll();
        }

        private void ClearingLoginCookie()
        {
            System.Web.HttpContext.Current.Session.Clear();
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName)
            {
                Expires = DateTime.Now.AddDays(-1)
            };
            System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
            FormsAuthentication.SignOut();
        }
    }
}
