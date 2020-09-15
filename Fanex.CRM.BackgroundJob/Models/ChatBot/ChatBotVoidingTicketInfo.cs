namespace Fanex.CRM.BackgroundJob.Models.ChatBot
{
    public class ChatBotVoidingTicketInfo
    {
        public int MessageId { get; set; }
        public int CustId { get; set; }
        public string Username { get; set; }
        public string Srecommend { get; set; }
        public string Mrecommend { get; set; }
        public string Recommend { get; set; }
        public bool IsTest { get; set; }
    }
}