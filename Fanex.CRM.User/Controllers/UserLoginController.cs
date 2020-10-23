namespace Fanex.CRM.User.Controllers
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using Enumerations;
    using Fanex.Common;
    using Fanex.CRM.Common.Extensions;
    using Fanex.CRM.Common.Helpers;
    using Fanex.CRM.Common.Services;
    using Fanex.CRM.User.Resources;
    using Models;
    using NAP.Core.Configuration;
    using NAP.Core.Web;
    using ONEbook.UM.Services;
    using ONEbook.UM.Services.Impl;
    using Services;

    public class UserLoginController : Controller
    {
        private const string FormTokenName = "__SecureFormToken";
        private const string LogInViewURL = "~/Areas/User/LogIn/Index{0}.cshtml";
        private readonly ILocalAuthenticationService _localService;
        private readonly ISSOAuthenticationService _ssoService;
        private readonly ICommonAuthenticationService _authenticationService;
        private readonly IMaintenanceService _maintenanceService;

        public UserLoginController()
            : this(null, null, null, null)
        {
        }

        public UserLoginController(
            ILocalAuthenticationService localService,
            ISSOAuthenticationService ssoService,
            ICommonAuthenticationService authenticationService,
            IMaintenanceService maintenanceService
            )
        {
            _localService = localService ?? new LocalAuthenticationService();
            _ssoService = ssoService ?? new SSOAuthenticationService();
            _authenticationService = authenticationService ?? new CommonAuthenticationService();
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

        protected string ExternalDomain => ExternalRedirectHelper.GetExternalDomain(Url.Content("~"));

        public ActionResult Index()
        {
            if (_maintenanceService.IsUnderMaintenance())
            {
                return RedirectToAction("Offline", "Common");
            }

            CreateNewToken();

            return View(string.Format(LogInViewURL, NapSettings.Settings.SiteId.ToString()
                == AppSetting.CN88SiteId ? ".CN88" : ""));
        }

        public ActionResult UnknowUserRole()
        {
            if (_maintenanceService.IsUnderMaintenance())
            {
                return RedirectToAction("Offline", "Common");
            }
            return View("~/Areas/User/LogIn/UnknowUserRole.cshtml");
        }

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

            var loginStatus = _localService.Login(localLogin);
            if (loginStatus == LoginStatus.LoginSuccessful)
            {
                var url = string.Format("{0}/auth/SignIn?Username={1}&Password={2}&OnUserId={3}&Token={4}",
                    ExternalDomain,
                    localLogin.Username,
                    localLogin.Password,
                    Cryptography.CfsEnCrypt(AppSession.CurrentUser.OnUserId.ToString()),
                    localLogin.LoginToken);
                return new RedirectResult(url, false);
            }

            return ProcessLoginResult(loginStatus);
        }

        private ActionResult ProcessLoginResult(LoginStatus loginStatus)
        {
            if (loginStatus == LoginStatus.LoginSuccessful)
            {
                return Redirect(FormsAuthentication.DefaultUrl);
            }

            if (loginStatus == LoginStatus.NotKnowUserRole)
            {
                return RedirectToAction("UnknowUserRole", "UserLogin");
            }

            return LoginError(GetLoginStatusError(loginStatus));
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

            var loginStatus = _ssoService.SingleSignOn(ssoLogin);
            if (loginStatus == LoginStatus.LoginSuccessful)
            {
                var url = string.Format("{0}/auth/SingleSignOn?UserId={1}&OnUserId={2}&Time={3}",
                    ExternalDomain,
                    ssoLogin.UserId,
                    ssoLogin.OnUserId,
                    ssoLogin.Time);
                return new RedirectResult(url, false);
            }

            return ProcessLoginResult(loginStatus);
        }

        public ActionResult LogOut()
        {
            try
            {
                _authenticationService.Logout(AppSession.CurrentUser);
            }
            catch (Exception exception)
            {
                LogExtensions.ExceptionHandler(exception);
            }

            var url = string.Format("{0}/auth/SignOut", ExternalDomain);
            return new RedirectResult(url, false);
        }

        [HttpPost]
        [Authorize]
        public ActionResult CheckUserOnlineStatus()
        {
            var user = AppSession.CurrentUser;
            RecheckInStatus recheckInStatus = user != null ? _authenticationService.RecheckIn(user) : RecheckInStatus.KickOut;

            return Json(new
            {
                user?.OnUserId,
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

            return View(string.Format(LogInViewURL, NapSettings.Settings.SiteId.ToString()
                == AppSetting.CN88SiteId ? ".CN88" : ""));
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

        private static string GetLoginStatusError(LoginStatus loginStatus)
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
                    error = Labels.You_dont_have_permission;
                    break;

                case LoginStatus.UserNoMenuPermission:
                    error = Labels.You_have_no_menu_permission;
                    break;

                default:
                    // nothing
                    break;
            }

            return error;
        }

        private static string GetRecheckInStatusError(RecheckInStatus recheckInStatus)
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
                    // nothing
                    break;
            }

            return error;
        }
    }
}