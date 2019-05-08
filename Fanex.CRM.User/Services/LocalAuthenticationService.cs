namespace Fanex.CRM.User.Services
{
    using System;
    using System.Linq;
    using System.Web;
    using Common.Services;
    using Enumerations;
    using Fanex.CRM.Common.Models;
    using Helpers;
    using Models;
    using NAP.Core.Context;
    using NAP.Core.Models;
    using NAP.Core.Models.ParameterObject;
    using NAP.Core.Service.Impl;
    using NAP.Core.Service.Interface;
    using NAP.Core.Web.Context;

    public class LocalAuthenticationService : ILocalAuthenticationService
    {
        private const string LoginTokenName = "__SecureFormToken";

        private readonly IUserService _userService;
        private readonly ISecurityService _securityService;
        private readonly IGroupService _groupService;
        private readonly ICurrentContext _currentContext;

        public LocalAuthenticationService()
            : this(null, null, null, null, null)
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
            _currentContext = currentContext ?? new CurrentContext();
        }

        public LoginStatus Login(LocalLogin localLogin)
        {
            var user = _userService.Get(localLogin.Username);
            if (user == null)
            {
                return LoginStatus.InvalidUsernameOrPassword;
            }

            var crmUser = new CRMUser(user);

            var validatedStatus = ValidateUser(crmUser, localLogin);

            if (validatedStatus != LoginStatus.ValidatedSuccess)
            {
                return validatedStatus;
            }

            return ProcessLogin(crmUser, localLogin);
        }

        #region Validation

        private LoginStatus ValidateUser(CRMUser user, LocalLogin localLogin)
        {

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

        private bool ValidatePassword(CRMUser user, LocalLogin localLogin)
        {
            var hashUserPassword = SecurityHelper.HashText(string.Format("{0}{1}", user.Password, localLogin.LoginToken), HashType.Sha256);

            return string.Equals(localLogin.Password, hashUserPassword, StringComparison.CurrentCulture);
        }

        private bool IsExistNapSite(CRMUser user, LocalLogin localLogin)
        {
            return _userService
                .GetSiteLevel(user.Id, localLogin.Environment)
                .FirstOrDefault(s => s.Site.Id == localLogin.SiteId && s.IsActive) != null;
        }

        private bool IsExistNapGroup(CRMUser user)
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

        private bool IsExistMenuItems(CRMUser user, LocalLogin localLogin)
        {
            return _userService.GetMenuItem(user.Id, localLogin.SiteId, localLogin.Environment, true).Any(m => m.Parent == null);
        }

        #endregion Validation

        private LoginStatus ProcessLogin(CRMUser user, LocalLogin localLogin)
        {
            var onuserid = ProcessNapLogin(user, localLogin);

            if (onuserid <= 0)
            {
                return LoginStatus.InvalidUsernameOrPassword;
            }

            var menus = _userService.GetMenuItem(user.Id, localLogin.SiteId, localLogin.Environment, true).ToList();
            user.OnUserId = onuserid;
            AuthencationHelper.AssignAuthenticationCookie(_currentContext.CurrentDomain.Path, user);
            AuthencationHelper.AssignAppSession(user, menus);

            return LoginStatus.LoginSuccessful;
        }

        private int ProcessNapLogin(CRMUser user, LocalLogin localLogin)
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
      
    }
}
