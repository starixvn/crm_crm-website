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
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Common;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Customer.Criteria.DetailInfo;
    using Fanex.CRM.Customer.Models;
    using Fanex.CRM.Customer.Repositories.Interfaces;
    using Fanex.CRM.Infrastructure.Extension;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Setting.Models;
    using Fanex.Data;

    /// <summary>
    /// Customer Detail Repository.
    /// </summary>
    public class CustomerDetailRepository : BaseCustomerRepository, ICustomerDetailRepository
    {
        /// <summary>
        /// Gets the customer information detail by identifier.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The customer information detail by identifier.</returns>
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

        /// <summary>
        /// Updates the customer information detail.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Update result.</returns>
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

        /// <summary>
        /// Mappings the information get customer information detail by identifier.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="customer">The customer.</param>
        private void MappingInfo_GetCustomerInfoDetailById(IDataReader reader, ref CustomerInfo customer)
        {
            customer = MappingDataFromSqlToCustomerInfoObject(reader);
            customer.BirthDayString = reader["BirthDay"] != DBNull.Value ?
                 reader.GetDateTimeNotNull("BirthDay").ToShortFormatedDate() :
                 string.Empty;
            customer.BankInfo.HolderName = reader.GetStringNotNull("AccountHolderName");
            customer.BankInfo.AddressInfo = new AddressInfo
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
            };

            customer.Creator.Id = reader.GetInt32NotNull("CreatorID");
            customer.HeadLine = Converter.ToBool(reader["IsGroupHeadLine"]);
            customer.CreatedDate = reader.GetDateTimeNotNull("CreateDate");
            customer.Passport = new BaseModel
            {
                Name = reader.GetStringNotNull("Passport")
            };

            customer.AddressInfo = new AddressInfo
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
            };
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
