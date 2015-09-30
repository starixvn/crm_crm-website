// --------------------------------------------------------------------------
// <copyright file="RedemptionLogRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Redemption.Repositories.RedemptionLog
{
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Common;
    using Fanex.CRM.Common.Extensions;
    using Fanex.CRM.Common.Helpers;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Common.Repositories;
    using Fanex.CRM.Infrastructure.Extensions;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Redemption.Criteria;
    using Fanex.CRM.Redemption.Models;
    using Fanex.CRM.Redemption.Repositories.RedemptionLog;
    using Fanex.Data;

    /// <summary>
    /// Redemption Log Repository.
    /// </summary>
    public class RedemptionLogRepository : BaseRepository, IRedemptionLogRepository
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
        /// Gets the log by identifier.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The log by identifier.</returns>
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
            return ExecuteNonQuery(criteria);
        }

        /// <summary>
        /// Inserts the log.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Insert result.</returns>
        public Result InsertLog(InsertLogCriteria criteria)
        {
            Result insertResult = null;
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                objectDB.AssignParamValues(criteria.GetSpParams());
                objectDB.ExecuteNonQuery();
                var outputParams = objectDB.GetOuputParams();
                insertResult = new Result
                {
                    Code = Converter.ToInt(outputParams["errcode"]),
                    Message = Converter.ToString2(outputParams["errmsg"])
                };
            }

            return insertResult;
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
            var logInfo = new LogInfo
            {
                Id = Converter.ToInt(reader["Id"]),
                ClaimedTime = DateTimeUtilize.ChangeTimeZoneToClientDateTime(Converter.ToDateTime(reader["ClaimTime"])).ToShortFormatedDate(),
                Subject = Converter.ToString2(reader["Subject"]),
                Status = new SettingInfo
                {
                    Value = Converter.ToString2(reader["Status"])
                },
                Currency = new BaseModel { Name = Converter.ToString2(reader["Currency"]) },
                DoneBy = Converter.ToString2(reader["DoneBy"]),
                Amount = Converter.ToDecimal(reader["Amount"])
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
                Id = Converter.ToInt(reader["Id"]),
                CustomerId = Converter.ToInt(reader["CustId"]),
                ClaimedTime = DateTimeUtilize.ChangeTimeZoneToClientDateTime(Converter.ToDateTime(reader["ClaimTime"])).ToShortFormatedDate(),
                Currency = new BaseModel { Id = Converter.ToInt(reader["CurrencyId"]) },
                DoneBy = Converter.ToString2(reader["DoneBy"]),
                Amount = Converter.ToDecimal(reader["Amount"]),
                Subject = Converter.ToString2(reader["Subject"]).HtmlEncode(),
                Remark = Converter.ToString2(reader["Remark"]),
                Status = new SettingInfo
                {
                    Id = Converter.ToInt(reader["StatusID"])
                }
            };

            return logInfo;
        }
    }
}
