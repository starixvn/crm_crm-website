// --------------------------------------------------------------------------
// <copyright file="CustomerReportRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Repositories.Implements
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Common;
    using Fanex.CRM.Infrastructure.Extension;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Report.Criteria.CustomerReport;
    using Fanex.CRM.Report.Models.CustomerReport;
    using Fanex.CRM.Report.Repositories.Interfaces;
    using Fanex.CRM.Setting.Models;
    using Fanex.Data;

    /// <summary>
    /// Customer Report Repository.
    /// </summary>
    public class CustomerReportRepository : ICustomerReportRepository
    {
        /// <summary>
        /// Gets the customer report.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The customer report.</returns>
        public List<CustomerInfo> GetCustomerReport(GetCustomerReportCriteria criteria)
        {
            List<CustomerInfo> customerReport = new List<CustomerInfo>();
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                objectDB.AssignParamValues(criteria.GetSpParams());
                using (var reader = objectDB.ExecuteReader())
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            customerReport.Add(MappingDataFromSqlToCustomerInfoObject(reader));
                        }
                    }
                }
            }

            return customerReport;
        }

        /// <summary>
        /// Mappings the data from SQL to log information object.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>
        /// Log Info Object.
        /// </returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private CustomerInfo MappingDataFromSqlToCustomerInfoObject(IDataReader reader)
        {
            var customerInfo = new CustomerInfo
             {
                 IsVIP = Converter.ToBool(reader["VIP"]),
                 Title = new SettingInfo
                 {
                     Name = Converter.ToString2(reader["Title"])
                 },
                 Username = Converter.ToString2(reader["UserName"]),
                 BirthDay = Converter.ToDateTime(reader["Birthday"]),
                 BirthDayString = reader["BirthDay"] != DBNull.Value ?
                     Converter.ToDateTime(reader["Birthday"]).ToShortFormatedDate() :
                     string.Empty,
                 SiteSetting = new BaseModel
                 {
                     Name = Converter.ToString2(reader["Site"])
                 },
                 Languages = new List<BaseModel>(),

                 Skype = Converter.ToString2(reader["Skype"]),
                 Email = Converter.ToString2(reader["Email"]),
                 PIC1 = Converter.ToString2(reader["Pic1"]),
                 PIC2 = Converter.ToString2(reader["Pic2"]),
                 PIC3 = Converter.ToString2(reader["Pic3"]),
                 PIC4 = Converter.ToString2(reader["Pic4"]),
             };

            var languages = Converter.ToString2(reader["Language"]);
            var languageArray = languages.Split(',');
            foreach (var language in languageArray)
            {
                customerInfo.Languages.Add(new BaseModel
                {
                    Name = language
                });
            }

            return customerInfo;
        }
    }
}
