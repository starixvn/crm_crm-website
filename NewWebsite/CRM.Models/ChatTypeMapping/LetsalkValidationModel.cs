namespace CRM.Models.ChatTypeMapping
{
    public class LetsalkValidationModel
    {
        public int BotId { get; set; }

        public string LetstalkId { get; set; }

        public bool IsFriendWithBot { get; set; }

        public string DisplayName { get; set; }

        public bool IsInvalid { get; set; }
    }
}