// --------------------------------------------------------------------------
// <copyright file="DynamicReportRepository.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Repositories.DynamicReport
{
    using System.Collections.Generic;
    using System.Data;
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
        public IEnumerable<DynamicReportInfo> GetDynamicReport(GetDynamicReportCriteria criteria)
        {
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objectDB.ExecuteReader(criteria.GetSpParams()))
                {
                    var communications = new List<DynamicReportInfo>();
                    while (reader.Read())
                    {
                        communications.Add(ReadDynamicReportInfo(reader));
                    }

                    return communications;
                }
            }
        }

        private DynamicReportInfo ReadDynamicReportInfo(IDataReader reader)
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
                MainCategory = new Setting { Name = Converter.ToString2(reader["MainCategory"]) },
                SubCategory = new Setting { Name = Converter.ToString2(reader["SubCategory"]) },
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
