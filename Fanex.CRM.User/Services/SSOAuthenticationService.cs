namespace Fanex.CRM.User.Services
{
    using System;
    using System.Linq;
    using Enumerations;
    using Fanex.Common;
    using Fanex.CRM.Common.Models;
    using Helpers;
    using Models;
    using NAP.Core.Context;
    using NAP.Core.Models.ParameterObject;
    using NAP.Core.Service.Impl;
    using NAP.Core.Service.Interface;
    using NAP.Core.Web.Context;
    using NAP.Core.Web.Session;

    public class SSOAuthenticationService : ISSOAuthenticationService
    {
        private const int SSOExpiredTimeInSeconds = 30;
        private readonly IUserService _userService;
        private readonly ISecurityService _securityService;
        private readonly ICurrentContext _currentContext;

        public SSOAuthenticationService()
            : this(null, null, null)
        {
        }

        public SSOAuthenticationService(
            IUserService userService,
            ISecurityService securityService,
            ICurrentContext currentContext)
        {
            _userService = userService ?? new UserService();
            _securityService = securityService ?? new SecurityService();
            _currentContext = currentContext ?? new CurrentContext();
        }

        public LoginStatus SingleSignOn(SSOLogin ssoLogin)
        {
            if (ValidateTime(ssoLogin.Time))
            {
                var userId = Converter.ToInt(Cryptography.CfsDeCrypt(ssoLogin.UserId.Replace(" ", "+")));
                var onuserid = Converter.ToInt(Cryptography.CfsDeCrypt(ssoLogin.OnUserId.Replace(" ", "+")));
                var afterLoginUserId = ProcessSSOLogin(userId, onuserid, ssoLogin);

                if (afterLoginUserId > 0)
                {
                    var user = _userService.Get(userId);
                    if (user != null)
                    {
                        var crmUser = new CRMUser(user);
                        var menus = _userService.GetMenuItem(userId, ssoLogin.SiteId, ssoLogin.Environment, true).ToList();

                        AuthencationHelper.SignIn(crmUser);
                        AuthencationHelper.AssignAppSession(crmUser, menus);

                        return LoginStatus.LoginSuccessful;
                    }
                }
            }

            return LoginStatus.InvalidUsernameOrPassword;
        }

        private bool ValidateTime(string time)
        {
            var cfsDescryptTime = Cryptography.CfsDeCrypt(time.Replace(" ", "+"));
            var expiration = Converter.ToDateTime(_securityService.GetTime()) - Converter.ToDateTime(cfsDescryptTime);

            return expiration.TotalSeconds <= SSOExpiredTimeInSeconds;
        }

        private int ProcessSSOLogin(int userId, int onuserid, SSOLogin ssoLogin)
        {
            var ssoLoginParameter = new SSOLoginParameter
            {
                OnUserId = 0,
                UserId = userId,
                CheckOnUserId = onuserid,
                LoginDate = DateTime.Now,
                NetSessionId = ssoLogin.SessionId,
                SiteId = ssoLogin.SiteId,
                IP = _currentContext.IP,
                BrowserName = _currentContext.BrowserName,
                CountryCode = _currentContext.CountryCode,
                Domain = _currentContext.DomainName,
                Platform = _currentContext.Platform
            };

            return _securityService.SSOLogin(ssoLoginParameter);
        }
    }
}
