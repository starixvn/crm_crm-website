// --------------------------------------------------------------------------
// <copyright file="CustomerRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Customer.Repositories.Implements
{
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.CRM.Customer.Criteria;
    using Fanex.CRM.Customer.Models;
    using Fanex.CRM.Customer.Repositories.Interfaces;
    using Fanex.CRM.Infrastructure.Extension;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.Data;
    using System.Collections.Generic;
    using Fanex.CRM.Setting.Services.Implements;

    /// <summary>
    /// Customer Repository.
    /// </summary>
    public class CustomerRepository : ICustomerRepository
    {
        /// <summary>
        /// Gets the customer information by username.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The customer information by username.</returns>
        public CustomerInfo GetCustomerInfoByUsername(GetCustomerInfoByUsernameCriteria criteria)
        {
            CustomerInfo customer = null;
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                objectDB.AssignParamValues(criteria.GetSpParams());
                using (var reader = objectDB.ExecuteReader())
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            MappingInfo_GetCustomerInfoByUsername(reader, ref customer);
                        }

                        while (reader.Read())
                        {
                            MappingLanguages_GetCustomerInfoByUsername(reader, ref customer);
                        }
                    }
                }
            }

            return customer;
        }

        public List<CustomerInfo> GetUplineInfoOfUser(GetUplineInfoOfUserCriteria criteria)
        {
            var uplineInfo = new List<CustomerInfo>();
            using (var obj = new ObjectDb(criteria.GetSettingKey()))
            {
                obj.AssignParamValues(criteria.GetSpParams());
                using (var reader = obj.ExecuteReader())
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            var customer = new CustomerInfo
                            {
                                Id = reader.GetInt32NotNull("CustId"),
                                Name = reader.GetStringNotNull("UserName"),
                                Role = new BaseModel
                                {
                                    Id = reader.GetByteNotNull("roleid")
                                },
                                IsActive = reader.GetInt32NotNull("Active") == 1,
                                IsClosed = reader.GetInt32NotNull("Closed") == 1,
                                IsSuspended = reader.GetInt32NotNull("Suspended") == 1
                            };

                            uplineInfo.Add(customer);
                        }
                    }
                }
            }

            return uplineInfo;
        }

        public bool UpdateCustomerSuggestion(UpdateCustomerSuggestionCriteria criteria)
        {
            var updateResult = false;
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                objectDB.AssignParamValues(criteria.GetSpParams());
                updateResult = objectDB.ExecuteNonQuery() > 0;
            }

            return updateResult;
        }

        /// <summary>
        /// Mappings the information when get customer information by username.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="customer">The customer.</param>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
             "SP2101:MethodMustNotContainMoreLinesThan",
             Justification = "Reviewed. Suppression is OK here.")]
        private void MappingInfo_GetCustomerInfoByUsername(IDataReader reader, ref CustomerInfo customer)
        {
            customer = new CustomerInfo
            {
                Title = new BaseModel
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
                    PhoneNumber = reader.GetStringNotNull("MobilePhone")
                },

                BirthDay = reader.GetDateTimeNotNull("BirthDay"),
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
                    Address = reader.GetStringNotNull("Wire_BankAddress"),
                    SwiftCode = reader.GetStringNotNull("Wire_SwiftCode"),
                    AccountName = reader.GetStringNotNull("Wire_AccountName"),
                    AccountNumber = reader.GetStringNotNull("Wire_AccountNo"),
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
                IsActive = reader.GetInt32NotNull("Active") == 1,
                IsClosed = reader.GetInt32NotNull("Closed") == 1,
                IsSuspended = reader.GetInt32NotNull("Suspended") == 1
            };

            customer.NickName = customer.Id > 0 ?
                reader.GetStringNotNull("UserName2") :
                string.Empty;

            customer.PersonalInfoContact.Add(reader.GetStringNotNull("PIC1"));
            customer.PersonalInfoContact.Add(reader.GetStringNotNull("PIC2"));
            customer.PersonalInfoContact.Add(reader.GetStringNotNull("PIC3"));

            customer.RefCode.Add(reader.GetStringNotNull("RefCode1"));
            customer.RefCode.Add(reader.GetStringNotNull("RefCode2"));

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
        }

        /// <summary>
        /// Mappings the languages when get customer information by username.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="customer">The customer.</param>
        private void MappingLanguages_GetCustomerInfoByUsername(IDataReader reader, ref CustomerInfo customer)
        {
            if (customer == null)
            {
                return;
            }

            customer.Languages.Add(new BaseModel
            {
                Id = reader.GetInt32NotNull("LanguageID")
            });
        }
    }
}
