namespace Fanex.CRM.Common.Services
{
    using Fanex.NAP.Core.Business;

    public interface IPermissionService
    {
        bool HasReadPermission(string functionName);

        bool HasHostPermission(string functionName);

        bool HasPermission(string functionName, Constant.Policy policy);

        Constant.Policy GetPolicy(string functionName);
    }
}