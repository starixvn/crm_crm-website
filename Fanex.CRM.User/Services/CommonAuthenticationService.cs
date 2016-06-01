namespace Fanex.CRM.User.Services
{
    using System;
    using System.Web;
    using System.Web.Security;
    using Enumerations;
    using NAP.Core.Configuration;
    using NAP.Core.Service.Impl;
    using NAP.Core.Service.Interface;
    using NAP.Core.Web.Session;
    using ONEbook.UM.Services;
    using ONEbook.UM.Services.Impl;

    public class CommonAuthenticationService : ICommonAuthenticationService
    {
        private readonly ISiteService _siteService;
        private readonly ISecurityService _securityService;
        private readonly IMaintenanceService _maintenanceService;

        public CommonAuthenticationService()
            : this(null, null, null)
        {
        }

        public CommonAuthenticationService(
            ISiteService siteService,
            ISecurityService securityService,
            IMaintenanceService maintenanceService)
        {
            _siteService = siteService ?? new SiteService();
            _securityService = securityService ?? new SecurityService();
            _maintenanceService = maintenanceService ?? new MaintenanceService();
        }

        public RecheckInStatus RecheckIn(ICurrentSession currentSession)
        {
            var settings = NapSettings.Settings;

            if (!settings.EnableReCheckIn)
            {
                return RecheckInStatus.Success;
            }

            if (_maintenanceService.IsUnderMaintenance())
            {
                Logout(currentSession);
                return RecheckInStatus.UnderMaintenance;
            }

            if (!_siteService.IsActivated(settings.SiteId))
            {
                Logout(currentSession);
                return RecheckInStatus.InvalidSite;
            }

            int status = _securityService.ReCheckIn(currentSession.OnUserId, settings.SiteId);

            return status > 0 ? RecheckInStatus.KickOut : RecheckInStatus.Success;
        }

        public void Logout(ICurrentSession currentSession)
        {
            _securityService.Logout(currentSession.OnUserId);
            HttpContext.Current.Session.Abandon();
            HttpContext.Current.Session.Clear();
            currentSession.ClearAll();
            HttpContext.Current.Response.Cookies.Clear();
            FormsAuthentication.SignOut();
        }

    }
}
