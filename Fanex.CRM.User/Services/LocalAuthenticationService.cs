namespace Fanex.CRM.User.Services
{
    using System;
    using System.Linq;
    using System.Web;
    using Common.Services;
    using Enumerations;
    using Helpers;
    using Models;
    using NAP.Core.Context;
    using NAP.Core.Models;
    using NAP.Core.Models.ParameterObject;
    using NAP.Core.Service.Impl;
    using NAP.Core.Service.Interface;
    using NAP.Core.Web.Context;
    using NAP.Core.Web.Session;

    public class LocalAuthenticationService : ILocalAuthenticationService
    {
        private const string LoginTokenName = "__SecureFormToken";

        private readonly IUserService _userService;
        private readonly ISecurityService _securityService;
        private readonly IGroupService _groupService;
        private readonly IPermissionService _permissionService;
        private readonly ICurrentContext _currentContext;

        public LocalAuthenticationService()
            :this(null, null, null, null, null)
        {
        }

        public LocalAuthenticationService(
            IUserService userService, 
            ISecurityService securityService,
            IGroupService groupService,
            IPermissionService permissionService,
            ICurrentContext currentContext)
        {
            _userService = userService ?? new UserService();
            _securityService = securityService ?? new SecurityService();
            _groupService = groupService ?? new GroupService();
            _permissionService = permissionService ?? new PermissionService();
            _currentContext = currentContext ?? new CurrentContext();
        }

        public LoginStatus Login(LocalLogin localLogin, ICurrentSession napCurrentSession)
        {
            var user = _userService.Get(localLogin.Username);
            var validatedStatus = ValidateUser(user, localLogin);

            if (validatedStatus != LoginStatus.ValidatedSuccess)
            {
                return validatedStatus;
            }

            return ProcessLogin(user, localLogin, napCurrentSession);
        }

        #region Validation

        private LoginStatus ValidateUser(User user, LocalLogin localLogin)
        {
            if (user == null)
            {
                return LoginStatus.InvalidUsernameOrPassword;
            }

            if (!ValidatePassword(user, localLogin))
            {
                return LoginStatus.InvalidUsernameOrPassword;
            }

            if (!IsExistNapSite(user, localLogin))
            {
                return LoginStatus.UserSuspended;
            }

            if (!IsExistNapGroup(user))
            {
                return LoginStatus.UserNoAuthorization;
            }

            if (!IsExistMenuItems(user, localLogin))
            {
                return LoginStatus.UserNoMenuPermission;
            }

            return LoginStatus.ValidatedSuccess;
        }

        private bool ValidatePassword(User user, LocalLogin localLogin)
        {
            var hashUserPassword = SecurityHelper.HashText(string.Format("{0}{1}", user.Password, localLogin.LoginToken), HashType.Sha256);

            return string.Equals(localLogin.Password, hashUserPassword, StringComparison.CurrentCulture);
        }

        private bool IsExistNapSite(User user, LocalLogin localLogin)
        {
            return _userService
                .GetSiteLevel(user.Id, localLogin.Environment)
                .FirstOrDefault(s =>  s.Site.Id == localLogin.SiteId && s.IsActive) != null;
        }

        private bool IsExistNapGroup(User user)
        {
            if (user.Group != null)
            {
                var group = _groupService.Get(user.Group.Id);

                if (group != null && !group.IsActive)
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsExistMenuItems(User user, LocalLogin localLogin)
        {
            return _userService.GetMenuItem(user.Id, localLogin.SiteId, localLogin.Environment, true).Any(m => m.Parent == null);
        }

        #endregion Validation

        private LoginStatus ProcessLogin(User user, LocalLogin localLogin, ICurrentSession napCurrentSession)
        {
            var onuserid = ProcessNapLogin(user, localLogin);

            if (onuserid <= 0)
            {
                return LoginStatus.InvalidUsernameOrPassword;
            }

            var menus = _userService.GetMenuItem(user.Id, localLogin.SiteId, localLogin.Environment, true).ToList();
            var userLevel = _userService.GetUserLevel(user.Id, localLogin.SiteId);

            ClearToken();
            
            AuthencationHelper.AssignAuthenticationCookie(_currentContext.CurrentDomain.Path, user, onuserid);
            AuthencationHelper.AssignCurrentSession(napCurrentSession, user, menus, userLevel, onuserid, localLogin.SessionId);
            AuthencationHelper.AssignAppSession(user, menus);

            return LoginStatus.LoginSuccessful;
        }
        
        private int ProcessNapLogin(User user, LocalLogin localLogin)
        {
            return _securityService.Login(
                new LoginParameter
                {
                    UserId = user.Id,
                    LoginDate = DateTime.Now,
                    NetSessionId = localLogin.SessionId,
                    SiteId = localLogin.SiteId,
                    Ip = _currentContext.IP,
                    BrowserName = _currentContext.BrowserName,
                    CountryCode = _currentContext.CountryCode,
                    Domain = _currentContext.DomainName,
                    Platform = _currentContext.Platform
                });
        }

        #region helper

        private void ClearToken()
        {
            var cookies = HttpContext.Current.Request.Cookies;
            if (cookies[LoginTokenName] != null)
            {
                var tokenCookie = new HttpCookie(LoginTokenName);
                tokenCookie.Expires = DateTime.Now.AddDays(-1);

                HttpContext.Current.Response.Cookies.Add(tokenCookie);
            }

            HttpContext.Current.Session.Remove(LoginTokenName);
        }
        #endregion helper
    }
}
