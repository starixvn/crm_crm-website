// --------------------------------------------------------------------------
// <copyright file="InformHeadline.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.05.09">Created</change>
// </history>

namespace Fanex.CRM.Customer.Models.InformHeadlines
{
    using System;
    using System.Collections.Generic;
    using Fanex.CRM.Infrastructure.Core;

    /// <summary>
    /// InformHeadline model.
    /// </summary>
    [Serializable]
    public class InformHeadline
    {
        public int InformId { get; set; }

        public int HeadlineId { get; set; }

        public string HeadlineName { get; set; }

        public int TitleId { get; set; }

        public string Title { get; set; }

        public int StatusId { get; set; }

        public DateTime UpdatedTime { get; set; }

        public string UpdatedTimeString
        {
            get
            {
                if (this.UpdatedTime == DateTime.MinValue)
                {
                    return string.Empty;
                }

                return DateTimeUtilize.ChangeFullTimeZoneToClientForActionLog(this.UpdatedTime);
            }
        }

        public string Remark { get; set; }

        public string Creator { get; set; }
    }
}