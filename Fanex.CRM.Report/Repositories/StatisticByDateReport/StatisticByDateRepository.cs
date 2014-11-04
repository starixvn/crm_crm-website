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
    using Fanex.Common;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Report.Criteria.StatisticByDateReport;
    using Fanex.CRM.Report.Entities.StatisticsReportByDate;
    using Fanex.CRM.Report.Models.StatisticsReportByDate;
    using Fanex.CRM.Report.Resources;
    using Fanex.CRM.Setting.Models;
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
        public List<StatisticsDetailbyDateModel> DetailReport(GetDetailReportCriteria criteria)
        {
            var result = new List<StatisticsDetailbyDateModel>();
            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objDb.AssignParamValues(criteria.GetSpParams());
                var reader = objDb.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(new StatisticsDetailbyDateModel
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

                        CommunicationTool = new SettingInfo { Name = Converter.ToString2(reader["Communication"]) },
                        MSN = Converter.ToString2(reader["MSN"]),
                        Skype = Converter.ToString2(reader["Skype"]),
                        Email = Converter.ToString2(reader["Email"]),
                        Phone = Converter.ToString2(reader["Phone"]),
                        MobilePhone = Converter.ToString2(reader["MobilePhone"])
                    });
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
