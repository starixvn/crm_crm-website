// --------------------------------------------------------------------------
// <copyright file="CommunicationLogRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Communication.Repositories
{
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Text;
    using Fanex.Common;
    using Fanex.CRM.Common.Helpers;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Common.Repositories;
    using Fanex.CRM.Communication.Criteria;
    using Fanex.CRM.Communication.Models;
    using Fanex.CRM.Infrastructure.Extensions;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.Data;

    /// <summary>
    /// Communication Log Repository.
    /// </summary>
    public class CommunicationLogRepository : BaseRepository, ICommunicationLogRepository
    {
        public IEnumerable<BaseCommunicationLogInfo> GetLogsByCustomerId(GetLogsByCustomerIdCriteria criteria)
        {
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objectDB.ExecuteReader(criteria.GetSpParams()))
                {
                    var logs = new List<BaseCommunicationLogInfo>();
                    while (reader.Read())
                    {
                        var log = MappingDataFromSqlToLogInfoObject(reader);
                        log.CustomerId = criteria.CustomerId;
                        logs.Add(log);
                    }

                    return logs;
                }
            }
        }

        public BaseCommunicationLogInfo GetLogsByRunningNumber(GetLogByRunningNumberCriteria criteria)
        {
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objectDB.ExecuteReader(criteria.GetSpParams()))
                {
                    var log = new BaseCommunicationLogInfo();
                    while (reader.Read())
                    {
                        log = MappingDataFromSqlToLogInfoObjectById(reader);
                    }

                    if (reader.NextResult())
                    {
                        var customerNames = new StringBuilder();
                        var customerIds = new StringBuilder();
                        while (reader.Read())
                        {
                            var name = Converter.ToString2(reader["UserName"]);
                            var id = Converter.ToInt(reader["CustID"]);
                            customerNames.Append(name + ",");
                            customerIds.Append(id + ",");
                        }

                        log.RelatedCustomers = customerNames.ToString().TrimEnd(',');
                        log.RelatedCustomerIds = customerIds.ToString().TrimEnd(',');
                    }

                    return log;
                }
            }
        }

        public Result UpdateLog(UpdateLogCriteria criteria)
        {
            return ExecuteNonQuery(criteria);
        }

        public Result InsertLog(InsertLogCriteria criteria)
        {
            return ExecuteNonQuery(criteria);
        }

        public CustomerDefaultSetting GetCustomerDefaultSetting(GetDefaultSettingsCriteria criteria)
        {
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                return objectDB.QueryEntity<CustomerDefaultSetting>(criteria.GetSpParams());
            }
        }

        /// <summary>
        /// Mappings the data from SQL to log information object.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>Log Info Object.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
             "SP2101:MethodMustNotContainMoreLinesThan",
             Justification = "Reviewed. Suppression is OK here.")]
        private BaseCommunicationLogInfo MappingDataFromSqlToLogInfoObject(IDataReader reader)
        {
            var durationTime = Converter.ToString2(reader["Duration"]);
            var durationTimeArray = durationTime.Split(':');

            var logInfo = new BaseCommunicationLogInfo
            {
                RunningNumber = Converter.ToInt(reader["FeedbackID"]),
                FeedbackDate = DateTimeUtilize.ChangeTimeZoneToClientDateTime(Converter.ToDateTime(reader["FeedbackDate"])).ToLongFormatedDateTime(),
                MainCategory = new SettingInfo
                {
                    Name = Converter.ToString2(reader["MainCategory"])
                },
                SubCategory = new SettingInfo
                {
                    Name = Converter.ToString2(reader["SubCategory"])
                },
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
        private BaseCommunicationLogInfo MappingDataFromSqlToLogInfoObjectById(IDataReader reader)
        {
            var logInfo = new BaseCommunicationLogInfo
            {
                RunningNumber = Converter.ToInt(reader["FeedbackID"]),
                FeedbackDate = DateTimeUtilize.ChangeTimeZoneToClientDateTime(Converter.ToDateTime(reader["FeedbackDate"])).ToShortFormatedDate(),
                MainCategory = new SettingInfo
                {
                    Id = Converter.ToInt(reader["MainCategoryID"])
                },
                SubCategory = new SettingInfo
                {
                    Id = Converter.ToInt(reader["SubCategoryID"])
                },
                Subject = Converter.ToString2(reader["Subject"]),
                StartTime = DateTimeUtilize.ChangeTimeZoneToClientDateTime(Converter.ToDateTime(reader["FeedbackDate"])).ToLongFormatedDateTime(),
                EndTime = DateTimeUtilize.ChangeTimeZoneToClientDateTime(Converter.ToDateTime(reader["ResolutionDate"])).ToLongFormatedDateTime(),
                Extension = new SettingInfo
                {
                    Id = Converter.ToInt(reader["StationID"])
                },
                CommunicationTool = new SettingInfo
                {
                    Id = Converter.ToInt(reader["CommunicationID"])
                },
                LanguageId = Converter.ToInt(reader["LanguageID"]),
                Product = new SettingInfo
                {
                    Id = Converter.ToInt(reader["ProductID"])
                },
                BetType = new SettingInfo
                {
                    Id = Converter.ToInt(reader["BetTypeID"])
                },
                Status = new SettingInfo
                {
                    Id = Converter.ToInt(reader["StatusID"])
                },
                SiteInfo = new SettingInfo
                {
                    Id = Converter.ToInt(reader["SiteID"])
                },
                RoleInfo = new SettingInfo
                {
                    Id = Converter.ToInt(reader["RoleID"])
                },
                Response = new SettingInfo
                {
                    Id = Converter.ToInt(reader["ResponseID"])
                },
                Remark = Converter.ToString2(reader["Description"]),
                Solution = Converter.ToString2(reader["Solution"]),
                Review = Converter.ToString2(reader["Review"]),
                IsReviewed = Converter.ToBool(reader["IsReviewed"]),
                Country = new CountryInfo
                {
                    Id = Converter.ToInt(reader["CountryID"])
                },
                CustomerId = Converter.ToInt(reader["CustId"]),
                CustomerName = Converter.ToString2(reader["UserName"]),

                Reviewer = Converter.ToString2(reader["reviewer"]),
                ReviewedTimeString = DateTimeUtilize.ChangeTimeZoneToClientDateTime(Converter.ToDateTime(reader["SolveDate"])).ToLongFormatedDateTime(),

                LicenseeType = new SettingInfo { Id = Converter.ToInt(reader["LicenseeType"]) }
            };

            return logInfo;
        }
    }
}
