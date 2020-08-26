namespace CRM.ViewModels.ChatTypeMapping
{
    using System.Collections.Generic;

    public class LetstalkVerification
    {
        public int BotId { get; set; }

        public string GroupConversationId { get; set; }

        public List<RecipientLetstalk> Recipients { get; set; }
    }
}