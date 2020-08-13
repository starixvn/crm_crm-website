namespace CRM.Criteria.ChatBot
{
    using Fanex.Data.Repository;

    public class GetRecipientsByTypeCriteria : CriteriaBase
    {
        public int RecipientType { get; set; }

        public override string GetSettingKey() => "Acc_CRM_ChatBot_Recipient_Sel";

        public override bool IsValid() => RecipientType > 0;
    }
}