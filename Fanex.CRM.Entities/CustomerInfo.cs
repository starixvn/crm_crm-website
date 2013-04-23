
namespace Fanex.CRM.Entities
{
    using System;
    using System.Runtime.Serialization;


    [Serializable]
    [DataContract]
    public class CustomerInfo
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public Setting Title { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string Fax { get; set; }
        [DataMember]
        public string Phone { get; set; }
        [DataMember]
        public string MobilePhone { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string Skype { get; set; }
        [DataMember]
        public string MSN { get; set; }
        [DataMember]
        public DateTime? BirthDay { get; set; }

       
    }
}
