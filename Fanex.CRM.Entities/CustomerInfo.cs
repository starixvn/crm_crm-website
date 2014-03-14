// --------------------------------------------------------------------------
// <copyright file="CustomerInfo.cs" company="Nexcel Solutions Vietnam">
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
    /// Customer Information.
    /// </summary>
    [Serializable]
    [DataContract]
    public class CustomerInfo
    {
        [DataMember]
        public Setting Title { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public PhoneInfo ContactNumber1 { get; set; }
        [DataMember]
        public PhoneInfo ContactNumber2 { get; set; }
        [DataMember]
        public PhoneInfo ContactNumber3 { get; set; }
        [DataMember]
        public FaxInfo Fax { get; set; }
        [DataMember]
        public PhoneInfo Mobile { get; set; }
        [DataMember]
        public string PIC1 { get; set; }
        [DataMember]
        public string PIC2 { get; set; }
        [DataMember]
        public string PIC3 { get; set; }
        [DataMember]
        public string PIC4 { get; set; }
        [DataMember]
        public string Skype { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string MSN { get; set; }
        [DataMember]
        public string PassportNumber { get; set; }
        [DataMember]
        public DateTime? BirthDay { get; set; }
        [DataMember]
        public string BirthDayString 
        {
            get
            {
                if (!BirthDay.HasValue || BirthDay.Value == DateTime.MinValue)
                {
                    return string.Empty;
                }

                return BirthDay.Value.ToString("MM/dd/yyyy");
            }
        }

        [DataMember]
        public int Site { get; set; }
        [DataMember]
        public string BettingCode { get; set; }
        [DataMember]
        public string RefCodes2 { get; set; }
        [DataMember]
        public bool IsVIP { get; set; }
    }
}
