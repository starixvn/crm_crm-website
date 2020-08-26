namespace CRM.ViewModels.ChatTypeMapping
{
    using System.Collections.Generic;

    public class RecipientLetstalk
    {
        public int RecipientId { get; set; }

        public string RecipientName { get; set; }

        public List<LetstalkInGroup> Letstalks { get; set; }
    }
}