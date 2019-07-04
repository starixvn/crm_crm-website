using System.Collections.Generic;

namespace Fanex.CRM.AdminTool.Models.SettingManagement
{
    public class SettingItemCN88
    {
        public int ParentID { get; set; }
        public int SettingID { get; set; }
        public string SettingValue { get; set; }
        public string SettingValueCS { get; set; }
        public string SettingValueCT { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public List<SettingItemCN88> ChildSettings { get; set; }
    }
}
