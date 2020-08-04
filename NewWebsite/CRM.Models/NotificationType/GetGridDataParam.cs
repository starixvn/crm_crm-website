namespace CRM.Models.NotificationType
{
    public class GetGridDataParam
    {
        public int NotificationTypeId { get; set; }

        public string Name { get; set; }

        public int ChatType { get; set; }

        public int RecipientType { get; set; }

        public int System { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}