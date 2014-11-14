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
    using System.Linq;
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
        /// <summary>
        /// Gets the logs by customer identifier.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The logs by customer identifier.</returns>
        public List<BaseCommunicationLogInfo> GetLogsByCustomerId(GetLogsByCustomerIdCriteria criteria)
        {
            List<BaseCommunicationLogInfo> logs = new List<BaseCommunicationLogInfo>();
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
        public BaseCommunicationLogInfo GetLogsByRunningNumber(GetLogByRunningNumberCriteria criteria)
        {
            BaseCommunicationLogInfo log = new BaseCommunicationLogInfo();
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
                        List<string> relatedCustomers = new List<string>();
                        List<int> relatedCustomerIds = new List<int>();
                        while (reader.Read())
                        {
                            var relatedCustomer = GetRelatedUserNameFromDataReader(reader);
                            relatedCustomers.Add(relatedCustomer.Name);
                            relatedCustomerIds.Add(relatedCustomer.Id);
                        }

                        log.RelatedCustomers = string.Join(",", relatedCustomers);
                        log.RelatedCustomerIds = string.Join(",", relatedCustomerIds);
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
            return RunSP(criteria);
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
        /// Gets the customer default setting.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The default setting object.</returns>
        public CustomerDefaultSetting GetCustomerDefaultSetting(GetDefaultSettingsCriteria criteria)
        {
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                var setting = objectDB.Query<CustomerDefaultSetting>(criteria.GetSpParams()).ToList();
                if (setting.Count > 0)
                {
                    return setting[0];
                }
            }

            return null;
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
        private BaseCommunicationLogInfo MappingDataFromSqlToLogInfoObject_ById(IDataReader reader)
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

                LicenseeType = new SettingInfo
                {
                    Id = 42002,
                    Value = "Boping API"
                }
            };

            return logInfo;
        }

        private BaseModel GetRelatedUserNameFromDataReader(IDataReader reader)
        {
            return new BaseModel
            {
                Name = Converter.ToString2(reader["UserName"]),
                Id = Converter.ToInt(reader["CustID"])
            };
        }
    }
}
