namespace Fanex.CRM.Common.Models
{
    using System;
    using System.Xml.Serialization;

    [Serializable]
    public class BaseModel
    {
        [XmlAttribute("Id")]
        public int Id { get; set; }

        [XmlAttribute("Name")]
        public string Name { get; set; }
    }
}