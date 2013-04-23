
namespace Fanex.CRM.Entities
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    [DataContract]
    public class FeedbackTemplate
    {
        [DataMember]
        public int TemplateId { get; set; }

        [DataMember]
        public string Subject { get; set; }
    }
   
}
