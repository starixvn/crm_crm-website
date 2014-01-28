// --------------------------------------------------------------------------
// <copyright file="LogInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Communication.Models
{
    using System;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Setting.Models;

    /// <summary>
    /// Log Information.
    /// </summary>
    [Serializable]
    public class LogInfo
    {
        public int RunningNumber { get; set; }

        public DateTime FeedbackDate { get; set; }

        public BaseModel MainCategory { get; set; }

        public BaseModel SubCategory { get; set; }

        public string StatusName { get; set; }

        public string Subject { get; set; }

        public TimeInfo Duration { get; set; }

        public string Handler { get; set; }

        public string Reviewer { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string Extension { get; set; }

        public CountryInfo Country { get; set; }

        public BaseModel CommunicationTool { get; set; }

        public string LanguageId { get; set; }

        public BaseModel Product { get; set; }

        public BaseModel BetType { get; set; }

        public BaseModel Status { get; set; }

        public BaseModel SiteInfo { get; set; }

        public BaseModel RoleInfo { get; set; }

        public string Reponse { get; set; }

        public string RelatedCustomers { get; set; }

        public string Remark { get; set; }

        public string Solution { get; set; }

        public string Review { get; set; }

        public bool IsReview { get; set; }
    }
}
