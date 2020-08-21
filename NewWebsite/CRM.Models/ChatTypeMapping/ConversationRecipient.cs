namespace CRM.Models.ChatTypeMapping
{
    public class ConversationRecipient
    {
        public int ConversationGroupId { get; set; }

        public int RecipientTypeId { get; set; }

        public string RecipientTypeName { get; set; }

        public int RecipientId { get; set; }

        public string RecipientName { get; set; }

        public string RoleName { get; set; }

        public string LetstalkId { get; set; }
    }
}