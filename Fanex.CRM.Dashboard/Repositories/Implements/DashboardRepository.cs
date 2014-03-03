// --------------------------------------------------------------------------
// <copyright file="DashboardRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Dashboard.Repositories.Implements
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Common;
    using Fanex.CRM.Dashboard.Criteria;
    using Fanex.CRM.Dashboard.Models;
    using Fanex.CRM.Dashboard.Repositories.Interfaces;
    using Fanex.CRM.Infrastructure.Core;
    using Fanex.CRM.Infrastructure.Extension;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Setting.Models;
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
                Email = reader.GetStringNotNull("email"),
                Passport = reader.GetStringNotNull("Passport"),
                SecretCode = reader.GetStringNotNull("SecretCode"),
                BirthDay = reader["BirthDay"] != DBNull.Value ?
                    reader.GetDateTimeNotNull("BirthDay").ToShortFormatedDate() :
                    string.Empty,

                IsVerifyEmail = reader.GetBooleanNotNull("IsVerifyEmail"),
                IsVerifyPassport = reader.GetBooleanNotNull("IsVerifyPassport"),
                IsVerifyBirthday = reader.GetBooleanNotNull("IsVerifyBirthday"),
                IsVerifySecretCode = reader.GetBooleanNotNull("IsVerifySecretCode"),
                IsVerifyBetCredit = reader.GetBooleanNotNull("IsVerifyBetCredit"),
                IsVerifyGivenCredit = reader.GetBooleanNotNull("IsVerifyGivenCredit"),

                RoleId = reader.GetInt32NotNull("RoleId"),
                IsCredit = reader.GetBooleanNotNull("IsCredit"),
                BetCredit = reader.GetDecimalNotNull("BetCredit"),
                GivenCredit = reader.GetDecimalNotNull("GivenCredit")
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
            string remark = reader.GetStringNotNull("Remark");
            return remark;
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
                Id = reader.GetInt32NotNull("RedemptionId"),
                ClaimedTime = DateTimeUtilize.ChangeTimeZoneToClientDateTime(reader.GetDateTimeNotNull("ClaimTime")).ToShortFormatedDate(),
                Subject = reader.GetStringNotNull("Subject"),
                Status = new SettingInfo
                {
                    Value = reader.GetStringNotNull("Status")
                }
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
            var durationTime = reader.GetStringNotNull("Duration");
            var durationTimeArray = durationTime.Split(':');
            var communicationLog = new CommunicationLogInfo
            {
                RunningNumber = reader.GetInt32NotNull("FeedbackID"),
                FeedbackDate = DateTimeUtilize.ChangeTimeZoneToClientDateTime(reader.GetDateTimeNotNull("FeedbackDate")).ToLongFormatedDateTime(),
                MainCategory = new SettingInfo
                {
                    Name = reader.GetStringNotNull("MainCategory")
                },
                SubCategory = new SettingInfo
                {
                    Name = reader.GetStringNotNull("SubCategory")
                },
                Subject = reader.GetStringNotNull("Subject"),
                StatusName = reader.GetStringNotNull("Status"),
                Duration = new TimeInfo
                {
                    Hours = Converter.ToInt(durationTimeArray[0]),
                    Minutes = Converter.ToInt(durationTimeArray[1]),
                    Seconds = Converter.ToInt(durationTimeArray[2])
                },
                Handler = reader.GetStringNotNull("HandleBy"),
                Reviewer = reader.GetStringNotNull("ReviewBy")
            };

            return communicationLog;
        }
    }
}
