namespace CRM.Models.ChatTypeMapping
{
    using System.Collections.Generic;

    public class GroupHeadlineDirectAccountModel : BaseModel
    {
        public IEnumerable<string> DirectAccounts { get; set; }
    }
}