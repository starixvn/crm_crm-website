using System.Xml.Serialization;

namespace Fanex.CRM.Communication.Models
{
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
                return this.MainCategoryID == null ? string.Empty : this.MainCategoryID;
            }

            set { }
        }

        [XmlAttribute("SubCategoryID")]
        public string SubCategoryIDContent
        {
            get
            {
                return this.SubCategoryID == null ? string.Empty : this.SubCategoryID;
            }

            set { }
        }
    }
}
