namespace CRM.Criteria.ChatTypeMapping
{
    using CRM.Core.Models;

    public class UpdateChatTypeMappingCommand : BaseNonQueryCommand
    {
        public int ConversationGroupId { get; set; }

        public int BotId { get; set; }

        public string ConversationGroupName { get; set; }

        public int RecipientTypeId { get; set; }

        public string HeadlineIds { get; set; }

        public string RecipientIds { get; set; }

        public string LetstalkId { get; set; }

        public int LanguageId { get; set; }

        public string Remark { get; set; }

        public int UserId { get; set; }

        public override string GetSettingKey() => "Acc_CRM_ChatBot_ChatTypeMapping_Upd";

        private bool IsValidStringProps() => !string.IsNullOrWhiteSpace(ConversationGroupName) &&
            !string.IsNullOrWhiteSpace(HeadlineIds) &&
            !string.IsNullOrWhiteSpace(RecipientIds) &&
            !string.IsNullOrWhiteSpace(LetstalkId);

        private bool isValidIntProps() => BotId > 0 &&
            RecipientTypeId > 0 &&
            ConversationGroupId > 0;

        public override bool IsValid() => IsValidStringProps() &&
            isValidIntProps() &&
            UserId > 0 &&
            LanguageId > 0;
    }
}