namespace Fanex.CRM.DataAccess.Impl
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.CRM.DataAccess.Interface;
    using Fanex.CRM.Entities;
    using Fanex.Data;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using Fanex.CRM.Infrastructure.Configuration;
    using MongoDB.Driver.Builders;

    public class CustomerDataAccess : ICustomerDataAccess
    {
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
                                                   CustId = reader.GetInt32(reader.GetOrdinal("custid")),
                                                   UserName = reader.GetString(reader.GetOrdinal("val"))
                                               };
                            result.Add(releatedCustomer);
                        }
                    }
                }
            }
            return result;
        }

        public Customer FindBy(int custId)
        {
            var customer = new Customer();
            
            using (var obj = new ObjectDb("CustomerSel"))
            {
                obj.AddInParameter("custid", DbType.Int32, custId);
                using (var reader = obj.ExecuteReader())
                {
                    if (reader != null)
                    {
                        #region assign data
                        while (reader.Read())
                        {
                            #region customer info
                            customer.CustomerInfo = new CustomerInfo
                            {
                                Title =
                                    new Setting
                                    {
                                        Id =
                                            reader["TitleID"] != DBNull.Value
                                                ? reader.GetInt32(reader["TitleID"] != null ? reader.GetOrdinal("TitleID") : 0)
                                                : 0
                                    },
                                FirstName =
                                    reader["FirstName"] != DBNull.Value
                                        ? (reader["FirstName"] != null ? reader.GetString(reader.GetOrdinal("FirstName")) : string.Empty)
                                        : string.Empty,
                                LastName =
                                    reader["LastName"] != DBNull.Value
                                        ? (reader["LastName"] != null ? reader.GetString(reader.GetOrdinal("LastName")) : string.Empty)
                                        : string.Empty,
                                Email =
                                    reader["Email"] != DBNull.Value
                                        ? (reader["Email"] != null ? reader.GetString(reader.GetOrdinal("Email")) : string.Empty)
                                        : string.Empty,
                                Fax =
                                    reader["Fax"] != DBNull.Value
                                        ? (reader["Fax"] != null ? reader.GetString(reader.GetOrdinal("Fax")) : string.Empty)
                                        : string.Empty,
                                MobilePhone =
                                    reader["MobilePhone"] != DBNull.Value
                                        ? (reader["MobilePhone"] != null ? reader.GetString(reader.GetOrdinal("MobilePhone")) : string.Empty)
                                        : string.Empty,
                                Phone =
                                    reader["Phone"] != DBNull.Value
                                        ? (reader["Phone"] != null ? reader.GetString(reader.GetOrdinal("Phone")) : string.Empty)
                                        : string.Empty,
                                BirthDay = reader["BirthDay"] != DBNull.Value ?
                                    (reader["BirthDay"] != null ? reader.GetDateTime(reader.GetOrdinal("BirthDay")) : DateTime.Now)
                                    : DateTime.Now,
                                Skype = reader["Skype"] != DBNull.Value
                                    ? (reader["Skype"] != null ? reader.GetString(reader.GetOrdinal("Skype")) : string.Empty)
                                    : string.Empty,
                                MSN = reader["msn"] != DBNull.Value
                                        ? (reader["msn"] != null ? reader.GetString(reader.GetOrdinal("msn")) : string.Empty)
                                        : string.Empty
                            };
                            #endregion

                            #region custid & name
                            customer.Id = reader["CustId"] != DBNull.Value ? reader.GetInt32(reader.GetOrdinal("CustId")) : 0;
                            customer.UserName = reader["UserName"] != DBNull.Value
                                                    ? reader.GetString(reader.GetOrdinal("UserName"))
                                                    : string.Empty;
                            customer.RoleId = reader["roleid"] != DBNull.Value ? reader.GetInt32(reader.GetOrdinal("roleid")) : 0;
                            customer.NickName = custId > 0 ? (reader["UserName2"] != DBNull.Value
                                                    ? reader.GetString(reader.GetOrdinal("UserName2"))
                                                    : string.Empty) : string.Empty;
                            #endregion

                            #region PIC
                            customer.Pic1 = reader["PIC1"] != DBNull.Value ? (reader["PIC1"] != null ? reader.GetString(reader.GetOrdinal("PIC1")) : string.Empty) : string.Empty;
                            customer.Pic2 = reader["PIC2"] != DBNull.Value ? (reader["PIC2"] != null ? reader.GetString(reader.GetOrdinal("PIC2")) : string.Empty) : string.Empty;
                            customer.Pic3 = reader["PIC3"] != DBNull.Value ? (reader["PIC3"] != null ? reader.GetString(reader.GetOrdinal("PIC3")) : string.Empty) : string.Empty;
                            #endregion
                            //-----remeber check it
                            #region related customer
                            #endregion

                            #region Site info
                            customer.Site = new Setting
                            {
                                Id =
                                    reader["SiteID"] != DBNull.Value
                                        ? reader.GetInt32(reader["SiteID"] != null ? reader.GetOrdinal("SiteID") : 0)
                                        : 0
                            };
                            #endregion

                            #region bank info
                            customer.BankInfo = new BankInfo
                            {
                                BankName =
                                    reader["Wire_BankName"] != DBNull.Value
                                        ? (reader["Wire_BankName"] != null ? reader.GetString(reader.GetOrdinal("Wire_BankName")) : string.Empty)
                                        : string.Empty,
                                BankAddress =
                                    reader["Wire_BankAddress"] != DBNull.Value
                                        ? (reader["Wire_BankAddress"] != null ? reader.GetString(reader.GetOrdinal("Wire_BankAddress")) : string.Empty)
                                        : string.Empty,
                                SwiftCode =
                                    reader["Wire_SwiftCode"] != DBNull.Value
                                        ? (reader["Wire_SwiftCode"] != null ? reader.GetString(reader.GetOrdinal("Wire_SwiftCode")) : string.Empty)
                                        : string.Empty,
                                AccountName =
                                    reader["Wire_AccountName"] != DBNull.Value
                                        ? (reader["Wire_AccountName"] != null ? reader.GetString(reader.GetOrdinal("Wire_AccountName")) : string.Empty)
                                        : string.Empty,
                                AccountNo =
                                    reader["Wire_AccountNo"] != DBNull.Value
                                        ? (reader["Wire_AccountNo"] != null ? reader.GetString(reader.GetOrdinal("Wire_AccountNo")) : string.Empty)
                                        : string.Empty,
                                AccountHolderName =
                                    reader["AccountHolderName"] != DBNull.Value
                                        ? (reader["AccountHolderName"] != null ? reader.GetString(reader.GetOrdinal("AccountHolderName")) : string.Empty)
                                        : string.Empty
                            };
                            #endregion

                            #region creator info
                            customer.Creator = new User
                            {
                                Id =
                                    reader["CreatorID"] != DBNull.Value
                                        ? reader.GetInt32(reader["CreatorID"] != null ? reader.GetOrdinal("CreatorID") : 0)
                                        : 0,
                                Username =
                                    reader["CreatorName"] != DBNull.Value
                                        ? (reader["CreatorName"] != null ? reader.GetString(reader.GetOrdinal("CreatorName")) : string.Empty)
                                        : string.Empty
                            };
                            #endregion

                            #region country info
                            customer.Country = reader["CountryID"] != DBNull.Value
                                ? reader.GetInt32(reader["CountryID"] != null ? reader.GetOrdinal("CountryID") : 0)
                                                    : 0;
                            #endregion

                            #region Question & Answer
                            customer.Question = reader["Question"] != DBNull.Value
                                ? (reader["Question"] != null ? reader.GetString(reader.GetOrdinal("Question")) : string.Empty)
                                                    : string.Empty;
                            customer.Answer = reader["Answer"] != DBNull.Value
                                ? (reader["Answer"] != null ? reader.GetString(reader.GetOrdinal("Answer")) : string.Empty)
                                                  : string.Empty;
                            #endregion

                            #region others
                            customer.IsVip = reader["IsVIP"] != DBNull.Value ?
                                (reader["IsVIP"] != null ? reader.GetBoolean(reader.GetOrdinal("IsVIP")) : false)
                                    : false;
                            customer.CreatedDate = reader["CreateDate"] != DBNull.Value
                                ? (reader["CreateDate"] != null ? reader.GetDateTime(reader.GetOrdinal("CreateDate")) : DateTime.Now)
                                                       : DateTime.Now;

                            customer.BettingCode = reader["RefCode1"] != DBNull.Value
                                ? (reader["RefCode1"] != null ? reader.GetString(reader.GetOrdinal("RefCode1")) : string.Empty)
                                : string.Empty;

                            customer.RefCode2 = reader["RefCode2"] != DBNull.Value
                                ? (reader["RefCode2"] != null ? reader.GetString(reader.GetOrdinal("RefCode2")) : string.Empty)
                                                    : string.Empty;
                            customer.Remark = reader["Remark"] != DBNull.Value
                                ? (reader["Remark"] != null ? reader.GetString(reader.GetOrdinal("Remark")) : string.Empty)
                                                    : string.Empty;
                            customer.SpecialRemark = reader["Remark2"] != DBNull.Value
                                ? (reader["Remark2"] != null ? reader.GetString(reader.GetOrdinal("Remark2")) : string.Empty)
                                                    : string.Empty;

                            customer.Language = reader["LanguageID"] != DBNull.Value
                                ? reader.GetInt32(reader["LanguageID"] != null ? reader.GetOrdinal("LanguageID") : 0)
                                                    : 0;
                            #endregion
                        }
                        #endregion
                    }
                }
            }

            return customer;
        }

        public Customer FindBy(string userName, bool isGuest)
        {
            Customer customer = null;
            
            using (var obj = new ObjectDb("CustomerSearch"))
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
                            #region assign data
                            customer = new Customer();
                            #region customer info
                            customer.CustomerInfo = new CustomerInfo
                            {
                                Title =
                                    new Setting
                                    {
                                        Id =
                                            reader["TitleID"] != DBNull.Value
                                                ? reader.GetInt32(reader.GetOrdinal("TitleID"))
                                                : 0,
                                        Value = ""
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
                                Fax =
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
                                        : string.Empty,
                                Skype = reader["Skype"] != DBNull.Value
                                    ? reader.GetString(reader.GetOrdinal("Skype"))
                                    : string.Empty,
                                MSN = reader["msn"] != DBNull.Value
                                    ? reader.GetString(reader.GetOrdinal("msn"))
                                    : string.Empty
                            };
                            #endregion
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

                            #region Site info
                            customer.Site = new Setting
                            {
                                Id =
                                    reader["SiteID"] != DBNull.Value
                                        ? reader.GetInt32(reader.GetOrdinal("SiteID"))
                                        : 0,
                                Value = ""
                            };

                            #endregion


                            #region bank info
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
                            #endregion

                            #region creator info
                            customer.Creator = new User
                            {

                                Username =
                                    reader["CreatorName"] != DBNull.Value
                                        ? reader.GetString(reader.GetOrdinal("CreatorName"))
                                        : string.Empty
                            };
                            #endregion

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
                        #endregion
                        }
                    }
                }
            }

            return customer;
            
        }

        public List<Customer> FindAllUpline(int custId)
        {
            var listCustomerUpline = new List<Customer>();

            using (var obj = new ObjectDb("CustomerGetUplineList"))
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

        public bool UpdateCustomerInfo(Customer customer, bool isUpdateUserName, string ip, ref Result result)
        {
            using (var obj = new ObjectDb("CustomerInfoUpd"))
            {
                #region add params
                var param = new Hashtable();
                param.Add("userid", customer.Creator.Id);
                param.Add("custid", customer.Id);
                param.Add("username", customer.UserName);
                param.Add("firstname", customer.CustomerInfo.FirstName);
                param.Add("lastname", customer.CustomerInfo.LastName);
                if (customer.CustomerInfo.Title.Id > 0)
                    param.Add("titleid", customer.CustomerInfo.Title.Id);
                if (customer.CustomerInfo.BirthDay != null)
                    param.Add("birthday", customer.CustomerInfo.BirthDay);
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
                    param.Add("relatedcustid", customer.RelatedCustomer.Id);
                if (customer.Site.Id > 0)
                    param.Add("siteid", customer.Site.Id);

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
                param.Add("isupdateusername", isUpdateUserName);

                param.Add("errcode", 4);
                param.Add("errmsg", 4000);
                #endregion
                obj.AssignParamValues(param);
                var isSuccess = obj.ExecuteNonQuery() > 0;
                result.ErrCode = Convert.ToInt32(obj.GetParameterValue("errcode"));
                result.ErrMsg = Convert.ToString(obj.GetParameterValue("errmsg"));
                return isSuccess;
            }
        }

        public DataSet QueryUserName(int userID, string userName, bool isGuest)
        {
            using (var obj = new ObjectDb("autoSearchSuggest"))
            {
                var param = new Hashtable();
                param.Add("userid", userID);
                param.Add("username", userName);
                param.Add("isguest", isGuest);
                obj.AssignParamValues(param);

                return obj.ExecuteDataSet();
            }
            
        }
        public bool UpdateCustomerSearch(int userID, int custID, string userName, ref Result result)
        {
            using (var obj = new ObjectDb("autoUpdateSearchSuggest"))
            {
                var param = new Hashtable();
                param.Add("userid", userID);
                param.Add("custid", custID);
                param.Add("username", userName);
                obj.AssignParamValues(param);

                var isSuccess = obj.ExecuteNonQuery() > 0;
                return isSuccess;
            }
        }


        public MongoCursor<BsonDocument> CustomerAutoSuggest(string customerName, bool isGuest)
        {
            var server = MongoServer.Create(WebConfigApplicationSettings.MongoDbServer);
            var crmDb = server.GetDatabase(WebConfigApplicationSettings.MongoDbName);

            IMongoQuery query = new QueryDocument();
            if (!string.IsNullOrEmpty(customerName))
                query = Query.And(
                    query,
                    Query.Matches("UserName", new BsonRegularExpression(string.Format("{0}", customerName.Replace(".", "2_2")))),
                    (!isGuest ? Query.GT("_id", BsonValue.Create(0)) : Query.LT("_id", BsonValue.Create(0))));

            if (string.IsNullOrEmpty(WebConfigApplicationSettings.MongoUserNameHintValue))
                return crmDb.GetCollection(WebConfigApplicationSettings.MongoCustomerTable).Find(query).SetSortOrder(SortBy.Descending("SearchCount")).SetSkip(0).SetLimit(10);
            else
                return crmDb.GetCollection(WebConfigApplicationSettings.MongoCustomerTable).Find(query).SetHint(WebConfigApplicationSettings.MongoUserNameHintValue).SetSortOrder(SortBy.Descending("SearchCount")).SetSkip(0).SetLimit(10);

        }

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

        public void AddNewCustomer(int customerId, string customerName)
        {
            var server = MongoServer.Create(WebConfigApplicationSettings.MongoDbServer);
            var crmDb = server.GetDatabase(WebConfigApplicationSettings.MongoDbName);

            var customers = crmDb.GetCollection<BsonDocument>(WebConfigApplicationSettings.MongoCustomerTable);
            var customer = new BsonDocument()
            {
                {"_id", customerId},
                {"UserName", customerName.ToLower()},
                {"SearchCount", 1}
            };
            customers.Insert(customer);
        }
    }
}