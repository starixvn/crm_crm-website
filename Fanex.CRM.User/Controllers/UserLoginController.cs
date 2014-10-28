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
    using System.Security.Cryptography;
    using System.Text;
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
        private const string FormTokenName = "__SecureFormToken";
        private const string LogInViewURL = "~/Areas/User/LogIn/Index.cshtml";
        private readonly int _timeExipration = 30;
        private IUserService _userNapService;
        private IMenuItemService _menuItemNapService;
        private ICurrentSession _napCurrentSession;
        private IGroupService _napGroupService;
        private ISiteService _napSiteService;
        private IFunctionService _napFunctionService;
        private IMaintenanceService _maintenanceService;
        private HashAlgorithm _hashAlgorithm = new SHA1Managed();

        public UserLoginController()
            : this(null, null, null, null, null, null, null)
        {
        }

        public UserLoginController(
            IUserService userService,
            IGroupService groupService,
            ISiteService siteService,
            IMenuItemService menuItemService,
            ICurrentSession currentSession,
            IFunctionService functionService,
            IMaintenanceService maintenanceService)
        {
            _userNapService = userService ?? new UserService();
            _menuItemNapService = menuItemService ?? new MenuItemService();
            _napCurrentSession = currentSession ?? new CurrentSession();

            _napGroupService = groupService ?? new GroupService();
            _napSiteService = siteService ?? new SiteService();
            _napFunctionService = functionService ?? new FunctionService();
            _maintenanceService = maintenanceService ?? new MaintenanceService();
        }

        public string FormToken
        {
            get
            {
                if (Session[FormTokenName] == null)
                {
                    return string.Empty;
                }

                return Session[FormTokenName].ToString();
            }

            set
            {
                Session[FormTokenName] = value;
            }
        }

        /// <summary>
        /// Login Page.
        /// </summary>
        /// <returns>Return view.</returns>
        public ActionResult Index()
        {
            var isCheckUM = NAP.Core.Configuration.NapSettings.Settings.EnableCheckUM;
            if (!isCheckUM)
            {
                CreateNewToken();
                return View(LogInViewURL);
            }

            var maintenanceInfo = _maintenanceService.GetDowntimeInfo();
            if (maintenanceInfo.IsDowntime)
            {
                return RedirectToAction("Offline", "Common");
            }

            CreateNewToken();
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
                return new RedirectResult("~/UserLogin/Index", true);
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
        /// <param name="loginToken">The login token.</param>
        /// <returns>The log in result.</returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public ActionResult Index(string username, string password, string loginToken)
        {
            try
            {
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    ViewBag.Message = Labels.LogIn_InvalidInput;
                    CreateNewToken();
                    return View(LogInViewURL);
                }

                var user = _userNapService.GetByUsername(username.Trim()) as Fanex.NAP.Core.Model.User;
                if (user == null)
                {
                    ViewBag.Message = ViewResources.Invalid_Username_or_Password;
                    CreateNewToken();
                    return View(LogInViewURL);
                }

                if (!CheckValidToken(loginToken))
                {
                    ViewBag.Message = Labels.Login_SessionTimeout;
                    CreateNewToken();
                    return View(LogInViewURL);
                }

                // use exactly the same way we create hash value at client side
                string hashPassword = SHA256(user.Password + loginToken);
                //// password = Cryptography.CfsEnCode(password);
                if (password != hashPassword)
                {
                    ViewBag.Message = ViewResources.Invalid_Username_or_Password;
                    CreateNewToken();
                    return View(LogInViewURL);
                }
                else
                {
                    //// Form token can be used only one time                
                    ClearToken();

                    var group = user.Group == null ? null : _napGroupService.GetById(user.Group.Id);

                    //// gets all sites of logging-in user
                    var siteLevelList = _napSiteService.GetSiteLevelByUser(user.Id, Environment);
                    if ((group != null && !group.IsActive) ||
                        !siteLevelList.Where(x => x.IsActive).Any())
                    {
                        //// user account was suspended on all sites
                        if (siteLevelList.Count() > 0)
                        {
                            ViewBag.Message = ViewResources.Your_account_was_suspended;
                        }
                        else
                        {
                            //// logging-in user belongs to no site
                            ViewBag.Message = ViewResources.You_have_no_authorization;
                        }

                        return View(LogInViewURL);
                    }

                    AssignUserPermissionToSession(username, user);

                    if (AppSession.CurrentUserMenu == null ||
                        !AppSession.CurrentUserMenu.Where(m => m.Parent == null).Any())
                    {
                        //// user has no menu permission
                        DoSignOut();
                        ViewBag.Message = Labels.You_have_no_menu_permission;
                        return View(LogInViewURL);
                    }

                    if (!this.IsSiteLogin)
                    {
                        //// user has access to multiple sites
                        if (siteLevelList.Count() > 1)
                        {
                            return RedirectToAction("Site", "UserSite");
                        }

                        return RedirectToAction("GoToSite", "UserSite", new { id = siteLevelList.First().Site.Id });
                    }

                    //// checks if logging-in user is active on this site
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
        /// <param name="redirectUrl">The redirect url.</param>
        /// <returns>The index page.</returns>
        [AllowAnonymous]
        [HttpGet]
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public ActionResult SSO(string userId, string onuserid, string time, Uri redirectUrl = null)
        {
            Session.Clear();

            string currentTime = this._userNapService.UserSSOGetTime();
            TimeSpan expiration = new TimeSpan();

            userId = Cryptography.CfsDeCrypt(userId.Replace(" ", "+"));
            onuserid = Cryptography.CfsDeCrypt(onuserid.Replace(" ", "+"));
            time = Cryptography.CfsDeCrypt(time.Replace(" ", "+"));
            expiration = Convert.ToDateTime(currentTime) - Convert.ToDateTime(time);

            if (expiration.TotalSeconds <= this._timeExipration)
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

                this._userNapService.UserSSOCheckIn(ref criteria);

                //// do SSO check-in
                if (criteria.OnUserId != 0)
                {
                    var user = _userNapService.GetById(criteria.UserId) as User;

                    //// saves user session & auth info
                    this._napCurrentSession.UserId = user.Id;
                    this._napCurrentSession.Username = user.Username;
                    this._napCurrentSession.OnUserId = criteria.OnUserId;
                    this._napCurrentSession.SessionId = Session.SessionID.ToString();
                    this._napCurrentSession.Level = this._userNapService.GetUserSiteLevel(user.Id, this.SiteId);

                    AppSession.CurrentUser = user;

                    AppSession.RoleAccess = GetRoleAccess((new FunctionService()).GetByUserAndSiteIncludeGroup(
                        user.Id,
                        NapSettings.Settings.SiteId,
                        NapSettings.Settings.Environment));

                    if (this.SiteId != 0)
                    {
                        _napCurrentSession.MenuItemList = this._menuItemNapService.GetByUserAndSiteIncludeGroup(
                                                                                user.Id,
                                                                                SiteId,
                                                                                NapSettings.Settings.Environment,
                                                                                true) as IList<MenuItem>;
                        AppSession.CurrentUserMenu = _napCurrentSession.MenuItemList;
                    }

                    CreateAuthInfo(user);
                    if (redirectUrl != null && !string.IsNullOrEmpty(redirectUrl.ToString()))
                    {
                        AppSession.RedirectUrl = redirectUrl.ToString();
                    }
                    //// saves user session & auth info
                    return this.Redirect(FormsAuthentication.DefaultUrl);
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

            return RedirectToAction("Index", "UserLogin");
        }

        /// <summary>
        /// Checks the user online status.
        /// </summary>
        /// <returns>The action result.</returns>
        [HttpPost]
        public ActionResult CheckUserOnlineStatus()
        {
            try
            {
                var settings = NapSettings.Settings;
                if (!settings.EnableReCheckIn)
                {
                    return Json(new { ErrCode = 0 });
                }

                if (settings.EnableCheckUM)
                {
                    if (_maintenanceService.GetDowntimeInfo().IsDowntime)
                    {
                        ClearingLoginCookie();
                        return Json(new { ErrCode = 3, ErrMsg = "UM" });
                    }
                }

                var siteStatus = _napSiteService.GetStatusById(this.SiteId);
                if (!siteStatus)
                {
                    ClearingLoginCookie();
                    return Json(new { ErrCode = 2, ErrMsg = "Site is invalid" });
                }

                UserReCheckInUserCriteria criteria = new UserReCheckInUserCriteria { OnUserId = _napCurrentSession.OnUserId };
                _userNapService.ReUserCheckIn(ref criteria);
                if (criteria.IsKickedOut)
                {
                    return Json(new { ErrCode = 1, ErrMsg = "Is kickout" });
                }

                return Json(new { OnUserId = _napCurrentSession.OnUserId, ErrCode = 0 });
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
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

        /// <summary>
        /// Add Token To Cookie.
        /// </summary>
        /// <param name="loginToken">The login Token.</param>
        private void AddTokenToCookie(string loginToken)
        {
            //// Create the non-persistent secure form token cookie
            var responseCookie = new HttpCookie(FormTokenName)
            {
                //// Set the HttpOnly property to prevent the cookie from
                //// being accessed by client side script
                HttpOnly = true,

                //// Add login token to the cookie value
                Value = loginToken
            };

            //// If we are using SSL, the cookie should be set to secure to
            //// prevent it from being sent over HTTP connections
            if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
            {
                responseCookie.Secure = true;
            }

            //// Add the cookie to the response
            Response.Cookies.Set(responseCookie);
        }

        /// <summary>
        /// Check Valid Token.
        /// </summary>
        /// <param name="loginToken">The login Token.</param>
        /// <returns>Return validation result.</returns>
        private bool CheckValidToken(string loginToken)
        {
            return !string.IsNullOrWhiteSpace(loginToken)
                && FormToken == loginToken
                && Request.Cookies[FormTokenName] != null
                && loginToken == Request.Cookies[FormTokenName].Value;
        }

        /// <summary>
        /// Clear Token.
        /// </summary>
        private void ClearToken()
        {
            //// delete token session
            Session.Remove(FormTokenName);

            //// delete token cookie 
            if (Request.Cookies[FormTokenName] != null)
            {
                HttpCookie tokenCookie = new HttpCookie(FormTokenName);
                //// Here, we are setting the time to a previous time.
                //// When the browser detect it next time, it will be deleted automatically.
                tokenCookie.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(tokenCookie);
            }
        }

        /// <summary>
        /// Create New Token.
        /// </summary>
        private void CreateNewToken()
        {
            // generate a new token
            string loginToken = Guid.NewGuid().ToString("N");
            FormToken = loginToken;
            ViewBag.LoginToken = loginToken;

            AddTokenToCookie(loginToken);
        }

        /// <summary>
        /// Get hashed string by hash algorithm.
        /// </summary>
        /// <param name="input">Must be not whitespace or null.</param>
        /// <returns>Return string without - and all chars are converted to lower.</returns>
        private string GetHash(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentException("Hey guy! I dont want to hash an empty or white space string");
            }

            var hashData = _hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));
            string encrypted = BitConverter.ToString(hashData).Replace("-", string.Empty).ToLower();

            return encrypted;
        }

        private string SHA256(string input)
        {
            _hashAlgorithm = new SHA256Managed();
            return GetHash(input);
        }

        /// <summary>
        /// Assign User Permission To Session.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="user">The user object.</param>
        private void AssignUserPermissionToSession(string username, User user)
        {
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
        }
    }
}
