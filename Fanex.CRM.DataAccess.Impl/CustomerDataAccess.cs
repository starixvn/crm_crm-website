// --------------------------------------------------------------------------
// <copyright file="CustomerDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.DataAccess.Impl
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Fanex.Common;
    using Fanex.CRM.DataAccess.Interface;
    using Fanex.CRM.Entities;
    using Fanex.CRM.Infrastructure.Configuration;
    using Fanex.Data;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using MongoDB.Driver.Builders;

    /// <summary>
    /// Customer Data Access.
    /// </summary>
    public class CustomerDataAccess : ICustomerDataAccess
    {
        /// <summary>
        /// Verifies the user name.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns>Verify result.</returns>
        public List<RelatedCustomer> VerifyUserName(string userName)
        {
            var result = new List<RelatedCustomer>();

            using (var obj = new ObjectDb("CustomerVerify"))
            {
                obj.AddInParameter("username", DbType.String, userName);
                using (var reader = obj.ExecuteReader())
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            var releatedCustomer = new RelatedCustomer()
                                               {
                                                   CustId = Fanex.Common.Converter.ToInt(reader["custid"]),
                                                   UserName = Fanex.Common.Converter.ToString2(reader["val"])
                                               };
                            result.Add(releatedCustomer);
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Finds the customer by identify.
        /// </summary>
        /// <param name="custId">The customer identifier.</param>
        /// <returns>The customer by identify.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public Customer FindBy(int custId)
        {
            var customer = new Customer();

            using (var obj = new ObjectDb("Customer_CustomerInfoDetail_GetInfoById_New"))
            {
                obj.AddInParameter("custid", DbType.Int32, custId);
                using (var reader = obj.ExecuteReader())
                {
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            var customerInfo = GetCustomerInformation(reader);
                            customer.CustomerInfo = customerInfo;

                            customer.Id = Fanex.Common.Converter.ToInt(reader.GetOrdinal("CustId"), 0);
                            customer.UserName = Fanex.Common.Converter.ToString2("UserName");
                            customer.RoleId = Converter.ToInt(reader["roleid"]);
                            customer.NickName = custId > 0 ? Fanex.Common.Converter.ToString2(reader.GetOrdinal("UserName2")) : string.Empty;

                            customer.Pic1 = customerInfo.PIC1;
                            customer.Pic2 = customerInfo.PIC2;
                            customer.Pic3 = customerInfo.PIC3;

                            customer.Site = new Setting
                            {
                                Id = customerInfo.Site
                            };

                            customer.BankInfo = GetCustomerBankInfo(reader);

                            customer.Creator = new User
                            {
                                Id =
                                    Fanex.Common.Converter.ToInt(reader.GetOrdinal("CreatorID"), 0),
                                Username =
                                    Fanex.Common.Converter.ToString2(reader.GetOrdinal("CreatorName"))
                            };

                            customer.Country = Fanex.Common.Converter.ToInt(reader.GetOrdinal("CountryID"), 0);

                            customer.Question = Fanex.Common.Converter.ToString2(reader.GetOrdinal("Question"));
                            customer.Answer = Fanex.Common.Converter.ToString2(reader.GetOrdinal("Answer"));

                            customer.IsVip = Fanex.Common.Converter.ToBool(reader.GetOrdinal("IsVIP"), false);
                            customer.CreatedDate = Fanex.Common.Converter.ToDateTime(reader.GetOrdinal("CreateDate"), DateTime.Now);

                            customer.BettingCode = Fanex.Common.Converter.ToString2(reader.GetOrdinal("RefCode1"));

                            customer.RefCode2 = Fanex.Common.Converter.ToString2(reader.GetOrdinal("RefCode2"));
                            //customer.Remark = reader["Remark"] != DBNull.Value
                            //    ? (reader["Remark"] != null ? reader.GetString(reader.GetOrdinal("Remark")) : string.Empty)
                            //                        : string.Empty;
                            //customer.SpecialRemark = reader["Remark2"] != DBNull.Value
                            //    ? (reader["Remark2"] != null ? reader.GetString(reader.GetOrdinal("Remark2")) : string.Empty)
                            //                        : string.Empty;

                            //customer.Language = reader["LanguageID"] != DBNull.Value
                            //    ? reader.GetInt32(reader["LanguageID"] != null ? reader.GetOrdinal("LanguageID") : 0)
                            //                        : 0;
                        }

                        /*if (reader.NextResult())
                        {
                            customer.CustomerInfo.Languages = GetCustomerLanguages(reader);
                        }*/
                    }
                }
            }

            return customer;
        }

        /// <summary>
        /// Finds the customer by username or guest indicator.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="isGuest">If set to <c>true</c> [is guest].</param>
        /// <returns>The customer by username or guest indicator.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public Customer FindBy(string userName, bool isGuest)
        {
            Customer customer = null;

            using (var obj = new ObjectDb("Customer_CustomerInfo_GetCustomerByUsername"))
            {
                var param = new Hashtable();
                param.Add("username", Uri.UnescapeDataString(userName));
                param.Add("isguest", isGuest);
                obj.AssignParamValues(param);
                using (var reader = obj.ExecuteReader())
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            customer = new Customer();
                            customer.CustomerInfo = new CustomerInfo
                            {
                                Title =
                                    new Setting
                                    {
                                        Id = reader["TitleID"] != DBNull.Value ?
                                            reader.GetInt32(reader.GetOrdinal("TitleID")) :
                                            0,
                                        Value = string.Empty
                                    },

                                FirstName =
                                    reader["FirstName"] != DBNull.Value
                                        ? reader.GetString(reader.GetOrdinal("FirstName"))
                                        : string.Empty,
                                LastName =
                                    reader["LastName"] != DBNull.Value
                                        ? reader.GetString(reader.GetOrdinal("LastName"))
                                        : string.Empty,
                                Email =
                                    reader["Email"] != DBNull.Value
                                        ? reader.GetString(reader.GetOrdinal("Email"))
                                        : string.Empty,
                                /*Fax =
                                    reader["Fax"] != DBNull.Value
                                        ? reader.GetString(reader.GetOrdinal("Fax"))
                                        : string.Empty,
                                MobilePhone =
                                    reader["MobilePhone"] != DBNull.Value
                                        ? reader.GetString(reader.GetOrdinal("MobilePhone"))
                                        : string.Empty,
                                Phone =
                                    reader["Phone"] != DBNull.Value
                                        ? reader.GetString(reader.GetOrdinal("Phone"))
                                        : string.Empty,*/
                                Skype = reader["Skype"] != DBNull.Value
                                    ? reader.GetString(reader.GetOrdinal("Skype"))
                                    : string.Empty,
                                MSN = reader["msn"] != DBNull.Value
                                    ? reader.GetString(reader.GetOrdinal("msn"))
                                    : string.Empty
                            };

                            customer.UserName = reader["UserName"] != DBNull.Value
                                                    ? reader.GetString(reader.GetOrdinal("UserName"))
                                                    : string.Empty;
                            customer.NickName = isGuest
                                                    ? string.Empty
                                                    : (reader["UserName2"] != DBNull.Value
                                                           ? reader.GetString(reader.GetOrdinal("UserName2"))
                                                           : string.Empty);

                            customer.Pic1 = reader["PIC1"] != DBNull.Value ? reader.GetString(reader.GetOrdinal("PIC1")) : string.Empty;
                            customer.Pic2 = reader["PIC2"] != DBNull.Value ? reader.GetString(reader.GetOrdinal("PIC2")) : string.Empty;
                            customer.Pic3 = reader["PIC3"] != DBNull.Value ? reader.GetString(reader.GetOrdinal("PIC3")) : string.Empty;
                            customer.Id = reader["CustId"] != DBNull.Value ? reader.GetInt32(reader.GetOrdinal("CustId")) : 0;
                            customer.RoleId = reader["RoleID"] != DBNull.Value
                                                  ? reader.GetInt32(reader.GetOrdinal("RoleID"))
                                                  : 0;
                            customer.RoleName = customer.RoleName;
                            customer.CountryName = reader["CountryName"] != DBNull.Value ? reader.GetString(reader.GetOrdinal("CountryName")) : string.Empty;

                            customer.Site = new Setting
                            {
                                Id =
                                    reader["SiteID"] != DBNull.Value
                                        ? reader.GetInt32(reader.GetOrdinal("SiteID"))
                                        : 0,
                                Value = string.Empty
                            };

                            customer.BankInfo = new BankInfo
                            {
                                BankName =
                                    reader["Wire_BankName"] != DBNull.Value
                                        ? reader.GetString(reader.GetOrdinal("Wire_BankName"))
                                        : string.Empty,
                                BankAddress =
                                    reader["Wire_BankAddress"] != DBNull.Value
                                        ? reader.GetString(reader.GetOrdinal("Wire_BankAddress"))
                                        : string.Empty,
                                SwiftCode =
                                    reader["Wire_SwiftCode"] != DBNull.Value
                                        ? reader.GetString(reader.GetOrdinal("Wire_SwiftCode"))
                                        : string.Empty,
                                AccountName =
                                    reader["Wire_AccountName"] != DBNull.Value
                                        ? reader.GetString(reader.GetOrdinal("Wire_AccountName"))
                                        : string.Empty,
                                AccountNo =
                                    reader["Wire_AccountNo"] != DBNull.Value
                                        ? reader.GetString(reader.GetOrdinal("Wire_AccountNo"))
                                        : string.Empty
                            };

                            customer.Creator = new User
                            {
                                Username =
                                    reader["CreatorName"] != DBNull.Value
                                        ? reader.GetString(reader.GetOrdinal("CreatorName"))
                                        : string.Empty
                            };

                            customer.Question = reader["Question"] != DBNull.Value
                                                    ? reader.GetString(reader.GetOrdinal("Question"))
                                                    : string.Empty;
                            customer.Answer = reader["Answer"] != DBNull.Value
                                                  ? reader.GetString(reader.GetOrdinal("Answer"))
                                                  : string.Empty;
                            customer.IsVip = reader["IsVIP"] != DBNull.Value ? reader.GetBoolean(reader.GetOrdinal("IsVIP")) : false;
                            customer.CreatedDate = reader["CreateDate"] != DBNull.Value
                                                       ? reader.GetDateTime(reader.GetOrdinal("CreateDate"))
                                                       : DateTime.Now;
                            customer.BettingCode = reader["RefCode1"] != DBNull.Value
                                                    ? reader.GetString(reader.GetOrdinal("RefCode1"))
                                                    : string.Empty;

                            customer.RefCode2 = reader["RefCode2"] != DBNull.Value
                                                    ? reader.GetString(reader.GetOrdinal("RefCode2"))
                                                    : string.Empty;
                        }
                    }
                }
            }

            return customer;
        }

        /// <summary>
        /// Finds all up line.
        /// </summary>
        /// <param name="custId">The customer identifier.</param>
        /// <returns>All up line.</returns>
        public List<Customer> FindAllUpline(int custId)
        {
            var listCustomerUpline = new List<Customer>();

            using (var obj = new ObjectDb("Customer_CustomerInfo_GetUplineList"))
            {
                obj.AddInParameter("custid", DbType.Int32, custId);
                using (var reader = obj.ExecuteReader())
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            var customer = new Customer
                            {
                                UserName = reader["UserName"] != DBNull.Value
                                               ? reader.GetString(reader.GetOrdinal("UserName"))
                                               : string.Empty,
                                Id = reader["CustId"] != DBNull.Value
                                         ? reader.GetInt32(reader.GetOrdinal("CustId"))
                                         : 0,
                                RoleId = reader["roleid"] != DBNull.Value ? reader.GetByte(reader.GetOrdinal("roleid")) : 0,
                            };
                            customer.RoleName = customer.RoleName;
                            listCustomerUpline.Add(customer);
                        }
                    }
                }
            }

            return listCustomerUpline;
        }

        /// <summary>
        /// Updates the customer information.
        /// </summary>
        /// <param name="customer">The customer.</param>
        /// <param name="isUpdateUserName">If set to <c>true</c> [is update user name].</param>
        /// <param name="ip">The IP address.</param>
        /// <param name="result">The result.</param>
        /// <returns>Update result.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public bool UpdateCustomerInfo(Customer customer, bool isUpdateUserName, string ip, ref Result result)
        {
            using (var obj = new ObjectDb("Customer_CustomerInfoDetail_Update"))
            {
                var param = new Hashtable();
                /*param.Add("userid", customer.Creator.Id);
                param.Add("custid", customer.Id);
                param.Add("username", customer.UserName);
                param.Add("firstname", customer.CustomerInfo.FirstName);
                param.Add("lastname", customer.CustomerInfo.LastName);
                if (customer.CustomerInfo.Title.Id > 0)
                {
                    param.Add("titleid", customer.CustomerInfo.Title.Id);
                }

                if (customer.CustomerInfo.BirthDay != null)
                {
                    param.Add("birthday", customer.CustomerInfo.BirthDay);
                }

                param.Add("languageid", customer.Language);
                param.Add("phone", customer.CustomerInfo.Phone);
                param.Add("mobilephone", customer.CustomerInfo.MobilePhone);
                param.Add("fax", customer.CustomerInfo.Fax);
                param.Add("email", customer.CustomerInfo.Email);
                param.Add("skype", customer.CustomerInfo.Skype);
                param.Add("msn", customer.CustomerInfo.MSN);
                param.Add("bettingcode", customer.BettingCode);
                param.Add("refcode2", customer.RefCode2);
                param.Add("pic1", customer.Pic1);
                param.Add("pic2", customer.Pic2);
                param.Add("pic3", customer.Pic3);
                param.Add("question", customer.Question);
                param.Add("answer", customer.Answer);
                param.Add("countryid", customer.Country);
                param.Add("isvip", customer.IsVip);
                if (customer.RelatedCustomer.Id > 0)
                {
                    param.Add("relatedcustid", customer.RelatedCustomer.Id);
                }

                if (customer.Site.Id > 0)
                {
                    param.Add("siteid", customer.Site.Id);
                }

                param.Add("wire_bankname", customer.BankInfo.BankName);
                param.Add("wire_bankaddress", customer.BankInfo.BankAddress);
                param.Add("wire_accountname", customer.BankInfo.AccountName);
                param.Add("wire_accountno", customer.BankInfo.AccountNo);
                param.Add("wire_swiftcode", customer.BankInfo.SwiftCode);
                param.Add("remark", customer.Remark);
                param.Add("creatorid", customer.Creator.Id);
                param.Add("ip", ip);

                param.Add("accountholdername", customer.BankInfo.AccountHolderName);
                param.Add("specialremark", customer.SpecialRemark);
                param.Add("isupdateusername", isUpdateUserName);*/

                param.Add("errcode", 0);
                param.Add("errmsg", string.Empty);

                obj.AssignParamValues(param);
                var isSuccess = obj.ExecuteNonQuery() > 0;
                result.ErrCode = Convert.ToInt32(obj.GetParameterValue("errcode"));
                result.ErrMsg = Convert.ToString(obj.GetParameterValue("errmsg"));
                return isSuccess;
            }
        }

        /// <summary>
        /// Queries the user name.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="username">Name of the user.</param>
        /// <param name="isGuest">If set to <c>true</c> [is guest].</param>
        /// <returns>The user name.</returns>
        public DataSet QueryUserName(int userId, string username, bool isGuest)
        {
            using (var obj = new ObjectDb("autoSearchSuggest"))
            {
                var param = new Hashtable();
                param.Add("userid", userId);
                param.Add("username", username);
                param.Add("isguest", isGuest);
                obj.AssignParamValues(param);

                return obj.ExecuteDataSet();
            }
        }

        /// <summary>
        /// Updates the customer search.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="custId">The customer identifier.</param>
        /// <param name="username">The username.</param>
        /// <param name="result">The result.</param>
        /// <returns>Update result.</returns>
        public bool UpdateCustomerSearch(int userId, int custId, string username, ref Result result)
        {
            using (var obj = new ObjectDb("Customer_CustomerInfo_UpdateCustomerSuggestion"))
            {
                var param = new Hashtable();
                param.Add("userid", userId);
                param.Add("custid", custId);
                param.Add("username", username);
                obj.AssignParamValues(param);

                var isSuccess = obj.ExecuteNonQuery() > 0;
                return isSuccess;
            }
        }

        /// <summary>
        /// Automatic suggest customer information.
        /// </summary>
        /// <param name="customerName">Name of the customer.</param>
        /// <param name="isGuest">If set to <c>true</c> [is guest].</param>
        /// <returns>Suggest result.</returns>
        public MongoCursor<BsonDocument> CustomerAutoSuggest(string customerName, bool isGuest)
        {
            var server = MongoServer.Create(WebConfigApplicationSettings.MongoDbServer);
            var crmDb = server.GetDatabase(WebConfigApplicationSettings.MongoDbName);

            IMongoQuery query = new QueryDocument();
            if (!string.IsNullOrEmpty(customerName))
            {
                query = Query.And(
                    query,
                    Query.Matches("UserName", new BsonRegularExpression(string.Format("{0}", customerName.Replace(".", "2_2")))),
                    !isGuest ? Query.GT("_id", BsonValue.Create(0)) : Query.LT("_id", BsonValue.Create(0)));
            }

            if (string.IsNullOrEmpty(WebConfigApplicationSettings.MongoUserNameHintValue))
            {
                return crmDb.GetCollection(WebConfigApplicationSettings.MongoCustomerTable).Find(query).SetSortOrder(SortBy.Descending("SearchCount")).SetSkip(0).SetLimit(10);
            }
            else
            {
                return crmDb.GetCollection(WebConfigApplicationSettings.MongoCustomerTable)
                    .Find(query)
                    .SetHint(WebConfigApplicationSettings.MongoUserNameHintValue)
                    .SetSortOrder(SortBy.Descending("SearchCount"))
                    .SetSkip(0)
                    .SetLimit(10);
            }
        }

        /// <summary>
        /// Updates the customer search count value.
        /// </summary>
        /// <param name="custId">The customer identifier.</param>
        /// <param name="customerName">Name of the customer.</param>
        /// <returns>Update result.</returns>
        public bool UpdateCustomerSearchCountValue(int custId, string customerName)
        {
            var server = MongoServer.Create(WebConfigApplicationSettings.MongoDbServer);
            var crmDb = server.GetDatabase(WebConfigApplicationSettings.MongoDbName);

            var searchCount = 0;

            IMongoQuery query = new QueryDocument();
            query = Query.And(Query.EQ("_id", custId));
            var customers = crmDb.GetCollection<BsonDocument>(WebConfigApplicationSettings.MongoCustomerTable);
            var custInfo = customers.FindOne(query);
            if (customers.FindOne(query) != null)
            {
                searchCount = custInfo["SearchCount"].AsInt32 + 1;
                var update = Update.Set("SearchCount", searchCount);
                customers.Update(query, update);

                return true;
            }
            else
            {
                AddNewCustomer(custId, customerName);
            }

            return false;
        }

        /// <summary>
        /// Adds the new customer.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <param name="customerName">Name of the customer.</param>
        public void AddNewCustomer(int customerId, string customerName)
        {
            var server = MongoServer.Create(WebConfigApplicationSettings.MongoDbServer);
            var crmDb = server.GetDatabase(WebConfigApplicationSettings.MongoDbName);

            var customers = crmDb.GetCollection<BsonDocument>(WebConfigApplicationSettings.MongoCustomerTable);
            var customer = new BsonDocument()
            {
                { "_id", customerId },
                { "UserName", customerName.ToLower() },
                { "SearchCount", 1 }
            };
            customers.Insert(customer);
        }

        /// <summary>
        /// Gets the customer information.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>The customer info object.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        private CustomerInfo GetCustomerInformation(IDataReader reader)
        {
            return new CustomerInfo
            {
                Title = new Setting
                {
                    Id = Converter.ToInt(reader["TitleID"])
                },
                FirstName = Converter.ToString2(reader["FirstName"]),
                LastName = Converter.ToString2(reader["LastName"]),
                ContactNumber1 = new PhoneInfo
                {
                    PhoneNumber = Converter.ToString2(reader["Phone"]),
                    CountryCode = Converter.ToString2(reader["PhoneCountryCode"]),
                    AreaCode = Converter.ToString2(reader["PhoneAreaCode"])
                },
                ContactNumber2 = new PhoneInfo
                {
                    PhoneNumber = Converter.ToString2(reader["Phone2"]),
                    CountryCode = Converter.ToString2(reader["Phone2CountryCode"]),
                    AreaCode = Converter.ToString2(reader["Phone2AreaCode"])
                },
                ContactNumber3 = new PhoneInfo
                {
                    PhoneNumber = Converter.ToString2(reader["Phone3"]),
                    CountryCode = Converter.ToString2(reader["Phone3CountryCode"]),
                    AreaCode = Converter.ToString2(reader["Phone3AreaCode"])
                },
                Fax = new FaxInfo
                {
                    FaxNumber = Converter.ToString2(reader["Fax"]),
                    CountryCode = Converter.ToString2(reader["FaxCountryCode"]),
                    AreaCode = Converter.ToString2(reader["FaxAreaCode"])
                },
                Mobile = new PhoneInfo
                {
                    PhoneNumber = Converter.ToString2(reader["MobilePhone"]),
                    CountryCode = Converter.ToString2(reader["MobilePhoneCountryCode"]),
                    AreaCode = Converter.ToString2(reader["MobilePhoneAreaCode"])
                },
                Skype = Converter.ToString2(reader["Skype"]),
                Email = Converter.ToString2(reader["Email"]),
                MSN = Converter.ToString2(reader["MSN"]),
                PassportNumber = Converter.ToString2(reader["Passport"]),
                BirthDay = Converter.ToDateTime(reader["Birthday"]),
                Site = Converter.ToInt(reader["SiteId"]),
                BettingCode = Converter.ToString2(reader["RefCode1"]),
                RefCodes2 = Converter.ToString2(reader["RefCode2"]),
                IsVIP = Converter.ToBool(reader["IsVIP"]),
                PIC1 = Converter.ToString2(reader["PIC1"]),
                PIC2 = Converter.ToString2(reader["PIC2"]),
                PIC3 = Converter.ToString2(reader["PIC3"]),
                PIC4 = Converter.ToString2(reader["PIC4"])
            };
        }

        /// <summary>
        /// Gets the customer bank information.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>The customer bank information.</returns>
        private BankInfo GetCustomerBankInfo(IDataReader reader)
        {
            /*return new BankInfo
            {
                Name = Converter.ToString2(reader["Wire_BankName"]),
                SwiftCode = Converter.ToString2(reader["Wire_SwiftCode"),
                AccountName = Converter.ToString2(reader["Wire_AccountName"),
                AccountNumber = Converter.ToString2(reader.GetStringNotNull("Wire_AccountNo"),
                HolderName = Converter.ToString2(reader.GetStringNotNull("AccountHolderName"),
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
            };*/
            return null;
        }

        /// <summary>
        /// Gets the customer language.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>The language list.</returns>
        private IList<int> GetCustomerLanguages(IDataReader reader)
        {
            IList<int> languages = new List<int>();
            while (reader.Read())
            {
                languages.Add(Converter.ToInt(reader["LanguageId"]));
            }

            return languages;
        }
    }
}