namespace Fanex.CRM.Common.Services
{
    using Fanex.NAP.Core.Business;
    using NAP.Core.Configuration;
    using NAP.Core.Service.Impl;
    using NAP.Core.Service.Interface;

    public class PermissionService : IPermissionService
    {
        private readonly IUserService _userService;

        public PermissionService()
            : this(null)
        {
        }

        public PermissionService(IUserService userService)
        {
            _userService = userService ?? new UserService();
        }

        public bool HasReadPermission(string functionName)
        {
            var permission = GetPermission(functionName);
            return permission == (int)Constant.Policy.Read ||
                   permission == (int)Constant.Policy.Host;
        }

        public bool HasHostPermission(string functionName)
        {
            return _userService.IsHavingPermission(AppSession.CurrentUser.Id, NapSettings.Settings.SiteId, functionName, Constant.Policy.Host);
        }

        public bool HasPermission(string functionName, Constant.Policy policy)
        {
            var permission = GetPermission(functionName);
            return permission >= (int)policy;
        }

        public Constant.Policy GetPolicy(string functionName)
        {
            return (Constant.Policy)GetPermission(functionName);
        }

        private int GetPermission(string functionName)
        {
            return _userService.GetPermission(AppSession.CurrentUser.Id, NapSettings.Settings.SiteId, functionName);
        }
    }
}