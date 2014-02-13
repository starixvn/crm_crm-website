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
        /// Gets the logs by running number.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The logs by running number.</returns>
        public LogInfo GetLogsByRunningNumber(GetLogByRunningNumberCriteria criteria)
        {
            LogInfo log = new LogInfo();
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                objectDB.AssignParamValues(criteria.GetSpParams());
                using (var reader = objectDB.ExecuteReader())
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            log = MappingDataFromSqlToLogInfoObject_ById(reader);
                        }
                    }

                    if (reader.NextResult())
                    {
                        List<string> relatedCustomer = new List<string>();
                        while (reader.Read())
                        {
                            relatedCustomer.Add(GetRelatedUserNameFromDataReader(reader));
                        }

                        log.RelatedCustomers = string.Join(",", relatedCustomer);
                    }
                }
            }

            return log;
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
                Reviewer = reader.GetStringNotNull("ReviewBy")
            };

            logInfo.IsReviewed = logInfo.Reviewer != string.Empty;
            return logInfo;
        }

        /// <summary>
        /// Mappings the data from SQL to log information object of function get log by identify.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>Log Info Object.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
             "SP2101:MethodMustNotContainMoreLinesThan",
             Justification = "Reviewed. Suppression is OK here.")]
        private LogInfo MappingDataFromSqlToLogInfoObject_ById(IDataReader reader)
        {
            var logInfo = new LogInfo
            {
                RunningNumber = reader.GetInt32NotNull("FeedbackID"),
                FeedbackDate = reader.GetDateTimeNotNull("FeedbackDate").ToShortFormatedDate(),
                MainCategory = new SettingInfo
                {
                    Id = reader.GetInt32NotNull("MainCategoryID")
                },
                SubCategory = new SettingInfo
                {
                    Id = reader.GetInt32NotNull("SubCategoryID")
                },
                Subject = reader.GetStringNotNull("Subject"),
                StartTime = reader.GetDateTimeNotNull("FeedbackDate"),
                EndTime = reader.GetDateTimeNotNull("ResolutionDate"),
                Extension = new SettingInfo
                {
                    Id = reader.GetInt32NotNull("StationID")
                },
                CommunicationTool = new SettingInfo
                {
                    Id = reader.GetInt32NotNull("CommunicationID")
                },
                LanguageId = reader.GetInt32NotNull("LanguageID"),
                Product = new SettingInfo
                {
                    Id = reader.GetInt32NotNull("ProductID")
                },
                BetType = new SettingInfo
                {
                    Id = reader.GetInt32NotNull("BetTypeID")
                },
                Status = new SettingInfo
                {
                    Id = reader.GetInt32NotNull("StatusID")
                },
                SiteInfo = new SettingInfo
                {
                    Id = reader.GetInt32NotNull("SiteID")
                },
                RoleInfo = new SettingInfo
                {
                    Id = reader.GetInt32NotNull("RoleID")
                },
                Response = new SettingInfo
                {
                    Id = reader.GetInt32NotNull("ResponseID")
                },
                Remark = reader.GetStringNotNull("Description"),
                Solution = reader.GetStringNotNull("Solution"),
                Review = reader.GetStringNotNull("Review"),
                IsReviewed = reader.GetBooleanNotNull("IsReviewed"),
                Country = new CountryInfo
                {
                    Id = reader.GetInt32NotNull("CountryID")
                },
                CustomerId = reader.GetInt32NotNull("CustId")
            };

            logInfo.IsReviewed = logInfo.Reviewer != string.Empty;
            return logInfo;
        }

        private string GetRelatedUserNameFromDataReader(IDataReader reader)
        {
            return reader.GetStringNotNull("UserName");
        }
    }
}
