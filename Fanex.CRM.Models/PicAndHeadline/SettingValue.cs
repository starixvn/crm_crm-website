namespace Fanex.CRM.Models.PicAndHeadline
{
    using System.Xml.Serialization;

    public class SettingValue
    {
        [XmlAttribute("Id")]
        public int Id { get; set; }

        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("Value")]
        public string Value { get; set; }
    }
}