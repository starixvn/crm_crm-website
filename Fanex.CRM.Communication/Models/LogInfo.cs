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

        public TimeSpan Duration { get; set; }

        public string Handler { get; set; }

        public string Reviewer { get; set; }
    }
}
