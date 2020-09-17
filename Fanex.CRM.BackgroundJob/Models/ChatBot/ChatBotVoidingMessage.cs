namespace Fanex.CRM.BackgroundJob.Models.ChatBot
{
    using System;
    using global::CRM.Core.Extensions.XML;

    [Serializable]
    [XMLName("r")]
    public class ChatBotVoidingMessage
    {
        [XMLName("MessageID")]
        public int MessageId { get; set; }

        [XMLName("RequestID")]
        public int RequestId { get; set; }
    }
}