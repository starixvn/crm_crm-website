namespace CRM.Letstalk.Core.Models
{
    public class WebhookConfiguration
    {
        public int BotId { get; set; }

        public bool IsAllowedSet { get; set; }

        public string WebhookBaseUrl { get; set; }
    }
}