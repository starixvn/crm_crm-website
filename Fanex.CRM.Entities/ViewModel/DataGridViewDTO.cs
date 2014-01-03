
namespace Fanex.CRM.Entities.ViewModel
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System;

    [Serializable]
    [DataContract]
    public class FeedbackTemplateDGViewDTO
    {
        [DataMember]
        public int pageindex { get; set; }
        [DataMember]
        public int total { get; set; }
        [DataMember]
        public List<FeedbackTemplate> rows;
    }
}
