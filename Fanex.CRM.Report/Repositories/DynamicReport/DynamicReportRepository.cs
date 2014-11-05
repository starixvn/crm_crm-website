// --------------------------------------------------------------------------
// <copyright file="DynamicReportRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Repositories.DynamicReport
{
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Common;
    using Fanex.CRM.Common.Helpers;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Report.Criteria.DynamicReport;
    using Fanex.CRM.Report.Models;
    using Fanex.Data;

    /// <summary>
    /// Dynamic Report Repository.
    /// </summary>
    public class DynamicReportRepository : IDynamicReportRepository
    {
        /// <summary>
        /// Gets the dynamic report.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The dynamic report.</returns>
        public List<DynamicReportInfo> GetDynamicReport(GetDynamicReportCriteria criteria)
        {
            List<DynamicReportInfo> communicationList = new List<DynamicReportInfo>();
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                objectDB.AssignParamValues(criteria.GetSpParams());
                using (var reader = objectDB.ExecuteReader())
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            var communication = MappingDataFromSqlToCommunicationInfoObject(reader);
                            communicationList.Add(communication);
                        }
                    }
                }
            }

            return communicationList;
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
        private DynamicReportInfo MappingDataFromSqlToCommunicationInfoObject(IDataReader reader)
        {
            var durationTime = Converter.ToString2(reader["Duration"]);
            var durationTimeArray = durationTime.Split(':');
            var communication = new DynamicReportInfo
            {
                RunningNumber = Converter.ToInt(reader["FeedbackID"]),
                PeriodRange = new PeriodRange
                {
                    FromDate = DateTimeUtilize.ChangeTimeZoneToClientDateTime(Converter.ToDateTime(reader["FeedbackDate"]))
                },
                MainCategory = new SettingInfo { Name = Converter.ToString2(reader["MainCategory"]) },
                SubCategory = new SettingInfo { Name = Converter.ToString2(reader["SubCategory"]) },
                Subject = Converter.ToString2(reader["Subject"]),
                StatusName = Converter.ToString2(reader["Status"]),
                Duration = new TimeInfo
                {
                    Hours = Converter.ToInt(durationTimeArray[0]),
                    Minutes = Converter.ToInt(durationTimeArray[1]),
                    Seconds = Converter.ToInt(durationTimeArray[2])
                },
                Handler = Converter.ToString2(reader["HandledBy"]),
                Reviewer = Converter.ToString2(reader["ReviewedBy"]),
                Username = Converter.ToString2(reader["Username"])
            };

            return communication;
        }
    }
}
