// --------------------------------------------------------------------------
// <copyright file="CustomerDetailRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Customer.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Common;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Customer.Criteria.DetailInfo;
    using Fanex.CRM.Customer.Models;
    using Fanex.CRM.Infrastructure.Extensions;
    using Fanex.CRM.Infrastructure.Models;
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
                objectDB.ExecuteNonQuery(criteria.GetSpParams());
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
                Converter.ToDateTime(reader["BirthDay"]).ToShortFormatedDate() :
                string.Empty;
            customer.BankInfo.HolderName = Converter.ToString2(reader["AccountHolderName"]);
            customer.BankInfo.AddressInfo = new AddressInfo
            {
                LineList = new List<string> 
                {
                    Converter.ToString2(reader["BankLine1"]),
                    Converter.ToString2(reader["BankLine2"]),
                    Converter.ToString2(reader["BankLine3"]),
                },

                City = Converter.ToString2(reader["BankCity"]),
                State = Converter.ToString2(reader["BankState"]),
                PostCode = Converter.ToString2(reader["BankCode"]),
                Country = new CountryInfo
                {
                    Id = Converter.ToInt(reader["BankCountryID"])
                }
            };

            customer.Creator.Id = Converter.ToInt(reader["CreatorID"]);
            customer.HeadLine = Converter.ToBool(reader["IsGroupHeadLine"]);
            customer.CreatedDate = Converter.ToDateTime(reader["CreateDate"]);
            customer.Passport = new BaseModel
            {
                Name = Converter.ToString2(reader["Passport"])
            };

            customer.AddressInfo = new AddressInfo
            {
                LineList = new List<string>
                {
                    Converter.ToString2(reader["Line1"]),
                    Converter.ToString2(reader["Line2"]),
                    Converter.ToString2(reader["Line3"]),
                },
                City = Converter.ToString2(reader["City"]),
                State = Converter.ToString2(reader["State"]),
                PostCode = Converter.ToString2(reader["PostCode"]),
                Country = new CountryInfo
                {
                    Id = Converter.ToInt(reader["CountryID"])
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
                Id = Converter.ToInt(reader["LanguageID"])
            });
        }
    }
}
