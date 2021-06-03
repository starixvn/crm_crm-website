namespace Fanex.CRM.Common.Models
{
    public class RelatedCustomer : BaseModel
    {
        public int SiteId { get; set; }
        public bool IsSiteActive { get; set; }
    }
}
