namespace CRM.Models.NotificationType
{
    public class GetGridDataParam
    {
        public string NotificationTypeName { get; set; }

        public string ChatTypeIds { get; set; }

        public string RecipientTypeIds { get; set; }

        public string SystemId { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalRows { get; set; }
    }
}