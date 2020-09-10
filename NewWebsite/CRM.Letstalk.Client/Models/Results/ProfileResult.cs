namespace CRM.Letstalk.Client.Models.Results
{
    using Newtonsoft.Json;

    public class ProfileResult : BaseResult
    {
        [JsonProperty(PropertyName = "avatar_url")]
        public string AvatarUrl { get; set; }

        [JsonProperty(PropertyName = "display_name")]
        public string DisplayName { get; set; }
    }
}