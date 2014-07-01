// --------------------------------------------------------------------------
// <copyright file="ManageFeedbackCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.06.03">Created</change>
// </history>
// --------------------------------------------------------------------------

namespace Fanex.CRM.Feedback.Criteria
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// ManageFeedback Criteria.
    /// </summary>
    public class ManageFeedbackCriteria : CriteriaBase
    {
        public int? FeedbackId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string Username { get; set; }

        public int IsGuest { get; set; }

        public int CommunicationId { get; set; }

        public int MainCategoryId { get; set; }

        public int SubCategoryId { get; set; }

        public int StatusId { get; set; }

        public int IsReviewed { get; set; }

        public int ResponseId { get; set; }

        public string HandledBy { get; set; }

        public int PageIndex { get; set; }

        public int ItemsPerPage { get; set; }

        public int TotalRows { get; set; }

        public override string GetSettingKey()
        {
            return "ManageFeedbackList";
        }
    }
}
