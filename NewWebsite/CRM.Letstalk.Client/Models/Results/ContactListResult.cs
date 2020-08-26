namespace CRM.Letstalk.Client.Models.Results
{
    using System.Collections.Generic;

    public class ContactListResult : BaseResult
    {
        public List<Contact> Contacts { get; set; }
    }
}