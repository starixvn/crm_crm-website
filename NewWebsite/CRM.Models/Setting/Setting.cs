namespace CRM.Models.Setting
{
    public class Setting
    {
        public int SettingId { get; set; }
        public string SettingName { get; set; }
        public string SettingValue { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
    }
}