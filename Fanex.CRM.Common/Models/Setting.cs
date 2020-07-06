namespace Fanex.CRM.Common.Models
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [Serializable]
    [XmlType("Setting")]
    public class Setting : BaseModel
    {
        [XmlAttribute("Value")]
        public string Value { get; set; }

        public string SettingType { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsChild { get; set; }

        public bool IsActive { get; set; }

        public int ParentId { get; set; }

        public string SettingSource { get; set; }

        public List<Setting> ChildSettings { get; set; }
    }
}