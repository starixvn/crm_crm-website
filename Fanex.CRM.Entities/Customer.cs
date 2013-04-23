
namespace Fanex.CRM.Entities
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    [DataContract]
    public class Customer
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public CustomerInfo CustomerInfo { get; set; }

        [DataMember]
        public int RoleId { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string NickName { get; set; }

        [DataMember]
        public string Pic1 { get; set; }

        [DataMember]
        public string Pic2 { get; set; }

        [DataMember]
        public string Pic3 { get; set; }

        [DataMember]
        public Customer RelatedCustomer { get; set; }

        [DataMember]
        public Setting Site { get; set; }

        [DataMember]
        public BankInfo BankInfo { get; set; }

        [DataMember]
        public User Creator { get; set; }

        [DataMember]
        public string Question { get; set; }

        [DataMember]
        public string Answer { get; set; }

        [DataMember]
        public string BettingCode { get; set; }

        [DataMember]
        public string RefCode2 { get; set; }

        [DataMember]
        public bool IsVip { get; set; }

        [DataMember]
        public DateTime? CreatedDate { get; set; }

        [DataMember]
        public int Country { get; set; }

        [DataMember]
        public string CountryName { get; set; }

        [DataMember]
        public int Language { get; set; }
        
        [DataMember]
        public string RoleName { 
            get { return GetRoleName(RoleId); }
            set { value = GetRoleName(RoleId); }
        }

        [DataMember]
        public string Remark { get; set; }

        [DataMember]
        public string SpecialRemark { get; set; }

        private string GetRoleName(int roleId)
        {
            switch (roleId)
            {
                case 1:
                case 7000:
                    return CustomerRole.Member.ToString();
                case 2:
                case 7001:
                    return CustomerRole.Agent.ToString();
                case 3:
                case 7002:
                    return CustomerRole.Master.ToString();
                case 4:
                case 7003:
                    return CustomerRole.Super.ToString();
                default:
                    return CustomerRole.Guest.ToString();
            }
        }

        public enum CustomerRole
        {
            Member,
            Agent,
            Master,
            Super,
            Guest
        }
    }

    [Serializable]
    [DataContract]
    public class RelatedCustomer
    {
        [DataMember]
        public int CustId { get; set; }
        [DataMember]
        public string UserName { get; set; }
    }

    [Serializable]
    [DataContract]
    public class SearchCustomerInfo 
    { 
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public bool IsGuest { get; set; }
    }
}
