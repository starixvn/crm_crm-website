// -----------------------------------------------------------------------
// <copyright file="DeletedFeedback.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.Report.Models.DeletedFeedbackStatistics
{
    using System;
    using Fanex.CRM.Infrastructure.Extensions;

    public class DeletedFeedback
    {
        public DateTime CreateDate { get; set; }

        public string CreateDateString 
        {
            get { return CreateDate.ToShortFormatedDate(); }
        }

        public string Handler { get; set; }

        public string Deletor { get; set; }

        public int FeedbackId { get; set; }

        public string Subject { get; set; }

        public string MainCategory { get; set; }

        public string SubCategory { get; set; }

        public string Customer { get; set; }

        public string RelatedCustomer { get; set; }
    }
}
