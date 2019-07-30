namespace Fanex.CRM.Communication.Models
{
    using System.Xml.Serialization;

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
                return ProductId ?? string.Empty;
            }

            set { }
        }

        [XmlAttribute("SubProductID")]
        public string SubProductIdContent
        {
            get
            {
                return SubProductId ?? string.Empty;
            }

            set { }
        }

        [XmlAttribute("BetTypeID")]
        public string BettypeIdsContent
        {
            get
            {
                return BettypeId ?? string.Empty;
            }

            set { }
        }
    }
}