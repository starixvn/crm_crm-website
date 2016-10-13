// -----------------------------------------------------------------------
// <copyright file="UserLoginController.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.User.Controllers
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using Enumerations;
    using Fanex.CRM.Common.Services;
    using Fanex.CRM.Infrastructure.Helpers;
    using Fanex.CRM.User.Resources;
    using Fanex.NAP.Core.Web.Session;
    using Models;
    using NAP.Core.Configuration;
    using NAP.Core.Web;
    using ONEbook.UM.Filters;
    using ONEbook.UM.Services;
    using ONEbook.UM.Services.Impl;
    using Services;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class UserLoginController : Controller
    {
        private const string FormTokenName = "__SecureFormToken";
        private const string LogInViewURL = "~/Areas/User/LogIn/Index.cshtml";

        private readonly ILocalAuthenticationService _localService;
        private readonly ISSOAuthenticationService _ssoService;
        private readonly ICommonAuthenticationService _authenticationService;
        private readonly IMaintenanceService _maintenanceService;
        private readonly CurrentSession _napCurrentSession;

        public UserLoginController()
            : this(null, null, null, null, null)
        {
        }

        public UserLoginController(
            ILocalAuthenticationService localService,
            ISSOAuthenticationService ssoService,
            ICommonAuthenticationService authenticationService,
            IMaintenanceService maintenanceService,
            CurrentSession currentSession)
        {
            _localService = localService ?? new LocalAuthenticationService();
            _ssoService = ssoService ?? new SSOAuthenticationService();
            _authenticationService = authenticationService ?? new CommonAuthenticationService();
            _maintenanceService = maintenanceService ?? new MaintenanceService();

            _napCurrentSession = currentSession ?? new CurrentSession();
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
            if(_maintenanceService.IsUnderMaintenance())
            {
                return RedirectToAction("Offline", "Common");
            }

            CreateNewToken();

            return View(LogInViewURL);
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
        public ActionResult Index(LocalLogin localLogin)
        {
            if (_maintenanceService.IsUnderMaintenance())
            {
                return RedirectToAction("Offline", "Common");
            }

            if (string.IsNullOrEmpty(localLogin.Username) || string.IsNullOrEmpty(localLogin.Password))
            {
                return LoginError(Labels.LogIn_InvalidInput);
            }

            if (!CheckValidToken(localLogin.LoginToken))
            {
                return LoginError(Labels.Login_SessionTimeout);
            }

            localLogin.Environment = NapSettings.Settings.Environment;
            localLogin.SiteId = NapSettings.Settings.SiteId;
            localLogin.SessionId = Session.SessionID;

            var loginStatus = _localService.Login(localLogin, _napCurrentSession);

            if (loginStatus != LoginStatus.LoginSuccessful)
            {
                return LoginError(GetLoginStatusError(loginStatus));
            }

            return Redirect(FormsAuthentication.DefaultUrl);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult SSO(SSOLogin ssoLogin)
        {
            if (_maintenanceService.IsUnderMaintenance())
            {
                return RedirectToAction("Offline", "Common");
            }

            ssoLogin.Environment = NapSettings.Settings.Environment;
            ssoLogin.SiteId = NapSettings.Settings.SiteId;
            ssoLogin.SessionId = Session.SessionID;

            var loginStatus = _ssoService.SingleSignOn(ssoLogin, _napCurrentSession);

            if (loginStatus != LoginStatus.LoginSuccessful)
            {
                return LoginError(GetLoginStatusError(loginStatus));
            }

            return Redirect(FormsAuthentication.DefaultUrl);
        }

        /// <summary>
        /// Log out action.
        /// </summary>
        /// <returns>Log out result.</returns>
        public ActionResult LogOut()
        {
            try
            {
                _authenticationService.Logout(_napCurrentSession);
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
        [Authorize]
        public ActionResult CheckUserOnlineStatus()
        {
            var recheckInStatus = _authenticationService.RecheckIn(_napCurrentSession);

            return Json(new
            {
                OnUserId = _napCurrentSession.OnUserId,
                ErrCode = (int)recheckInStatus,
                ErrMsg = GetRecheckInStatusError(recheckInStatus)
            });
        }

        private bool CheckValidToken(string loginToken)
        {
            return !string.IsNullOrWhiteSpace(loginToken)
                && FormToken == loginToken
                && Request.Cookies[FormTokenName] != null
                && loginToken == Request.Cookies[FormTokenName].Value;
        }

        private void CreateNewToken()
        {
            string loginToken = Guid.NewGuid().ToString("N");
            FormToken = loginToken;
            ViewBag.LoginToken = loginToken;

            AddTokenToCookie(loginToken);
        }

        private ActionResult LoginError(string errorMessage)
        {
            ViewBag.Message = errorMessage;
            CreateNewToken();

            return View(LogInViewURL);
        }

        private void AddTokenToCookie(string loginToken)
        {
            // Create the non-persistent secure form token cookie
            var responseCookie = new HttpCookie(FormTokenName)
            {
                // Set the HttpOnly property to prevent the cookie from
                // being accessed by client side script
                HttpOnly = true,

                // Add login token to the cookie value
                Value = loginToken
            };

            // If we are using SSL, the cookie should be set to secure to
            // prevent it from being sent over HTTP connections
            if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
            {
                responseCookie.Secure = true;
            }

            // Add the cookie to the response
            Response.Cookies.Set(responseCookie);
        }

        private string GetLoginStatusError(LoginStatus loginStatus)
        {
            string error = string.Empty;
            switch (loginStatus)
            {
                case LoginStatus.InvalidUsernameOrPassword:
                    error = ViewResources.Invalid_Username_or_Password;
                    break;

                case LoginStatus.UserSuspended:
                    error = ViewResources.Your_account_was_suspended;
                    break;

                case LoginStatus.UserNoAuthorization:
                    error = ViewResources.You_have_no_authorization;
                    break;

                case LoginStatus.UserNoMenuPermission:
                    error = Labels.You_have_no_menu_permission;
                    break;

                default:
                    break;
            }

            return error;
        }

        private string GetRecheckInStatusError(RecheckInStatus recheckInStatus)
        {
            string error = string.Empty;
            switch (recheckInStatus)
            {
                case RecheckInStatus.KickOut:
                    error = "Is kickout";
                    break;

                case RecheckInStatus.InvalidSite:
                    error = "Site is invalid";
                    break;

                case RecheckInStatus.UnderMaintenance:
                    error = "UM";
                    break;

                default:
                    break;
            }

            return error;
        }
    }
}