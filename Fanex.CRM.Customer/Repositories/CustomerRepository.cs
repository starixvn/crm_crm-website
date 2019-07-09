// --------------------------------------------------------------------------
// <copyright file="CustomerRepository.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Customer.Repositories
{
    using Fanex.Common;
    using Fanex.CRM.Customer.Criteria;
    using Fanex.CRM.Customer.Models;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.Data;
    using System.Collections.Generic;

    /// <summary>
    /// Customer Repository.
    /// </summary>
    public class CustomerRepository : BaseCustomerRepository, ICustomerRepository
    {
        public CustomerInfo GetCustomerInfoByUsername(GetCustomerInfoByUsernameCriteria criteria)
        {
            using (var objectDB = new ObjectDb(criteria.SettingKey()))
            {
                using (var reader = objectDB.ExecuteReader(criteria.SPParams()))
                {
                    CustomerInfo customer = null;
                    if (reader.Read())
                    {
                        customer = ReadCustomerBasicInformation(reader);
                        customer.HeadLine = Converter.ToBool(reader["IsGroupHeadLine"]);
                        customer.BankInfo.Address = Converter.ToString2(reader["Wire_BankAddress"]);

                        customer.CustStatus = Converter.ToString2(reader["CustStatus"]);
                        customer.Locked = Converter.ToBool(reader["Locked"]);
                        customer.OriginalDangerLevel = Converter.ToInt(reader["danger"]);
                        customer.ABTDangerLevel = Converter.ToInt(reader["danger2"]);
                        customer.OtherSportsDangerLevel = Converter.ToInt(reader["danger3"]);
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

        public IEnumerable<CustomerInfo> GetUplineInfoOfUser(GetUplineInfoOfUserCriteria criteria)
        {
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objectDB.ExecuteReader(criteria.GetSpParams()))
                {
                    var uplineInfo = new List<CustomerInfo>();
                    while (reader.Read())
                    {
                        var customer = new CustomerInfo
                        {
                            Id = Converter.ToInt(reader["CustId"]),
                            Name = Converter.ToString2(reader["UserName"]),
                            Role = new BaseModel
                            {
                                Id = Converter.ToInt(reader["roleid"])
                            },
                            CustStatus = Converter.ToString2(reader["CustStatus"]),
                            Locked = Converter.ToBool(reader["Locked"])
                        };

                        uplineInfo.Add(customer);
                    }

                    return uplineInfo;
                }
            }
        }

        public Result UpdateCustomerInfo(UpdateCustomerInfoCriteria criteria)
        {
            return ExecuteNonQuery(criteria);
        }

        public IEnumerable<BaseModel> VerifyUsername(VerifyRelatedCustomerCriteria criteria)
        {
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objectDB.ExecuteReader(criteria.GetSpParams()))
                {
                    var result = new List<BaseModel>();
                    while (reader.Read())
                    {
                        var customer = new BaseModel()
                        {
                            Id = Converter.ToInt(reader["custid"]),
                            Name = Converter.ToString2(reader["val"])
                        };

                        result.Add(customer);
                    }

                    return result;
                }
            }
        }

        public IEnumerable<string> GetCustomersByUsername(CustomerSuggestionCriteria criteria)
        {
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                return objectDB.Query<string>(criteria.GetSpParams());
            }
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
    }
}