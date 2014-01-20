// --------------------------------------------------------------------------
// <copyright file="RoleSettingService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Setting.Services.Implements
{
    using Fanex.CRM.Setting.Constants;
    using Fanex.CRM.Setting.Resources;

    /// <summary>
    /// Setting Service.
    /// </summary>
    public static class RoleSettingService
    {
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
