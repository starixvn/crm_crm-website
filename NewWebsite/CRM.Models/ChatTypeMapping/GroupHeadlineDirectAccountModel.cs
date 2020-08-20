namespace CRM.Models.ChatTypeMapping
{
    using System.Collections.Generic;

    public class GroupHeadlineDirectAccountModel
    {
        public int HeadlineId { get; set; }

        public string HeadlineName { get; set; }

        public IList<string> DirectAccounts { get; set; }
    }
}