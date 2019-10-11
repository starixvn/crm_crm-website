namespace Fanex.CRM.Feedback.Repositories
{
    using System.Data;
    using Common.Helpers;
    using Common.Models;
    using Criteria;
    using Data;
    using Fanex.Common;
    using Common.Extensions;

    public class MultipleLogsRepository : IMultipleLogsRepository
    {
        public BaseCommunicationLog GetLogByFeedbackId(GetLogByFeedbackIdCriteria criteria)
        {
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objectDB.ExecuteReader(criteria.GetSpParams()))
                {
                    var log = new BaseCommunicationLog();

                    while (reader.Read())
                    {
                        log = MappingLogData(reader);
                    }

                    reader.NextResult();
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

        private BaseCommunicationLog MappingLogData(IDataReader reader)
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
                Remark = Converter.ToString2(reader["Description"]),
                Solution = Converter.ToString2(reader["Solution"]),
                AutoField = Converter.ToString2(reader["AutoField"]),
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

                LicenseeType = new Setting { Id = Converter.ToInt(reader["LicenseeType"]) }
            };

            return logInfo;
        }
    }
}