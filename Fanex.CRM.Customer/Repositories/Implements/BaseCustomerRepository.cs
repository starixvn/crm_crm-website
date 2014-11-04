// --------------------------------------------------------------------------
// <copyright file="BaseCustomerRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Customer.Repositories.Implements
{
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Common;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Customer.Criteria;
    using Fanex.CRM.Customer.Models;
    using Fanex.CRM.Customer.Repositories.Interfaces;
    using Fanex.CRM.Infrastructure.Extension;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Setting.Models;
    using Fanex.Data;

    /// <summary>
    /// Base Customer Repository.
    /// </summary>
    public class BaseCustomerRepository
    {
        /// <summary>
        /// Mappings the information when get customer information by username.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>Customer information.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
             "SP2101:MethodMustNotContainMoreLinesThan",
             Justification = "Reviewed. Suppression is OK here.")]
        public CustomerInfo MappingDataFromSqlToCustomerInfoObject(IDataReader reader)
        {
            var customer = new CustomerInfo
            {
                Title = new SettingInfo
                {
                    Id = reader.GetInt32NotNull("TitleID"),
                    Name = string.Empty
                },

                FirstName = reader.GetStringNotNull("FirstName"),
                LastName = reader.GetStringNotNull("LastName"),
                Email = reader.GetStringNotNull("Email"),
                FaxInfo = new FaxInfo
                {
                    CountryCode = reader.GetStringNotNull("FaxCountryCode"),
                    AreaCode = reader.GetStringNotNull("FaxAreaCode"),
                    FaxNumber = reader.GetStringNotNull("Fax")
                },
                MobilePhone = new PhoneInfo
                {
                    CountryCode = reader.GetStringNotNull("MobilePhoneCountryCode"),
                    AreaCode = reader.GetStringNotNull("MobilePhoneAreaCode"),
                    PhoneNumber = reader.GetStringNotNull("MobilePhone")
                },

                Skype = reader.GetStringNotNull("Skype"),
                MSN = reader.GetStringNotNull("msn"),
                Id = reader.GetInt32NotNull("CustId"),
                Name = reader.GetStringNotNull("UserName"),
                Role = new BaseModel
                {
                    Id = reader.GetInt32NotNull("roleid")
                },
                SiteSetting = new BaseModel
                {
                    Id = reader.GetInt32NotNull("SiteID")
                },
                BankInfo = new BankInfo
                {
                    Name = reader.GetStringNotNull("Wire_BankName"),
                    SwiftCode = reader.GetStringNotNull("Wire_SwiftCode"),
                    AccountName = reader.GetStringNotNull("Wire_AccountName"),
                    AccountNumber = reader.GetStringNotNull("Wire_AccountNo")
                },
                Creator = new UserInfo
                {
                    Name = reader.GetStringNotNull("CreatorName")
                },
                Country = new BaseModel
                {
                    Id = reader.GetInt32NotNull("CountryID")
                },

                Question = reader.GetStringNotNull("Question"),
                Answer = reader.GetStringNotNull("Answer"),
                IsVIP = reader.GetBooleanNotNull("IsVIP"),
                CreatedDate = reader.GetDateTimeNotNull("CreateDate"),
                BirthDay = reader.GetDateTimeNotNull("BirthDay").ToShortFormatedDate()
            };

            customer.NickName = customer.Id > 0 ?
                reader.GetStringNotNull("UserName2") :
                string.Empty;
            customer.PersonalInfoContact.Add(reader.GetStringNotNull("PIC1"));
            customer.PersonalInfoContact.Add(reader.GetStringNotNull("PIC2"));
            customer.PersonalInfoContact.Add(reader.GetStringNotNull("PIC3"));
            customer.PersonalInfoContact.Add(reader.GetStringNotNull("PIC4"));

            customer.BettingCode.Add(reader.GetStringNotNull("RefCode1"));
            customer.BettingCode.Add(reader.GetStringNotNull("RefCode2"));

            customer.PhoneInfo.Add(new PhoneInfo
            {
                CountryCode = reader.GetStringNotNull("PhoneCountryCode"),
                AreaCode = reader.GetStringNotNull("PhoneAreaCode"),
                PhoneNumber = reader.GetStringNotNull("Phone")
            });

            customer.PhoneInfo.Add(new PhoneInfo
            {
                CountryCode = reader.GetStringNotNull("Phone2CountryCode"),
                AreaCode = reader.GetStringNotNull("Phone2AreaCode"),
                PhoneNumber = reader.GetStringNotNull("Phone2")
            });

            customer.PhoneInfo.Add(new PhoneInfo
            {
                CountryCode = reader.GetStringNotNull("Phone3CountryCode"),
                AreaCode = reader.GetStringNotNull("Phone3AreaCode"),
                PhoneNumber = reader.GetStringNotNull("Phone3")
            });

            return customer;
        }
    }
}