// --------------------------------------------------------------------------
// <copyright file="UserDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.DataAccess.Impl
{
    using System.Collections;
    using System.Collections.Generic;
    using DNAide.Core;
    using Fanex.CRM.DataAccess.Interface;
    using Fanex.CRM.Entities;
    using Fanex.Data;

    /// <summary>
    /// User Data Access.
    /// </summary>
    public class UserDataAccess : IUserDataAccess
    {
        /// <summary>
        /// Gets the user information.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns>The user information.</returns>
        public User GetUserInfo(string userName)
        {
            var user = new User();
            using (ObjectDb obj = new ObjectDb("UserInfoSel"))
            {
                var param = new Hashtable { { "username", userName }, { "userid", 0 }, { "roleid", 0 } };
                obj.AssignParamValues(param);
                obj.ExecuteNonQuery();
                user.Username = userName;
                user.Id = CastAide.AsInt32(obj.GetParameterValue("userid"));
                user.RoleId = CastAide.AsInt32(obj.GetParameterValue("roleid"));
                user.IsAllowUpdateReview = false;
            }

            return user;
        }
        
        /// <summary>
        /// Update online user information.
        /// </summary>
        /// <param name="onlineUserId">The online user identifier.</param>
        /// <returns>Update result.</returns>
        public bool OnlineUserUpdate(int onlineUserId)
        {
            bool online = false;

            try
            {
                using (var obj = new ObjectDb("UserOnlineUpdate"))
                {
                    var param = new Hashtable { { "on_userid", onlineUserId }, { "iskickedout", 0 } };
                    obj.AssignParamValues(param);
                    obj.ExecuteNonQuery();
                    online = !CastAide.AsBoolean(obj.GetParameterValue("iskickedout"));
                }
            }
            catch
            {
                online = false;
            }

            return online;
        }
    }
}
