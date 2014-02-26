// --------------------------------------------------------------------------
// <copyright file="Feedback.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Entities
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Feedback class.
    /// </summary>
    [Serializable]
    [DataContract]
    public class Feedback
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public CustomerInfo CustomerInfo { get; set; }
        [DataMember]
        public Customer Customer { get; set; }
        [DataMember]
        public string FeedbackDate { get; set; }
        [DataMember]
        public string StrFeedbackDate { get; set; }
        [DataMember]
        public string CreateDate { get; set; }
        [DataMember]
        public string SolvedDate { get; set; }
        [DataMember]
        public string ResolutionTime { get; set; }
        [DataMember]
        public Setting CsExtension { get; set; }
        [DataMember]
        public Setting Category { get; set; }
        [DataMember]
        public Setting SubCategory { get; set; }
        [DataMember]
        public Setting Product { get; set; }
        [DataMember]
        public Setting BetType { get; set; }
        [DataMember]
        public Setting Site { get; set; }
        [DataMember]
        public Setting Status { get; set; }
        [DataMember]
        public Setting Response { get; set; }
        [DataMember]
        public Setting Role { get; set; }
        [DataMember]
        public Setting Priority { get; set; }
        [DataMember]
        public Setting Language { get; set; }
        [DataMember]
        public Country Country { get; set; }
        [DataMember]
        public Setting Communication { get; set; }
        [DataMember]
        public string Subject { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string Solution { get; set; }
        [DataMember]
        public string Review { get; set; }
        [DataMember]
        public User Reviewer { get; set; }
        [DataMember]
        public Customer RelatedCustomer { get; set; }
        [DataMember]
        public User Creator { get; set; }
        [DataMember]
        public bool IsReviewed { get; set; }
        [DataMember]
        public int UpdateReviewMode { get; set; }
        [DataMember]
        public bool IsOverrideCrmInfo { get; set; }
    }
}
