
namespace Fanex.CRM.DataAccess.Interface
{
    using System.Collections.Generic;
    using Fanex.CRM.Entities;

    public interface IUserDataAccess
    {
        IEnumerable<Menu> GetMenu(int userId);
        User GetUesrInfo(string username);
        bool OnlineUserUpdate(int onlineUserId);
    }
}
