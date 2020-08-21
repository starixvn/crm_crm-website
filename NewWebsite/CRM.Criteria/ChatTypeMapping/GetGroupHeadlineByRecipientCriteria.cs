namespace CRM.Criteria.ChatTypeMapping
{
    using Fanex.Data.Repository;

    public class GetGroupHeadlineByRecipientCriteria : CriteriaBase
    {
        public string RecipientIds { get; set; }

        public bool IsGetDirectAccount { get; set; }

        public bool ExcludeNotAssignedGroup { get; set; }

        public override string GetSettingKey() => "Acc_CRM_HeadlineSel";

        public override bool IsValid() => !string.IsNullOrWhiteSpace(RecipientIds);
    }
}