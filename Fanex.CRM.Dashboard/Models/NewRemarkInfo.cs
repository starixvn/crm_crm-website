namespace Fanex.CRM.Dashboard.Models
{
    using System;

    public class NewRemarkInfo
    {
        public int RemarkID { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdate { get; set; }
        public string CreatedDateString { get; set; }
        public int HeadlineID { get; set; }
        public string HeadlineName { get; set; }
        public int CustID { get; set; }
        public bool IsGroupHeadLine { get; set; }
        public string UserName { get; set; }
        public string RemarkTitle { get; set; }
        public string RemarkName { get; set; }
        public string Remark { get; set; }
        public int Ext { get; set; }
        public string ExtStation { get; set; }
        public string RelatedPICs { get; set; }
        public int HandlerID { get; set; }
        public int UpdaterID { get; set; }
        public int ApproverID { get; set; }
        public int AcknowledgerID { get; set; }
        public string Requester { get; set; }
        public string Creator { get; set; }
        public string Handler { get; set; }
        public string Approver { get; set; }
        public string Acknowledger { get; set; }
        public string Updater { get; set; }
        public int CreatorID { get; set; }
        public int ModifierID { get; set; }
    }
}
