namespace CRM.Models.ChatTypeMapping
{
    public class ConversationRecipient
    {
        public int ConversationGroupId { get; set; }

        public int RecipientId { get; set; }

        public string RecipientName { get; set; }

        public string LetstalkIds { get; set; }
    }
}