// --------------------------------------------------------------------------
// <copyright file="FeedbackStatisticDetail.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.10.15">Created</change>
// </history>

namespace Fanex.CRM.Report.Models.FeedbackStatisticReport
{
    using System;
    using System.Collections.Generic;
    using Fanex.CRM.Common.Models;

    /// <summary>
    /// Feedback statistic detail.
    /// </summary>
    [Serializable]
    public class FeedbackStatisticDetail
    {
        public string Title { get; set; }

        public int TotalItems { get; set; }

        public IList<FeedbackDetail> Feedbacks { get; set; }
    }
}
