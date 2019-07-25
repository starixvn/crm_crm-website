namespace Fanex.CRM.Common.Models
{
    using System;
    using System.Collections.Generic;
    using System.Web.Script.Serialization;
    using Fanex.CRM.Common.Helpers;

    [Serializable]
    public class FeedbackDetail
    {
        public FeedbackDetail()
        {
            this.Settings = new List<SettingItem>();
        }

        public int FeedbackId { get; set; }

        public string Username { get; set; }

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

        public int MainCategoryId { get; set; }

        public string MainCategory { get; set; }

        public int SubCategoryId { get; set; }

        public string SubCategory { get; set; }

        public int StatusId { get; set; }

        public string Status { get; set; }

        public bool IsReviewed { get; set; }

        public string ReviewerName { get; set; }

        public bool IsRelated { get; set; }

        public string Subject { get; set; }

        public string RelatedCustomer { get; set; }

        public bool IsChecked { get; set; }

        public List<SettingItem> Settings { get; set; }

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