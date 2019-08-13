namespace Fanex.CRM.Setting.Services
{
    using Fanex.CRM.Setting.Constants;
    using Fanex.CRM.Setting.Resources;

    public static class RoleSettingService
    {
        public static string GetRoleName(int roleId)
        {
            var roleName = string.Empty;
            switch (roleId)
            {
                case RoleIdConstants.Member:
                case RoleIdConstants.CRMMember:
                case RoleIdConstants.CN88Member:
                    roleName = Labels.Member;
                    break;

                case RoleIdConstants.Agent:
                case RoleIdConstants.CRMAgent:
                case RoleIdConstants.CN88Agent:
                    roleName = Labels.Agent;
                    break;

                case RoleIdConstants.Master:
                case RoleIdConstants.CRMMaster:
                case RoleIdConstants.CN88Master:
                    roleName = Labels.Master;
                    break;

                case RoleIdConstants.Super:
                case RoleIdConstants.CRMSuper:
                case RoleIdConstants.CN88Super:
                    roleName = Labels.Super;
                    break;

                case RoleIdConstants.Headline:
                    roleName = Labels.Headline;
                    break;

                default:
                    roleName = Labels.Guest;
                    break;
            }

            return roleName;
        }
    }
}