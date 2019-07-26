namespace Fanex.CRM.AdminTool.Services.SettingRelation
{
    using System.Collections.Generic;
    using Fanex.CRM.AdminTool.Criteria.SettingRelation;
    using Fanex.CRM.Common.Models;
    using Models.SettingManagement;
    using Models.SettingRelation;

    public interface ISettingRelationService
    {
        IEnumerable<Setting> GetMainSettings();

        IEnumerable<Setting> GetSettingByTypes(GetSettingByTypeCriteria criteria);

        IEnumerable<Setting> GetMainSettingForAddRelations(GetMainSettingForAddRelationCriteria criteria);

        IEnumerable<SettingRelation> MainSettingRelations(GetMainSettingRelationsCriteria criteria);

        CommandResult GetAddSettingRelations(AddSettingRelationsCriteria criteria);

        List<MainSetting> GetMainSettingCN88(GetMainSettingCriteria criteria);

        List<SettingItemCN88> GetSettingByTypeCN88(GetSettingByTypeCN88Criteria criteria);

        IEnumerable<SettingRelationItem> MainSettingRelationsCN88(GetMainSettingRelationsCriteria criteria);
    }
}