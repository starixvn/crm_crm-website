namespace CRM.Models.ChatTypeMapping
{
    public class ChatTypeMappingModel
    {
        public int ChatTypeId { get; set; }

        public string ChatType { get; set; }

        public string BotName { get; set; }

        public int ConversationId { get; set; }

        public string Name { get; set; }

        public string RecipientType { get; set; }

        public string GroupHeadline { get; set; }

        public string Recipient { get; set; }

        public string LetstalkId { get; set; }

        public int MappingStatusId { get; set; }

        public string MappingStatus { get; set; }

        public int LanguageId { get; set; }

        public string Language { get; set; }

        public bool IsActive { get; set; }

        public string Remark { get; set; }
    }
}