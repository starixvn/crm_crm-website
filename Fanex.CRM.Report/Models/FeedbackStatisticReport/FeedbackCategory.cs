// --------------------------------------------------------------------------
// <copyright file="FeedbackCategory.cs" company="">
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

    /// <summary>
    /// Feedback category.
    /// </summary>
    [Serializable]
    public class FeedbackCategory
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IList<FeedbackSubcategory> FeedbackSubcategories { get; set; }

        public FeedbackSubcategory SubTotal { get; set; }
    }
}
