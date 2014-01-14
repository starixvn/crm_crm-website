// --------------------------------------------------------------------------
// <copyright file="UserService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Service.Impl
{
    using System;
    using System.Web;
    using DNAide.Core;
    using Fanex.CRM.DataAccess.Impl;
    using Fanex.CRM.DataAccess.Interface;
    using Fanex.CRM.Entities;
    using Fanex.CRM.Infrastructure.Common;
    using Fanex.CRM.Infrastructure.Extension;
    using Fanex.CRM.Infrastructure.Helper;
    using Fanex.CRM.Infrastructure.Logging;
    using Fanex.CRM.Service.Interface;

    /// <summary>
    /// User Service.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUserDataAccess _userRepository;
        private readonly IPermissionService _permissionService;

        public UserService()
            : this(null, null)
        {
        }

        public UserService(IUserDataAccess userRepository, IPermissionService permissionService)
        {
            this._userRepository = userRepository ?? new UserDataAccess();
            this._permissionService = permissionService ?? new PermissionService();
        }

        /// <summary>
        /// Checks the user online status.
        /// </summary>
        /// <returns>Check result.</returns>
        public Result CheckUserOnlineStatus()
        {
            var result = new Result();
            try
            {
                var isOnline = false;
                var onlineUserId = CastAide.AsInt32(SessionExtension.OnlineUserId);

                if (!AppSetting.IsLocalTest)
                {
                    isOnline = this._userRepository.OnlineUserUpdate(onlineUserId);
                    if (isOnline)
                    {
                        result.ErrCode = 0;
                    }
                    else
                    {
                        result.ErrCode = 500;
                        result.ErrMsg = ResourceExtension.GetResourceValue(ResourceExtension.Resources.ErrorMsg, "lblErrorSessionIsExpired");
                        HttpContext.Current.Session.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                result.ErrCode = 600;
                result.ErrMsg = ex.Message;
                HttpContext.Current.Session.Clear();
                LogExtensions.ExceptionHandler(ex);
                Helper.SendMailErrorNotice(string.Format("(CheckUserOnlineStatus) - {0}", ex.Message));
            }

            return result;
        }
    }
}
