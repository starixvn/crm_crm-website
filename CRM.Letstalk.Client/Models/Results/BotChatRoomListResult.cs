namespace CRM.Letstalk.Client.Models.Results
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class BotChatRoomListResult : BaseResult
    {
        [JsonProperty(PropertyName = "rooms")]
        public List<string> Rooms { get; set; }

        public int Count { get; set; }
    }
}