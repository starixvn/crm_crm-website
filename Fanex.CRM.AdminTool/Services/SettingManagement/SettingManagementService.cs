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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CRM.AdminTool.Criteria.SettingManagement;
    using Fanex.CRM.AdminTool.Repositories.SettingManagement;
    using Fanex.CRM.AdminTool.Resources;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Infrastructure.Constants;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Setting.Services;

    /// <summary>
    /// Setting Relation Service.
    /// </summary>
    public class SettingManagementService : ISettingManagementService
    {
        private ISettingManagementRepository _settingManagementRepository;

        private ISettingService _settingService;

        public SettingManagementService()
            : this(null, null)
        {
        }

        public SettingManagementService(
            ISettingManagementRepository settingManagementRepository,
            ISettingService settingService)
        {
            _settingManagementRepository = settingManagementRepository ?? new SettingManagementRepository();
            _settingService = settingService ?? new SettingService();
        }

        public IEnumerable<Setting> GetMainSettings()
        {
            return _settingService.GetMainSettings();
        }

        public IEnumerable<Setting> GetParentSettings()
        {
            var allSettings = _settingService.GetAllSettings();
            return allSettings.OrderBy(s => s.SettingType).ToList();
        }

        public Result InsertSetting(InsertSettingCriteria criteria)
        {
            var result = IsExistedSetting(criteria.NewSetting) ?
                new Result(ErrorCodeConstants.ServerError, Labels.SettingManagement_Error_ExistedSetting) :
                _settingManagementRepository.InsertSetting(criteria);

            if (result.Code == ErrorCodeConstants.NoneError)
            {
                result.Message = Labels.SettingManagement_InsertSetting_InsertSuccess;
                _settingService.ClearCache();
            }

            return result;
        }

        public IEnumerable<Setting> SettingByType(GetSettingByTypeCriteria criteria)
        {
            var allSettings = _settingService.GetAllSettings();
            return allSettings
                .Where(s => s.SettingType == criteria.MainSetting.SettingType)
                .OrderBy(s => s.DisplayOrder)
                .ToList();
        }

        public bool IsExistedSetting(Setting setting)
        {
            if (setting == null)
            {
                return false;
            }

            return _settingService.GetAllSettings().Any(
                s => string.Equals(s.Value, setting.Value, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(s.SettingType, setting.SettingType, StringComparison.OrdinalIgnoreCase));
        }

        public Result DeleteSetting(DeleteSettingCriteria criteria)
        {
            var result = new Result();
            result = _settingManagementRepository.DeleteSetting(criteria);

            if (result.Code == ErrorCodeConstants.NoneError)
            {
                result.Message = criteria.OldSetting.IsActive ?
                    Labels.DeactiveSettingSuccessMessage : Labels.ActiveSettingSuccessMessage;
                _settingService.ClearCache();
            }

            return result;
        }

        public Result EditSetting(EditSettingCriteria criteria)
        {
            var result = !string.Equals(criteria.OldSetting.Value, criteria.UpdatedSetting.Value, StringComparison.OrdinalIgnoreCase) &&
                IsExistedSetting(criteria.UpdatedSetting) ?
                new Result(ErrorCodeConstants.ServerError, Labels.SettingManagement_Error_ExistedSetting) :
                _settingManagementRepository.EditSetting(criteria);

            if (result.Code == ErrorCodeConstants.NoneError)
            {
                result.Message = Labels.SettingManagement_EditSetting_EditSuccess;
                _settingService.ClearCache();
            }

            return result;
        }

        public Setting GetSettingById(GetSettingByIdCriteria criteria)
        {
            return _settingService.GetAllSettings()
                .Where(s => s.Id == criteria.SettingId)
                .FirstOrDefault();
        }
    }
}
