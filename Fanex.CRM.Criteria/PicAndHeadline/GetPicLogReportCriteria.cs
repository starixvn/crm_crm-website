namespace Fanex.CRM.Criteria.PicAndHeadline
{
    using System;
    using Fanex.Data.Repository;

    public class GetPicLogReportCriteria : CriteriaBase
    {
        public string PicName { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string EditBy { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalRows { get; set; }

        public override string GetSettingKey() => "Acc_CRM_PicLogSel";

        public override bool IsValid() => true;
    }
}