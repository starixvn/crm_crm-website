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
                 FirstName = Converter.ToString2(reader["FirstName"]),
                 LastName = Converter.ToString2(reader["LastName"]),
                 BirthDay = Converter.ToDateTime(reader["Birthday"]),
                 BirthDayString = reader["BirthDay"] != DBNull.Value ?
                     Converter.ToDateTime(reader["Birthday"]).ToShortFormatedDate() :
                     string.Empty,
                 Country = new BaseModel
                 {
                     Name = Converter.ToString2(reader["Country"])
                 },
                 SiteSetting = new BaseModel
                 {
                     Name = Converter.ToString2(reader["Site"])
                 },
                 Languages = new List<BaseModel>(),

                 FaxInfo = new FaxInfo
                 {
                     CountryCode = Converter.ToString2(reader["FaxCountryCode"]),
                     AreaCode = Converter.ToString2(reader.GetStringNotNull("FaxAreaCode")),
                     FaxNumber = Converter.ToString2(reader.GetStringNotNull("Fax"))
                 },
                 MobilePhone = new PhoneInfo
                 {
                     CountryCode = Converter.ToString2(reader["MobilePhoneCountryCode"]),
                     AreaCode = Converter.ToString2(reader["MobilePhoneAreaCode"]),
                     PhoneNumber = Converter.ToString2(reader["MobilePhone"])
                 },
                 Skype = Converter.ToString2(reader["Skype"]),
                 Email = Converter.ToString2(reader["Email"]),

                 AddressInfo = new AddressInfo
                 {
                     LineList = new List<string>
                     {
                         reader.GetStringNotNull("Line1"),
                         reader.GetStringNotNull("Line2"),
                         reader.GetStringNotNull("Line3"),
                     },

                     City = reader.GetStringNotNull("City"),
                     State = reader.GetStringNotNull("State"),
                     PostCode = reader.GetStringNotNull("PostCode")
                 }
             };

            customerInfo.PhoneInfo.Add(new PhoneInfo
            {
                CountryCode = Converter.ToString2(reader["PhoneCountryCode"]),
                AreaCode = Converter.ToString2(reader["PhoneAreaCode"]),
                PhoneNumber = Converter.ToString2(reader["Phone"])
            });

            customerInfo.PhoneInfo.Add(new PhoneInfo
            {
                CountryCode = Converter.ToString2(reader["Phone2CountryCode"]),
                AreaCode = Converter.ToString2(reader["Phone2AreaCode"]),
                PhoneNumber = Converter.ToString2(reader["Phone2"])
            });

            customerInfo.PhoneInfo.Add(new PhoneInfo
            {
                CountryCode = Converter.ToString2(reader["Phone3CountryCode"]),
                AreaCode = Converter.ToString2(reader["Phone3AreaCode"]),
                PhoneNumber = Converter.ToString2(reader["Phone3"])
            });

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
