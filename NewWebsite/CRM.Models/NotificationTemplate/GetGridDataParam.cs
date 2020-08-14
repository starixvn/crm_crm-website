namespace CRM.Models.NotificationTemplate
{
    public class GetGridDataParam
    {
        public int? NotificationTypeId { get; set; }
        public string NotificationTemplateName { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalRows { get; set; }
    }
}