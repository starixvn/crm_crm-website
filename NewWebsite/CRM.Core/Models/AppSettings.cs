namespace CRM.Core.Models
{
    public class AppSettings
    {
        public short TimeSpan { get; set; }
        public short ClientTimezone { get; set; }
        public short ServerTimezone { get; set; }
        public int SingleSignOnExpiredTime { get; set; }
    }
}