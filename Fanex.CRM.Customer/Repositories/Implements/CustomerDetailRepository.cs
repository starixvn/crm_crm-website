// --------------------------------------------------------------------------
// <copyright file="CustomerDetailRepository.cs" company="Nexcel Solutions Vietnam">
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
    using Fanex.Common;
    using Fanex.CRM.Customer.Criteria.DetailInfo;
    using Fanex.CRM.Customer.Models;
    using Fanex.CRM.Customer.Repositories.Interfaces;
    using Fanex.CRM.Infrastructure.Extension;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Setting.Models;
    using Fanex.Data;
    using System.Collections.Generic;

    /// <summary>
    /// Customer Detail Repository.
    /// </summary>
    public class CustomerDetailRepository : ICustomerDetailRepository
    {
        public CustomerInfo GetCustomerInfoDetailById(GetCustomerInfoDetailByIdCriteria criteria)
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
                            MappingInfo_GetCustomerInfoDetailById(reader, ref customer);
                        }
                    }

                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            MappingLanguages_GetCustomerInfoDetailById(reader, ref customer);
                        }
                    }
                }
            }

            return customer;
        }

        public BaseModel UpdateCustomerInfoDetail(UpdateCustomerInfoDetailCriteria criteria)
        {
            BaseModel updateResult = null;
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                objectDB.AssignParamValues(criteria.GetSpParams());
                objectDB.ExecuteNonQuery();
                var outputParams = objectDB.GetOuputParams();
                updateResult = new BaseModel
                {
                    Id = Converter.ToInt(outputParams["errcode"]),
                    Name = Converter.ToString2(outputParams["errmsg"])
                };
            }

            return updateResult;
        }

        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private void MappingInfo_GetCustomerInfoDetailById(IDataReader reader, ref CustomerInfo customer)
        {
            customer = new CustomerInfo
            {
                Title = new BaseModel
                {
                    Id = reader.GetInt32NotNull("TitleID")
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
                    HolderName = reader.GetStringNotNull("AccountHolderName"),
                    AddressInfo = new AddressInfo
                    {
                        LineList = new List<string> 
                        {
                            reader.GetStringNotNull("BankLine1"),
                            reader.GetStringNotNull("BankLine2"),
                            reader.GetStringNotNull("BankLine3"),
                        },

                        City = reader.GetStringNotNull("BankCity"),
                        State = reader.GetStringNotNull("BankState"),
                        PostCode = reader.GetStringNotNull("BankCode"),
                        Country = new CountryInfo
                        {
                            Id = reader.GetInt32NotNull("BankCountryID")
                        }
                    }
                },
                Creator = new UserInfo
                {
                    Id = reader.GetInt32NotNull("CreatorID"),
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
                Passport = new BaseModel
                {
                    Name = reader.GetStringNotNull("Passport")
                },

                AddressInfo = new AddressInfo
                {
                    LineList = new List<string>
                    {
                        reader.GetStringNotNull("Line1"),
                        reader.GetStringNotNull("Line2"),
                        reader.GetStringNotNull("Line3"),
                    },
                    City = reader.GetStringNotNull("City"),
                    State = reader.GetStringNotNull("State"),
                    PostCode = reader.GetStringNotNull("PostCode"),
                    Country = new CountryInfo
                    {
                        Id = reader.GetInt32NotNull("CountryID")
                    }
                }
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
        }

        /// <summary>
        /// Mappings the languages when get customer information by username.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="customer">The customer.</param>
        private void MappingLanguages_GetCustomerInfoDetailById(IDataReader reader, ref CustomerInfo customer)
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
