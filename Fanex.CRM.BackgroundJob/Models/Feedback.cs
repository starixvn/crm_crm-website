namespace Fanex.CRM.BackgroundJob.Models
{
    using System;

    public class Feedback
    {
        public DateTime FeedbackDate { get; set; }
        public DateTime ResolutionTime => FeedbackDate.AddMinutes(1);
        public string Description { get; set; }
        public string Solution { get; set; }
    }
}