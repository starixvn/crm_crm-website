namespace Fanex.CRM.BackgroundJob.Models
{
    using System;
    using global::CRM.Core.Extensions.XML;

    [Serializable]
    [XMLName("r")]
    public class VoidingMessage
    {
        [XMLName("MsgID")]
        public int MessageId { get; set; }

        [XMLName("ReqID")]
        public int RequestId { get; set; }
    }
}