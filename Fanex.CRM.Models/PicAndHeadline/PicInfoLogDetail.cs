namespace Fanex.CRM.Models.PicAndHeadline
{
    using System.Xml.Serialization;

    public class PicInfoLogDetail
    {
        [XmlAttribute("PicID")]
        public int PicId { get; set; }

        [XmlAttribute("PicName")]
        public string PicName { get; set; }

        [XmlAttribute("RoleID")]
        public int RoleId { get; set; }

        [XmlAttribute("RoleName")]
        public string RoleName { get; set; }
    }
}