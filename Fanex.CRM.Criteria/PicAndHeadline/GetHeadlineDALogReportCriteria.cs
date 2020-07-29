namespace Fanex.CRM.Criteria.PicAndHeadline
{
    using System;
    using Fanex.Data.Repository;

    public class GetHeadlineDALogReportCriteria : CriteriaBase
    {
        public string UserName { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string ActionTypeIds { get; set; }

        public string EditBy { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalRows { get; set; }

        public override string GetSettingKey() => "Acc_CRM_DirectAccountLogSel";

        public override bool IsValid() => DateTime.Compare(FromDate, ToDate) < 0;
    }
}