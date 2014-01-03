
namespace Fanex.CRM.Entities
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [Serializable]
    [DataContract]
    public class FeedbackStatistics
    {
        [DataMember]
        public ArrayList Cells { get; set; }
        [DataMember]
        public string MainCategory { get; set; }
        [DataMember]
        public string SubCategory { get; set; }
        [DataMember]
        public int MainCategoryId { get; set; }
        [DataMember]
        public int  SubCategoryId { get; set; }
        [DataMember]
        public bool IsTotal { get; set; }
    }

    [Serializable]
    [DataContract]
    public class FeedbackStatisticsGroup
    {
        [DataMember]
        public List<FeedbackStatistics> FeedbackStatistics { get; set; }
        [DataMember]
        public int TotalMembers { get; set; }
        [DataMember]
        public string Title { get; set; }
    }
}
