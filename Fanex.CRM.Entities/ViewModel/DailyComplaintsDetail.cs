// --------------------------------------------------------------------------
// <copyright file="DailyComplaintsDetail.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Entities.ViewModel
{
    using System;
    using System.Runtime.Serialization;
    using DNAide.Core;

    /// <summary>
    /// Daily Complaints Detail.
    /// </summary>
    [Serializable]
    [DataContract]
    public class DailyComplaintsDetail
    {
        [DataMember]
        public string FeedbackDate { get; set; }
        [DataMember]
        public string FeedbackTime { get; set; }
        [DataMember]
        public string CustomerName { get; set; }
        [DataMember]
        public string Licensee { get; set; }
        [DataMember]
        public CommunicationTool CommunicationTool { get; set; }
        [DataMember]
        public string ExtNo { get; set; }
        [DataMember]
        public string ProductName { get; set; }
        [DataMember]
        public string Country { get; set; }
        [DataMember]
        public string Category { get; set; }
        [DataMember]
        public string SubCategory { get; set; }
        [DataMember]
        public string Complaints { get; set; }
        [DataMember]
        public string Solution { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public string Response { get; set; }
        [DataMember]
        public string HandlyBy { get; set; }
        [DataMember]
        public string DutyShift { get; set; }
        [DataMember]
        public string CreationTime { get; set; }

        public string ResolutionDate { get; set; }

        public string Duration
        {
            get
            {
                if (this.ResolutionDate != null)
                {
                    var totalDays = CastAide.AsDateTime(this.ResolutionDate) -
                        CastAide.AsDateTime(string.Format("{0} {1}", this.FeedbackDate, this.FeedbackTime));
                    var resolvingTime = string.Format(
                        "{0}:{1}:{2}",
                        totalDays.Days.ToString("00"),
                        totalDays.Hours.ToString("00"),
                        totalDays.Minutes.ToString("00"));
                    return resolvingTime;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
    }
}
