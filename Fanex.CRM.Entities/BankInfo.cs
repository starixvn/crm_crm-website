
namespace Fanex.CRM.Entities
{ 
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    [DataContract]
    public class BankInfo
    {
        [DataMember]
        public string BankName { get; set; }

        [DataMember]
        public string BankAddress { get; set; }

        [DataMember]
        public string SwiftCode { get; set; }

        [DataMember]
        public string AccountName { get; set; }

        [DataMember]
        public string AccountHolderName { get; set; }

        [DataMember]
        public string AccountNo { get; set; }
    }
}
