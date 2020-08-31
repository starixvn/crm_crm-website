namespace CRM.Letstalk.Core.Models
{
    public class WebhookConfiguration
    {
        public int BotId { get; set; }

        public bool IsAllowedSet { get; set; }

        public string WebhookUrl { get; set; }
    }
}