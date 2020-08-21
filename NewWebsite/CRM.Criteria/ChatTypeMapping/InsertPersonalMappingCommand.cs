namespace CRM.Criteria.ChatTypeMapping
{
    using CRM.Core.Models;

    public class InsertPersonalMappingCommand : BaseNonQueryCommand
    {
        public int ConversationBotId { get; set; }

        public string ConversationGroupName { get; set; }

        public int ChatTypeId { get; set; }

        public int RecipientTypeId { get; set; }

        public string HeadlineIds { get; set; }

        public int RecipientId { get; set; }

        public string LetstalkId { get; set; }

        public int LanguageId { get; set; }

        public string Remark { get; set; }

        public int UserId { get; set; }

        public override string GetSettingKey() => "Acc_CRM_ChatBot_ChatTypeMapping_Ins";

        private bool IsValidStringProps() => !string.IsNullOrWhiteSpace(ConversationGroupName) &&
            !string.IsNullOrWhiteSpace(HeadlineIds) &&
            !string.IsNullOrWhiteSpace(LetstalkId);

        private bool isValidIntProps() => ConversationBotId > 0 &&
            ChatTypeId > 0 &&
            RecipientTypeId > 0 &&
            RecipientId > 0;

        public override bool IsValid() => IsValidStringProps() &&
            isValidIntProps() &&
            UserId > 0 &&
            LanguageId > 0;
    }
}