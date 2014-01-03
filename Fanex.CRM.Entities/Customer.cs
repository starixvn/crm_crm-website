// --------------------------------------------------------------------------
// <copyright file="Customer.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Entities
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Customer information.
    /// </summary>
    [Serializable]
    [DataContract]
    public class Customer
    {
        public enum CustomerRole
        {
            /// <summary>
            /// The member.
            /// </summary>
            Member,

            /// <summary>
            /// The agent.
            /// </summary>
            Agent,

            /// <summary>
            /// The master.
            /// </summary>
            Master,

            /// <summary>
            /// The super.
            /// </summary>
            Super,

            /// <summary>
            /// The guest.
            /// </summary>
            Guest
        }

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
        public string RoleName
        {
            get
            {
                return GetRoleName(RoleId);
            }

            set
            {
                value = GetRoleName(RoleId);
            }
        }

        [DataMember]
        public string Remark { get; set; }

        [DataMember]
        public string SpecialRemark { get; set; }

        /// <summary>
        /// Gets the name of the role.
        /// </summary>
        /// <param name="roleId">The role identifier.</param>
        /// <returns>The name of the role.</returns>
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
    }
}
