namespace CRM.Letstalk.Client.Models.Results
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class BotContactListResult : BaseResult
    {
        [JsonProperty(PropertyName = "contacts")]
        public List<string> Contacts { get; set; }

        public int Count { get; set; }
    }
}