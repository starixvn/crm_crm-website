namespace Fanex.CRM.InformHeadline.Criteria
{
    using Fanex.Data.Repository;

    public class GetRemovedAccountLogDetailsCriteria : CriteriaBase
    {
        public int LogId { get; set; }
        public int PageIndex { get; set; }
        public int ItemsPerPage { get; set; }
        public int TotalRows { get; set; }

        public override string GetSettingKey() => "Acc_CRM_InformHeadlineHistorySel";

        public override bool IsValid() => LogId > 0;
    }
}