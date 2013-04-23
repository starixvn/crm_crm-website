
namespace Fanex.CRM.Service.Interface
{
    using System.Collections.Generic;
    using Fanex.CRM.Entities;

    public interface IUserService
    {
        User GetUseInfo(string userName);
        IEnumerable<Menu> GetUserMenu(int userId);
        Result CheckUserOnlineStatus();
        void AssignRoles(int custid, ref Dictionary<string, int> userRoleAccess, ref Dictionary<string, int> userRageAccess);
    }
}
