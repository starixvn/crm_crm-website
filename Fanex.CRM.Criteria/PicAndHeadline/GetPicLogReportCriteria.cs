namespace Fanex.CRM.Criteria.PicAndHeadline
{
    using System;
    using Fanex.Data.Repository;

    public class GetPicLogReportCriteria : CriteriaBase
    {
        public string Pic { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string EditBy { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public override string GetSettingKey() => "xyz";

        public override bool IsValid() => true;
    }
}