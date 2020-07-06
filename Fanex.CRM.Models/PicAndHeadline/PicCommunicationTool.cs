using System.Xml.Serialization;

namespace Fanex.CRM.Models.PicAndHeadline
{
    [XmlType("CommunicationTool")]
    public class PicCommunicationTool
    {
        [XmlAttribute("SettingID")]
        public int Type { get; set; }

        [XmlAttribute("SettingValue")]
        public string Value { get; set; }
    }
}