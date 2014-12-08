// --------------------------------------------------------------------------
// <copyright file="DashboardRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Dashboard.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Common;
    using Fanex.CRM.Common.Extensions;
    using Fanex.CRM.Common.Helpers;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Dashboard.Criteria;
    using Fanex.CRM.Dashboard.Models;
    using Fanex.CRM.Infrastructure.Extensions;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.Data;

    /// <summary>
    /// Dashboard Repository.
    /// </summary>
    public class DashboardRepository : IDashboardRepository
    {
        /// <summary>
        /// Gets the customer remarks.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The customer remarks.</returns>
        public List<string> GetCustomerRemarks(GetCustomerRemarksCriteria criteria)
        {
            List<string> remarks = new List<string>();
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                objectDB.AssignParamValues(criteria.GetSpParams());
                using (var reader = objectDB.ExecuteReader())
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            remarks.Add(MappingDataFromSqlToRemarkObject(reader));
                        }
                    }
                }
            }

            return remarks;
        }

        /// <summary>
        /// Gets the customer information.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The customer information.</returns>
        public CustomerInfo GetCustomerInfo(GetCustomerInfoCriteria criteria)
        {
            CustomerInfo customerInfo = new CustomerInfo();

            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                objectDB.AssignParamValues(criteria.GetSpParams());
                using (var reader = objectDB.ExecuteReader())
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            customerInfo = MappingDataFromSqlToCustomerInfoObject(reader);
                        }

                        customerInfo.Id = criteria.CustomerId;
                    }
                }
            }

            return customerInfo;
        }

        /// <summary>
        /// Gets the redemption logs.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The redemption logs.</returns>
        public List<RedemptionLogInfo> GetRedemptionLogs(GetRedemptionLogsCriteria criteria)
        {
            List<RedemptionLogInfo> redemptionLogs = new List<RedemptionLogInfo>();
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                objectDB.AssignParamValues(criteria.GetSpParams());
                using (var reader = objectDB.ExecuteReader())
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            redemptionLogs.Add(MappingDataFromSqlToRedemptionLogObject(reader));
                        }
                    }
                }
            }

            return redemptionLogs;
        }

        /// <summary>
        /// Gets the communication logs.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The communication logs.</returns>
        public List<CommunicationLogInfo> GetCommunicationLogs(GetCommunicationLogsCriteria criteria)
        {
            List<CommunicationLogInfo> communicationLogs = new List<CommunicationLogInfo>();
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                objectDB.AssignParamValues(criteria.GetSpParams());
                using (var reader = objectDB.ExecuteReader())
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            communicationLogs.Add(MappingDataFromSqlToCommunicationLogObject(reader));
                        }
                    }
                }
            }

            return communicationLogs;
        }

        /// <summary>
        /// Updates the customer verify flag.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Update result.</returns>
        public Result UpdateCustomerVerifyFlag(UpdateCustomerVerifyFlagCriteria criteria)
        {
            Result updateResult = null;
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                objectDB.AssignParamValues(criteria.GetSpParams());
                objectDB.ExecuteNonQuery();
                var outputParams = objectDB.GetOuputParams();
                updateResult = new Result
                {
                    Code = Converter.ToInt(outputParams["errcode"]),
                    Message = Converter.ToString2(outputParams["errmsg"])
                };
            }

            return updateResult;
        }

        /// <summary>
        /// Mappings the data from SQL to customer information object.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>Customer information object.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private CustomerInfo MappingDataFromSqlToCustomerInfoObject(IDataReader reader)
        {
            CustomerInfo customerInfo = new CustomerInfo
            {
                Email = Converter.ToString2(reader["email"]),
                Passport = Converter.ToString2(reader["Passport"]),
                SecretCode = Converter.ToString2(reader["SecretCode"]),
                BirthDay = reader["BirthDay"] != DBNull.Value ?
                    Converter.ToDateTime(reader["BirthDay"]).ToShortFormatedDate() :
                    string.Empty,

                IsVerifyEmail = Converter.ToBool(reader["IsVerifyEmail"]),
                IsVerifyPassport = Converter.ToBool(reader["IsVerifyPassport"]),
                IsVerifyBirthday = Converter.ToBool(reader["IsVerifyBirthday"]),
                IsVerifySecretCode = Converter.ToBool(reader["IsVerifySecretCode"]),
                IsVerifyBetCredit = Converter.ToBool(reader["IsVerifyBetCredit"]),
                IsVerifyGivenCredit = Converter.ToBool(reader["IsVerifyGivenCredit"]),

                RoleId = Converter.ToInt(reader["RoleId"]),
                IsCredit = Converter.ToBool(reader["IsCredit"]),
                BetCredit = Converter.ToDecimal(reader["BetCredit"]),
                GivenCredit = Converter.ToDecimal(reader["GivenCredit"])
            };

            return customerInfo;
        }

        /// <summary>
        /// Mappings the data from SQL to remark object.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>Remark object.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private string MappingDataFromSqlToRemarkObject(IDataReader reader)
        {
            return HtmlHelper.DecodeXml(Converter.ToString2(reader["Remark"])).HtmlEncode();
        }

        /// <summary>
        /// Mappings the data from SQL to redemption log object.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>Redemption log object.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private RedemptionLogInfo MappingDataFromSqlToRedemptionLogObject(IDataReader reader)
        {
            var redemptionLog = new RedemptionLogInfo
            {
                Id = Converter.ToInt(reader["RedemptionId"]),
                ClaimedTime = DateTimeUtilize.ChangeTimeZoneToClientDateTime(Converter.ToDateTime(reader["ClaimTime"])).ToShortFormatedDate(),
                Subject = Converter.ToString2(reader["Subject"]),
                Status = new SettingInfo
                {
                    Value = Converter.ToString2(reader["Status"])
                },
                Amount = Converter.ToDecimal(reader["Amount"]),
                DoneBy = Converter.ToString2(reader["DoneBy"]),
                Currency = new BaseModel { Name = Converter.ToString2(reader["Currency"]) }
            };

            return redemptionLog;
        }

        /// <summary>
        /// Mappings the data from SQL to communication log object.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>Communication log object.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private CommunicationLogInfo MappingDataFromSqlToCommunicationLogObject(IDataReader reader)
        {
            var durationTime = Converter.ToString2(reader["Duration"]);
            var durationTimeArray = durationTime.Split(':');
            var communicationLog = new CommunicationLogInfo
            {
                RunningNumber = Converter.ToInt(reader["FeedbackID"]),
                FeedbackDate = DateTimeUtilize.ChangeTimeZoneToClientDateTime(Converter.ToDateTime(reader["FeedbackDate"])).ToLongFormatedDateTime(),
                MainCategory = new SettingInfo { Name = Converter.ToString2(reader["MainCategory"]) },
                SubCategory = new SettingInfo { Name = Converter.ToString2(reader["SubCategory"]) },
                Subject = Converter.ToString2(reader["Subject"]),
                StatusName = Converter.ToString2(reader["Status"]),
                Duration = new TimeInfo
                {
                    Hours = Converter.ToInt(durationTimeArray[0]),
                    Minutes = Converter.ToInt(durationTimeArray[1]),
                    Seconds = Converter.ToInt(durationTimeArray[2])
                },
                Handler = Converter.ToString2(reader["HandleBy"]),
                Reviewer = Converter.ToString2(reader["ReviewBy"])
            };

            return communicationLog;
        }
    }
}
