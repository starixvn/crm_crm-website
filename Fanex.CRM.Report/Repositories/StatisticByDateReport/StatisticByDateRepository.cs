namespace Fanex.CRM.Report.Repositories.StatisticByDateReport
{
    using Fanex.Common;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Report.Criteria.StatisticByDateReport;
    using Fanex.CRM.Report.Entities.StatisticsReportByDate;
    using Fanex.CRM.Report.Models.StatisticsReportByDate;
    using Fanex.CRM.Report.Resources;
    using Fanex.Data;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    public class StatisticByDateRepository : IStatisticByDateRepository
    {
        public StatisticsInfoByDateRepositoryResultModel GetMainReport(GetMainReportCriteria criteria)
        {
            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objectDb.ExecuteReader(criteria.GetSpParams()))
                {
                    var result = new StatisticsInfoByDateRepositoryResultModel
                    {
                        RawData = new List<StatisticByDateEntity>(),
                        CommunicationTools = GetCommunicationTools()
                    };

                    var communicationToolLength = result.CommunicationTools.Count;
                    while (reader.Read())
                    {
                        var customerInfo = new StatisticByDateEntity
                        {
                            MainCategoryId = Converter.ToInt(reader["MainCategoryId"]),
                            MainCategoryName = Converter.ToString2(reader["MainCategory"]),
                            SubCategoryId = Converter.ToInt(reader["SubCategoryId"]),
                            SubCategoryName = Converter.ToString2(reader["SubCategory"]),
                            NumberOfIssues = new List<int>()
                        };

                        for (var i = 0; i < communicationToolLength; i++)
                        {
                            var numberOfIssue = Converter.ToInt(reader[result.CommunicationTools[i].Name]);
                            customerInfo.NumberOfIssues.Add(numberOfIssue);
                        }

                        result.RawData.Add(customerInfo);
                    }

                    return result;
                }
            }
        }

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules",
             "SP2101:MethodMustNotContainMoreLinesThan",
             Justification = "Reviewed. Suppression is OK here.")]
        public IList<StatisticsDetailbyDateModel> GetDetailReport(GetDetailReportCriteria criteria)
        {
            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objectDb.ExecuteReader(criteria.GetSpParams()))
                {
                    var result = new List<StatisticsDetailbyDateModel>();
                    while (reader.Read())
                    {
                        var statisticsDetail = new StatisticsDetailbyDateModel
                        {
                            FeedbackID = Converter.ToInt(reader["FeedbackID"]),
                            StartDate = Converter.ToDateTime(reader["FeedbackDate"]),
                            ResolutionTime = Converter.ToDateTime(reader["ResolutionDate"]),
                            Remark = Converter.ToString2(reader["Description"]),
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

                        statisticsDetail.Phones.Add(ReadPhoneInfo(reader, "Phone"));
                        statisticsDetail.Phones.Add(ReadPhoneInfo(reader, "Phone2"));
                        statisticsDetail.Phones.Add(ReadPhoneInfo(reader, "Phone3"));

                        statisticsDetail.PICs.Add(ReadPersonalInfoContact(reader, "PIC1"));
                        statisticsDetail.PICs.Add(ReadPersonalInfoContact(reader, "PIC2"));
                        statisticsDetail.PICs.Add(ReadPersonalInfoContact(reader, "PIC3"));
                        statisticsDetail.PICs.Add(ReadPersonalInfoContact(reader, "PIC4"));

                        result.Add(statisticsDetail);
                    }

                    var settings = new List<SettingItem>();
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

                    result = result.Select(l =>
                    {
                        l.Settings = settings.Where(s => s.FeedbackID == l.FeedbackID).ToList();
                        return l;
                    }).ToList();

                    return result;
                }
            }
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

        private static IList<BaseModel> GetCommunicationTools()
        {
            var result = new List<BaseModel>();
            result.Add(new BaseModel { Name = Labels.Call });
            result.Add(new BaseModel { Name = Labels.Email });
            result.Add(new BaseModel { Name = Labels.MSN });
            result.Add(new BaseModel { Name = Labels.Skype });
            result.Add(new BaseModel { Name = Labels.LiveChat });
            result.Add(new BaseModel { Name = Labels.NA });
            result.Add(new BaseModel { Name = Labels.Yahoo });
            result.Add(new BaseModel { Name = Labels.WeChat });
            result.Add(new BaseModel { Name = Labels.Line });
            result.Add(new BaseModel { Name = Labels.WhatsApp });
            result.Add(new BaseModel { Name = Labels.QQ });
            return result;
        }
    }
}
