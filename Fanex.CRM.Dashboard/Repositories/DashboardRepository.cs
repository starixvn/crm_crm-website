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
    using Fanex.CRM.Common.Helpers;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Common.Repositories;
    using Fanex.CRM.Dashboard.Criteria;
    using Fanex.CRM.Dashboard.Models;
    using Fanex.CRM.Infrastructure.Extensions;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.Data;

    /// <summary>
    /// Dashboard Repository.
    /// </summary>
    public class DashboardRepository : BaseRepository, IDashboardRepository
    {
        public IEnumerable<string> GetCustomerRemarks(GetCustomerRemarksCriteria criteria)
        {
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                return objectDB.Query<string>(criteria.GetSpParams());
            }
        }

        public CustomerInfo GetCustomerInfo(GetCustomerInfoCriteria criteria)
        {
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objectDB.ExecuteReader(criteria.GetSpParams()))
                {
                    CustomerInfo customerInfo = reader.Read() ?
                        ReadCustomerInfo(reader) :
                        new CustomerInfo();

                    customerInfo.Id = criteria.CustomerId;
                    return customerInfo;
                }
            }
        }

        public IEnumerable<RedemptionLogInfo> GetRedemptionLogs(GetRedemptionLogsCriteria criteria)
        {
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objectDB.ExecuteReader(criteria.GetSpParams()))
                {
                    var redemptionLogs = new List<RedemptionLogInfo>();
                    while (reader.Read())
                    {
                        redemptionLogs.Add(ReadRedemptionLog(reader));
                    }

                    return redemptionLogs;
                }
            }
        }

        public IEnumerable<CommunicationLogInfo> GetCommunicationLogs(GetCommunicationLogsCriteria criteria)
        {
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objectDB.ExecuteReader(criteria.GetSpParams()))
                {
                    var communicationLogs = new List<CommunicationLogInfo>();
                    while (reader.Read())
                    {
                        communicationLogs.Add(ReadCommunicationLog(reader));
                    }

                    return communicationLogs;
                }
            }
        }

        public Result UpdateCustomerVerifyFlag(UpdateCustomerVerifyFlagCriteria criteria)
        {
            return ExecuteNonQuery(criteria);
        }

        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private CustomerInfo ReadCustomerInfo(IDataReader reader)
        {
            var customerInfo = new CustomerInfo
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

        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private RedemptionLogInfo ReadRedemptionLog(IDataReader reader)
        {
            var redemptionLog = new RedemptionLogInfo
            {
                Id = Converter.ToInt(reader["RedemptionId"]),
                ClaimedTime = DateTimeUtilize.ChangeTimeZoneToClientDateTime(Converter.ToDateTime(reader["ClaimTime"])).ToShortFormatedDate(),
                Subject = Converter.ToString2(reader["Subject"]),
                Status = new Setting
                {
                    Value = Converter.ToString2(reader["Status"])
                },
                Amount = Converter.ToDecimal(reader["Amount"]),
                DoneBy = Converter.ToString2(reader["DoneBy"]),
                Currency = new BaseModel { Name = Converter.ToString2(reader["Currency"]) }
            };

            return redemptionLog;
        }

        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private CommunicationLogInfo ReadCommunicationLog(IDataReader reader)
        {
            var durationTime = Converter.ToString2(reader["Duration"]);
            var durationTimeArray = durationTime.Split(':');
            var communicationLog = new CommunicationLogInfo
            {
                RunningNumber = Converter.ToInt(reader["FeedbackID"]),
                FeedbackDate = DateTimeUtilize.ChangeTimeZoneToClientDateTime(Converter.ToDateTime(reader["FeedbackDate"])).ToLongFormatedDateTime(),
                MainCategory = new Setting { Name = Converter.ToString2(reader["MainCategory"]) },
                SubCategory = new Setting { Name = Converter.ToString2(reader["SubCategory"]) },
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
