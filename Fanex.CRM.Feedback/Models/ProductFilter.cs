using System.Xml.Serialization;

namespace Fanex.CRM.Feedback.Models
{
    [XmlType("Product")]
    public class ProductFilter
    {
        [XmlIgnore]
        public string ProductId { get; set; }

        [XmlIgnore]
        public string SubProductId { get; set; }

        [XmlIgnore]
        public string BettypeId { get; set; } = string.Empty;

        [XmlAttribute("ProductID")]
        public string ProductIdContent
        {
            get
            {
                return this.ProductId == null ? string.Empty : this.ProductId;
            }

            set { }
        }

        [XmlAttribute("SubProductID")]
        public string SubProductIdContent
        {
            get
            {
                return this.SubProductId == null ? string.Empty : this.SubProductId;
            }

            set { }
        }

        [XmlAttribute("BetTypeID")]
        public string BettypeIdsContent
        {
            get
            {
                return this.BettypeId == null ? string.Empty : this.BettypeId;
            }

            set { }
        }
    }
}
