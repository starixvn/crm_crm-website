namespace Fanex.CRM.Common.Models
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class BaseCommunicationLog
    {
        public BaseCommunicationLog()
        {
            this.Settings = new List<SettingItem>();
        }

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

        public Province Province { get; set; }

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

        public IEnumerable<BaseModel> RelatedCustomers { get; set; }

        public string Remark { get; set; }

        public string Solution { get; set; }

        public string Review { get; set; }

        public bool IsReviewed { get; set; }

        public string AutoField { get; set; }

        public List<SettingItem> Settings { get; set; }
    }
}