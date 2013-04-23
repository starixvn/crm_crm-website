
namespace Fanex.CRM.Entities
{
    using System;
    using System.Runtime.Serialization;
    


    [Serializable]
    [DataContract]
    public class Site
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string SiteId { get; set; }
        [DataMember]
        public string SiteName { get; set; }
        [DataMember]//0: credit; 1: licensee; 2: other
        public int SiteType { get; set; }
        [DataMember]
        public int Order { get; set; }
        [DataMember]
        public bool IsActive { get; set; }

    }
}
