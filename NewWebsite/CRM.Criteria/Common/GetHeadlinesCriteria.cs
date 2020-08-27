namespace CRM.Criteria.Common
{
    using Fanex.Data.Repository;

    public class GetHeadlinesCriteria : CriteriaBase
    {
        public int? Limit { get; set; }

        public string Username { get; set; }

        public bool? ExcludeNotAssignedGroup { get; set; }

        public override string GetSettingKey() => "Acc_CRM_HeadlineSel";

        public override bool IsValid() => true;
    }
}