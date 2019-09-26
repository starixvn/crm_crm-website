namespace Fanex.CRM.Report.Repositories.LogStatistics
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Common;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Report.Criteria.LogStatistics;
    using Fanex.CRM.Report.Models.StatisticsReportByDate;
    using Fanex.Data;

    public class LogStatisticsRepository : ILogStatisticsRepository
    {
        public IEnumerable<StatisticsDetailbyDateModel> GetReportDetail(LogStatisticsReportDetailCriteria criteria)
        {
            var feedbacks = new List<StatisticsDetailbyDateModel>();
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
                            Bbm = Converter.ToString2(reader["BBM"]),
                            ProductID = Converter.ToInt(reader["ProductID"]),
                            SubProductID = Converter.ToInt(reader["SubProductID"]),
                            CategoryID = Converter.ToInt(reader["CategoryID"]),
                            SubCategoryID = Converter.ToInt(reader["SubCategoryID"]),
                            CommunicationID = Converter.ToInt(reader["CommunicationID"]),
                            StationID = Converter.ToInt(reader["StationID"]),
                            NoInquiries = Converter.ToInt(reader["NoInquiries"])
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
                }

                var outputParams = objDb.GetOuputParams();
                criteria.TotalRows = (int)outputParams["totalrows"];
            }

            return feedbacks;
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