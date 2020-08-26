namespace CRM.Letstalk.Client.Models.Results
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class PushMessageResult : BaseResult
    {
        [JsonProperty(PropertyName = "failList")]
        public List<string> FailList { get; set; }
    }
}