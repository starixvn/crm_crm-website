
namespace Fanex.CRM.Entities.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Runtime.Serialization;

    public class CasinoSummaryDTO
    {
    }

    [Serializable]
    [DataContract]
    public class SystemDailySummary
    {
        [DataMember]
        public int SystemId { get; set; }
        [DataMember]
        public string Sytem { get; set; }
        [DataMember]
        public List<DailySummaryType> Cells { get; set; }
    }

    [Serializable]
    [DataContract]
    public class CasinoDailySummaryDetail
    {
        [DataMember]
        public int MainCategoryId { get; set; }
        [DataMember]
        public string MainCategory { get; set; }
        [DataMember]
        public List<SystemDailySummary> SystemSummary { get; set; }
    }

    [Serializable]
    [DataContract]
    public class CasinoDailySummary
    {
        [DataMember]
        public DateTime ReportDate { get; set; }
        [DataMember]
        public List<CasinoDailySummaryDetail> DailySummary { get; set; }
        [DataMember]
        public CasinoDailySummaryDetail GrandTotal { get; set; }
        [DataMember]
        public List<int> SumAllFeedbacks { get; set; }
    }

}
