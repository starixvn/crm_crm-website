// --------------------------------------------------------------------------
// <copyright file="CommunicationLogRepository.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Communication.Repositories
{
    using Fanex.Common;
    using Fanex.CRM.Common.Helpers;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Common.Repositories;
    using Fanex.CRM.Communication.Criteria;
    using Fanex.CRM.Communication.Models;
    using Fanex.CRM.Infrastructure.Extensions;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.Data;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    /// <summary>
    /// Communication Log Repository.
    /// </summary>
    public class CommunicationLogRepository : BaseRepository, ICommunicationLogRepository
    {
        public IEnumerable<BaseCommunicationLog> GetLogsByCustomerId(GetLogsByCustomerIdCriteria criteria)
        {
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objectDB.ExecuteReader(criteria.GetSpParams()))
                {
                    var logs = new List<BaseCommunicationLog>();
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

        public BaseCommunicationLog GetLogsByRunningNumber(GetLogByRunningNumberCriteria criteria)
        {
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objectDB.ExecuteReader(criteria.GetSpParams()))
                {
                    var log = new BaseCommunicationLog();
                    while (reader.Read())
                    {
                        log = MappingDataFromSqlToLogInfoObjectById(reader);
                    }

                    if (reader.NextResult())
                    {
                        var relatedCustomers = new List<BaseModel>();
                        while (reader.Read())
                        {
                            relatedCustomers.Add(new BaseModel
                            {
                                Id = Converter.ToInt(reader["CustID"]),
                                Name = Converter.ToString2(reader["UserName"])
                            });
                        }

                        log.RelatedCustomers = relatedCustomers.OrderBy(c => c.Name);
                    }

                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            log.Settings.Add(new SettingItem
                            {
                                SettingType = Converter.ToString2(reader["SettingType"]),
                                SettingIdLv1 = Converter.ToInt(reader["SettingID"]),
                                SettingIdLv2 = Converter.ToInt(reader["SubSettingID"]),
                                SettingIdLv3 = Converter.ToInt(reader["SubSettingIDLevel3"]),
                            });
                        }
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
        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules",
             "SP2101:MethodMustNotContainMoreLinesThan",
             Justification = "Reviewed. Suppression is OK here.")]
        private BaseCommunicationLog MappingDataFromSqlToLogInfoObject(IDataReader reader)
        {
            var durationTime = Converter.ToString2(reader["Duration"]);
            var durationTimeArray = durationTime.Split(':');

            var logInfo = new BaseCommunicationLog
            {
                RunningNumber = Converter.ToInt(reader["FeedbackID"]),
                FeedbackDate = DateTimeUtilize.ChangeTimeZoneToClientDateTime(Converter.ToDateTime(reader["FeedbackDate"])).ToLongFormatedDateTime(),
                MainCategory = new Setting
                {
                    Name = Converter.ToString2(reader["MainCategory"])
                },
                SubCategory = new Setting
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
        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules",
             "SP2101:MethodMustNotContainMoreLinesThan",
             Justification = "Reviewed. Suppression is OK here.")]
        private BaseCommunicationLog MappingDataFromSqlToLogInfoObjectById(IDataReader reader)
        {
            var logInfo = new BaseCommunicationLog
            {
                RunningNumber = Converter.ToInt(reader["FeedbackID"]),
                FeedbackDate = DateTimeUtilize.ChangeTimeZoneToClientDateTime(Converter.ToDateTime(reader["FeedbackDate"])).ToShortFormatedDate(),
                Subject = Converter.ToString2(reader["Subject"]),
                StartTime = DateTimeUtilize.ChangeTimeZoneToClientDateTime(Converter.ToDateTime(reader["FeedbackDate"])).ToLongFormatedDateTime(),
                EndTime = DateTimeUtilize.ChangeTimeZoneToClientDateTime(Converter.ToDateTime(reader["ResolutionDate"])).ToLongFormatedDateTime(),
                Language = new Setting
                {
                    Id = Converter.ToInt(reader["LanguageID"])
                },
                Status = new Setting
                {
                    Id = Converter.ToInt(reader["StatusID"])
                },
                SiteInfo = new Site
                {
                    Id = Converter.ToInt(reader["SiteID"])
                },
                RoleInfo = new Setting
                {
                    Id = Converter.ToInt(reader["RoleID"])
                },
                Response = new Setting
                {
                    Id = Converter.ToInt(reader["ResponseID"])
                },
                AutoField = Converter.ToString2(reader["AutoField"]),
                Remark = Converter.ToString2(reader["Description"]),
                Solution = Converter.ToString2(reader["Solution"]),
                Review = Converter.ToString2(reader["Review"]),
                IsReviewed = Converter.ToBool(reader["IsReviewed"]),
                Country = new Country
                {
                    Id = Converter.ToInt(reader["CountryID"])
                },
                CustomerId = Converter.ToInt(reader["CustId"]),
                CustomerName = Converter.ToString2(reader["UserName"]),

                Reviewer = Converter.ToString2(reader["reviewer"]),
                ReviewedTimeString = DateTimeUtilize.ChangeTimeZoneToClientDateTime(Converter.ToDateTime(reader["SolveDate"])).ToLongFormatedDateTime(),

                LicenseeType = new Setting { Id = Converter.ToInt(reader["LicenseeType"]) }
            };

            return logInfo;
        }
    }
}
