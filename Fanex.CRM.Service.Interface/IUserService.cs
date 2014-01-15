// --------------------------------------------------------------------------
// <copyright file="IUserService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

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
