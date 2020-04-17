namespace Fanex.CRM.BackgroundJob.Models
{
    public class Message
    {
        public long MessageId { get; set; }
        public int MsgContentsType { get; set; }
        public string Content_en { get; set; }
        public string Content_cs { get; set; }
    }
}