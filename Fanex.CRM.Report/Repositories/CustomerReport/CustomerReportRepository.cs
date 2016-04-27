// --------------------------------------------------------------------------
// <copyright file="CustomerReportRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Repositories.CustomerReport
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Common;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Infrastructure.Extensions;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Report.Criteria.CustomerReport;
    using Fanex.CRM.Report.Models.CustomerReport;
    using Fanex.Data;

    /// <summary>
    /// Customer Report Repository.
    /// </summary>
    public class CustomerReportRepository : ICustomerReportRepository
    {
        public IEnumerable<CustomerInfo> GetCustomerReport(GetCustomerReportCriteria criteria)
        {
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objectDB.ExecuteReader(criteria.GetSpParams()))
                {
                    var customerReport = new List<CustomerInfo>();
                    while (reader.Read())
                    {
                        customerReport.Add(ReadCustomerInfo(reader));
                    }

                    return customerReport;
                }
            }            
        }

        private static CustomerInfo ReadCustomerInfo(IDataReader reader)
        {
            var customerInfo = new CustomerInfo
             {
                 IsVIP = Converter.ToBool(reader["VIP"]),
                 Title = new Setting
                 {
                     Name = Converter.ToString2(reader["Title"])
                 },
                 Username = Converter.ToString2(reader["UserName"]),
                 BirthDay = Converter.ToDateTime(reader["Birthday"]),
                 BirthDayString = reader["BirthDay"] != DBNull.Value ?
                     Converter.ToDateTime(reader["Birthday"]).ToShortFormatedDate() :
                     string.Empty,
                 SiteSetting = new Site
                 {
                     Name = Converter.ToString2(reader["Site"])
                 },
                 Languages = new List<BaseModel>(),

                 Skype = Converter.ToString2(reader["Skype"]),
                 Email = Converter.ToString2(reader["Email"])
             };

            customerInfo.PersonalInfoContacts.Add(ReadPersonalInfoContact(reader, 1));
            customerInfo.PersonalInfoContacts.Add(ReadPersonalInfoContact(reader, 2));
            customerInfo.PersonalInfoContacts.Add(ReadPersonalInfoContact(reader, 3));
            customerInfo.PersonalInfoContacts.Add(ReadPersonalInfoContact(reader, 4));            

            var languages = Converter.ToString2(reader["Language"]).Split(',');
            foreach (var language in languages)
            {
                customerInfo.Languages.Add(new BaseModel
                {
                    Name = language
                });
            }

            return customerInfo;
        }

        private static PersonalInfoContact ReadPersonalInfoContact(IDataReader reader, int index)
        {
            return new PersonalInfoContact
            {
                Name = Converter.ToString2(reader["Pic" + index + "Name"]),
                Phone = Converter.ToString2(reader["Pic" + index])
            };
        }
    }
}
