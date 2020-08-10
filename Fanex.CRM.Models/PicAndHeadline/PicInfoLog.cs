namespace Fanex.CRM.Models.PicAndHeadline
{
    using System.Collections.Generic;
    using System.Xml.Serialization;
    using Fanex.CRM.Common.Models;

    public class PicInfoLog
    {
        public BaseModel Role { get; set; }

        public BaseModel Country { get; set; }

        [XmlElement(ElementName = "Language")]
        public List<BaseModel> Languages { get; set; }

        [XmlElement(ElementName = "CommunicationTool")]
        public List<SettingValue> CommunicationTools { get; set; }

        [XmlElement(ElementName = "DirectAccount")]
        public List<BaseModel> DirectAccounts { get; set; }

        [XmlAttribute("IsActive")]
        public bool IsActive { get; set; }

        [XmlAttribute("PicName")]
        public string Name { get; set; }

        [XmlAttribute("Remark")]
        public string Remark { get; set; }
    }
}