// --------------------------------------------------------------------------
// <copyright file="CustomerRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Customer.Repositories
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Common;
    using Fanex.CRM.Customer.Criteria;
    using Fanex.CRM.Customer.Models;
    using Fanex.CRM.Infrastructure.Extensions;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.Data;

    /// <summary>
    /// Customer Repository.
    /// </summary>
    public class CustomerRepository : BaseCustomerRepository, ICustomerRepository
    {
        /// <summary>
        /// Gets the customer information by username.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The customer information by username.</returns>
        public CustomerInfo GetCustomerInfoByUsername(GetCustomerInfoByUsernameCriteria criteria)
        {
            CustomerInfo customer = null;
            using (var objectDB = new ObjectDb(criteria.SettingKey()))
            {
                objectDB.AssignParamValues(criteria.SPParams());
                using (var reader = objectDB.ExecuteReader())
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            customer = MappingDataFromSqlToCustomerInfoObject(reader);
                            customer.HeadLine = Converter.ToBool(reader["IsGroupHeadLine"]);
                            customer.BankInfo.Address = Converter.ToString2(reader["Wire_BankAddress"]);

                            customer.IsActive = Converter.ToInt(reader["Active"]) == 1;
                            customer.IsClosed = Converter.ToInt(reader["Closed"]) == 1;
                            customer.IsSuspended = Converter.ToInt(reader["Suspended"]) == 1;
                            customer.OriginalDangerLevel = Converter.ToInt(reader["danger"]);
                            customer.ABTDangerLevel = Converter.ToInt(reader["danger2"]);
                            customer.OtherSportsDangerLevel = Converter.ToInt(reader["danger3"]);
                        }

                        if (reader.NextResult())
                        {
                            while (reader.Read())
                            {
                                MappingDataFromSqlToLanguageObject(reader, ref customer);
                            }
                        }
                    }
                }
            }

            return customer;
        }

        /// <summary>
        /// Gets the up line information of user.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The up line information of user.</returns>
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
                                Id = Converter.ToInt(reader["CustId"]),
                                Name = Converter.ToString2(reader["UserName"]),
                                Role = new BaseModel
                                {
                                    Id = Converter.ToInt(reader["roleid"])
                                },
                                IsActive = Converter.ToInt(reader["Active"]) == 1,
                                IsClosed = Converter.ToInt(reader["Closed"]) == 1,
                                IsSuspended = Converter.ToInt(reader["Suspended"]) == 1
                            };

                            uplineInfo.Add(customer);
                        }
                    }
                }
            }

            return uplineInfo;
        }

        /// <summary>
        /// Updates the customer information.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Update result.</returns>
        public BaseModel UpdateCustomerInfo(UpdateCustomerInfoCriteria criteria)
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
        /// Verifies the username.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Verified customer information.</returns>
        public List<CustomerInfo> VerifyUsername(VerifyRelatedCustomerCriteria criteria)
        {
            var result = new List<CustomerInfo>();
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                objectDB.AssignParamValues(criteria.GetSpParams());
                using (var reader = objectDB.ExecuteReader())
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            var releatedCustomer = new CustomerInfo()
                            {
                                Id = Converter.ToInt(reader["custid"]),
                                Name = Converter.ToString2(reader["val"])
                            };

                            result.Add(releatedCustomer);
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Queries the customer by username.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>
        /// The list of Customer.
        /// </returns>
        public IList<string> QueryCustomerByUsername(CustomerSuggestionCriteria criteria)
        {
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objectDB.ExecuteReader(criteria.GetSpParams()))
                {
                    IList<string> suggestions = new List<string>();
                    while (reader.Read())
                    {
                        suggestions.Add(Converter.ToString2(reader["UserName"]));
                    }

                    return suggestions;
                }
            }
        }

        /// <summary>
        /// Suggests related customer.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Related customers.</returns>
        public List<RelatedCustomerSuggestion> RelatedCustomerAutoSuggestion(RelatedCustomerAutoSuggestionCriteria criteria)
        {
            List<RelatedCustomerSuggestion> result = new List<RelatedCustomerSuggestion>();
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                objectDB.AssignParamValues(criteria.GetSpParams());
                using (var reader = objectDB.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new RelatedCustomerSuggestion
                        {
                            Key = Converter.ToString2(reader["UserName"]),
                            Value = Converter.ToString2(reader["UserName"])
                        });
                    }
                }
            }

            return result;
        }

        public IEnumerable<string> CustomerAutoSuggestion(CustomerAutoSuggestionCriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                return db.Query<string>(criteria.GetSpParams());
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

        /// <summary>
        /// Mappings the languages when get customer information by username.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="customer">The customer.</param>
        private void MappingDataFromSqlToLanguageObject(IDataReader reader, ref CustomerInfo customer)
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