namespace Fanex.CRM.AdminTool.Models.CommunicationToolSetting
{
    public class GetCommunicationToolSettingParam
    {
        public string Username { get; set; }
        public string CategoryIds { get; set; }
        public string SubCategoryIds { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}