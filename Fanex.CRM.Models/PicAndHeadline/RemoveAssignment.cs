namespace Fanex.CRM.Models.PicAndHeadline
{
    using System.Xml.Serialization;

    [XmlType("RemovedPIC")]
    public class RemoveAssignment
    {
        [XmlAttribute("HeadlineID")]
        public int HeadlineId { get; set; }

        [XmlAttribute("CustID")]
        public int CustId { get; set; }

        [XmlAttribute("PICName")]
        public string PicName { get; set; }
    }
}