// --------------------------------------------------------------------------
// <copyright file="CommunicationLogRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Communication.Repositories.Implements
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Common;
    using Fanex.CRM.Communication.Criteria;
    using Fanex.CRM.Communication.Models;
    using Fanex.CRM.Communication.Repositories.Interfaces;
    using Fanex.CRM.Infrastructure.Extension;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Setting.Models;
    using Fanex.Data;

    /// <summary>
    /// Communication Log Repository.
    /// </summary>
    public class CommunicationLogRepository : ICommunicationLogRepository
    {
        /// <summary>
        /// Gets the logs by customer identifier.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The logs by customer identifier.</returns>
        public List<LogInfo> GetLogsByCustomerId(GetLogsByCustomerIdCriteria criteria)
        {
            List<LogInfo> logs = new List<LogInfo>();
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                objectDB.AssignParamValues(criteria.GetSpParams());
                using (var reader = objectDB.ExecuteReader())
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            var log = MappingDataFromSqlToLogInfoObject(reader);
                            log.CustomerId = criteria.CustomerId;
                            logs.Add(log);
                        }
                    }
                }
            }

            return logs;
        }

        /// <summary>
        /// Updates the log.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Update result.</returns>
        public Result UpdateLog(UpdateLogCriteria criteria)
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
        /// Inserts the log.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Insert result.</returns>
        public Result InsertLog(InsertLogCriteria criteria)
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
        /// Mappings the data from SQL to log information object.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>Log Info Object.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
             "SP2101:MethodMustNotContainMoreLinesThan",
             Justification = "Reviewed. Suppression is OK here.")]
        private LogInfo MappingDataFromSqlToLogInfoObject(IDataReader reader)
        {
            var durationTime = reader.GetStringNotNull("Duration");
            var durationTimeArray = durationTime.Split(':');

            var logInfo = new LogInfo
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
                Reviewer = reader.GetStringNotNull("ReviewBy"),

                StartTime = new DateTime(2014, 1, 31, 13, 30, 0),
                EndTime = new DateTime(2014, 1, 31, 13, 35, 0),
                Extension = new SettingInfo
                {
                    Id = 2000,
                    Value = "946"
                },
                CommunicationTool = new SettingInfo
                {
                    Id = 1000,
                    Value = "Phone"
                },
                LanguageId = 12002,
                Product = new SettingInfo
                {
                    Id = 8001,
                    Value = "Soccer"
                },
                BetType = new SettingInfo
                {
                    Id = 9019,
                    Value = "Clean Sheet"
                },
                Status = new SettingInfo
                {
                    Id = 11002,
                    Value = "Closed"
                },
                SiteInfo = new SettingInfo
                {
                    Id = 6004,
                    Value = "BEST-ODDS"
                },
                RoleInfo = new SettingInfo
                {
                    Id = 7003,
                    Value = "Super "
                },
                Response = new SettingInfo
                {
                    Id = 10000,
                    Value = "Satisfied "
                },
                RelatedCustomers = "testcs, TESTCS0101",
                Remark = "Remark value",
                Solution = "Solution value",
                Review = "Review value",
                Country = new CountryInfo
                {
                    Id = 2
                }
            };

            logInfo.IsReviewed = logInfo.Reviewer != string.Empty;
            return logInfo;
        }
    }
}
