namespace Fanex.CRM.BackgroundJob.Models
{
    public class AppSettings
    {
        public short TimeSpan { get; set; }
        public string LicenseeUsername { get; set; }
        public int CreatorId { get; set; }
        public int NoOfRelatedCustomerPerFeedback { get; set; }
    }
}