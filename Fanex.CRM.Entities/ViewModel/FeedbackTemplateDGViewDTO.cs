
namespace Fanex.CRM.Entities.ViewModel
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System;

    [Serializable]
    [DataContract]
    public class DataGridViewDTO
    {
        [DataMember]
        public int pageindex { get; set; }
        [DataMember]
        public int total { get; set; }
        [DataMember]
        public List<FeedbackView> rows;
    }
}
