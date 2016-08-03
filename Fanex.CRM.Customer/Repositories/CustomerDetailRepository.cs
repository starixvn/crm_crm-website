// --------------------------------------------------------------------------
// <copyright file="CustomerDetailRepository.cs" company="">
// Copyright (c) . All rights reserved.
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
        public CustomerInfo GetCustomerInfoDetailById(GetCustomerInfoDetailByIdCriteria criteria)
        {
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                objectDB.AssignParamValues(criteria.GetSpParams());
                using (var reader = objectDB.ExecuteReader())
                {
                    CustomerInfo customer = null;

                    if (reader.Read())
                    {
                        customer = ReadCustomer(reader);
                    }

                    if (customer != null && reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            customer.Languages.Add(new BaseModel
                            {
                                Id = Converter.ToInt(reader["LanguageID"])
                            });
                        }
                    }

                    return customer;
                }
            }
        }

        public Result UpdateCustomerInfoDetail(UpdateCustomerInfoDetailCriteria criteria)
        {
            return ExecuteNonQuery(criteria);
        }

        private CustomerInfo ReadCustomer(IDataReader reader)
        {
            var customer = ReadCustomerBasicInformation(reader);
            customer.BirthDayString = reader["BirthDay"] != DBNull.Value ?
                Converter.ToDateTime(reader["BirthDay"]).ToShortFormatedDate() :
                string.Empty;
            customer.BankInfo.HolderName = Converter.ToString2(reader["AccountHolderName"]);
            customer.BankInfo.AddressInfo = new Address
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
                Country = new Country
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

            customer.AddressInfo = new Address
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
                Country = new Country
                {
                    Id = Converter.ToInt(reader["CountryID"])
                }
            };

            return customer;
        }
    }
}
