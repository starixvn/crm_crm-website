namespace CRM.Letstalk.Client.Models.Results
{
    using Newtonsoft.Json;

    public class ProfileResult : BaseResult
    {
        private const int AccountDoesNotExistStatus = 21;

        [JsonProperty(PropertyName = "avatar_url")]
        public string AvatarUrl { get; set; }

        [JsonProperty(PropertyName = "display_name")]
        public string DisplayName { get; set; }

        public bool IsAccountNotExistent => Status == AccountDoesNotExistStatus;
    }
}