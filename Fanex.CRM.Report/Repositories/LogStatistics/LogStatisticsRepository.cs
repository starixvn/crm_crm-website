namespace Fanex.CRM.Report.Repositories.LogStatistics
{
    using Fanex.Common;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Report.Criteria.LogStatistics;
    using Fanex.CRM.Report.Entities.LogStatistics;
    using Fanex.CRM.Report.Models.StatisticsReportByDate;
    using Fanex.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    public class LogStatisticsRepository : ILogStatisticsRepository
    {
        public IEnumerable<LogStatisticsEntity> GetReportData(LogStatisticsCriteria criteria)
        {
            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                return objectDb.Query<LogStatisticsEntity>(criteria.GetSpParams());
            }
        }

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules",
             "SP2101:MethodMustNotContainMoreLinesThan",
             Justification = "Reviewed. Suppression is OK here.")]
        public IEnumerable<StatisticsDetailbyDateModel> GetReportDetail(LogStatisticsReportDetailCriteria criteria)
        {
            var feedbacks = new List<StatisticsDetailbyDateModel>();
            var settings = new List<SettingItem>();
            var result = new List<StatisticsDetailbyDateModel>();
            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objDb.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        StatisticsDetailbyDateModel statisticsDetail = new StatisticsDetailbyDateModel
                        {
                            FeedbackID = Converter.ToInt(reader["FeedbackID"]),
                            StartDate = Converter.ToDateTime(reader["FeedbackDate"]),
                            ResolutionTime = Converter.ToDateTime(reader["ResolutionDate"]),
                            Remark = Converter.ToString2(reader["Remark"]),
                            AutoField = Converter.ToString2(reader["AutoField"]),
                            Solution = Converter.ToString2(reader["Solution"]),
                            Username = Converter.ToString2(reader["UserName"]),
                            SiteInfo = new Setting { Name = Converter.ToString2(reader["SiteName"]) },
                            Country = new Country { Name = Converter.ToString2(reader["CountryName"]) },
                            StatusName = Converter.ToString2(reader["Status"]),
                            Response = new Setting { Name = Converter.ToString2(reader["Response"]) },
                            Handler = Converter.ToString2(reader["CreatorName"]),
                            CreationTime = Converter.ToDateTime(reader["CreateDate"]),
                            Role = Converter.ToString2(reader["Role"]),
                            Fax = new Fax
                            {
                                CountryCode = Converter.ToString2(reader["FaxCountryCode"]),
                                AreaCode = Converter.ToString2(reader["FaxAreaCode"]),
                                FaxNumber = Converter.ToString2(reader["Fax"])
                            },
                            MobilePhone = new Phone
                            {
                                CountryCode = Converter.ToString2(reader["MobilePhoneCountryCode"]),
                                AreaCode = Converter.ToString2(reader["MobilePhoneAreaCode"]),
                                PhoneNumber = Converter.ToString2(reader["MobilePhone"])
                            },
                            MSN = Converter.ToString2(reader["MSN"]),
                            Skype = Converter.ToString2(reader["Skype"]),
                            Email = Converter.ToString2(reader["Email"]),
                            WhatsApp = Converter.ToString2(reader["WhatsApp"]),
                            Line = Converter.ToString2(reader["Line"]),
                            WEChat = Converter.ToString2(reader["WeChat"]),
                            Yahoo = Converter.ToString2(reader["Yahoo"]),
                            QQ = Converter.ToString2(reader["QQ"]),
                            Bbm = Converter.ToString2(reader["BBM"])
                        };

                        var autoFieldText = string.IsNullOrEmpty(statisticsDetail.AutoField) ?
                            string.Empty :
                            (new System.Text.RegularExpressions.Regex("\n")).Replace(statisticsDetail.AutoField, "<br>") + "<br><br>";
                        statisticsDetail.RemarkText = autoFieldText + statisticsDetail.Remark;

                        statisticsDetail.Phones.Add(ReadPhoneInfo(reader, "Phone"));
                        statisticsDetail.Phones.Add(ReadPhoneInfo(reader, "Phone2"));
                        statisticsDetail.Phones.Add(ReadPhoneInfo(reader, "Phone3"));

                        statisticsDetail.PICs.Add(ReadPersonalInfoContact(reader, "PIC1"));
                        statisticsDetail.PICs.Add(ReadPersonalInfoContact(reader, "PIC2"));
                        statisticsDetail.PICs.Add(ReadPersonalInfoContact(reader, "PIC3"));
                        statisticsDetail.PICs.Add(ReadPersonalInfoContact(reader, "PIC4"));

                        feedbacks.Add(statisticsDetail);
                    }


                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            settings.Add(new SettingItem
                            {
                                FeedbackID = Converter.ToInt(reader["FeedbackID"]),
                                SettingType = Converter.ToString2(reader["SettingType"]),
                                SettingIdLv1 = Converter.ToInt(reader["SettingID"]),
                                SettingIdLv2 = Converter.ToInt(reader["SubSettingID"]),
                                SettingIdLv3 = Converter.ToInt(reader["SubSettingIDLevel3"]),
                            });
                        }
                    }

                    var groups = from f in feedbacks
                                 join s in settings on f.FeedbackID equals s.FeedbackID
                                 select new { f, s } into t1
                                 group t1 by t1.f.FeedbackID into g
                                 select new
                                 {
                                     feedback = g.FirstOrDefault().f,
                                     settings = g.Select(r => r.s)
                                 };

                    foreach (var i in groups)
                    {
                        i.feedback.Settings = i.settings;
                        result.Add(i.feedback);
                    }
                }

                var outputParams = objDb.GetOuputParams();
                criteria.TotalRows = (int)outputParams["totalrows"];
            }

            return GenerateFeedBackDetailWithSetting(feedbacks, settings);
        }

        private IEnumerable<StatisticsDetailbyDateModel> GenerateFeedBackDetailWithSetting(IEnumerable<StatisticsDetailbyDateModel> feedbacks, IEnumerable<SettingItem> settings)
        {
            Func<StatisticsDetailbyDateModel, List<SettingItem>, StatisticsDetailbyDateModel> regenerateFeedBackDetail = delegate (StatisticsDetailbyDateModel f, List<SettingItem> s)
            {
                f.Settings = s;
                return f;
            };
            var result = from f in feedbacks
                         join s in settings on f.FeedbackID equals s.FeedbackID
                         select new { f, s } into t1
                         group t1 by t1.f.FeedbackID into g
                         select regenerateFeedBackDetail(g.FirstOrDefault().f, g.Select(i => i.s).ToList());

            return result.ToList();
        }

        private static PersonalInfoContact ReadPersonalInfoContact(IDataReader reader, string prefix)
        {
            return new PersonalInfoContact
            {
                Name = Converter.ToString2(reader[prefix + "Name"]),
                Phone = Converter.ToString2(reader[prefix])
            };
        }

        private static Phone ReadPhoneInfo(IDataReader reader, string prefix)
        {
            return new Phone
            {
                CountryCode = Converter.ToString2(reader[prefix + "CountryCode"]),
                AreaCode = Converter.ToString2(reader[prefix + "AreaCode"]),
                PhoneNumber = Converter.ToString2(reader[prefix])
            };
        }
    }
}
