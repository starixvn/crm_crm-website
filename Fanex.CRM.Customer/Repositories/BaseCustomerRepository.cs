// --------------------------------------------------------------------------
// <copyright file="BaseCustomerRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Customer.Repositories
{
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Common;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Customer.Models;
    using Fanex.CRM.Infrastructure.Extensions;
    using Fanex.CRM.Infrastructure.Models;

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
                    Id = Converter.ToInt(reader["TitleID"]),
                    Name = string.Empty
                },

                FirstName = Converter.ToString2(reader["FirstName"]),
                LastName = Converter.ToString2(reader["LastName"]),
                Email = Converter.ToString2(reader["Email"]),
                FaxInfo = new FaxInfo
                {
                    CountryCode = Converter.ToString2(reader["FaxCountryCode"]),
                    AreaCode = Converter.ToString2(reader["FaxAreaCode"]),
                    FaxNumber = Converter.ToString2(reader["Fax"])
                },
                MobilePhone = PhoneInfo(reader, "MobilePhone"),
                Skype = Converter.ToString2(reader["Skype"]),
                MSN = Converter.ToString2(reader["msn"]),
                Id = Converter.ToInt(reader["CustId"]),
                Name = Converter.ToString2(reader["UserName"]),
                Role = new BaseModel
                {
                    Id = Converter.ToInt(reader["roleid"])
                },
                SiteSetting = new BaseModel
                {
                    Id = Converter.ToInt(reader["SiteID"])
                },
                BankInfo = new BankInfo
                {
                    Name = Converter.ToString2(reader["Wire_BankName"]),
                    SwiftCode = Converter.ToString2(reader["Wire_SwiftCode"]),
                    AccountName = Converter.ToString2(reader["Wire_AccountName"]),
                    AccountNumber = Converter.ToString2(reader["Wire_AccountNo"])
                },
                Creator = new UserInfo
                {
                    Name = Converter.ToString2(reader["CreatorName"])
                },
                Country = new BaseModel
                {
                    Id = Converter.ToInt(reader["CountryID"])
                },
                //LicenseeType = new BaseModel 
                //{
                //    Id = Converter.ToInt(reader["LicenseeType"])
                //},

                Question = Converter.ToString2(reader["Question"]),
                Answer = Converter.ToString2(reader["Answer"]),
                IsVIP = Converter.ToBool(reader["IsVIP"]),
                CreatedDate = Converter.ToDateTime(reader["CreateDate"]),
                BirthDay = Converter.ToDateTime(reader["BirthDay"]).ToShortFormatedDate(),
                WeChat = Converter.ToString2(reader["WeChat"]),
                WhatsApp = Converter.ToString2(reader["WhatsApp"]),
                Line = Converter.ToString2(reader["Line"]),
                Yahoo = Converter.ToString2(reader["Yahoo"]),
                QQ = Converter.ToString2(reader["QQ"]),
                BBM = Converter.ToString2(reader["BBM"])
            };

            customer.NickName = customer.Id > 0 ?
                Converter.ToString2(reader["UserName2"]) :
                string.Empty;
            
            customer.PersonalInfoContacts.Add(new PersonalInfoContact
            {
                Name = Converter.ToString2(reader["Pic1Name"]),
                Phone = Converter.ToString2(reader["Pic1"])
            });
            customer.PersonalInfoContacts.Add(new PersonalInfoContact
            {
                Name = Converter.ToString2(reader["Pic2Name"]),
                Phone = Converter.ToString2(reader["Pic2"])
            });
            customer.PersonalInfoContacts.Add(new PersonalInfoContact
            {
                Name = Converter.ToString2(reader["Pic3Name"]),
                Phone = Converter.ToString2(reader["Pic3"])
            });
            customer.PersonalInfoContacts.Add(new PersonalInfoContact
            {
                Name = Converter.ToString2(reader["Pic4Name"]),
                Phone = Converter.ToString2(reader["Pic4"])
            });

            customer.BettingCode.Add(Converter.ToString2(reader["RefCode1"]));
            customer.BettingCode.Add(Converter.ToString2(reader["RefCode2"]));

            customer.PhoneInfo.Add(new PhoneInfo
            {
                CountryCode = Converter.ToString2(reader["PhoneCountryCode"]),
                AreaCode = Converter.ToString2(reader["PhoneAreaCode"]),
                PhoneNumber = Converter.ToString2(reader["Phone"])
            });

            customer.PhoneInfo.Add(new PhoneInfo
            {
                CountryCode = Converter.ToString2(reader["Phone2CountryCode"]),
                AreaCode = Converter.ToString2(reader["Phone2AreaCode"]),
                PhoneNumber = Converter.ToString2(reader["Phone2"])
            });

            customer.PhoneInfo.Add(new PhoneInfo
            {
                CountryCode = Converter.ToString2(reader["Phone3CountryCode"]),
                AreaCode = Converter.ToString2(reader["Phone3AreaCode"]),
                PhoneNumber = Converter.ToString2(reader["Phone3"])
            });

            return customer;
        }

        private PhoneInfo PhoneInfo(IDataReader reader, string fieldName)
        {
            return new PhoneInfo
            {
                CountryCode = Converter.ToString2(reader[fieldName + "CountryCode"]),
                AreaCode = Converter.ToString2(reader[fieldName + "AreaCode"]),
                PhoneNumber = Converter.ToString2(reader[fieldName])
            };
        }
    }
}