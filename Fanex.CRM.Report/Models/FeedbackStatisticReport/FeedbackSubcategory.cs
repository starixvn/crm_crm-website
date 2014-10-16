// --------------------------------------------------------------------------
// <copyright file="FeedbackSubcategory.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.10.10">Created</change>
// </history>

namespace Fanex.CRM.Report.Models.FeedbackStatisticReport
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Feedback sub-category.
    /// </summary>
    [Serializable]
    public class FeedbackSubcategory
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IList<int> Resolves { get; set; }

        public int Total 
        {
            get
            {
                return Resolves == null || Resolves.Count == 0
                    ? 0
                    : Resolves.Sum();
            }
        }
    }
}
