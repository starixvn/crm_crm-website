
namespace Fanex.CRM.Entities
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    [DataContract]
    public class SettingRelation 
    {
        [DataMember]
        public int SettingId { get; set; }
        [DataMember]
        public int ParentId { get; set; }
        [DataMember]
        public int Order { get; set; }

    }
}
