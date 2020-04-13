namespace Fanex.CRM.Models
{
    using System.Xml.Serialization;

    [XmlType("Category")]
    public class CategoryFilter
    {
        [XmlIgnore]
        public string MainCategoryID { get; set; }

        [XmlIgnore]
        public string SubCategoryID { get; set; }

        [XmlAttribute("MainCategoryID")]
        public string MainCategoryIDContent
        {
            get
            {
                return MainCategoryID ?? string.Empty;
            }

            set { }
        }

        [XmlAttribute("SubCategoryID")]
        public string SubCategoryIDContent
        {
            get
            {
                return SubCategoryID ?? string.Empty;
            }

            set { }
        }
    }
}