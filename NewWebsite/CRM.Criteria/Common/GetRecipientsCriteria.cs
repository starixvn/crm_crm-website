namespace CRM.Criteria.ChatBot
{
    using Fanex.Data.Repository;

    public class GetRecipientsCriteria : CriteriaBase
    {
        public string RecipientName { get; set; }

        public int? Limit { get; set; }

        public int? RecipientType { get; set; }

        public override string GetSettingKey() => "Acc_CRM_ChatBot_Recipient_Sel";

        public override bool IsValid() => true;
    }
}