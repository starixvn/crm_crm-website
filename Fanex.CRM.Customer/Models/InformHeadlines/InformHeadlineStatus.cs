// --------------------------------------------------------------------------
// <copyright file="InformHeadlineStatus.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.05.09">Created</change>
// </history>

namespace Fanex.CRM.Customer.Models.InformHeadlines
{
    using System;
    using Fanex.CRM.Infrastructure.Core;
    using Fanex.CRM.Infrastructure.Extension;

    /// <summary>
    /// InformHeadline model.
    /// </summary>
    [Serializable]
    public class InformHeadlineStatus
    {
        public int TitleId { get; set; }

        public string Title { get; set; }

        public DateTime CreatedTime { get; set; }

        public string CreatedTimeString
        {
            get
            {
                if (this.CreatedTime == DateTime.MinValue)
                {
                    return string.Empty;
                }

                return DateTimeUtilize.ChangeFullTimeZoneToClientForActionLog(this.CreatedTime);
            }
        }

        public string CreatedDateString
        {
            get
            {
                if (this.CreatedTime == DateTime.MinValue)
                {
                    return string.Empty;
                }

                return DateTimeUtilize.ChangeTimeZoneToClientDateTime(this.CreatedTime).ToShortFormatedDate();
            }
        }

        public int Total { get; set; }

        public int Done { get; set; }

        public int RequestToCallback { get; set; }

        public int NoNeedToCall { get; set; }

        public int NoReply { get; set; }

        public int Uncontactable { get; set; }

        public int InternalAccount { get; set; }

        public int NeverTouch { get; set; }

        public int Request_to_call_back { get; set; }
    }
}
