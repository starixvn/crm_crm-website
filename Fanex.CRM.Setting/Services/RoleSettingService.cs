// --------------------------------------------------------------------------
// <copyright file="RoleSettingService.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Setting.Services
{
    using Fanex.CRM.Setting.Constants;
    using Fanex.CRM.Setting.Resources;

    /// <summary>
    /// Setting Service.
    /// </summary>
    public static class RoleSettingService
    {
        /// <summary>
        /// Gets the name of the role.
        /// </summary>
        /// <param name="roleId">The role identifier.</param>
        /// <returns>The name of the role.</returns>
        public static string GetRoleName(int roleId)
        {
            var roleName = string.Empty;
            switch (roleId)
            {
                case RoleIdConstants.Member:
                case RoleIdConstants.CRMMember:
                    roleName = Labels.Member;
                    break;
                case RoleIdConstants.Agent:
                case RoleIdConstants.CRMAgent:
                    roleName = Labels.Agent;
                    break;
                case RoleIdConstants.Master:
                case RoleIdConstants.CRMMaster:
                    roleName = Labels.Master;
                    break;
                case RoleIdConstants.Super:
                case RoleIdConstants.CRMSuper:
                    roleName = Labels.Super;
                    break;
                default:
                    roleName = Labels.Guest;
                    break;
            }

            return roleName;
        }
    }
}
