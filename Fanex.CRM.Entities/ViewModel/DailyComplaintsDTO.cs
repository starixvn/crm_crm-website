
namespace Fanex.CRM.Entities.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [Serializable]
    [DataContract]
    public class DailyComplaintsDTO
    {
        [DataMember]
        public List<DailyComplaintsDetail> DailyComplaints { get; set; }
        [DataMember]
        public int pageindex { get; set; }
        [DataMember]
        public int total { get; set; }
    }

    [Serializable]
    [DataContract]
    public class CommunicationTool
    {
        [DataMember]
        public string ContactBy { get; set; }
        [DataMember]
        public string Address { get; set; }
    }

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
    }
}
