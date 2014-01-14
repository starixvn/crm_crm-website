// --------------------------------------------------------------------------
// <copyright file="IUserDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.DataAccess.Interface
{
    using Fanex.CRM.Entities;

    public interface IUserDataAccess
    {
        User GetUserInfo(string username);

        bool OnlineUserUpdate(int onlineUserId);
    }
}
