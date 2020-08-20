namespace CRM.Criteria.ChatTypeMapping
{
    using Fanex.Data.Repository;

    public class GetGroupHeadlineByRecipientCriteria : CriteriaBase
    {
        public string RecipientIds { get; set; }

        public override string GetSettingKey() => "Acc_CRM_ChatBot_HeadlineByRecipient_Sel";

        public override bool IsValid() => !string.IsNullOrWhiteSpace(RecipientIds);
    }
}