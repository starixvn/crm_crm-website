// --------------------------------------------------------------------------
// <copyright file="FeedbackStatistic.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.10.10">Created</change>
// </history>

namespace Fanex.CRM.Report.Models.FeedbackStatisticReport
{
    using System;
    using System.Collections.Generic;
    using Fanex.CRM.Infrastructure.Models;

    /// <summary>
    /// Feedback statistic.
    /// </summary>
    [Serializable]
    public class FeedbackStatistic
    {
        public string Title { get; set; }

        public IList<BaseModel> Usernames { get; set; }

        public int TotalItems { get; set; }

        public IList<FeedbackCategory> FeedbackCategories { get; set; }

        public int[] GrandTotals { get; set; }
    }
}
