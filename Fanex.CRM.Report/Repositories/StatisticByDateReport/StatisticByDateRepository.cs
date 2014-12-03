// --------------------------------------------------------------------------
// <copyright file="StatisticByDateRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Repositories.StatisticByDateReport
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Common;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Report.Criteria.StatisticByDateReport;
    using Fanex.CRM.Report.Entities.StatisticsReportByDate;
    using Fanex.CRM.Report.Models.StatisticsReportByDate;
    using Fanex.CRM.Report.Resources;
    using Fanex.Data;

    /// <summary>
    /// Statistic By Date Repository.
    /// </summary>
    public class StatisticByDateRepository : IStatisticByDateRepository
    {
        /// <summary>
        /// Gets the main report.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The main report.</returns>
        public StatisticsInfoByDateRepositoryResultModel MainReport(GetMainReportCriteria criteria)
        {
            var result = new StatisticsInfoByDateRepositoryResultModel
            {
                RawData = new List<StatisticByDateEntity>(),
                CommunicationTools = CommunicationToolList()
            };

            var communicationToolLength = result.CommunicationTools.Count;
            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objDb.AssignParamValues(criteria.GetSpParams());
                var reader = objDb.ExecuteReader();
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
            }

            return result;
        }

        /// <summary>
        /// Gets the detail report.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The detail report.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
             "SP2101:MethodMustNotContainMoreLinesThan",
             Justification = "Reviewed. Suppression is OK here.")]
        public List<StatisticsDetailbyDateModel> DetailReport(GetDetailReportCriteria criteria)
        {
            var result = new List<StatisticsDetailbyDateModel>();
            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objDb.AssignParamValues(criteria.GetSpParams());
                var reader = objDb.ExecuteReader();
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

        /// <summary>
        ///  Gets the communication tool list.
        /// </summary>
        /// <returns>The communication tool list.</returns>
        private List<BaseModel> CommunicationToolList()
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
