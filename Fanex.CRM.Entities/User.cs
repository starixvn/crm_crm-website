
namespace Fanex.CRM.Entities
{
    using System;
    using System.Runtime.Serialization;
    using System.Collections.Generic;
    using System.Linq;


    [Serializable]
    [DataContract]
    public class User
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Username { get; set; }
        [DataMember]
        public int RoleId { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public bool IsAllowUpdateReview { get; set; }

        private IEnumerable<Menu> ListMenu { get; set; }

    }
}
