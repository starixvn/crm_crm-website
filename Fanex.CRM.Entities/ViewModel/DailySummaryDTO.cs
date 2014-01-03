
namespace Fanex.CRM.Entities.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Runtime.Serialization;

    public class DailySummaryDTO
    {
    }

    [Serializable]
    [DataContract]
    public class DailySummaryType
    {
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public int Value { get; set; }
    }

    [Serializable]
    [DataContract]
    public class DailySummaryDetail
    {
        [DataMember]
        public int MainCategoryId { get; set; }
        [DataMember]
        public string MainCategory { get; set; }
        [DataMember]
        public int SubCaregoryId { get; set; }
        [DataMember]
        public string Subcategory { get; set; }
        [DataMember]
        public List<DailySummaryType> Cells { get; set; }
        [DataMember]
        public int Total { get; set; }
    }

    [Serializable]
    [DataContract]
    public class DailySummaryGroup
    {
        [DataMember]
        public List<DailySummaryDetail> DailySummaries { get; set; }
        [DataMember]
        public DailySummaryDetail Total { get; set; }
    }

    [Serializable]
    [DataContract]
    public class DailySummaryBO
    {
        [DataMember]
        public DateTime ReportDay { get; set; }
        [DataMember]
        public List<DailySummaryGroup> DailySummaries { get; set; }
        [DataMember]
        public DailySummaryDetail GrandTotal { get; set; }
    }
}
