namespace CRM.Models.NotificationType
{
    public class GetGridDataParam
    {
        public string NotificationTypeName { get; set; }

        public string ChatTypeId { get; set; }

        public string RecipientTypeId { get; set; }

        public string SystemId { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalRows { get; set; }
    }
}