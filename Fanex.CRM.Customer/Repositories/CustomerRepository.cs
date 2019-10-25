namespace Fanex.CRM.Customer.Repositories
{
    using System.Collections.Generic;
    using Fanex.Common;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Customer.Criteria;
    using Fanex.CRM.Customer.Models;
    using Fanex.Data;

    public class CustomerRepository : BaseCustomerRepository, ICustomerRepository
    {
        public CustomerInfo GetCustomerInfoByUsername(GetCustomerInfoByUsernameCriteria criteria)
        {
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objectDB.ExecuteReader(criteria.GetSpParams()))
                {
                    CustomerInfo customer = null;
                    if (reader.Read())
                    {
                        customer = ReadCustomerBasicInformation(reader);
                        customer.HeadLine = Converter.ToBool(reader["IsGroupHeadLine"]);
                        customer.BankInfo.Address = Converter.ToString2(reader["Wire_BankAddress"]);

                        customer.CustStatus = Converter.ToString2(reader["CustStatus"]);
                        customer.Locked = Converter.ToInt(reader["Locked"]) == 1;
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