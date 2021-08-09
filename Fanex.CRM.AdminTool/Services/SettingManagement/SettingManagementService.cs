namespace Fanex.CRM.AdminTool.Services.SettingManagement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using Fanex.CRM.AdminTool.Criteria.SettingManagement;
    using Fanex.CRM.AdminTool.Resources;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Common.Services;
    using Fanex.CRM.Setting.Services;
    using Models.SettingManagement;

    public class SettingManagementService : ISettingManagementService
    {
        private readonly ISettingService _settingService;
        private readonly IBaseService baseService;
        public const string GetAllSettingSessionKey = "Fanex.CRM.Setting.GetAllSettingSessionKey";
        public SettingManagementService() : this(null, null)
        {
        }

        public SettingManagementService(
            IBaseService baseService,
            ISettingService settingService)
        {
            _settingService = settingService ?? new SettingService();
            this.baseService = baseService ?? new BaseService();
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

        public CommandResult InsertSetting(InsertSettingCriteria criteria)
        {
            var result = IsExistedSetting(criteria.NewSetting) ?
                new CommandResult((int)HttpStatusCode.InternalServerError, Labels.SettingManagement_Error_ExistedSetting) :
                baseService.ExecuteNonQuery(criteria);

            if (result.IsSuccessful)
            {
                result.Message = Labels.SettingManagement_InsertSetting_InsertSuccess;
                _settingService.ClearAllCaches();
            }

            return result;
        }

        public IEnumerable<Setting> SettingByType(GetSettingByTypeCriteria criteria)
        {
            var allSettings = Enumerable.Empty<Setting>();
            if (criteria.MainSetting.SettingType == "Site")
            {
                allSettings = _settingService.GetAllSettingsRemoveCache();
            }
            else
            {
                allSettings = _settingService.GetAllSettings();
            }
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

        public CommandResult DeleteSetting(DeleteSettingCriteria criteria)
        {
            CommandResult result = baseService.ExecuteNonQuery(criteria);
            if (result.IsSuccessful)
            {
                result.Message = criteria.OldSetting.IsActive ?
                    Labels.DeactiveSettingSuccessMessage : Labels.ActiveSettingSuccessMessage;
                _settingService.ClearAllCaches();
            }

            return result;
        }

        public CommandResult EditSetting(EditSettingCriteria criteria)
        {
            var result = !string.Equals(criteria.OldSetting.Value, criteria.UpdatedSetting.Value, StringComparison.OrdinalIgnoreCase) &&
                IsExistedSetting(criteria.UpdatedSetting) ?
                new CommandResult((int)HttpStatusCode.InternalServerError, Labels.SettingManagement_Error_ExistedSetting) :
                baseService.ExecuteNonQuery(criteria);

            baseService.ExecuteNonQuery(new EditSourceSettingMappingCriteria
            {
                SettingID = criteria.UpdatedSetting.Id,
                SettingSource = criteria.UpdatedSetting.SettingSource
            });

            if (result.IsSuccessful)
            {
                result.Message = Labels.SettingManagement_EditSetting_EditSuccess;
                _settingService.ClearAllCaches();
            }

            return result;
        }

        public Setting GetSettingById(GetSettingByIdCriteria criteria)
        {
            return _settingService.GetAllSettings().FirstOrDefault(s => s.Id == criteria.SettingId);
        }

        public SettingCN88 GetSettingCN88ById(GetSettingCN88ByIdCriteria criteria)
        {
            var setting = baseService.GetDataEntity<SettingCN88>(criteria);
            return setting ?? new SettingCN88();
        }

        public List<MainSetting> GetMainSettingCN88(GetMainSettingCriteria criteria)
        {
            return baseService.GetData<MainSetting>(criteria).ToList();
        }

        public List<SettingItemCN88> GetSettingByTypeCN88(GetSettingByTypeCN88Criteria criteria)
        {
            return baseService.GetData<SettingItemCN88>(criteria).Where(s => s.ParentID == 0).ToList();
        }

        public SettingItemName GetSettingName(GetSettingNameCriteria criteria)
        {
            var setting = baseService.GetDataEntity<SettingItemName>(criteria);
            return setting ?? new SettingItemName();
        }

        public CommandResult EditSettingCN88(EditSettingCN88Criteria criteria)
        {
            CommandResult result = baseService.ExecuteNonQuery(criteria);
            if (result.IsSuccessful)
            {
                result.Message = Labels.SettingManagement_EditSetting_EditSuccess;
                _settingService.ClearAllCaches();
            }

            return result;
        }

        public CommandResult InsertSettingCN88(InsertSettingCN88Criteria criteria)
        {
            CommandResult result = baseService.ExecuteNonQuery(criteria);
            if (result.IsSuccessful)
            {
                result.Message = Labels.SettingManagement_InsertSetting_InsertSuccess;
                _settingService.ClearAllCaches();
            }

            return result;
        }
    }
}