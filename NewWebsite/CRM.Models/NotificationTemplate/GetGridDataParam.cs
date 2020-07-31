namespace CRM.Models.NotificationTemplate
{
    public class GetGridDataParam
    {
        public int NotificationTypeId { get; set; }
        public string TemplateName { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}