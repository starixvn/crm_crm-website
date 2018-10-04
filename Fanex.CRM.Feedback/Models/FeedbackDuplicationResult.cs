namespace Fanex.CRM.Feedback.Models
{
    public class FeedbackDuplicationResult
    {
        public int FeedbackId { get; set; }

        public bool HasDuplication
        {
            get
            {
                return FeedbackId != 0;
            }
        }
    }
}
