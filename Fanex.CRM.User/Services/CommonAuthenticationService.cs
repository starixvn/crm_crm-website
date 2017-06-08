namespace Fanex.CRM.User.Services
{
    using Enumerations;
    using NAP.Core.Configuration;
    using NAP.Core.Service.Impl;
    using NAP.Core.Service.Interface;
    using NAP.Core.Web.Session;
    using ONEbook.UM.Services;
    using ONEbook.UM.Services.Impl;
    using System.Web;
    using System.Web.Security;

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

            if (_maintenanceService.IsUnderMaintenance())
            {
                return RecheckInStatus.UnderMaintenance;
            }

            if (!settings.EnableReCheckIn || currentSession.OnUserId <= 0)
            {
                return RecheckInStatus.Success;
            }

            if (!_siteService.IsActivated(settings.SiteId))
            {
                return RecheckInStatus.InvalidSite;
            }

            int status = _securityService.ReCheckIn(currentSession.OnUserId, settings.SiteId);

            return status > 0 ? RecheckInStatus.KickOut : RecheckInStatus.Success;
        }

        public void Logout(ICurrentSession currentSession)
        {
            if (currentSession != null)
            {
                if (currentSession.OnUserId > 0)
                {
                    _securityService.Logout(currentSession.OnUserId);
                }

                currentSession.ClearAll();
            }

            HttpContext.Current.Response.Cookies.Clear();
            FormsAuthentication.SignOut();
        }
    }
}