namespace Fanex.CRM.BackgroundJob.Models
{
    public class AbnormalBetSystemSetting
    {
        public string JobStatus { get; set; }
        public int BatchSize { get; set; }
        public int LastMsgId { get; set; }
        public bool ConfigUAT { get; set; }
        public bool ConfigPRO { get; set; }
    }
}