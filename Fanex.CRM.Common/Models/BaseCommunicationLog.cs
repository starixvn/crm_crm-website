// --------------------------------------------------------------------------
// <copyright file="BaseCommunicationLogInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Common.Models
{
    using System;
    using Fanex.CRM.Infrastructure.Models;

    /// <summary>
    /// Base Communication Log Information.
    /// </summary>
    [Serializable]
    public class BaseCommunicationLog
    {
        public string FeedbackDate { get; set; }

        public int CustomerId { get; set; }

        public string CustomerName { get; set; }

        public int RunningNumber { get; set; }

        public Setting MainCategory { get; set; }

        public Setting SubCategory { get; set; }

        public string StatusName { get; set; }

        public string Subject { get; set; }

        public TimeInfo Duration { get; set; }

        public string Handler { get; set; }

        public string Reviewer { get; set; }

        public string ReviewedTimeString { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public Setting Extension { get; set; }

        public Country Country { get; set; }

        public Setting CommunicationTool { get; set; }

        public Setting Language { get; set; }

        public Setting Product { get; set; }

        public Setting SubProduct { get; set; }

        public Setting BetType { get; set; }

        public Setting Status { get; set; }

        public Site SiteInfo { get; set; }

        public Setting RoleInfo { get; set; }

        public Setting Response { get; set; }

        public Setting LicenseeType { get; set; }

        public string RelatedCustomers { get; set; }

        public string RelatedCustomerIds { get; set; }

        public string Remark { get; set; }

        public string Solution { get; set; }

        public string Review { get; set; }

        public bool IsReviewed { get; set; }
    }
}
