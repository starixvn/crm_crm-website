namespace Fanex.CRM.Communication.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Fanex.Common;
    using Fanex.CRM.Common.Extensions;
    using Fanex.CRM.Common.Helpers;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Communication.Criteria;
    using Fanex.Data;

    public class CommunicationLogRepository : ICommunicationLogRepository
    {
        public IEnumerable<BaseCommunicationLog> GetLogsByCustomerId(GetLogsByCustomerIdCriteria criteria)
        {
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objectDB.ExecuteReader(criteria.GetSpParams()))
                {
                    var feedbacks = new List<BaseCommunicationLog>();
                    while (reader.Read())
                    {
                        var log = MappingDataFromSqlToLogInfoObject(reader);
                        log.CustomerId = criteria.CustomerId;
                        feedbacks.Add(log);
                    }

                    var settings = new List<SettingItem>();
                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            settings.Add(new SettingItem
                            {
                                FeedbackID = Converter.ToInt(reader["FeedbackID"]),
                                SettingIdLv1 = Converter.ToInt(reader["MainCategoryID"]),
                                SettingIdLv2 = Converter.ToInt(reader["SubCategoryID"])
                            });
                        }
                    }

                    return GenerateFeedBackDetailWithSetting(feedbacks, settings);
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
                        var relatedCustomers = new List<RelatedCustomer>();
                        while (reader.Read())
                        {
                            relatedCustomers.Add(new RelatedCustomer
                            {
                                Id = Converter.ToInt(reader["CustID"]),
                                Name = Converter.ToString2(reader["UserName"]),
                                IsSiteActive = Converter.ToBool(reader["IsSiteActive"]),
                                IsLicensee = Convert.ToBoolean(reader["IsLicensee"])
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

        private BaseCommunicationLog MappingDataFromSqlToLogInfoObject(IDataReader reader)
        {
            var durationTime = Converter.ToString2(reader["Duration"]);
            var durationTimeArray = durationTime.Split(':');

            var logInfo = new BaseCommunicationLog
            {
                RunningNumber = Converter.ToInt(reader["FeedbackID"]),
                FeedbackDate = DateTimeUtilize.ChangeTimeZoneToClientDateTime(Converter.ToDateTime(reader["FeedbackDate"])).ToLongFormatedDateTime(),
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
                Province = new Province
                {
                    Id = Converter.ToInt(reader["ProvinceID"])
                },
                CustomerId = Converter.ToInt(reader["CustId"]),
                CustomerName = Converter.ToString2(reader["UserName"]),
                NoInquiries = Converter.ToInt(reader["NoInquiries"]),

                Reviewer = Converter.ToString2(reader["reviewer"]),
                ReviewedTimeString = DateTimeUtilize.ChangeTimeZoneToClientDateTime(Converter.ToDateTime(reader["SolveDate"])).ToLongFormatedDateTime(),

                LicenseeType = new Setting { Id = Converter.ToInt(reader["LicenseeType"]) },
                IsSiteActive = Converter.ToBool(reader["IsSiteActive"]),
                IsLicensee = Convert.ToBoolean(reader["IsLicensee"])
            };

            return logInfo;
        }

        private static IEnumerable<BaseCommunicationLog> GenerateFeedBackDetailWithSetting(
            IEnumerable<BaseCommunicationLog> feedbacks,
            IEnumerable<SettingItem> settings)
        {
            Func<BaseCommunicationLog, List<SettingItem>, BaseCommunicationLog> regenerateFeedBackDetail =
                delegate (BaseCommunicationLog f, List<SettingItem> s)
                {
                    f.Settings = s;
                    return f;
                };
            var result = from f in feedbacks
                         join s in settings on f.RunningNumber equals s.FeedbackID
                         select new { f, s } into t1
                         group t1 by t1.f.RunningNumber into g
                         select regenerateFeedBackDetail(g.FirstOrDefault().f, g.Select(i => i.s).ToList());

            return result.ToList();
        }
    }
}