namespace Fanex.CRM.Models.MultipleLogs
{
    public class VerifyCustomerResult
    {
        public int HeadlineId { get; set; }
        public string Headline { get; set; }
        public int CustId { get; set; }
        public string Username { get; set; }
        public bool IsHeadline { get; set; }
        public bool IsDirectAccount { get; set; }
    }
}