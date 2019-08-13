namespace Fanex.CRM.Common.Models
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class SettingCN88
    {
        public int SettingId { get; set; }

        public string SettingName { get; set; }

        public string SettingValue { get; set; }

        public string SettingNameCS { get; set; }

        public string SettingValueCS { get; set; }

        public string SettingNameCT { get; set; }

        public string SettingValueCT { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsChild { get; set; }

        public bool IsActive { get; set; }

        public int ParentId { get; set; }

        public List<SettingCN88> ChildSettings { get; set; }
    }
}