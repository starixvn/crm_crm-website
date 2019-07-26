namespace Fanex.CRM.AdminTool.Services.SettingManagement
{
    using System.Collections.Generic;
    using Fanex.CRM.AdminTool.Criteria.SettingManagement;
    using Fanex.CRM.Common.Models;
    using Models.SettingManagement;

    public interface ISettingManagementService
    {
        IEnumerable<Setting> GetMainSettings();

        IEnumerable<Setting> GetParentSettings();

        IEnumerable<Setting> SettingByType(GetSettingByTypeCriteria criteria);

        CommandResult InsertSetting(InsertSettingCriteria criteria);

        CommandResult DeleteSetting(DeleteSettingCriteria criteria);

        Setting GetSettingById(GetSettingByIdCriteria criteria);

        CommandResult EditSetting(EditSettingCriteria criteria);

        SettingCN88 GetSettingCN88ById(GetSettingCN88ByIdCriteria criteria);

        CommandResult EditSettingCN88(EditSettingCN88Criteria criteria);

        CommandResult InsertSettingCN88(InsertSettingCN88Criteria criteria);

        List<MainSetting> GetMainSettingCN88(GetMainSettingCriteria criteria);

        SettingItemName GetSettingName(GetSettingNameCriteria criteria);

        List<SettingItemCN88> GetSettingByTypeCN88(GetSettingByTypeCN88Criteria criteria);
    }
}