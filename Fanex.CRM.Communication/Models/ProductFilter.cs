using System.Xml.Serialization;

namespace Fanex.CRM.Communication.Models
{
    [XmlType("Product")]
    public class ProductFilter
    {
        [XmlIgnore]
        public string ProductId { get; set; }

        [XmlIgnore]
        public string SubProductId { get; set; }

        [XmlIgnore]
        public string BettypeIds { get; set; } = string.Empty;

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
                return this.BettypeIds == null ? string.Empty : this.BettypeIds;
            }

            set { }
        }
    }
}
