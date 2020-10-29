namespace Fanex.CRM.Feedback.Models
{
    using Fanex.CRM.Common.Models;
    using System;

    [Serializable]
    public class DeletedFeedback : FeedbackDetail
    {
        public string Description { get; set; }

        public string Solution { get; set; }

        public string AutoField { get; set; }
    }
}
