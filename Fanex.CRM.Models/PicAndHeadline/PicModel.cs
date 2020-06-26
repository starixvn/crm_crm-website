namespace Fanex.CRM.Models.PicAndHeadline
{
    using System.Xml.Serialization;

    [XmlType("PIC")]
    public class PicModel
    {
        [XmlAttribute("PicID")]
        public int PicId { get; set; }

        [XmlAttribute("PicName")]
        public string PicName { get; set; }

        [XmlAttribute("Role")]
        public string Role { get; set; }

        [XmlAttribute("Oder")]
        public int Order { get; set; }
    }
}