namespace CRM.Models.NotificationTemplate
{
    public class CreateNotificationTemplate
    {
        public int NotificationTypeId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string ContentEN { get; set; }
        public string ContentSC { get; set; }
        public string ContentENSC { get; set; }
        public bool IsCombineContentENSC { get; set; }
    }
}