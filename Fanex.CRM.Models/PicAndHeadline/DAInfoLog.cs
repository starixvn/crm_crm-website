namespace Fanex.CRM.Models.PicAndHeadline
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    public class DAInfoLog
    {
        [XmlAttribute("HeadlineName")]
        public string GroupHeadline { get; set; }

        [XmlElement(ElementName = "PIC")]
        public List<PicInfoLogDetail> Pics { get; set; }

        [XmlAttribute("IsGroupHeadline")]
        public bool IsGroupHeadline { get; set; }
    }
}