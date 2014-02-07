// --------------------------------------------------------------------------
// <copyright file="RedemptionLogRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Redemption.Repositories.Implements
{
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Common;
    using Fanex.CRM.Infrastructure.Extension;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Redemption.Criteria;
    using Fanex.CRM.Redemption.Models;
    using Fanex.CRM.Redemption.Repositories.Interfaces;
    using Fanex.CRM.Setting.Models;
    using Fanex.Data;

    /// <summary>
    /// Redemption Log Repository.
    /// </summary>
    public class RedemptionLogRepository : IRedemptionLogRepository
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

        public LogInfo GetLogById(GetLogByIdCriteria criteria)
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
                Id = reader.GetInt32NotNull("RedemptionId"),
                ClaimedTime = reader.GetDateTimeNotNull("ClaimedTime").ToShortFormatedDate(),
                Subject = reader.GetStringNotNull("Subject"),
                Status = new SettingInfo
                {
                    Id = reader.GetInt32NotNull("StatusID")
                }
            };

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
                Id = reader.GetInt32NotNull("RedemptionId"),
                CustomerId = reader.GetInt32NotNull("CustId"),
                ClaimedTime = reader.GetDateTimeNotNull("ClaimedTime").ToShortFormatedDate(),
                Subject = reader.GetStringNotNull("Subject"),
                Remark = reader.GetStringNotNull("Description"),
                Status = new SettingInfo
                {
                    Id = reader.GetInt32NotNull("StatusID")
                }
            };

            return logInfo;
        }
    }
}
