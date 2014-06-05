// --------------------------------------------------------------------------
// <copyright file="ManageFeedback.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.06.03">Created</change>
// </history>
// --------------------------------------------------------------------------

namespace Fanex.CRM.Feedback.Models
{
    using System;
    using System.Web.Script.Serialization;
    using Fanex.CRM.Infrastructure.Core;

    /// <summary>
    /// Manage Feedback.
    /// </summary>
    public class ManageFeedback
    {
        public int FeedbackId { get; set; }

        ////public int CustId { get; set; }

        public string Username { get; set; }

        ////public int CreatorId { get; set; }

        public string CreatorName { get; set; }

        [ScriptIgnore]
        public DateTime FeedbackDate { get; set; }

        public string FeedbackDateString 
        {
            get
            {
                return DateTimeUtilize.ChangeTimeZoneToClient(FeedbackDate);
            }
        }

        [ScriptIgnore]
        public DateTime ResolutionDate { get; set; }

        [ScriptIgnore]
        public DateTime CreateDate { get; set; }

        public string CreateDateString
        {
            get
            {
                return DateTimeUtilize.ChangeTimeZoneToClient(CreateDate); 
            }
        }

        [ScriptIgnore]
        public DateTime SolveDate { get; set; }

        public string SolveDateString
        {
            get
            {
                return DateTimeUtilize.ChangeTimeZoneToClient(SolveDate); 
            }
        }

        public string MainCategory { get; set; }

        public string SubCategory { get; set; }

        public string Status { get; set; }

        ////public string Description { get; set; }

        ////public string Solution { get; set; }

        public bool IsReviewed { get; set; }

        ////public string Review { get; set; }

        ////public int ReviewerId { get; set; }

        public string ReviewerName { get; set; }

        public bool IsRelated { get; set; }

        public string Subject { get; set; }

        public string RelatedCustomer { get; set; }

        public string DurationTime
        {
            get
            {
                return ResolutionDate != DateTime.MinValue ? GetResolvingTime() : string.Empty;
            }
        }

        private string GetResolvingTime()
        {
            var totalDays = ResolutionDate - FeedbackDate;
            return string.Format(
                "{0}:{1}:{2}:{3}",
                totalDays.Days,
                totalDays.Hours,
                totalDays.Minutes,
                totalDays.Seconds);
        }
    }
}
