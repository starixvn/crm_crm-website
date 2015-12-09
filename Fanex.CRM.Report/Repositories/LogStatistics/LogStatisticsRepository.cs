// -----------------------------------------------------------------------
// <copyright file="LogStatisticRepository.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.Report.Repositories.LogStatistics
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Fanex.Common;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Report.Criteria.LogStatistics;
    using Fanex.CRM.Report.Criteria.StatisticByDateReport;
    using Fanex.CRM.Report.Entities.LogStatistics;
    using Fanex.CRM.Report.Models.StatisticsReportByDate;
    using Fanex.Data;

    public class LogStatisticsRepository : ILogStatisticsRepository
    {
        public IEnumerable<LogStatisticsEntity> GetReportData(LogStatisticsCriteria criteria)
        {
            var result = new List<LogStatisticsEntity>();

            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objectDb.AssignParamValues(criteria.GetSpParams());

                result = objectDb.Query<LogStatisticsEntity>(criteria.GetSpParams()).ToList();
            }

            return result;
        }

        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
             "SP2101:MethodMustNotContainMoreLinesThan",
             Justification = "Reviewed. Suppression is OK here.")]
        public IEnumerable<StatisticsDetailbyDateModel> GetReportDetail(LogStatisticsReportDetailCriteria criteria)
        {
            var result = new List<StatisticsDetailbyDateModel>();
            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                var reader = objDb.ExecuteReader(criteria.GetSpParams());
                while (reader.Read())
                {
                    StatisticsDetailbyDateModel statisticsDetail = new StatisticsDetailbyDateModel
                    {
                        StartDate = Converter.ToDateTime(reader["FeedbackDate"]),
                        ResolutionTime = Converter.ToDateTime(reader["ResolutionDate"]),
                        Remark = Converter.ToString2(reader["Description"]),
                        Solution = Converter.ToString2(reader["Solution"]),
                        Username = Converter.ToString2(reader["UserName"]),
                        SiteInfo = new SettingInfo { Name = Converter.ToString2(reader["SiteName"]) },
                        Product = new SettingInfo { Name = Converter.ToString2(reader["Product"]) },
                        Extension = new SettingInfo { Name = Converter.ToString2(reader["Station"]) },
                        Country = new CountryInfo { Name = Converter.ToString2(reader["CountryName"]) },
                        MainCategory = new SettingInfo { Name = Converter.ToString2(reader["MainCategory"]) },
                        SubCategory = new SettingInfo { Name = Converter.ToString2(reader["SubCategory"]) },
                        StatusName = Converter.ToString2(reader["Status"]),
                        Response = new SettingInfo { Name = Converter.ToString2(reader["Response"]) },
                        Handler = Converter.ToString2(reader["CreatorName"]),
                        CreationTime = Converter.ToDateTime(reader["CreateDate"]),
                        Role = Converter.ToString2(reader["Role"]),
                        CommunicationTool = new SettingInfo { Name = Converter.ToString2(reader["Communication"]) },
                        Fax = new FaxInfo
                        {
                            CountryCode = Converter.ToString2(reader["FaxCountryCode"]),
                            AreaCode = Converter.ToString2(reader["FaxAreaCode"]),
                            FaxNumber = Converter.ToString2(reader["Fax"])
                        },
                        MobilePhone = new PhoneInfo
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

                    statisticsDetail.Phones.Add(new PhoneInfo
                    {
                        CountryCode = Converter.ToString2(reader["PhoneCountryCode"]),
                        AreaCode = Converter.ToString2(reader["PhoneAreaCode"]),
                        PhoneNumber = Converter.ToString2(reader["Phone"])
                    });
                    statisticsDetail.Phones.Add(new PhoneInfo
                    {
                        CountryCode = Converter.ToString2(reader["Phone2CountryCode"]),
                        AreaCode = Converter.ToString2(reader["Phone2AreaCode"]),
                        PhoneNumber = Converter.ToString2(reader["Phone2"])
                    });
                    statisticsDetail.Phones.Add(new PhoneInfo
                    {
                        CountryCode = Converter.ToString2(reader["Phone3CountryCode"]),
                        AreaCode = Converter.ToString2(reader["Phone3AreaCode"]),
                        PhoneNumber = Converter.ToString2(reader["Phone3"])
                    });

                    statisticsDetail.PICs.Add(new PersonalInfoContact
                    {
                        Name = Converter.ToString2(reader["PIC1Name"]),
                        Phone = Converter.ToString2(reader["PIC1"])
                    });
                    statisticsDetail.PICs.Add(new PersonalInfoContact
                    {
                        Name = Converter.ToString2(reader["PIC2Name"]),
                        Phone = Converter.ToString2(reader["PIC2"])
                    });
                    statisticsDetail.PICs.Add(new PersonalInfoContact
                    {
                        Name = Converter.ToString2(reader["PIC3Name"]),
                        Phone = Converter.ToString2(reader["PIC3"])
                    });
                    statisticsDetail.PICs.Add(new PersonalInfoContact
                    {
                        Name = Converter.ToString2(reader["PIC4Name"]),
                        Phone = Converter.ToString2(reader["PIC4"])
                    });

                    result.Add(statisticsDetail);
                }
            }

            return result;
        }
    }
}
