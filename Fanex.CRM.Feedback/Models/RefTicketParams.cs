namespace Fanex.CRM.Feedback.Models
{
    public class RefTicketParams
    {
        public int CustId { get; set; }
        public string Refno { get; set; }
        public bool IsGuest { get; set; } = false;
    }
}
