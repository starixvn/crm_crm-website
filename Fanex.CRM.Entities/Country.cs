
namespace Fanex.CRM.Entities
{ 
    using System;
    using System.Runtime.Serialization;
    

    [Serializable]
    [DataContract]
    public class Country
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int CountryId { get; set; }

        [DataMember]
        public string CountryName { get; set; }

        [DataMember]
        public int CountryCode { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public int DisplayOrder { get; set; }
        
    }
}
