namespace CRM.Criteria.Common
{
    using Fanex.Data.Repository;

    public class GetHeadlinesCriteria : CriteriaBase
    {
        public int? Limit { get; set; }

        public string Username { get; set; }

        public bool? ExcludeNotAssignedGroup { get; set; }

        public string RecipientIds { get; set; } = string.Empty;

        public bool IsGetDirectAccount { get; set; } = true;

        public override string GetSettingKey() => "Acc_CRM_HeadlineSel";

        public override bool IsValid() => true;
    }
}