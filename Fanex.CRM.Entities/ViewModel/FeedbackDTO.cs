
namespace Fanex.CRM.Entities.ViewModel
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class FeedbackDTO
    {
        public int TotalPage { get; set; }
        public int PageIndex { get; set; }
        public IEnumerable<Feedback> ListFeedback;
    }
}
