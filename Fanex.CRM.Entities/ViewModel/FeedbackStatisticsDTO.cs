
namespace Fanex.CRM.Entities.ViewModel
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [Serializable]
    [DataContract]
    public class FeedbackStatisticsDTO
    {
        [DataMember]
        public ArrayList ColumnNames { get; set; }
        [DataMember]
        public ArrayList ColumnIDs { get; set; }
        [DataMember]
        public int Total { get; set; }
        [DataMember]
        public int PageIndex { get; set; }
        [DataMember]
        public IEnumerable<FeedbackStatisticsGroup> rows { get; set; }
        [DataMember]
        public FeedbackStatistics GrandTotal { get; set; }
    }

    [DataContract]
    public class Params
    {
        [DataMember]
        public int Type { get; set; }
        [DataMember]
        public int ValueId { get; set; }
        [DataMember]
        public int MainId { get; set; }
        [DataMember]
        public int SubId { get; set; }
        [DataMember]
        public string FromDate { get; set; }
        [DataMember]
        public string ToDate { get; set; }
        [DataMember]
        public bool FromCache { get; set; }
        [DataMember]
        public int ItemPerPage { get; set; }
        [DataMember]
        public int CurrentPage { get; set; }
        [DataMember]
        public SolvingDuration FromDuration { get; set; }
        [DataMember]
        public SolvingDuration ToDuration { get; set; }
    }

    [DataContract]
    public class SolvingDuration
    {
        [DataMember]
        public string Day { get; set; }
        [DataMember]
        public string Hour { get; set; }
        [DataMember]
        public string Minute { get; set; }
        [DataMember]
        public string Second { get; set; }
    }
}
