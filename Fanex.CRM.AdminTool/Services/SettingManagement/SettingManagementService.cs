// --------------------------------------------------------------------------
// <copyright file="SettingManagementService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.06.03">Created</change>
// </history>
// --------------------------------------------------------------------------

namespace Fanex.CRM.AdminTool.Services.SettingManagement
{
    using Fanex.CRM.AdminTool.Repositories.SettingManagement;

    /// <summary>
    /// Setting Relation Service.
    /// </summary>
    public class SettingManagementService : ISettingManagementService
    {
        private ISettingManagementRepository _settingManagementRepository;

        public SettingManagementService()
            : this(null)
        {
        }

        public SettingManagementService(ISettingManagementRepository settingManagementRepository)
        {
            _settingManagementRepository = settingManagementRepository ?? new SettingManagementRepository();
        }
    }
}
