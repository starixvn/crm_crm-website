
namespace Fanex.CRM.Service.Interface
{
    using System.Collections.Generic;
    using Fanex.CRM.Entities;

    public interface ISettingService
    {
        IEnumerable<Setting> GetAll();
        Result Create(Setting setting, string ip, int userId, int parentId);
        Result Create(string settingId, string settingName, string ip, int userId, int parentId);
        Result Update(Setting setting, string ip, int userId);
        Result Delete(int settingId, string ip, int userId);
        Result Delete(int settingId, string ip, int userId, int parentId);
        Setting GetById(int settingId);
        IEnumerable<Setting> GetByType(string type);
        List<Setting> GetByType(string type, string excludeItem);
        IEnumerable<Setting> GetByTypeNoChild(string type);
        IEnumerable<Setting> GetByParent(int? parentId);
        IEnumerable<Setting> GetAllGroupByType();
        bool IsExistedSetting(Setting setting);
        bool IsExistedSetting(int settingId);
        IEnumerable<string> SearchSettingType(string settingType);

        IEnumerable<Setting> GetSettingRelationByName(string settingName, int parentId);

        int GetNextIdByType(SettingType type);
    
       
        AllSetting SelAllSetting();

        List<Site> SelAllSite();
    }
}
