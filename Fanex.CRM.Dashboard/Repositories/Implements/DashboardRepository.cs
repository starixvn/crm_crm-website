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
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Common;
    using Fanex.CRM.Dashboard.Criteria;
    using Fanex.CRM.Dashboard.Models;
    using Fanex.CRM.Dashboard.Repositories.Interfaces;
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
        /// Gets the customer dashboard information.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The customer dashboard information.</returns>
        public CustomerDashboardInfo GetCustomerDashboardInfo(GetCustomerDashboardInfoCriteria criteria)
        {
            CustomerDashboardInfo result = new CustomerDashboardInfo
            {
                Remarks = new List<string>(),
                RedemptionLogs = new List<RedemptionLogInfo>(),
                CommunicationLogs = new List<CommunicationLogInfo>()
            };

            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                objectDB.AssignParamValues(criteria.GetSpParams());
                using (var reader = objectDB.ExecuteReader())
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            result.CustomerInfo = MappingDataFromSqlToCustomerInfoObject(reader);
                        }

                        result.CustomerInfo.Id = criteria.CustomerId;
                    }

                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            result.Remarks.Add(MappingDataFromSqlToRemarkObject(reader));
                        }
                    }

                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            result.RedemptionLogs.Add(MappingDataFromSqlToRedemptionLogObject(reader));
                        }
                    }

                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            result.CommunicationLogs.Add(MappingDataFromSqlToCommunicationLogObject(reader));
                        }
                    }
                }
            }

            return result;
        }

        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private CustomerInfo MappingDataFromSqlToCustomerInfoObject(IDataReader reader)
        {
            CustomerInfo result = new CustomerInfo
            {
                Email = reader.GetStringNotNull("email"),
                Passport = reader.GetStringNotNull("Passport"),
                SecretCode = reader.GetStringNotNull("SecretCode"),
                BetCredit = reader.GetDecimalNotNull("BetCredit"),

                IsVerifyEmail = reader.GetBooleanNotNull("IsVerifyEmail"),
                IsVerifyPassport = reader.GetBooleanNotNull("IsVerifyPassport"),
                IsVerifyBirthday = reader.GetBooleanNotNull("IsVerifyBirthday"),
                IsVerifySecretCode = reader.GetBooleanNotNull("IsVerifySecretCode"),
                IsVerifyBetCredit = reader.GetBooleanNotNull("IsVerifyBetCredit")
            };

            return result;
        }

        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private string MappingDataFromSqlToRemarkObject(IDataReader reader)
        {
            string result = reader.GetStringNotNull("Remark");
            return result;
        }

        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private RedemptionLogInfo MappingDataFromSqlToRedemptionLogObject(IDataReader reader)
        {
            var result = new RedemptionLogInfo
            {
                Id = reader.GetInt32NotNull("RedemptionId"),
                ClaimedTime = reader.GetDateTimeNotNull("ClaimTime").ToShortFormatedDate(),
                Subject = reader.GetStringNotNull("Subject"),
                Status = new SettingInfo
                {
                    Value = reader.GetStringNotNull("Status")
                }
            };

            return result;
        }

        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private CommunicationLogInfo MappingDataFromSqlToCommunicationLogObject(IDataReader reader)
        {
            var durationTime = reader.GetStringNotNull("Duration");
            var durationTimeArray = durationTime.Split(':');
            var result = new CommunicationLogInfo
            {
                RunningNumber = reader.GetInt32NotNull("FeedbackID"),
                FeedbackDate = reader.GetDateTimeNotNull("FeedbackDate").ToShortFormatedDate(),
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

            return result;
        }
    }
}
