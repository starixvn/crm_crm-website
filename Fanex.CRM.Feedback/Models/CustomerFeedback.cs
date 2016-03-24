// --------------------------------------------------------------------------
// <copyright file="CustomerFeedback.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.10.20">Created</change>
// </history>

namespace Fanex.CRM.Feedback.Models
{
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Infrastructure.Models;

    /// <summary>
    /// Customer feedback.
    /// </summary>
    public class CustomerFeedback
    {
        public CustomerInfo CustomerInfo { get; set; }

        public int Id { get; set; }

        public string FeedbackTimeString { get; set; }

        public string ResolutionTimeString { get; set; }

        public string SolveTimeString { get; set; }

        public BaseModel Extension { get; set; }

        public BaseModel Communication { get; set; }

        public BaseModel Category { get; set; }

        public BaseModel SubCategory { get; set; }

        public BaseModel Product { get; set; }

        public BaseModel SubProduct { get; set; }

        public BaseModel BetType { get; set; }

        public BaseModel Country { get; set; }

        public BaseModel Language { get; set; }

        public BaseModel Status { get; set; }

        public BaseModel Role { get; set; }

        public BaseModel Response { get; set; }

        public bool HasRelated { get; set; }

        public string RelatedCustomer { get; set; }

        public string Subject { get; set; }

        public string Description { get; set; }

        public string Solution { get; set; }

        public bool IsReviewed { get; set; }

        public string Review { get; set; }

        public int ReviewerId { get; set; }

        public string ReviewerName { get; set; }

        public int CreatorId { get; set; }

        public int UserId { get; set; }
    }
}
