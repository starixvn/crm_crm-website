namespace Fanex.CRM.BackgroundJob.Models
{
    public class AutoFeedbackInfo
    {
        public int MessageId { get; set; }
        public int CustId { get; set; }
        public int HeadlineId { get; set; }
        public int RelatedCustId { get; set; }
        public string EventInfo { get; set; }
    }
}