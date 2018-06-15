// -----------------------------------------------------------------------
// <copyright file="DeletedFeedback.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.Report.Models.DeletedFeedbackStatistics
{
    using Common.Models;
    using Fanex.CRM.Infrastructure.Extensions;
    using System;
    using System.Collections.Generic;

    public class DeletedFeedback
    {
        public DeletedFeedback()
        {
            Settings = new List<SettingItem>();
        }

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

        public IEnumerable<SettingItem> Settings { get; set; }
    }
}
